using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace refactor_this.Models
{
    public class TransactionModel
    {
        // singleton instance
        private static TransactionModel transactionDao;
        public static TransactionModel GetInstance()
        {
            if (transactionDao == null)
            {
                transactionDao = new TransactionModel();
            }
            return transactionDao;
        }


        public List<Transaction> GetTransactions(Guid id)
        {
            using (var connection = Helpers.NewConnection())
            {
                SqlCommand command = new SqlCommand($"select Amount, Date from Transactions where AccountId = '{id}'", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                var transactions = new List<Transaction>();
                while (reader.Read())
                {
                    var amount = (float)reader.GetDouble(0);
                    var date = reader.GetDateTime(1);
                    transactions.Add(new Transaction(amount, date));
                }
                return transactions;
            }
        }

        public async void AddTransaction(Guid id, Transaction transaction)
        {
            using (var connection = Helpers.NewConnection())
            {

                SqlCommand command = new SqlCommand($"INSERT INTO Transactions (Id, Amount, Date, AccountId) VALUES ('{Guid.NewGuid()}', {transaction.Amount}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{id}')", connection);
                await connection.OpenAsync();

                // open transaction
                using (SqlTransaction tran = connection.BeginTransaction())
                {
                    //int ret = command.ExecuteNonQuery();
                    int ret = await command.ExecuteNonQueryAsync();
                    if (ret != 1)
                        throw new Exception("Could not insert the transaction");

                    command = new SqlCommand($"update Accounts set Amount = Amount + {transaction.Amount} where Id = '{id}'", connection);
                    if (command.ExecuteNonQuery() != 1)
                    {
                        try
                        {
                            // rollback when sql transaction failed
                            tran.Rollback();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Rollback Exception Type: {0}", e.GetType());
                            Console.WriteLine("  Message: {0}", e.Message);
                        }

                        // rollback Transactions code here
                        throw new Exception("Could not update account amount");
                    }
                    else
                    {
                        tran.Commit();
                    }
                }
            }
        }
    }



    public class Transaction
    {
        public float Amount { get; set; }

        public DateTime Date { get; set; }

        public Transaction(float amount, DateTime date)
        {
            Amount = amount;
            Date = date;
        }
    }
}