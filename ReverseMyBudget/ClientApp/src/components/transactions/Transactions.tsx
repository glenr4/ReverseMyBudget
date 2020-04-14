import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";
import Currency from "../shared/formatters/Currency";
import "./Transactions.css";
import GetResponseHeader from "../shared/GetResponseHeader";
import Pagination from "react-js-pagination";
import SearchBar from "../shared/SearchBar";
import "react-day-picker/lib/style.css";
import DateFormat, {
  DateFormatIso8601,
} from "./../shared/formatters/DateFormat";
import DatePicker from "../shared/datePicker/DatePicker";
import moment from "moment";

export interface ITransactionsProps {}

export interface ITransactionsState {
  Loading: boolean;
  Data: ReverseMyBudget.ITransaction[];
  TotalCount: number;
  CurrentPage: number;
  PageSize: number;
  StartDate?: Date;
  EndDate?: Date;
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
      <>
        {this.renderTable(this.state.Data)}
        <div className="pagination justify-content-center">
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
      </>
    );

    return (
      <div className={"glyphicon glyphicon-menu-right"}>
        <h1 id="tabelLabel">Transactions</h1>
        <div className="container">
          <div className="row">
            <div className="col">
              <SearchBar onChange={this.setDescription} />
            </div>
            <div className="col-4 col-lg-3">
              <DatePicker
                placeholder={"Start Date"}
                onDateChange={this.setStartDate}
              />
            </div>
            <div className="col-4  col-lg-3">
              <DatePicker
                placeholder={"End Date"}
                onDateChange={this.setEndDate}
              />
            </div>
            {contents}
          </div>
        </div>
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

  filterDescription: string = "";
  filterStartDate: string = "";
  filterEndDate: string = "";

  setDescription = (description: string) => {
    this.filterDescription = description;

    this.getData(this.state.CurrentPage);
  };

  setStartDate = (date: Date) => {
    this.setState({ StartDate: date });
    this.filterStartDate = date && DateFormatIso8601(date);

    this.getData(this.state.CurrentPage);

    if (this.state.EndDate && date) {
      if (moment(date).isAfter(this.state.EndDate)) {
        alert("End date must be after start date");
      }
    }
  };

  setEndDate = (date: Date) => {
    this.setState({ EndDate: date });
    this.filterEndDate = date && DateFormatIso8601(date);

    this.getData(this.state.CurrentPage);

    if (this.state.StartDate && date) {
      if (moment(this.state.StartDate).isAfter(date)) {
        alert("End date must be after start date");
      }
    }
  };

  buildFilter = () => {
    let filter = "";

    if (this.filterDescription) {
      filter += `Description=${this.filterDescription}&`;
    }

    if (this.filterStartDate) {
      filter += `DateLocal.StartDate=${this.filterStartDate}&`;
    }

    if (this.filterEndDate) {
      filter += `DateLocal.EndDate=${this.filterEndDate}&`;
    }

    return filter;
  };

  getData = async (pageNumber: number) => {
    const filter = this.buildFilter();

    const token = await authService.getAccessToken();

    const options: RequestInit = {
      method: "GET",
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    };

    await fetch(
      `transactions?PageSize=10&PageNumber=${pageNumber}&${filter}`,
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

      const pageData: ReverseMyBudget.IPageData = JSON.parse(
        GetResponseHeader(response, "x-pagination")
      );

      console.log("successful");
      this.setState({
        Loading: false,
        Data: transactions,
        TotalCount: pageData.TotalCount,
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
