using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper_Functions
{
    public static class HelperFunctions
    {
        public static int CalculateAge(DateTime dateOfBirth)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            // Check if the birthday for this year has passed
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }



    }
}
