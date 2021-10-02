import { render, screen } from "@testing-library/react";
import DataGrid from "./DataGrid";

test("renders learn react link", () => {
  render(<DataGrid />);
  const element = screen.getByText(/Hello from DataGrid/i);
  expect(element).toBeInTheDocument();
});
