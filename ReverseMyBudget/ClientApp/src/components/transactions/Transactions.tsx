import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";

export interface ITransactionsProps {}

export interface ITransactionsState {
  Loading: boolean;
  Data: ReverseMyBudget.ITransaction[];
}

// export class OwnersApp extends React.Component<IOwnersAppProps, IOwnersAppState> {

export class Transactions extends Component<
  ITransactionsProps,
  ITransactionsState
> {
  // static displayName = FetchData.name;

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
        <h1 id="tabelLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
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
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item) => (
            <tr key={item.DateLocal}>
              <td>{item.Description}</td>
              <td>{item.Amount}</td>
              <td>{item.Balance}</td>
              <td>{item.IsDuplicate}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  };

  getData = async () => {
    debugger;
    const token = await authService.getAccessToken();

    const options: RequestInit = {
      method: "GET",
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    };

    await fetch(`transactions`, options)
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

  processResponse = (response: Response) => {
    if (response.status < 400) {
      debugger;
      const transactions = response.json();

      console.log("successful");
      this.setState({
        Loading: false,
        // Data: transactions,
      });

      // TODO redirect to the Transactions view
    } else {
      console.log("error");
      alert("There was an error during upload, please try again later");
    }
    this.setState({ Loading: false });
  };
}
