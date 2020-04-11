function GetResponseHeader(response, headerName) {
  for (var pair of response.headers.entries()) {
    if (pair[0] === headerName) {
      return pair[1];
    }
  }
}
export default GetResponseHeader;
