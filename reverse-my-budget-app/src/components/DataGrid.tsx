import { useState } from "react";
import { GridApi, ValueFormatterParams } from "ag-grid-community";
import { AgGridReact } from "ag-grid-react";
import { DateTime } from "luxon";
import "ag-grid-community/dist/styles/ag-grid.css";
import "ag-grid-community/dist/styles/ag-theme-alpine.css";
import styles from "./DataGrid.module.css";

const DataGrid = () => {
  const [gridApi, setGridApi] = useState<GridApi>();

  const getRowData = () => {
    const data: Transaction[] = [];

    for (let i = 0; i < 50; i++) {
      data.push({
        id: i.toString(),
        dateLocal: DateTime.now().toISODate(),
        amount: i * Math.random(),
        type: i.toString(),
        description: `Description ${i}`,
        balance: i * Math.random(),
        accountId: i.toString(),
        importOrder: i,
        categoryId: i.toString(),
      });
    }

    return data;
  };

  const currencyValueFormatter = (params: ValueFormatterParams) => {
    const field = params.colDef.field as string;

    return `$${params.data[field].toFixed(2)}`;
  };

  const defaultColDef = {
    flex: 1,
    minWidth: 100,
    sortable: true,
    filter: true,
    resizable: true,
  };

  const columns = [
    {
      field: "id",
    },
    {
      field: "dateLocal",
    },
    {
      field: "amount",
      valueFormatter: currencyValueFormatter,
    },
    {
      field: "type",
    },
    {
      field: "description",
      flex: 4,
    },
    {
      field: "balance",
      valueFormatter: currencyValueFormatter,
    },
    {
      field: "acountId",
    },
    {
      field: "importOrder",
    },
    {
      field: "categoryId",
    },
  ];

  return (
    <div className={`ag-theme-alpine ${styles.gridContainer}`}>
      <AgGridReact
        rowData={getRowData()}
        rowSelection="multiple"
        onGridReady={(params) => setGridApi(params.api)}
        reactUi={true}
        defaultColDef={defaultColDef}
        columnDefs={[...columns]}
      />
    </div>
  );
};

export default DataGrid;
