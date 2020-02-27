import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { AmClient } from './AmClient';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') ?? "/";
const rootElement = document.getElementById('root');
const client = new AmClient(baseUrl);

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <App Client={client} />
  </BrowserRouter>,
  rootElement);

registerServiceWorker();

