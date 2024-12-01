using System;
using System.Collections.Generic;

namespace MobileShop.Entity.Models;

public partial class Image
{
    public int ImageId { get; set; }

    public string? ImageLink { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
