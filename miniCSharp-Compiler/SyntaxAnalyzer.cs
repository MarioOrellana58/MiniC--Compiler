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
        Stack<ConflictNode> ConflictsStack { get; set; }
        SyntaxAnalyzer()
        {            
            StatusStack = new Stack<int>();            
            ConsumedSymbols = new Stack<string>();
            Helper = new HelperStructures();
        }

        public void AnalyzeLexemesSyntax(List<LexemeNode> lexemes)
        {
            StatusStack.Push(0);
            for (int i = 0; i < lexemes.Count; i++)
            {
                ParseLexemes(lexemes[i], ref i);
            }
        }

        void ParseLexemes(LexemeNode lexeme, ref int lexemesIndex)
        {
            var column = 0;
            var isLexemeValue = true;
            //consume terminal symbol
            var headerFound = Helper.ActionsDict.TryGetValue(lexeme.Value, out column);
            if (!headerFound)
            {
                isLexemeValue = false;
                headerFound = Helper.ActionsDict.TryGetValue(lexeme.Token.ToString(), out column);
            }

            if (headerFound)
            {                
                column++;
                var analysisTableIns = getAnalysisTableInstruction(column);
                if (analysisTableIns.Contains('/'))
                {
                    /*make a backup of the actual status
                      to restore later in case the
                      path of the conflict taken 
                      by the analyzer resulted in
                      an error
                     */
                    var conflicNode = new ConflictNode();
                    conflicNode.Instructions = analysisTableIns.Split('/');
                    conflicNode.LexemesIndex = lexemesIndex;
                    conflicNode.NextInstructionIndex = 1;
                    conflicNode.StatusStack = StatusStack;
                    ConflictsStack.Push(conflicNode);

                    analysisTableIns = conflicNode.Instructions[0];
                    
                }

                if (analysisTableIns[0] == 's')
                {
                    shiftTo(analysisTableIns, (isLexemeValue ? lexeme.Value : lexeme.Token.ToString()));
                }
                else if (analysisTableIns[0] == 'r')
                {
                    //reduction
                }
                else
                {
                    //error
                    if (ConflictsStack.Count == 0)
                    {
                        //Final error, not admitted by parser
                    }
                    else
                    {
                        //Restore to previous status before conflict
                    }
                }
            }
            else
            {
                //error
            }
                
            
        }

        string getAnalysisTableInstruction(int column)
        {
            return Helper.AnalysisTable[StatusStack.Peek() + 1 , column];//to sync with table row index
        }

        void shiftTo(string actualInstruction, string consumedSymbol)
        {
            var nextStatus = Convert.ToInt32(actualInstruction.Substring(1, actualInstruction.Length - 2));
            StatusStack.Push(nextStatus);
            ConsumedSymbols.Push(consumedSymbol);

        }
    }
}
