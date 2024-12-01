using System;
using System.Collections.Generic;

namespace MobileShop.Entity.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int ProductId { get; set; }

    public int CustomerId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Account Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
