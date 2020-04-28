declare namespace ReverseMyBudget {
  interface IPageData {
    totalCount: number;
    pageSize: number;
    currentPage: number;
    totalPages: number;
    hasNext: boolean;
    hasPrevious: boolean;
  }
}
