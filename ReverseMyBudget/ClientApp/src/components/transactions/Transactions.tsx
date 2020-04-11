import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";
import DateFormat from "../shared/formatters/DateFormat";
import Currency from "../shared/formatters/Currency";
import "./Transactions.css";

export interface ITransactionsProps {}

export interface ITransactionsState {
  Loading: boolean;
  Data: ReverseMyBudget.ITransaction[];
}

export class Transactions extends Component<
  ITransactionsProps,
  ITransactionsState
> {
  constructor(props: ITransactionsProps) {
    super(props);
    this.state = {
      Data: [],
      Loading: true,
    };
  }

  componentDidMount() {
    this.getData();
  }

  render() {
    let contents = this.state.Loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      this.renderTable(this.state.Data)
    );

    return (
      <div>
        <h1 id="tabelLabel">Transactions</h1>
        {contents}
      </div>
    );
  }

  renderTable = (data: ReverseMyBudget.ITransaction[]) => {
    return (
      <table className="table table-striped" aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Description</th>
            <th className="right-align">Amount</th>
            <th className="right-align">Balance</th>
            <th>Duplicate?</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item) => (
            <tr key={item.Id}>
              <td>{DateFormat(item.DateLocal)}</td>
              <td>{item.Description}</td>
              <td className="right-align">{Currency(item.Amount)}</td>
              <td className="right-align">{Currency(item.Balance)}</td>
              <td>{item.IsDuplicate}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  };

  getData = async () => {
    const token = await authService.getAccessToken();

    const options: RequestInit = {
      method: "GET",
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    };

    await fetch(
      `transactions?description=coles&datelocal.startdate=2020-03-17`,
      options
    )
      .then((response: Response) => {
        console.log(response);

        this.processResponse(response);
      })
      .catch((reason) => {
        console.log(reason);

        alert("There was an error, please try again later");

        this.setState({ Loading: false });
      });
  };

  processResponse = async (response: Response) => {
    if (response.status < 400) {
      const transactions = await response.json();

      console.log("successful");
      this.setState({
        Loading: false,
        Data: transactions,
      });

      // TODO redirect to the Transactions view
    } else {
      console.log("error");
      alert("There was an error, please try again later");
    }
    this.setState({ Loading: false });
  };
}
