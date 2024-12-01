using System;
using System.Collections.Generic;

namespace MobileShop.Entity.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public double Price { get; set; }

    public int Quantity { get; set; }

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public int ImageId { get; set; }

    public DateTime? CreateDate { get; set; }

    public int CreateBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Account CreateByNavigation { get; set; } = null!;

    public virtual Image Image { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
