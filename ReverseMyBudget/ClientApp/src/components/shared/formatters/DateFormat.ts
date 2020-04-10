import moment from "moment";

const DateFormat = (value: string): string => {
  return moment(value).format("D/MM/YYYY");
};

export default DateFormat;
