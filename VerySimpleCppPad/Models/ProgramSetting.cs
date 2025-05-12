using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerySimpleCppPad.Models
{
    public class ProgramSetting
    {
        public string CompilerPath { get; set; } = "g++ {0} -O2 -o {1}";
        public string EditorPath { get; set; } = "notepad {0}";
        public string Font { get; set; } = "Consolas";
    }
}
