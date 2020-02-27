using System;
using System.Collections.Generic;
using System.Linq;

namespace LoFF.Features.Helpers
{
    public static class KeyGenHelper
    {
        public static string GenerateKey<T>(this T val)
        {
            var key = typeof(T).GetProperties()
                .OrderBy(i => i.Name)
                .Select(i => i.GetValue(val)?.ToString()?.Trim()?.ToLower())
                .Aggregate((i, j) => $"{i}|||{j}");
            return key;
        }
        public static Dictionary<string, object> GetObjDict<T>(this T val)
        {
            var arr = typeof(T).GetProperties().Select(i => (i.Name, i.GetValue(val))).ToDictionary(i => i.Name, i => i.Item2);
            return arr;
        }
        public static List<MetaData> GetMetaData<T>(this T val)
        {
            return GetMetaData<T>();
        }
        public static List<MetaData> GetMetaData<T>()
        {
            var fields = typeof(T).GetProperties().Select(i =>
            {
                var type = i.PropertyType == typeof(int) || i.PropertyType == typeof(float) || i.PropertyType == typeof(double) ? "number" :
                    i.PropertyType == typeof(int?) || i.PropertyType == typeof(float) || i.PropertyType == typeof(double) ? "number" :
                    i.PropertyType == typeof(string) ? "string" :
                    i.PropertyType == typeof(DateTime) || i.PropertyType == typeof(DateTimeOffset) || i.PropertyType == typeof(DateTime?) || i.PropertyType == typeof(DateTimeOffset?) ? "date" :
                    "undefined";
                var meta = new MetaData(i.Name, type);
                return meta;
            }).ToList();
            return fields;
        }
    }

    public class MetaData
    {
        public string Name;
        public string Type;

        public MetaData(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            return obj is MetaData other &&
                   Name == other.Name &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type);
        }

        public void Deconstruct(out string name, out string type)
        {
            name = Name;
            type = Type;
        }

        public static implicit operator (string Name, string Type)(MetaData value)
        {
            return (value.Name, value.Type);
        }

        public static implicit operator MetaData((string Name, string Type) value)
        {
            return new MetaData(value.Name, value.Type);
        }
    }
}
