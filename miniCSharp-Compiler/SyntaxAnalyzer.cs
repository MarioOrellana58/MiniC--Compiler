using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace miniCSharp_Compiler
{
    class SyntaxAnalyzer
    {
        public Stack<int> StatusStack { get; set; }
        public Stack<string> ConsumedSymbols { get; set; }
        public HelperStructures Helper { get; set; }
        public SyntaxAnalyzer()
        {
            StatusStack = new Stack<int>();
            ConsumedSymbols = new Stack<string>();
            Helper = new HelperStructures();
        }


    }
}
