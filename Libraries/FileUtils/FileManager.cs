using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace OnlineStore.Libraries.FileUtils
{
    public static class FileManager
    {
        private static string currentDirectoryPath = Directory.GetCurrentDirectory();
        private static string tempUploadFolder = "wwwroot/uploads/temp";

        public static string SaveProductImage(IFormFile file)
        {
            Directory.CreateDirectory(tempUploadFolder);

            try
            {
                string fileName = Path.GetFileName(file.FileName);
                string pathWhereTheFileWillBeSaved = Path.Combine(currentDirectoryPath, tempUploadFolder, fileName);

                using (var fileStream = new FileStream(pathWhereTheFileWillBeSaved, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return Path.Combine(tempUploadFolder.Replace("wwwroot", ""), fileName);
            }
            catch
            {
                return null;
            } 
        }

        public static bool DeleteProductImage(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string pathWhereIsTheFileToBeDeleted = Path.Combine(currentDirectoryPath, tempUploadFolder, fileName);

            if (File.Exists(pathWhereIsTheFileToBeDeleted))
            {
                File.Delete(pathWhereIsTheFileToBeDeleted);
                return true;
            }

            return false;
        }
    }
}