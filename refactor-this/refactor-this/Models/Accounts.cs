using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace refactor_this.Models
{
    public class AccountModel
    {
        // singleton instance
        private static AccountModel accountDao;
        public static AccountModel GetInstance()
        {
            if (accountDao == null)
            {
                accountDao = new AccountModel();
            }
            return accountDao;
        }

        public Account GetById(Guid id)
        {
            using (var connection = Helpers.NewConnection())
            {
                SqlCommand command = new SqlCommand($"select * from Accounts where Id = '{id}'", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                if (!reader.Read())
                    throw new ArgumentException();

                var account = new Account(id);
                account.Name = reader["Name"].ToString();
                account.Number = reader["Number"].ToString();
                account.Amount = float.Parse(reader["Amount"].ToString());
                return account;
            }
        }

        public List<Account> GetAll()
        {
            using (var connection = Helpers.NewConnection())
            {
                SqlCommand command = new SqlCommand($"select Id from Accounts", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                var accounts = new List<Account>();
                while (reader.Read())
                {
                    var id = Guid.Parse(reader["Id"].ToString());
                    var account = GetById(id);
                    accounts.Add(account);
                }

                return accounts;
            }
        }

        public void Save(Account account)
        {
            using (var connection = Helpers.NewConnection())
            {
                SqlCommand command;
                if (account.IsNew)
                    command = new SqlCommand($"insert into Accounts (Id, Name, Number, Amount) values ('{Guid.NewGuid()}', '{account.Name}', {account.Number}, 0)", connection);
                else
                    command = new SqlCommand($"update Accounts set Name = '{account.Name}' where Id = '{account.Id}'", connection);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Guid Id)
        {
            using (var connection = Helpers.NewConnection())
            {
                SqlCommand command = new SqlCommand($"delete from Accounts where Id = '{Id}'", connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }

    public class Account
    {
        public bool IsNew { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public float Amount { get; set; }

        public Account()
        {
            IsNew = true;
        }

        public Account(Guid id)
        {
            IsNew = false;
            Id = id;
        }


    }
}