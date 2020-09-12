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
    class SyntaxAnalyzer
    {
        Dictionary<string, SyntaxNode> SyntaxTreesDic = new Dictionary<string, SyntaxNode>();
        List<LexemeNode> TokensList = new List<LexemeNode>();

        List<syntaxErrors> syntaxErrorsList = new List<syntaxErrors>();
        string expected = string.Empty;
        bool AnalyzeTokens(ref int tokensIndex, SyntaxNode actualRoot, ref string expectedToken)
        {
            if (TokensList[tokensIndex].Value == "(" && actualRoot.Value == "(")
            {

            }
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
                                        //syntaxErrorsList = new List<syntaxErrors>();
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
                                    //syntaxErrorsList = new List<syntaxErrors>();
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
                                //syntaxErrorsList = new List<syntaxErrors>();
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
                                var a = actualRoot.Sons[0];
                                matchFound = true;
                                //syntaxErrorsList = new List<syntaxErrors>();
                                matchFound = true;
                            }
                        }
                        else if (AnalyzeTokens(ref tokensIndex, actualRoot.Sons[0], ref expectedToken))
                        {
                            //syntaxErrorsList = new List<syntaxErrors>();
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

    }
}
