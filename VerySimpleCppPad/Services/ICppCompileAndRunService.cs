using VerySimpleCppPad.Models;

namespace VerySimpleCppPad.Services
{
    public interface ICppCompileAndRunService
    {
        Task<CompileAndRunResult> CompileAndRunAsync(string fileName, string stdin, string outfileName, string compiler);
        void StopRunningProcess();
        bool IsRunning();
    }
}