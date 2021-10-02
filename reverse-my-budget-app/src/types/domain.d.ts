interface Transaction {
  id: string;
  dateLocal: string;
  amount: number;
  type: string;
  description: string;
  balance: number;
  accountId: string;
  importOrder: number;
  categoryId: string;
}
