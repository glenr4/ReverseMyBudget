// This component is from: https://www.carlrippon.com/react-drop-down-data-binding/
import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";

export class Dropdown extends Component {
  state = {
    items: [],
    selectedItem: "",
  };

  async componentDidMount() {
    const token = await authService.getAccessToken();
    fetch(this.props.itemsUrl, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    })
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        // Need to put this call in the parent component and use API Context, so that this
        // can stay generic
        let items = data.map((item) => {
          return { value: item.id, display: item.type };
        });
        this.setState({
          items: [
            {
              value: "",
              display: "(Select an item)",
            },
          ].concat(items),
        });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  render() {
    return (
      <div>
        <select
          value={this.state.selectedItem}
          onChange={(e) => {
            this.props.itemSelected(e.target.value);

            this.setState({
              selectedItem: e.target.value,
            });
          }}
        >
          {this.state.items.map((item) => (
            <option key={item.value} value={item.value}>
              {item.display}
            </option>
          ))}
        </select>
        <div
          style={{
            color: "red",
            marginTop: "5px",
          }}
        ></div>
      </div>
    );
  }
}
