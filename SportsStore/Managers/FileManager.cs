using Microsoft.AspNetCore.Http;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Managers
{

    public class FileManager : IFileManager
    {
        public async Task<string> SaveFileAsync(IFormFile uploadedFile, string path)
        {
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
            catch (Exception)
            {
                path = null;
            }
            
            return path;
        }

        public string MoveFile(string oldPath, string newPath)
        {
            throw new NotImplementedException();
        }
    }
}
