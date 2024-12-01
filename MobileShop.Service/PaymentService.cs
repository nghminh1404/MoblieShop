
using MobileShop.Entity.DTOs.PaymentDTO;
using MobileShop.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShop.Service
{
    public interface IPaymentService
    {
        List<Payment> GetAllPayment();
        Payment? GetPaymentById(int id);
        CreatePaymentResponse AddPayment(CreatePaymentRequest payment);
        UpdatePaymentResponse UpdatePayment(UpdatePaymentRequest payment);
        bool UpdateDeleteStatusPayment(int id);
    }
    public class PaymentService : IPaymentService   
    {
        private readonly FstoreContext _context;

        public PaymentService(FstoreContext context)
        {
            _context = context;
        }

        public List<Payment> GetAllPayment()
        {
            try
            {
                var payment = _context.Payments.Where(p => p.IsDeleted == false)
                     .ToList();
                return payment;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Payment? GetPaymentById(int id)
        {
            try
            {
                var payment = _context.Payments.FirstOrDefault(p => p.IsDeleted == false && p.PaymentId == id);
                if (payment != null)
                {
                    return payment;
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CreatePaymentResponse AddPayment(CreatePaymentRequest payment)
        {
            try
            {
                var requestPayment = new Payment
                {
                    PaymentName = payment.PaymentName,
                    IsDeleted = payment.IsDeleted
                };
                _context.Payments.Add(requestPayment);
                _context.SaveChanges();
                return new CreatePaymentResponse { IsSuccess = true, Message = "Add payment complete" };
            }
            catch (Exception e)
            {
                return new CreatePaymentResponse { IsSuccess = false, Message = $"Add payment failed {e.Message}" };

            }
        }

        public UpdatePaymentResponse UpdatePayment(UpdatePaymentRequest payment)
        {
            try
            {
                var requestPayment = new Payment
                {
                    PaymentId = payment.PaymentId,
                    PaymentName = payment.PaymentName,
                    IsDeleted = payment.IsDeleted
                };
                _context.Update(requestPayment);
                _context.SaveChanges();
                return new UpdatePaymentResponse { IsSuccess = true, Message = "Update payment complete" };
            }
            catch (Exception e)
            {
                return new UpdatePaymentResponse { IsSuccess = false, Message = $"Update payment failed {e.Message}" };
            }
        }

        public bool UpdateDeleteStatusPayment(int id)
        {
            try
            {
                var payment = GetPaymentById(id);
                if (payment == null)
                {
                    return false;
                }
                payment.IsDeleted = true;
                _context.Update(payment);
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
