using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.Entity.DTOs.CouponDTO
{
    public class UpdateCouponRequest
    {
        public int CouponId { get; set; }

        public string Code { get; set; } = null!;

        public int DiscountPercent { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
