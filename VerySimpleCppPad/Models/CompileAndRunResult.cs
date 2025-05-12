using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerySimpleCppPad.Models
{
    public class CompileAndRunResult
    {
        public enum CompileResultType
        {
            Success,
            CompilationError,
            Terminated,
            RuntimeError,
            Other
        }
        public CompileResultType Result { get; set; }
        public string Message { get; set; } = "";
    }
}
