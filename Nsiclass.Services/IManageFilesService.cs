using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Nsiclass.Services
{
   public interface IManageFilesService
    {
        Task<bool> AddFile(string classCode, string versionCode, IFormFile file);

        bool DeleteClassFile(string classCode, string versionCode, string fileName);

        bool DeleteClassVersionDirectory(string classCode, string versionCode);

        IList<string> GetFilesInDirectory(string classCode, string versionCode);

        bool DeleteFile(string classCode, string versionCode, string fileName);

        Task<byte[]> ExportFile(string classCode, string versionCode, string fileName);
    }
}
