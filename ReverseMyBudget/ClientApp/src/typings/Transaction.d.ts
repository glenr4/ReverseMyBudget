declare namespace ReverseMyBudget {
  interface ITransaction {
    Id: string;
    DateLocal: string;
    Amount: number;
    Type: string;
    Description: string;
    Balance: number;
    AccountId: string;
    ImportOrder;
    CategoryId: string;
    IsDuplicate: boolean;
  }
}
