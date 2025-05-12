using VerySimpleCppPad.Models;

namespace VerySimpleCppPad.Services
{
    public interface ISettingService
    {
        string Path { get; set; }
        ProgramSetting Setting { get; set; }

        bool CreateOrLoad(string path);
        void Save();
    }
}