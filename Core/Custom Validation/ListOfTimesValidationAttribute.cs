using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Custom_Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class ListOfTimesValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is List<List<string>> timesList)
            {
                var regexPattern = @"^(?:[01]\d|2[0-3]):[0-5]\d$";
                return timesList.SelectMany(times => times).All(time => IsValidTimeFormat(time, regexPattern));
            }
            return false;
        }

        private bool IsValidTimeFormat(string time, string regexPattern)
        {
            return !string.IsNullOrEmpty(time) && System.Text.RegularExpressions.Regex.IsMatch(time, regexPattern);
        }
    }
}
