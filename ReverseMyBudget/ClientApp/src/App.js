import React, { Component } from "react";
import { Route } from "react-router";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import AuthorizeRoute from "./components/api-authorization/AuthorizeRoute";
import ApiAuthorizationRoutes from "./components/api-authorization/ApiAuthorizationRoutes";
import { ApplicationPaths } from "./components/api-authorization/ApiAuthorizationConstants";
import { UploadTransactions } from "./components/transactions/upload/UploadTransactions";
import { Transactions } from "./components/transactions/Transactions";
import "./custom.css";

export default class App extends Component {
  static displayName = App.name;

  // Note: if this app and the API are hosted on the same server, then the route paths here
  // must be different to the API paths, otherwise on browser refresh, the browser will
  // call directly to the API and will get a 401 response.
  render() {
    return (
      <Layout>
        <Route exact path="/" component={Home} />
        <AuthorizeRoute
          path="/upload-transactions"
          component={UploadTransactions}
        />
        <AuthorizeRoute path="/get-transactions" component={Transactions} />
        <Route
          path={ApplicationPaths.ApiAuthorizationPrefix}
          component={ApiAuthorizationRoutes}
        />
      </Layout>
    );
  }
}
