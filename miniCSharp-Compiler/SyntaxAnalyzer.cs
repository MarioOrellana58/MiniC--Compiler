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
        int MaxLexemesIndex { get; set; }
        List<string> MaxExpected { get; set; }
        LexemeNode MaxReceived { get; set; }
        public bool IsSyntacticallyCorrect { get; set; }
        public bool EnglishVersion { get; set; }
        public SyntaxAnalyzer(bool englishVersion)
        {
            StatusStack = new Stack<int>();
            ConsumedSymbols = new Stack<string>();
            Helper = new HelperStructures();
            ConflictsStack = new Stack<ConflictNode>();
            MaxExpected = new List<string>();
            IsSyntacticallyCorrect = true;
            this.EnglishVersion = englishVersion;
        }

        public void AnalyzeLexemesSyntax(List<LexemeNode> lexemes)
        {
            StatusStack.Push(0);
            for (int i = 0; i < lexemes.Count; i++)
            {
                if (lexemes[i].Token != 'M' && lexemes[i].Token != 'C')
                {
                    ParseLexemes(lexemes[i], ref i);
                }
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
                var actualToken = lexeme.Token.ToString();
                if (actualToken == "H")
                {
                    actualToken = "N";
                }
                else if (actualToken == "X")
                {
                    actualToken = "D";
                }
                headerFound = Helper.ActionsDict.TryGetValue(actualToken, out column);
            }

            if (headerFound)
            {
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
                    conflicNode.StatusStack = new Stack<int>(new Stack<int>(StatusStack));
                    conflicNode.ConsumedSymbolsStack = new Stack<string>(new Stack<string>(ConsumedSymbols));
                    conflicNode.IsLexemeValue = isLexemeValue;
                    conflicNode.Lexeme = lexeme;
                    ConflictsStack.Push(conflicNode);

                    analysisTableIns = conflicNode.Instructions[0];

                }
                if (analysisTableIns == string.Empty)
                {
                    RestoreStatus(ref lexemesIndex, isLexemeValue, lexeme);
                }
                else
                {
                    ShiftOrReduce(analysisTableIns, isLexemeValue, lexeme, ref lexemesIndex);
                }

            }
            else
            {
                //Final error detected, unrecognized symbol
                PrintActualErrors();
                ClearVariables(ref lexemesIndex);
            }


        }
        void ClearVariables(ref int lexemesIndex)
        {
            StatusStack = new Stack<int>();
            ConsumedSymbols = new Stack<string>();
            MaxReceived = new LexemeNode();
            MaxExpected = new List<string>();
            lexemesIndex = MaxLexemesIndex;
            MaxLexemesIndex = 0;
            StatusStack.Push(0);
        }
        void RestoreStatus(ref int lexemesIndex, bool isLexemeValue, LexemeNode lexeme)
        {
            //error
            if (MaxLexemesIndex < lexemesIndex)
            {
                MaxLexemesIndex = lexemesIndex;
                MaxReceived = lexeme;
                //Recorrer la matriz buscando header contra valor en el estado actual (fila), hasta encontrar el $
                LexemesExpected(true);//Clear list and add new values
            }
            else if (MaxLexemesIndex == lexemesIndex)
            {//MaxExpected
                LexemesExpected(false);//Add to list
            }

            if (ConflictsStack.Count == 0)
            {
                //Final error, input not admitted by parser
                PrintActualErrors();
                ClearVariables(ref lexemesIndex);
            }
            else
            {
                var previousStatus = ConflictsStack.Pop();
                if (previousStatus.NextInstructionIndex < previousStatus.Instructions.Count())
                {
                    lexemesIndex = previousStatus.LexemesIndex;
                    isLexemeValue = previousStatus.IsLexemeValue;
                    lexeme = previousStatus.Lexeme;
                    var analysisTableIns = previousStatus.Instructions[previousStatus.NextInstructionIndex];
                    previousStatus.NextInstructionIndex++;
                    ConsumedSymbols = new Stack<string>(new Stack<string>(previousStatus.ConsumedSymbolsStack));
                    StatusStack = new Stack<int>(new Stack<int>(previousStatus.StatusStack));
                    ConflictsStack.Push(previousStatus);
                    ShiftOrReduce(analysisTableIns, isLexemeValue, lexeme, ref lexemesIndex);
                }
                else
                {
                    //error, check the stack again
                    RestoreStatus(ref lexemesIndex, isLexemeValue, lexeme);
                }
            }
        }

        void PrintActualErrors()
        {
            var firstMessage = EnglishVersion ? "Was expecting ": "Se esperaba ";
            var secondMessage = EnglishVersion ? ", recieved " : ", venía ";
            var onColumns = EnglishVersion ? " on columns " : " en las columnas ";
            var onLineNumber = EnglishVersion ? " on line number: " : "en la línea número: ";
            var endColumn = EnglishVersion ? " to " : " hasta ";
            var tokensInfo = new LexicalAnalyzer(EnglishVersion);

            for (int i = 0; i < MaxExpected.Count; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(firstMessage + "\"" + MaxExpected[i] + "\"" + secondMessage + "\"" + MaxReceived.Value + "\"" +
                                    onColumns + MaxReceived.StartColumn.ToString() + endColumn + MaxReceived.EndColumn.ToString() +
                                    onLineNumber + MaxReceived.StartRow);
                Console.WriteLine("\n");
            }
            IsSyntacticallyCorrect = false;
        }
        void LexemesExpected(bool createOrAdd)
        {
            if (createOrAdd)
            {
                MaxExpected = new List<string>();
            }
            
            var helperColumns = Helper.ActionsDict.Count - 1;//-1 to ignore de $ symbol
            for (int i = 1; i < helperColumns; i++)
            {
                var actualExpected = getAnalysisTableInstruction(i);
                if (actualExpected != string.Empty && !MaxExpected.Contains(Helper.AnalysisTable[0, i]))
                {
                    MaxExpected.Add(Helper.AnalysisTable[0, i]);
                }
            }
        }
        void ShiftOrReduce(string analysisTableIns, bool isLexemeValue, LexemeNode lexeme, ref int lexemesIndex)
        {
            if (analysisTableIns[0] == 's')
            {
                shiftTo(analysisTableIns, (isLexemeValue ? lexeme.Value : lexeme.Token.ToString()));

            }
            else if (analysisTableIns[0] == 'r')
            {
                var column = 0;
                var headerFound = false;
                reduceBy(analysisTableIns);
                headerFound = Helper.GotoDict.TryGetValue(ConsumedSymbols.Peek(), out column);
                if (headerFound)
                {
                    analysisTableIns = getAnalysisTableInstruction(column);
                    
                    gotoStatus(Convert.ToInt32(analysisTableIns));

                    lexemesIndex--;
                }
            }
        }
        string getAnalysisTableInstruction(int column)
        {
            return Helper.AnalysisTable[StatusStack.Peek() + 1, column];//to sync with table row index
        }

        void shiftTo(string actualInstruction, string consumedSymbol)
        {
            var nextStatus = Convert.ToInt32(actualInstruction.Substring(1, actualInstruction.Length - 1));
            StatusStack.Push(nextStatus);
            ConsumedSymbols.Push(consumedSymbol);
        }

        void reduceBy(string actualInstruction)
        {
            var reductionProdId = Convert.ToInt32(actualInstruction.Substring(1, actualInstruction.Length - 1));

            var production = Helper.Productions[reductionProdId];

            popStacksAndReplaceStatus(production.SymbolsProducedQty, production.NonTerminalName);
        }

        void gotoStatus(int nextStatus)
        {
            StatusStack.Push(nextStatus);
        }

        void popStacksAndReplaceStatus(int elementsQtyToPop, string productionName)
        {
            for (int i = 0; i < elementsQtyToPop; i++)
            {
                if (StatusStack.Count > 0)
                {
                    StatusStack.Pop();
                }
                else
                {
                    //error
                }

                if (ConsumedSymbols.Count > 0)
                {
                    ConsumedSymbols.Pop();
                }
                else
                {
                    //error
                }
            }

            ConsumedSymbols.Push(productionName);
        }
    }
}
