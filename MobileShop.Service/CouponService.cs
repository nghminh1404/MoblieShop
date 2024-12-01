using Microsoft.EntityFrameworkCore;
using MobileShop.Entity.DTOs.CouponDTO;
using MobileShop.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.Service
{
    public interface ICouponService
    {
        List<Coupon> GetALLCoupon();
        Coupon? GetCouponById(int id);
        CreateCouponResponse AddCoupon(CreateCouponRequest coupon);
        UpdateCouponResponse UpdateCoupon(UpdateCouponRequest coupon);
        bool UpdateDeleteStatusCoupon(int id);
        bool CheckExpiryCoupon(int id);
        Coupon? GetCouponByCode(string code);
        List<Coupon> GetCouponByKey(string key);
        Coupon? GetCouponById2(int id);
    }
    public class CouponService : ICouponService
    {
        private readonly FstoreContext _context;

        public CouponService(FstoreContext context)
        {
            _context = context;
        }
        public List<Coupon> GetALLCoupon()
        {
            try
            {
                var coupons = _context.Coupons.ToList();
                return coupons;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public List<Coupon> GetCouponByKey(string key)
        {
            try
            {
                var list = new List<Coupon>();
                var sql = $"select * from Coupons where code like '%{key}%'";
                var coupons = _context.Coupons.FromSqlRaw(sql);
                foreach (var item in coupons)
                {
                    var coupon = new Coupon
                    {
                        CouponId = item.CouponId,
                        Code = item.Code,
                        DiscountPercent = item.DiscountPercent,
                        ExpirationDate = item.ExpirationDate,
                        IsDeleted = item.IsDeleted
                    };
                    list.Add(coupon);
                }

                return list;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public Coupon? GetCouponById(int id)
        {

            try
            {
                var coupon = _context.Coupons.FirstOrDefault(c => c.IsDeleted == false && c.CouponId == id);
                return coupon ?? null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //get coupon when discount code enable
        public Coupon? GetCouponById2(int id)
        {

            try
            {
                var coupon = _context.Coupons.FirstOrDefault(c =>c.CouponId == id);
                return coupon ?? null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Coupon? GetCouponByCode(string code)
        {

            try
            {
                var coupon = _context.Coupons.FirstOrDefault(c => c.IsDeleted == false && c.Code.ToLower().Equals(code.ToLower()));
                return coupon ?? null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CreateCouponResponse AddCoupon(CreateCouponRequest coupon)
        {
            try
            {
                var requestCoupon = new Coupon
                {
                    Code = coupon.Code,
                    DiscountPercent = coupon.DiscountPercent,
                    ExpirationDate = coupon.ExpirationDate,
                    IsDeleted = coupon.IsDeleted
                };
                _context.Coupons.Add(requestCoupon);
                _context.SaveChanges();
                return new CreateCouponResponse { IsSuccess = true, Message = "Add coupon complete" };
            }
            catch (Exception e)
            {
                return new CreateCouponResponse { IsSuccess = false, Message = $"Add coupon failed {e.Message}" };

            }
        }

        public UpdateCouponResponse UpdateCoupon(UpdateCouponRequest coupon)
        {
            try
            {

                var requestCoupon = new Coupon
                {
                    CouponId = coupon.CouponId,
                    Code = coupon.Code,
                    DiscountPercent = coupon.DiscountPercent,
                    ExpirationDate = coupon.ExpirationDate,
                    IsDeleted = coupon.IsDeleted
                };
                _context.Update(requestCoupon);
                _context.SaveChanges();
                return new UpdateCouponResponse { IsSuccess = true, Message = "Update coupon complete" };
            }
            catch (Exception e)
            {
                return new UpdateCouponResponse { IsSuccess = false, Message = "Update coupon failed" + e.Message };
            }
        }

        public bool UpdateDeleteStatusCoupon(int id)
        {
            try
            {
                var coupon = GetCouponById(id);
                if (coupon == null)
                {
                    return false;
                }
                coupon.IsDeleted = true;
                _context.Update(coupon);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CheckExpiryCoupon(int id)
        {
            try
            {
                var coupon = GetCouponById(id);
                if (coupon == null)
                {
                    return false;
                }
                if (coupon.ExpirationDate == null)
                {
                    return true;
                }
                if (coupon.ExpirationDate > DateTime.Now)
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
