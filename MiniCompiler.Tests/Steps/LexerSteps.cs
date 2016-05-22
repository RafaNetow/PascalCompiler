using System;
using TechTalk.SpecFlow;
using System.Collections.Generic;


using Mini_Compiler.Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompiler.Tests.Steps
{
    [Binding]
    public class LexerSteps
    {

        Lexer lexer;
        List<Token> tokenList = new List<Token>();
        [Given(@"I have an input of '(.*)'")]
        public void GivenIHaveAnInputOf(string p0)
        {

            lexer = new Lexer(new StringContent(p0));
        }
        
        [When(@"We Tokenize")]
        public void WhenWeTokenize()
        {
            Token currentToken = lexer.GetNextToken();

            while (currentToken.Type != TokenTypes.Eof)
            {

                tokenList.Add(currentToken);
                currentToken = lexer.GetNextToken();

            }
            tokenList.Add(currentToken);
        }

        [Then(@"the result should be")]
        public void ThenTheResultShouldBe(Table table)
        {
            for (int i =0; i < table.RowCount; i++)
            {
                Assert.AreEqual(table.Rows[i]["Type"] , tokenList[i].Type.ToString(), "The TokenTypes do not match.");
                Assert.AreEqual(table.Rows[i]["Lexeme"], tokenList[i].Lexeme.ToString(), "The TokenTypes do not match.");
                Assert.AreEqual(table.Rows[i]["Row"], tokenList[i].Row.ToString(), "The TokenTypes do not match.");
                Assert.AreEqual(table.Rows[i]["Column"], tokenList[i].Column.ToString(), "The TokenTypes do not match.");
            }
        }
    }
}
