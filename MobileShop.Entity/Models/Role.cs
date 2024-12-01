using System;
using System.Collections.Generic;

namespace MobileShop.Entity.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
