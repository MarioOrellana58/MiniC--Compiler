using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniCSharp_Compiler
{
    class ConflictNode
    {
        public Stack<int> StatusStack { get; set; }
        public int LexemesIndex { get; set; }
        public int NextInstructionIndex { get; set; }
        public string[] Instructions { get; set; }
    }
}
