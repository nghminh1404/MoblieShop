using MobileShop.Entity.DTOs.OrderDetailDTO;
using MobileShop.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.Service
{
    public interface IOrderDetailService
    {
        List<OrderDetail> GetAllOrderDetails();
        OrderDetail? GetOrderDetailByOrderIdAndProductId(int oid, int pid);
        CreateOrderDetailResponse AddOrderDetail(CreateOrderDetailRequest detail);
        UpdateOrderDetailResponse UpdateOrderDetail(UpdateOrderDetailRequest detail);
        bool UpdateDeleteStatusOrderDetail(int oid, int pid);
        List<OrderDetail> GetOrderDetailsProcessByCustomerId(int cid);
        bool DeleteOrderDetail(int oid, int pid);
        List<OrderDetail> GetOrderDetailsByOrderID(int oid);
    }
    public class OrderDetailService : IOrderDetailService
    {
        private readonly FstoreContext _context;
        private readonly IOrderService _orderService;

        public OrderDetailService(FstoreContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public List<OrderDetail> GetAllOrderDetails()
        {
            try
            {
                var details = _context.OrderDetails.Where(o => o.IsDeleted == false)
                     .ToList();
                return details;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<OrderDetail> GetOrderDetailsByOrderID(int oid)
        {
            try
            {
                var details = _context.OrderDetails.Where(o => o.IsDeleted == false && o.OrderId == oid)
                     .ToList();
                return details;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<OrderDetail> GetOrderDetailsProcessByCustomerId(int cid)
        {
            try
            {
                var order = _orderService.GetOrderZeroStatusByCustomerId(cid);
                if (order == null)
                {
                    return null;
                }
                var details = _context.OrderDetails.Where(o => o.IsDeleted == false && o.OrderId == order.OrderId)
                     .ToList();
                return details;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public OrderDetail? GetOrderDetailByOrderIdAndProductId(int oid, int pid)
        {
            try
            {
                var detail = _context.OrderDetails.FirstOrDefault(o => o.IsDeleted == false && o.OrderId == oid && o.ProductId == pid);
                if (detail != null)
                {
                    return detail;
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CreateOrderDetailResponse AddOrderDetail(CreateOrderDetailRequest detail)
        {
            try
            {
                var requestOrder = new OrderDetail
                {
                    OrderId = detail.OrderId,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    IsDeleted = detail.IsDeleted
                };
                _context.OrderDetails.Add(requestOrder);
                _context.SaveChanges();
                return new CreateOrderDetailResponse { IsSuccess = true, Message = "Add order detail complete" };
            }
            catch (Exception e)
            {
                return new CreateOrderDetailResponse { IsSuccess = false, Message = $"Add order detail failed {e.Message}" };

            }
        }

        public UpdateOrderDetailResponse UpdateOrderDetail(UpdateOrderDetailRequest detail)
        {
            try
            {
                var requestOrder = new OrderDetail
                {
                    OrderId = detail.OrderId,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    IsDeleted = detail.IsDeleted
                };
                _context.Update(requestOrder);
                //_context.Entry<OrderDetail>(requestOrder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return new UpdateOrderDetailResponse { IsSuccess = true, Message = "Update order detail complete" };
            }
            catch (Exception e)
            {
                return new UpdateOrderDetailResponse { IsSuccess = false, Message = $"Update order detail failed {e.Message}" };
            }
        }

        public bool UpdateDeleteStatusOrderDetail(int oid, int pid)
        {
            try
            {
                var order = GetOrderDetailByOrderIdAndProductId(oid, pid);
                if (order == null)
                {
                    return false;
                }
                order.IsDeleted = true;
                _context.Update(order);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteOrderDetail(int oid, int pid)
        {
            try
            {
                var order = GetOrderDetailByOrderIdAndProductId(oid, pid);
                if (order == null)
                {
                    return false;
                }
                _context.OrderDetails.Remove(order);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
