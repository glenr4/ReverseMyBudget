import moment from "moment";

const format = "D/MM/YYYY";
const iso8601Format = "YYYY-MM-DD";

const DateFormat = (value: any): string => {
  return moment(value).format(format);
};

export const DateFormatIso8601 = (value: any): string => {
  return moment(value).format(iso8601Format);
};

export default DateFormat;
