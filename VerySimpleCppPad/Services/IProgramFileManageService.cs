
namespace VerySimpleCppPad.Services
{
    public interface IProgramFileManageService
    {
        string CreateNewProgram(string fileName, string content);
        List<KeyValuePair<string, DateTime>> GetAllPrograms();
        string GetTemplate();
        string GetTemplatePath();
    }
}