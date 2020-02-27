import React, { Component } from 'react';
import { AmClient, MetaData, Paramz, FlightOffers, PricingDetail } from "../AmClient";
import { Table } from 'reactstrap';

type FlightTableParams = {
    flights: FlightOffers
}
type FlightTableState = {
}

export class FlightTable extends Component<FlightTableParams, FlightTableState> {
    static displayName = FlightTable.name;
    getKeyValues = (prefix: string, obj: any) => {
        const outObj: { [key: string]: string } = {}
        for (const el in obj) {
            const val = obj[el];
            if (typeof val === "number" || typeof val === "string") {
                outObj[prefix + el] = val.toString();
            }
        }
        return outObj;
    }
    getData = () => {
        const data = this.props.flights.data?.flatMap(i => {
            return i.offerItems?.flatMap(j => {
                const b = j.services?.flatMap(k => {
                    return k.segments?.map(g => {
                        return {
                            TotalPrice: j.price?.totalTaxes,
                            Departure: g.flightSegment?.departure?.iataCode,
                            DepartureDate: g.flightSegment?.departure?.at,
                            Arrival: g.flightSegment?.arrival?.iataCode,
                            ArrivalDate: g.flightSegment?.arrival?.at,
                            PricePerAdult: g.pricingDetailPerAdult?.fareBasis,
                            PricePerChild: g.pricingDetailPerChild?.fareBasis,
                            PricePerSenior: g.pricingDetailPerSenior?.fareBasis,
                            PricePerInfant: g.pricingDetailPerInfant?.fareBasis,
                            Stops: g.flightSegment?.stops?.length ?? 0,
                            id: `${i.id} ${g.flightSegment?.number}`
                        };
                    });
                });
                return b;
            });
        });
        return data;
    }
    getKeys = (obj: any[]) => {
        const keys: {[key: string]: number} = {};
        for(let i=0;i<obj.length; i++) {
            for(const key in obj[i]) {
                if (obj[i][key]) {
                    if (!keys[key])
                        keys[key] = 1;
                    else
                        keys[key]++;
                }
            }
        }
        const out: string[] = [];
        for(const k in keys) {
            out.push(k);
        }
        return out;
    }
    getList = (keys: string[], val: any) => {
        return keys.map(i => val[i] ?? " - ").map(i => i.toString() as string);
    }
    render = () => {
        const vals = this.getData();
        if (!vals) {
            return <div>NO DATA</div>
        }
        const keys = this.getKeys(vals);
        return (
        <React.Fragment>
            <Table>
                <thead>
                    <tr>
                        {keys.map(i => <th key={i}>{i}</th>)}
                    </tr>
                </thead>
                <tbody>
                    {vals.map(i => (
                    <tr key={i?.id}>
                        {this.getList(keys, i).map(j => <td key={j}>{j}</td>)}
                    </tr>))}
                    
                </tbody>
            </Table>
        </React.Fragment>);
    }
}
