using Microsoft.AspNetCore.Http;
using Nsiclass.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Nsiclass.Services.Implementations
{
    public class ManageFilesService : IManageFilesService
    {
        public bool DeleteClassFile(string classCode, string versionCode, string fileName)
        {
            string directoryName = string.Concat(classCode, versionCode);
            string globalPath = Environment.CurrentDirectory;

            var path = Path.Combine(globalPath, DataConstants.ClassFilesSubDirectory, directoryName,fileName);

            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
                return true;
            }
            return false;
        }

        public bool DeleteClassVersionDirectory(string classCode, string versionCode)
        {
            string directoryName = string.Concat(classCode, versionCode);
            string globalPath = Environment.CurrentDirectory;

            var path = Path.Combine(globalPath, DataConstants.ClassFilesSubDirectory,directoryName);

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            return true;
        }

        public async Task<bool> AddFile(string classCode, string versionCode, IFormFile file)
        {
            string directoryName = string.Concat(classCode, versionCode);
            string globalPath = Environment.CurrentDirectory;

            var classDirectory = Path.Combine(
                        globalPath, DataConstants.ClassFilesSubDirectory, directoryName);

            if (!Directory.Exists(classDirectory))
            {
                Directory.CreateDirectory(classDirectory);
            }

            var filePath = Path.Combine(classDirectory,file.FileName);

            //var filePath = globalPath + "/" + DataConstants.ClassFilesSubDirectory + "/" + directoryName + "/" + file.FileName;

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    await file.CopyToAsync(stream);
                }
                catch (Exception)
                {

                    return false;
                }

            }
            return true;
        }

        public IList<string> GetFilesInDirectory(string classCode, string versionCode)
        {
            var fileList = new List<string>();
            try
            {
                string directoryName = string.Concat(classCode, versionCode);
                string globalPath = Environment.CurrentDirectory;

                var classDirectory = Path.Combine(
                            globalPath, DataConstants.ClassFilesSubDirectory, directoryName);

                if (!Directory.Exists(classDirectory))
                {
                    return null;
                }



                foreach (var file in Directory.GetFiles(classDirectory))
                {
                    FileInfo info = new FileInfo(file);
                    fileList.Add(Path.GetFileName(info.FullName));
                }

            }
            catch (Exception)
            {
                return fileList;
            }

            return fileList;
        }

        public bool DeleteFile(string classCode, string versionCode, string fileName)
        {
            try
            {
                string directoryName = string.Concat(classCode, versionCode);
                string globalPath = Environment.CurrentDirectory;

                var classDirectory = Path.Combine(
                            globalPath, DataConstants.ClassFilesSubDirectory, directoryName);

                if (!Directory.Exists(classDirectory))
                {
                    return false;
                }

                var filePath = Path.Combine(classDirectory, fileName);

                FileInfo file = new FileInfo(filePath);
                file.Delete();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

