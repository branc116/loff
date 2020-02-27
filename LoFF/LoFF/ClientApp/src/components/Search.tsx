import React, { Component } from 'react';
import { AmClient, MetaData, Paramz } from "../AmClient";
import { Col, Row, Container, InputGroup, InputGroupText, Input, Form, Button} from "reactstrap"
import DatePicker, { DatePickerProps } from "react-date-picker";

type SearchParams = {
  Paramz: MetaData[],
  OnSubmit: (paramz: Paramz) => void;
}
type SearchState = {
    paramz: { [key: string]: string }
}
export class Search extends Component<SearchParams, SearchState> {
  static displayName = Search.name;
  /**
   *
   */
  constructor(paramz: Readonly<SearchParams>) {
      super(paramz);
      const old = localStorage.getItem("oldNews");
      this.state = {paramz: old ? JSON.parse(old) : {}};
  }
  private changeValue =  (key: string, val: string)  => {
      const k: {[key: string]: string} = {};
      k[key] = val;
      this.setState({paramz: {...this.state.paramz, ...k}});
      localStorage.setItem("oldNews", JSON.stringify(this.state.paramz));
  }
  private submit = () => {
    this.props.OnSubmit(this.state.paramz as unknown as Paramz);
  }
  render () {
    return (
      <React.Fragment>
          <Form onSubmit={(e) => {e.preventDefault(); this.submit();}} className="d-flex justify-content-between flex-wrap">
            {this.props.Paramz.map(i => {
                const type = i.type;
                if (type === undefined || type === "undefined")
                    return <React.Fragment key={i.name}></React.Fragment>;
                else if (type === "string")
                    return (
                        <InputGroup key={i.name} className="d-flex w-50 justify-content-between">
                            <InputGroupText>{i.name}</InputGroupText>
                            <Input value={this.state.paramz[i.name ?? "nothing"]} onChange={(c) => {this.changeValue(i.name ?? "extra", c.target.value)}}></Input>
                        </InputGroup>
                    );
                else if (type === "number")
                    return (
                        <InputGroup key={i.name} className="d-flex w-50 justify-content-between">
                            <InputGroupText>{i.name}</InputGroupText>
                            <Input type="number" value={this.state.paramz[i.name ?? "nothing"]} onChange={(c) => {this.changeValue(i.name ?? "extra", c.target.value)}}></Input>
                        </InputGroup>
                    );
                else if (type === "date") {
                    const name = i.name;
                    const old = name ? this.state.paramz[name] : undefined;
                    const date = old ? new Date(old) : undefined;
                    return (
                        <InputGroup key={i.name} className="d-flex w-50 justify-content-between">
                            <InputGroupText>{i.name}</InputGroupText>
                            <DatePicker 
                                onChange={(c) => {const d = c instanceof Date ? c : c[0]; this.changeValue(i.name ?? "extra", d.toISOString())}}
                                value={date}></DatePicker>
                        </InputGroup>
                    )
                }
                else
                    return <React.Fragment></React.Fragment>
            })}
            <Button color="primary" type="Submmit">Search</Button>
        </Form>
      </React.Fragment>
    );
  }
}
