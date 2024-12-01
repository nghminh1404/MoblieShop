using Microsoft.AspNetCore.Mvc;
using MobileShop.Entity.DTOs.AccountDTO;
using MobileShop.Service;

namespace MobileShop.API.Controllers
{

    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("add-account")]
        public IActionResult AddAccount([FromBody] CreateAccountRequest account)
        {
            var result = _accountService.AddAccount(account);
            if (result == null)
            {
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpGet("get-account-id/{id}")]
        public IActionResult GetAccountById(int id)
        {
            var account = _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound("Account does not exist");
            }
            return Ok(account);
        }

        [HttpGet("get-account-mail/{mail}")]
        public IActionResult GetAccountByMail(string mail)
        {
            var account = _accountService.GetAccountByEmail(mail);
            if (account == null)
            {
                return NotFound("Account does not exist");
            }
            return Ok(account);
        }

        [HttpGet("get-account-mailguest/{mail}")]
        public IActionResult GetAccountGuestByMail(string mail)
        {
            var account = _accountService.GetAccountGuestByEmail(mail);
            if (account == null)
            {
                return NotFound("Account does not exist");
            }
            return Ok(account);
        }

        [HttpGet("get-account-mail-password/{mail}&{password}")]
        public IActionResult GetAccountByMailAndPassword(string mail, string password)
        {
            var account = _accountService.GetAccountByEmailAndPassword(mail, password);
            if (account == null)
            {
                return NotFound("Account does not exist");
            }
            return Ok(account);
        }


        [HttpGet("get-accounts-role/{role}")]
        public IActionResult GetAccountsByKeyword(int role)
        {

            var accounts = _accountService.GetAccountsByRoleId(role);
            if (accounts != null && accounts.Count == 0)
            {
                return Ok("Don't have account");
            }
            return Ok(accounts);
        }

        [HttpGet("get-accounts-keyword/{keyword}")]
        public IActionResult GetAccountsByKeyword(string keyword)
        {

            var accounts = _accountService.GetAccountsByKeyword(keyword);
            if (accounts != null && accounts.Count == 0)
            {
                return Ok("Don't have account");
            }
            return Ok(accounts);
        }

        [HttpGet("get-all-accounts")]
        public IActionResult GetAllAccount()
        {

            var accounts = _accountService.GetAllAccount();
            if (accounts != null && accounts.Count == 0)
            {
                return Ok("Don't have account");
            }
            return Ok(accounts);
        }

        [HttpPut("put-account")]
        public IActionResult UpdateAccount(UpdateAccountRequest account)
        {
            var result = _accountService.UpdateAccount(account);
            if (result == null)
            {
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpPut("delete-account/{id}")]
        public IActionResult DeleteAccount(int id)
        {
            var result = _accountService.UpdateDeleteStatusAccount(id);
            if (result == false)
            {
                return StatusCode(500);
            }
            return Ok("Delete account complete");
        }
    }
}