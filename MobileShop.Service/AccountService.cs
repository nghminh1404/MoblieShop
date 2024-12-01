using MobileShop.Entity.DTOs.AccountDTO;
using MobileShop.Entity.Models;

namespace MobileShop.Service
{
    public interface IAccountService
    {
        List<Account>? GetAccountsByKeyword(string keyword);
        List<Account>? GetAllAccount();
        Account? GetAccountById(int id);
        Account? GetAccountByEmailAndPassword(string email, string password);
        Account? GetAccountByEmail(string email);
        CreateAccountResponse AddAccount(CreateAccountRequest account);
        UpdateAccountResponse UpdateAccount(UpdateAccountRequest account);
        bool UpdateDeleteStatusAccount(int id);
        List<Account> GetAccountsByRoleId(int roleId);
        Account? GetAccountGuestByEmail(string email);
    }
    public class AccountService : IAccountService
    {
        private readonly FstoreContext _context;
        private readonly IEncryptionService _encryptionService;

        public AccountService(FstoreContext context, IEncryptionService encryptionService)
        {
            _context = context;
            _encryptionService = encryptionService;
        }


        public List<Account> GetAccountsByKeyword(string keyword)
        {
            try
            {
                var accounts = _context.Accounts.Where(x => x.FullName.ToLower().Contains(keyword.ToLower())).ToList();
                return accounts;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Account> GetAccountsByRoleId(int roleId)
        {
            try
            {
                var accounts = _context.Accounts.Where(x => x.RoleId == roleId).ToList();
                return accounts;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Account> GetAllAccount()
        {
            try
            {
                var accounts = _context.Accounts.Where(a => a.IsDeleted == false)
                     .ToList();
                return accounts;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Account? GetAccountById(int id)
        {

            try
            {
                var account = _context.Accounts.FirstOrDefault(a => a.AccountId == id && a.IsDeleted == false);
                return account ?? null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public Account? GetAccountByEmailAndPassword(string email, string password)
        {
            try
            {
                var account = _context.Accounts.FirstOrDefault(a => !string.IsNullOrEmpty(a.Password) && a.Mail.Equals(email) && a.Password.Equals(password));
                return account ?? null;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Account? GetAccountByEmail(string email)
        {
            try
            {
                var account = _context.Accounts.Where(a => a.Mail.Equals(email) && a.IsDeleted == false && a.Active == true && a.RoleId != 1002).FirstOrDefault();
                return account ?? null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Account? GetAccountGuestByEmail(string email)
        {
            try
            {
                var account = _context.Accounts.Where(a => a.Mail.Equals(email) && a.IsDeleted == false && a.Active == false && a.RoleId == 1002).FirstOrDefault();
                return account ?? null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CreateAccountResponse AddAccount(CreateAccountRequest account)
        {
            try
            {
                var password = string.Empty;
                if (account.Password is not null)
                {
                    password = _encryptionService.HashMD5(account.Password);
                }
                var requestAccount = new Account
                {
                    FullName = account.FullName,
                    Mail = account.Mail,
                    Address = account.Address,
                    Dob = account.Dob,
                    Gender = account.Gender,
                    Phone = account.Phone,
                    Password = password,
                    Active = account.Active,
                    RoleId = account.RoleId,
                    IsDeleted = account.IsDeleted
                };
                _context.Accounts.Add(requestAccount);
                _context.SaveChanges();
                return new CreateAccountResponse { IsSuccess = true, Message = "Add account complete" };
            }
            catch (Exception e)
            {
                return new CreateAccountResponse { IsSuccess = false, Message = $"Add account failed {e.Message}" };

            }
        }

        public UpdateAccountResponse UpdateAccount(UpdateAccountRequest account)
        {
            try
            {

                var requestAccount = new Account
                {
                    AccountId = account.AccountId,
                    FullName = account.FullName,
                    Mail = account.Mail,
                    Address = account.Address,
                    Dob = account.Dob,
                    Gender = account.Gender,
                    Phone = account.Phone,
                    Password = account.Password,
                    Active = account.Active,
                    RoleId = account.RoleId,
                    IsDeleted = account.IsDeleted
                };
                _context.Update(requestAccount);
                _context.SaveChanges();
                return new UpdateAccountResponse { IsSuccess = true, Message = "Update account complete" };
            }
            catch (Exception e)
            {
                return new UpdateAccountResponse { IsSuccess = false, Message = "Update account failed" + e.Message };
            }
        }

        public bool UpdateDeleteStatusAccount(int id)
        {
            try
            {
                var account = GetAccountById(id);
                if (account == null)
                {
                    return false;
                }
                account.IsDeleted = true;
                _context.Update(account);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
