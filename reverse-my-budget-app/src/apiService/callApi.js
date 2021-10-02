const callApi = (endpoint, token) => {
  const headers = new Headers();
  const bearer = `Bearer ${token}`;

  headers.append("Authorization", bearer);

  const options = {
    method: "GET",
    headers: headers,
  };

  console.log("Calling web API...");

  fetch(endpoint, options)
    .then((response) => response.json())
    .then((response) => {
      if (response) {
        console.log("Web API responded: " + response.name);
      }

      return response;
    })
    .catch((error) => {
      console.error(error);
    });
};
export default callApi;
