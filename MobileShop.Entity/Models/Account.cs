using System;
using System.Collections.Generic;

namespace MobileShop.Entity.Models;

public partial class Account
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

    public int RoleId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Role Role { get; set; } = null!;
}
