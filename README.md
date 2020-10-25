# Yanglong Li's work:
## Solved issues
| Issue | Solved by |
|-------|-----------|
| Data persistence code coupled in Controller | Decoupled `DAO` codes from `Controller` to `Model`
| Bad sql transaction process | Recognise code sequence to: <br/> 1, Update transaction firstly, then account <br/> 2, When transaction failed, rollback data <br/> 3, When both transaction and account update successfully, then commit
| SQL Transaction hasn't Asynchronized | Asynchronized transaction data's persistence
| Account's property isNew has not encapsulated | Encapsulated IsNew
| DELETE a not existing data | Return `Data not found` when deleting a not existing Account

## Issues need to solve
| Issue | Solution |
|-------|----------|
| No exception catching | Need try/catch exception in Controller or Model section. Return message to View

---

# Refactoring Assessment

This repository contains a terribly written Web API project. It's terrible on purpose, so you can show us how we can improve it.

## Getting Started

Fork this repository, and make the changes you would want to see if you had to maintain this api. To set up the project:

 - Open in Visual Studio (2015 or later is preferred)
 - Restore the NuGet packages and rebuild
 - Run the project
 
 Once you are satisied, replace the contents of the readme with a summary of what you have changed, and why. If there are more things that could be improved, list them as well.

The api is composed of the following endpoints:

| Verb     | Path                                   | Description
|----------|----------------------------------------|--------------------------------------------------------
| `GET`    | `/api/Accounts`                        | Gets the list of all accounts
| `GET`    | `/api/Accounts/{id:guid}`              | Gets an account by the specified id
| `POST`   | `/api/Accounts`                        | Creates a new account
| `PUT`    | `/api/Accounts/{id:guid}`              | Updates an account
| `DELETE` | `/api/Accounts/{id:guid}`              | Deletes an account
| `GET`    | `/api/Accounts/{id:guid}/Transactions` | Gets the list of transactions for an account
| `POST`   | `/api/Accounts/{id:guid}/Transactions` | Adds a transaction to an account, and updates the amount of money in the account

Models should conform to the following formats:

**Account**
```
{
    "Id": "01234567-89ab-cdef-0123-456789abcdef",
	"Name": "Savings",
	"Number": "012345678901234",
	"Amount": 123.4
}
```	

**Transaction**
```
{
    "Date": "2018-09-01",
    "Amount": -12.3
}
```