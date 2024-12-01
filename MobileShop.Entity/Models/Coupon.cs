using System;
using System.Collections.Generic;

namespace MobileShop.Entity.Models;

public partial class Coupon
{
    public int CouponId { get; set; }

    public string Code { get; set; } = null!;

    public int DiscountPercent { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
