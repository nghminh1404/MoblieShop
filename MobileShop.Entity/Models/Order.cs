using System;
using System.Collections.Generic;

namespace MobileShop.Entity.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string Address { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public DateTime? ShippingDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public int? Status { get; set; }

    public int? PaymentId { get; set; }

    public int? CouponId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Coupon? Coupon { get; set; }

    public virtual Account Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Payment? Payment { get; set; }
}
