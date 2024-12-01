namespace MobileShop.Entity.DTOs.AccountDTO
{
    public class UpdateAccountRequest
    {
        public int AccountId { get; init; }

        public string FullName { get; init; } = null!;

        public string Mail { get; init; } = null!;

        public string Address { get; init; } = null!;

        public DateTime? Dob { get; init; }

        public bool? Gender { get; init; }

        public string? Phone { get; init; }

        public string? Password { get; init; }

        public bool Active { get; set; }

        public int RoleId { get; init; }

        public bool? IsDeleted { get; init; }
    }
}
