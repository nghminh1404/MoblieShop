using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.Entity.DTOs.CouponDTO
{
    public class CreateCouponRequest
    {
        public string Code { get; set; } = null!;

        public int DiscountPercent { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
