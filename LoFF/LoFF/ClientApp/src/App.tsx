import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';

import './custom.css'
import { AmClient } from './AmClient';

type AppParams = {
  Client: AmClient
}

export default class App extends Component<AppParams> {
  static displayName = App.name;
  
  render () {
    return (
      <Layout>
        <Route exact path='/'>
          <Home Client={this.props.Client}></Home>
        </Route>
      </Layout>
    );
  }
}
