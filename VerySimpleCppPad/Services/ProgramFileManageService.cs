using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerySimpleCppPad.Services
{
    public class ProgramFileManageService : IProgramFileManageService
    {
        private readonly string directoryPath;
        private readonly string templatePath;

        public ProgramFileManageService(string directoryPath, string templatePath)
        {
            this.directoryPath = directoryPath;
            this.templatePath = templatePath;
            if (!Directory.Exists(this.directoryPath)) Directory.CreateDirectory(this.directoryPath);
            if (!File.Exists(this.templatePath)) File.WriteAllText(this.templatePath, "");
        }

        // 获得 directoryPath 下所有 .cpp 文件, 并按时间排序
        public List<KeyValuePair<string, DateTime>> GetAllPrograms()
        {
            return Directory.GetFiles(this.directoryPath, "*.cpp")
                .Select(filePath => new KeyValuePair<string, DateTime>(filePath, File.GetLastWriteTime(filePath)))
                .OrderByDescending(pair => pair.Value)
                .ToList();
        }

        public string CreateNewProgram(string fileName, string content)
        {
            File.WriteAllText(Path.Combine(this.directoryPath, fileName), content);
            return Path.Combine(this.directoryPath, fileName);
        }

        public string GetTemplate()
        {
            return File.ReadAllText(this.templatePath);
        }

        public string GetTemplatePath()
        {
            return this.templatePath;
        }

    }
}
