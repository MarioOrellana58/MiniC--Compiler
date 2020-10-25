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

        public Stack<string> ConsumedSymbolsStack { get; set; }
        public int LexemesIndex { get; set; }
        public int NextInstructionIndex { get; set; }
        public string[] Instructions { get; set; }
        public LexemeNode Lexeme { get; set; }
        public bool IsLexemeValue { get; set; }
    }
}
