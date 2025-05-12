using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerySimpleCppPad.Models;

namespace VerySimpleCppPad.Services
{
    public class CppCompileAndRunService : ICppCompileAndRunService
    {
        CancellationTokenSource? cts = null;

        public void StopRunningProcess()
        {
            cts?.Cancel();
        }

        public async Task<CompileAndRunResult> CompileAndRunAsync(string fileName, string stdin, string outfileName, string compiler)
        {
            cts = new();
            var token = cts.Token;
            Process runningProcess = null;
            string? output = null, error = null;
            try
            {
                compiler = string.Format(compiler, fileName, outfileName);
                int firstSpaceIndex = compiler.IndexOf(' ');
                string proc = compiler, arg = "";
                if (firstSpaceIndex != -1)
                {
                    proc = compiler.Substring(0, firstSpaceIndex);
                    arg = compiler.Substring(firstSpaceIndex + 1);
                }
                var compilerProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = proc,
                        Arguments = arg,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                runningProcess = compilerProcess;

                compilerProcess.Start();
                string compilerOutput = await compilerProcess.StandardOutput.ReadToEndAsync(token);
                string compilerError = await compilerProcess.StandardError.ReadToEndAsync(token);
                await compilerProcess.WaitForExitAsync(token);

                if (compilerProcess.ExitCode != 0)
                {
                    return new CompileAndRunResult
                    {
                        Result = CompileAndRunResult.CompileResultType.CompilationError,
                        Message = compilerError
                    };
                }

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = outfileName,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                runningProcess = process;

                process.Start();
                await process.StandardInput.WriteAsync(new StringBuilder(stdin), token);
                process.StandardInput.Close();
                output = await process.StandardOutput.ReadToEndAsync(token);
                error = await process.StandardError.ReadToEndAsync(token);
                await process.WaitForExitAsync(token);

                if (process.ExitCode != 0)
                {
                    return new CompileAndRunResult
                    {
                        Result = CompileAndRunResult.CompileResultType.RuntimeError,
                        Message = $"Runtime Error, Exit Code: {process.ExitCode}.\n============ OUTPUT ============:\n{output ?? ""}\n\n============ ERROR ============:\n{error ?? ""}"
                    };
                }

                return new CompileAndRunResult
                {
                    Result = CompileAndRunResult.CompileResultType.Success,
                    Message = output
                };
            }
            catch (OperationCanceledException)
            {
                runningProcess?.Kill();
                return new CompileAndRunResult
                {
                    Result = CompileAndRunResult.CompileResultType.Terminated,
                    Message = $"Terminated.\n============ OUTPUT ============:\n{output ?? ""}\n\n============ ERROR ============:\n{error ?? ""}"
                };
            }
            finally
            {
                cts.Dispose();
                cts = null;
            }
        }
    }
}
