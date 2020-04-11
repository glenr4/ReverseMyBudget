import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";
import DateFormat from "../shared/formatters/DateFormat";
import Currency from "../shared/formatters/Currency";
import "./Transactions.css";
import GetResponseHeader from "../shared/GetResponseHeader";
import Pagination from "react-js-pagination";

export interface ITransactionsProps {}

export interface ITransactionsState {
  Loading: boolean;
  Data: ReverseMyBudget.ITransaction[];
  TotalCount: number;
  TotalPages: number;
  CurrentPage: number;
  PageSize: number;
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
      TotalCount: 0,
      TotalPages: 0,
      CurrentPage: 0,
      PageSize: 0,
    };
  }

  componentDidMount() {
    this.getData(1);
  }

  render() {
    let contents = this.state.Loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      <div>
        {this.renderTable(this.state.Data)}
        <Pagination
          activePage={this.state.CurrentPage}
          itemsCountPerPage={this.state.PageSize}
          totalItemsCount={this.state.TotalCount}
          pageRangeDisplayed={5}
          itemClass={"page-item"}
          linkClass={"page-link"}
          activeClass={"page-item active"}
          onChange={this.getData}
        />
      </div>
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

  getData = async (pageNumber: number) => {
    const token = await authService.getAccessToken();

    const options: RequestInit = {
      method: "GET",
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    };

    await fetch(`transactions?PageSize=10&PageNumber=${pageNumber}`, options)
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

      const pageData: ReverseMyBudget.IPageData = JSON.parse(
        GetResponseHeader(response, "x-pagination")
      );

      console.log("successful");
      this.setState({
        Loading: false,
        Data: transactions,
        TotalCount: pageData.TotalCount,
        TotalPages: pageData.TotalPages,
        CurrentPage: pageData.CurrentPage,
        PageSize: pageData.PageSize,
      });
    } else {
      console.log("error");
      alert("There was an error, please try again later");
    }
    this.setState({ Loading: false });
  };
}
