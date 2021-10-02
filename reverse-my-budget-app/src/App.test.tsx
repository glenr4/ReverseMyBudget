import { render, screen } from "@testing-library/react";
import App from "./App";

jest.mock("./auth/auth", () => {});
jest.mock("./components/DataGrid", () => {
  return {
    __esModule: true,
    default: () => <div data-testid="DataGrid" />,
  };
});

it("Renders Sign In Button", () => {
  render(<App />);

  const element = screen.getByRole("button", { name: /Sign In/i });

  expect(element).toBeInTheDocument();
});

it("Renders DataGrid", () => {
  render(<App />);

  const element = screen.getByTestId(/DataGrid/i);

  expect(element).toBeInTheDocument();
});
