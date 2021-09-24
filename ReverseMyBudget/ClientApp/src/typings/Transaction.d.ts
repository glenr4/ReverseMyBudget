declare namespace ReverseMyBudget {
  interface ITransaction {
    id: string;
    dateLocal: string;
    amount: number;
    type: string;
    description: string;
    balance: number;
    accountId: string;
    importOrder;
    categoryId: string;
    isDuplicate: boolean;
  }
}
