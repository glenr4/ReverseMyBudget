declare namespace ReverseMyBudget {
  interface IPageData {
    TotalCount: number;
    PageSize: number;
    CurrentPage: number;
    TotalPages: number;
    HasNext: boolean;
    HasPrevious: boolean;
  }
}
