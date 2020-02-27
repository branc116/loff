import React, { Component } from 'react';
import { AmClient, MetaData, Paramz, FlightOffers } from "../AmClient";
import { Search } from './Search';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSpinner } from "@fortawesome/free-solid-svg-icons"
import { Container } from 'reactstrap';
import { FlightTable } from './FlightTable';
type HomeParams = {
  Client: AmClient
}
type HomeState = {
  paramz?: MetaData[],
  flights?: FlightOffers
}
export class Home extends Component<HomeParams, HomeState> {
  static displayName = Home.name;
  public async componentDidMount() {
    const metaData = await this.props.Client.getMetaData();
    this.setState({paramz: metaData});
  }
  public GetParamz() {
    // this.props.Client.getFlightOffers()
  }
  public OnSearchWantedAsync = async (paramz: Paramz) => {
    const a = await this.props.Client.getFlightOffers(paramz);
    this.setState({flights: a});
  }
  render () {
    const meta = this.state?.paramz;
    const flights = this.state?.flights;
    return (
      <React.Fragment>
        {meta ?
          <Search OnSubmit={this.OnSearchWantedAsync} Paramz={meta}></Search> : 
          <FontAwesomeIcon icon={faSpinner}></FontAwesomeIcon>
          }
        {
          flights ? 
          <FlightTable flights={flights}></FlightTable> :
          <React.Fragment></React.Fragment>
        }
      </React.Fragment>
    );
  }
}
