import * as React from "react";
import lodash from "lodash";

export interface SearchBarProps {
  onChange: any;
}

export interface SearchBarState {}

class SearchBar extends React.Component<SearchBarProps, SearchBarState> {
  constructor(props: SearchBarProps) {
    super(props);

    // Delay search action
    this.onChangeDebounced = lodash.debounce(this.onChangeDebounced, 1000);
  }

  render() {
    return (
      <div className="input-group mb-3">
        <div className="input-group-prepend">
          <span className="input-group-text fa fa-search" />
        </div>
        <input
          type="text"
          placeholder="Search"
          onChange={this.onChange}
          className="form-control"
          aria-label="Search"
        />
      </div>
    );
  }

  onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    console.log("Immediate value: " + event.target.value);

    this.onChangeDebounced(event.target.value);
  };

  onChangeDebounced = (value: string) => {
    console.log("Delayed value: " + value);
    this.props.onChange(value);
  };
}

export default SearchBar;
