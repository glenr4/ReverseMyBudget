import * as React from "react";
import DayPickerInput from "react-day-picker/DayPickerInput";
import DateFormat from "../formatters/DateFormat";
import "./DatePicker.css";

export interface IDatePickerProps {
  onDateChange: (date: Date) => void;
  placeholder: string;
}

export interface IDatePickerState {}

class DatePicker extends React.Component<IDatePickerProps, IDatePickerState> {
  render() {
    return (
      <div className="input-group mb-3">
        <div className="input-group-prepend">
          <span className="input-group-text fa fa-search" />
          <DayPickerInput
            onDayChange={this.props.onDateChange}
            placeholder={this.props.placeholder}
            formatDate={DateFormat}
          />
        </div>
      </div>
    );
  }
}

export default DatePicker;
