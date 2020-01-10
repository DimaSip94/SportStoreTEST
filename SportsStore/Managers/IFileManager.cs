using Microsoft.AspNetCore.Http;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Managers
{
    public interface IFileManager
    {
        /// <summary>
        /// Сохраняет файл, возвращает путь к нему
        /// </summary>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        Task<string> SaveFileAsync(IFormFile uploadedFile, string path);
    }
}
