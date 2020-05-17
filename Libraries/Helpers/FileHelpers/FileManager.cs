using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OnlineStore.Libraries.Helpers.StringHelpers;

namespace OnlineStore.Libraries.Helpers.FileHelpers
{
    public static class FileManager
    {
        private static string currentDirectoryPath = Directory.GetCurrentDirectory();
        private static string tempUploadFolder = "wwwroot/uploads/temp";
        private static string permanentUploadFolder = "wwwroot/uploads/{0}";
        private const int FileNameLength = 64;

        public static string SaveProductImage(IFormFile file)
        {
            Directory.CreateDirectory(tempUploadFolder);

            try
            {
                string fileName = 
                    RandomString.GenerateRandomString(FileNameLength) + Path.GetExtension(file.FileName);

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
            string pathWhereIsTheFileToBeDeleted = Path.Combine(currentDirectoryPath, "wwwroot") + filePath;

            if (File.Exists(pathWhereIsTheFileToBeDeleted))
            {
                File.Delete(pathWhereIsTheFileToBeDeleted);
                return true;
            }

            return false;
        }

        public static void ClearTempUploadFolder()
        {
            DirectoryInfo directoryInfo = 
                new DirectoryInfo(Path.Combine(currentDirectoryPath, tempUploadFolder));

            foreach (var file in directoryInfo.GetFiles())
                file.Delete();
        }

        ///<summary>Moves the product images from the temporary folder to the permanent folder and returns the permanent relative paths.</summary>
        public static List<string> MoveImagesToThePermanentFolder(List<string> tempImagesPaths, int productId)
        {
            string permanentFolderPath = 
                Path.Combine(currentDirectoryPath, String.Format(permanentUploadFolder, productId));
            Directory.CreateDirectory(permanentFolderPath);
            
            List<string> permanentPaths = new List<string>();

            foreach (string path in tempImagesPaths)
            {
                var fileName = Path.GetFileName(path);
                string fileTempPath = Path.Combine(currentDirectoryPath, tempUploadFolder, fileName);
                string filePermanentPath = Path.Combine(permanentFolderPath, fileName);

                if(File.Exists(fileTempPath))
                {
                    File.Move(fileTempPath, filePermanentPath);

                    string permanentRelativePath = 
                        Path.Combine(String.Format(permanentUploadFolder, productId), fileName).Replace("wwwroot", "");
                    permanentPaths.Add(permanentRelativePath);
                }
                else
                    permanentPaths.Add("");
            }

            return permanentPaths;
        }
    }
}