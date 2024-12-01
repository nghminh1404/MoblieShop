using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.Entity.DTOs.OrderDTO
{
    public class UpdateOrderRequest
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
    }
}
