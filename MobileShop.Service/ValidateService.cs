using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MobileShop.Service
{
    public interface IValidateService
    {
        string DateNow();
        bool ValidatePhone(string input);
        bool ValidateMail(string input);
        bool ValidateName(string input);
    }
    public class ValidateService : IValidateService
    {
        public string DateNow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public bool ValidatePhone(string input)
        {
            string regex = @"(^[0]+[0-9]{9}$)";
            Regex re = new Regex(regex);

            if (re.IsMatch(input))
                return (true);
            else
                return (false);
        }

        public bool ValidateMail(string input)
        {
            string regex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            Regex re = new Regex(regex);

            if (re.IsMatch(input))
                return (true);
            else
                return (false);
        }

        public bool ValidateName(string input)
        {
            string regex = @"\D+\z";
            Regex re = new Regex(regex);

            if (re.IsMatch(input))
                return (true);
            else
                return (false);
        }
    }
}
