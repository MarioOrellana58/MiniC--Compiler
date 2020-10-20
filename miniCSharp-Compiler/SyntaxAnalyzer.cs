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
        Stack<int> StatusStack { get; set; }
        Stack<string> ConsumedSymbols { get; set; }
        HelperStructures Helper { get; set; }
        Stack<int> ConflictsStack { get; set; }
        SyntaxAnalyzer()
        {            
            StatusStack = new Stack<int>();
            ConsumedSymbols = new Stack<string>();
            Helper = new HelperStructures();
        }
        
        public void AnalyzeLexemesSyntax(List<LexemeNode> lexemes)
        {
            for (int i = 0; i < lexemes.Count; i++)
            {
                ParseLexemes(lexemes[i], i);
            }
        }

        void ParseLexemes(LexemeNode lexeme, int lexemesIndex)
        {
            bool consumeLexeme = false;
            while (!consumeLexeme)
            {

            }

        }


    }
}
