using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniCSharp_Compiler
{
    struct syntaxErrors
    {
        public int tokenIndex { get; set; }
        public string expectedValue { get; set; }

    }
    public class SyntaxAnalyzer
    {
        Dictionary<string, SyntaxNode> SyntaxTreesDic = new Dictionary<string, SyntaxNode>();
        List<LexemeNode> TokensList = new List<LexemeNode>();

        List<syntaxErrors> syntaxErrorsList = new List<syntaxErrors>();
        string expected = string.Empty;
        public SyntaxAnalyzer(List<LexemeNode> tokensList)
        {
            var ProductionTrees = new SyntaxTrees();
            this.SyntaxTreesDic = ProductionTrees.SyntaxTreesDic;
            this.TokensList = tokensList;
        }
        public void ReadLexemes()
        {
            var errors = new List<string>();
            for (int i = 0; i < TokensList.Count; i++)
            {
                syntaxErrorsList = new List<syntaxErrors>();
                expected = string.Empty;
                if (!AnalyzeTokens(ref i, SyntaxTreesDic["Decl"], ref expected))
                {
                    i = syntaxErrorsList.Max(x => x.tokenIndex);
                    expected = "Error en       " + TokensList[i - 1].Value + "        se esperaban cualquiera de estas opciones:       ";
                    foreach (var error in syntaxErrorsList)
                    {
                        if (error.tokenIndex == i)
                        {
                            expected += error.expectedValue + "       ";
                        }
                    }
                    errors.Add(expected);
                }
                else
                {
                    i--;
                }

            }
            foreach (var item in errors)
            {
                Console.WriteLine(item);
            }
        }

        bool AnalyzeTokens(ref int tokensIndex, SyntaxNode actualRoot, ref string expectedToken)
        {

            if (actualRoot.Sons.Count != 0)
            {
                var sonsCounter = 0;
                var matchFound = false;
                while (sonsCounter < actualRoot.Sons.Count && !matchFound)
                {
                    if (SyntaxTreesDic.TryGetValue(actualRoot.Value, out SyntaxNode value))
                    {
                        var actualIndex = tokensIndex;
                        if (SyntaxTreesDic.TryGetValue(actualRoot.Sons[sonsCounter].Value, out SyntaxNode value2))
                        {
                            if (AnalyzeTokens(ref tokensIndex, SyntaxTreesDic[actualRoot.Sons[sonsCounter].Value], ref expectedToken))
                            {
                                if (actualRoot.Sons[sonsCounter].Sons.Count != 0)
                                {
                                    if (AnalyzeTokens(ref tokensIndex, actualRoot.Sons[sonsCounter].Sons[0], ref expectedToken))
                                    {
                                        matchFound = true;
                                    }
                                    else
                                    {
                                        syntaxErrorsList.Add(new syntaxErrors { expectedValue = expectedToken, tokenIndex = tokensIndex });
                                        tokensIndex = actualIndex;
                                        sonsCounter++;
                                    }
                                }
                                else
                                {
                                    matchFound = true;
                                }
                            }
                            else
                            {
                                sonsCounter++;
                            }
                        }
                        else
                        {
                            if (AnalyzeTokens(ref tokensIndex, actualRoot.Sons[sonsCounter], ref expectedToken))
                            {
                                matchFound = true;
                            }
                            else
                            {
                                syntaxErrorsList.Add(new syntaxErrors { expectedValue = expectedToken, tokenIndex = tokensIndex });
                                tokensIndex = actualIndex;
                                sonsCounter++;
                            }
                        }
                    }
                    else if (isMatch(actualRoot.Value, TokensList[tokensIndex]))
                    {
                        tokensIndex++;
                        if (SyntaxTreesDic.TryGetValue(actualRoot.Sons[0].Value, out SyntaxNode value2))
                        {
                            if (AnalyzeTokens(ref tokensIndex, SyntaxTreesDic[actualRoot.Sons[0].Value], ref expectedToken))
                            {
                                if (actualRoot.Sons[0].Sons.Count != 0)
                                {
                                    var sons = 0;
                                    var match = true;
                                    while (sons < actualRoot.Sons[0].Sons.Count && match)
                                    {
                                        if (!AnalyzeTokens(ref tokensIndex, actualRoot.Sons[0].Sons[sons], ref expectedToken))
                                        {
                                            match = false;
                                        }
                                        else
                                        {
                                            sons++;
                                        }
                                    }
                                    if (!match)
                                    {
                                        matchFound = false;
                                    }
                                }
                                else
                                {
                                    matchFound = true;
                                }
                                matchFound = true;
                            }
                        }
                        else if (AnalyzeTokens(ref tokensIndex, actualRoot.Sons[0], ref expectedToken))
                        {

                            matchFound = true;
                        }
                    }
                    else
                    {
                        expectedToken = actualRoot.Value;
                        sonsCounter++;
                    }
                }
                return matchFound;
            }
            else if (SyntaxTreesDic.TryGetValue(actualRoot.Value, out SyntaxNode value))
            {
                if (AnalyzeTokens(ref tokensIndex, SyntaxTreesDic[actualRoot.Value], ref expectedToken))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (actualRoot.Value == "Eps")
            {
                return true;
            }
            else
            {
                if (isMatch(actualRoot.Value, TokensList[tokensIndex]))
                {
                    tokensIndex++;
                    return true;
                }
                else
                {
                    expectedToken = actualRoot.Value;
                    return false;
                }
            }
        }

        bool isMatch(string rootValue, LexemeNode tokenToMatch)
        {
            if (rootValue == tokenToMatch.Value || rootValue == tokenToMatch.Token.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
