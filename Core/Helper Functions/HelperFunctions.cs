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

        public static string AddFullImagePath(string projectRoot,string imagePath)
        {
            if (imagePath == null)

            {
                return null;
            }

            string uploadsFolder = Path.Combine(projectRoot, "images");
            string filePath = Path.Combine(uploadsFolder, imagePath);
            return filePath;
        }

        public static string ProcessUploadedFile(string imgPath)
        {
            string uniqueFileName = null;

            if (imgPath != null && System.IO.File.Exists(imgPath))
            {
                string projectRoot = Directory.GetCurrentDirectory();
                string uploadsFolder = Path.Combine(projectRoot, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imgPath);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Check if the directory exists, if not, create it
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                System.IO.File.Copy(imgPath, filePath); // Copy the file to the target location



            }



            return uniqueFileName;
        }

        public static void UndoUploadedFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath); // Delete the file
            }
        }

    }
}
