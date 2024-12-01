namespace MobileShop.Entity.DTOs.AccountDTO
{
    public class AccountRequest
    {
        public int AccountId { get; set; }

        public string FullName { get; set; } = null!;

        public string Mail { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime? Dob { get; set; }

        public bool? Gender { get; set; }

        public string? Phone { get; set; }

        public string? Password { get; set; }

        public bool Active { get; set; }

        public string? RoleName { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
