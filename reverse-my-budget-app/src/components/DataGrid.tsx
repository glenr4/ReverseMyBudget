import { useState } from "react";
import { GridApi } from "ag-grid-community";
import { AgGridColumn, AgGridReact } from "ag-grid-react";
import { DateTime } from "luxon";
import "ag-grid-community/dist/styles/ag-grid.css";
import "ag-grid-community/dist/styles/ag-theme-alpine.css";

const DataGrid = () => {
  const [gridApi, setGridApi] = useState<GridApi>();

  const getRowData = () => {
    const data = [];

    for (let i = 0; i < 50; i++) {
      data.push({
        id: i,
        dateLocal: DateTime.now().toISODate(),
        amount: i * Math.random(),
        type: i,
        description: `Description ${i}`,
        balance: i * Math.random(),
        accountId: i,
        importOrder: i,
        categoryId: i,
      });
    }

    return data;
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
    },
    {
      field: "type",
    },
    {
      field: "description",
    },
    {
      field: "balance",
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
    <div
      className="ag-theme-alpine"
      style={{ height: "100vh", width: "100vw" }}
    >
      <AgGridReact
        // ref={gridRef}
        rowData={getRowData()}
        rowSelection="multiple"
        onGridReady={(params) => setGridApi(params.api)}
        reactUi={true}
        defaultColDef={defaultColDef}
        columnDefs={[...columns]}
      >
        <AgGridColumn field="id" sortable={true} filter={true} />
        <AgGridColumn field="text" sortable={true} filter={true} />
      </AgGridReact>
    </div>
  );
};

export default DataGrid;
