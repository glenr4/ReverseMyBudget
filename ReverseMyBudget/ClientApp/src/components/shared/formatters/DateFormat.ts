import moment from "moment";

const format = "D/MM/YYYY";

const DateFormat = (value: string): string => {
  return moment(value).format(format);
};

export default DateFormat;
