using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoFF.Features.Auth
{
    public sealed class RefresTokensServiceWorker : IHostedService, IDisposable
    {
        private HttpClient _privateClient;
        private Task<Task> _task;
        private readonly ILogger<RefresTokensServiceWorker> _logger;
        private readonly HttpClient _publicClient;
        private readonly IEnumerable<KeyValuePair<string, string>> _data;
        private bool _stop;
        public RefresTokensServiceWorker(IConfiguration config, ILogger<RefresTokensServiceWorker> logger, HttpClient publicClient)
        {
            _logger = logger;
            _publicClient = publicClient;
            _data = new Dictionary<string, string>()
            {
                {"grant_type", "client_credentials" },
                {"client_id", config.GetValue<string>("apikey") },
                {"client_secret", config.GetValue<string>("apisecret") }
            }.AsEnumerable();
        }

        public void Dispose()
        {
            _privateClient.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _privateClient = new HttpClient();
            _task = Task.Factory.StartNew(async () =>
            {
                while (!_stop)
                    await DoItAsync();
            });
        }

        private async Task DoItAsync()
        {
            var res = await _privateClient.PostAsync("https://test.api.amadeus.com/v1/security/oauth2/token", new FormUrlEncodedContent(_data));
            var content = await res.Content.ReadAsStreamAsync();
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var ret = await System.Text.Json.JsonSerializer.DeserializeAsync<AuthRetun>(content);
                _publicClient.DefaultRequestHeaders.Remove("Authorization");
                _publicClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ret.access_token}");

                await Task.Delay(TimeSpan.FromSeconds(ret.expires_in) - TimeSpan.FromMinutes(2));
            }
            else
            {
                _logger.LogError($"Can't get access token {res.StatusCode} {res.ReasonPhrase}");
                await Task.Delay(TimeSpan.FromSeconds(4));
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _stop = true;
            Dispose();
        }
    }
}
