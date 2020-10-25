using refactor_this.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace refactor_this.Controllers
{
    public class AccountController : ApiController
    {
        [HttpGet, Route("api/Accounts/{id}")]
        public IHttpActionResult GetById(Guid id)
        {
            return Ok(AccountModel.GetInstance().GetById(id));
        }

        [HttpGet, Route("api/Accounts")]
        public IHttpActionResult Get()
        {
            List<Account> accounts = AccountModel.GetInstance().GetAll();
            return Ok(accounts);
        }

        [HttpPost, Route("api/Accounts")]
        public IHttpActionResult Add(Account account)
        {
            AccountModel.GetInstance().Save(account);
            return Ok();
        }

        [HttpPut, Route("api/Accounts/{id}")]
        public IHttpActionResult Update(Guid id, Account account)
        {
            var existing = AccountModel.GetInstance().GetById(id);
            existing.Name = account.Name;
            AccountModel.GetInstance().Save(account);
            return Ok();
        }

        [HttpDelete, Route("api/Accounts/{id}")]
        public IHttpActionResult Delete(Guid id)
        {
            var existing = AccountModel.GetInstance().GetById(id);
            if (existing != null)
            {
                AccountModel.GetInstance().Delete(id);
                return Ok();
            }
            else return NotFound();
        }

    }
}