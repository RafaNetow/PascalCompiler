using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Mini_Compiler.Lexer;

namespace Mini_Compiler.Sintactico
{
    class Syntactic
    {
        private Lexer.Lexer lexer;
        private Token currentToken;

        public Syntactic(Lexer.Lexer lexer)
        {
            this.lexer = lexer;
            ConsumeNextToken();
        }

        public Syntactic(Token currentToken)
        {
            this.currentToken = currentToken;
        }

        public Syntactic(
            )
        {
        }

        public void Parse()
        {
            ListSentence();
            if (currentToken.Type != TokenTypes.Eof)
            {
                throw new SyntaticException("Se esperaba EOF",currentToken.Row, currentToken.Column);
            }
        }

        private void ListSentence()
        {
            if (currentToken.Type == TokenTypes.Html)
            {         
                ConsumeNextToken();
                ListSentence();       
              
            }        
          else  if (currentToken.Type == TokenTypes.RwVar)
            {
                Declaration();
            }
          
       
          
        }

        private void Declaration()
        {
            FactorComunId();

        }

        private void FactorComunId()
        {
            ConsumeNextToken();
            if (currentToken.Type==TokenTypes.Id)
            {
                FactorComunIdPrime();
            }
            else
            {
                throw new SyntaticException("Se esperaba un Id", currentToken.Row, currentToken.Column);
            }
        }
        //Y en mi Gramatica
        private void FactorComunIdPrime()
        {
            //IdOpcional
            ConsumeNextToken();
            if (currentToken.Type == TokenTypes.CommaOperator)
            {
                OptonialId();
                if (currentToken.Type == TokenTypes.AsiggnationOp)
                {
                    ConsumeNextToken();
                    if (currentToken.Type == TokenTypes.Id)
                    {
                        ConsumeNextToken();
                        if (currentToken.Type == TokenTypes.Eos)
                        {
                            ConsumeNextToken();
                            ListSentence();
                        }
                        else
                        {
                            throw  new SyntaticException("Expected EOS",currentToken.Row, currentToken.Column);
                        }
                    }

                    else if (currentToken.Type == TokenTypes.Eof)
                    {
                        return;
                    }
                    else
                    {
                        throw  new SyntaticException("Expected Id", currentToken.Row, currentToken.Column);
                    }
                }
                else if (currentToken.Type == TokenTypes.Eof)
                {
                    return;
                }
                else
                {
                   throw new SyntaticException("Expected AsiggnationOp", currentToken.Row, currentToken.Column);
                }
            }
            else if (currentToken.Type == TokenTypes.AsiggnationOp)
            {
                AssignValue();
            }
            else if (currentToken.Type == TokenTypes.Eos)
            {
                ListSentence();
            }
            else
            {
                throw  new SyntaticException("Missing EOS",currentToken.Row, currentToken.Column);
            }
            
        }

        private void AssignValue()
        {
            ConsumeNextToken();
            E();
            if (currentToken.Type == TokenTypes.Eos)
            {
                ConsumeNextToken();
                return;
            }
            else
            {
                throw new SyntaticException("Expected Eos",currentToken.Row,currentToken.Column);
            }
        }

        private void OptonialId()
        {
           ConsumeNextToken();
            if (currentToken.Type == TokenTypes.Id)
            {
                IdList();
            }
             
            else
            {
                throw new SyntaticException("Expected ID",currentToken.Row,currentToken.Column);
            }     
        }
        
        private void IdList()
        {
            ConsumeNextToken();
            if (currentToken.Type == TokenTypes.CommaOperator)
            {
                OptonialId();
            }
            else if (currentToken.Type == TokenTypes.AsiggnationOp)
            {
                ConsumeNextToken();
                if (currentToken.Type == TokenTypes.Id)
                {
                    ConsumeNextToken();
                    if (currentToken.Type == TokenTypes.Eos)
                    {
                        ConsumeNextToken();
                        ListSentence();
                    }          
                    else if (currentToken.Type == TokenTypes.EqualOp)
                    {
                        AssignValue();
                    }
               //     else if(currentToken.Type == TokenT)
                    else
                    {
                        throw  new SyntaticException("Expected EOS",currentToken.Row, currentToken.Column);
                        throw  new SyntaticException("Expected EOS",currentToken.Row, currentToken.Column);
                    }
                    
                }
                else
                {
                    throw new SyntaticException("Expected Id", currentToken.Row, currentToken.Column);
                }
                
            }
            else
            {
                 throw new SyntaticException("Expected AssingOp", currentToken.Row, currentToken.Column);
            }

            {
                
            }
        }

        public void ConsumeNextToken()
        {
            currentToken = lexer.GetNextToken();
        }
        private ExpressionNode E()
        {
            var expression = Expression();

            var relational = RelationalExpression(expression);

            if (relational != null)
                return relational;

            return expression;
        }

        private ExpressionNode Expression() //expression
        {
            var tValue = T();
            return EP(tValue);

        }
        private ExpressionNode EP(ExpressionNode param) //expression
        {
            if (currentToken.Type == TokenTypes.SumOp)
            {
                currentToken = lexer.GetNextToken();
                var tValue = T();
                var node = new AddNode { LeftOperand = param, RightOperand = tValue };

                return EP(node);
            }
            else if (currentToken.Type == TokenTypes.SubOp)
            {
                currentToken = lexer.GetNextToken();
                var tValue = T();
                var node = new SubNode { LeftOperand = param, RightOperand = tValue };

                return EP(node);
            }
            else
            {
                return param;
            }
        }

        private ExpressionNode T() //int string
        {
            var fValue = F();
            return TP(fValue);
        }

        private ExpressionNode TP(ExpressionNode param)
        {
            if (currentToken.Type == TokenTypes.MultOp)
            {
                currentToken = lexer.GetNextToken();
                var fValue = F();
                var node = new MultNode { LeftOperand = param, RightOperand = fValue };
                return TP(node);
            }
            else if (currentToken.Type == TokenTypes.DivOp || currentToken.Type == TokenTypes.RwDiv)
            {
                currentToken = lexer.GetNextToken();
                var fValue = F();
                var node = new DivNode() { LeftOperand = param, RightOperand = fValue };
                return TP(node);
            }
            else
            {
                return param;
            }

        }

        private ExpressionNode F() //id, num (E) Epsilon
        {
            if (currentToken.Type == TokenTypes.NumericLiteral)
            {
                var value = float.Parse(currentToken.Lexeme);
                currentToken = lexer.GetNextToken();
                return new NumberNode { Value = value };
            }
            else if (currentToken.Type == TokenTypes.Id)
            {
                string id = currentToken.Lexeme;
                currentToken = lexer.GetNextToken();
                return new IdNode
                {
                    Value = id

                };
            }
            else if (currentToken.Type == TokenTypes.SbLeftParent)
            {
                currentToken = lexer.GetNextToken();
                var value = E();
                if (currentToken.Type != TokenTypes.SbRightParent)
                {
                    throw new SyntaticException("(", currentToken.Row, currentToken.Column);

                }
                currentToken = lexer.GetNextToken();
                return value;
            }
            else
            {
                throw new SyntaticException("Expected Expression",currentToken.Row,currentToken.Column);
            }
        }

        public ExpressionNode RelationalExpression(ExpressionNode expresion)
        {
            if (CompareTokenType(TokenTypes.LessThanOp))
            {
                var node = new LesserOperationNode() { LeftOperand = expresion, RightOperand = E() };
                return node;
            }
            else if (CompareTokenType(TokenTypes.GreaterOp))
            {
                var node = new GreaterOperationNode() { LeftOperand = expresion, RightOperand = E() };
                return node;
            }
            else if (CompareTokenType(TokenTypes.LessThanOrEqualOp))
            {
                var node = new LesserOrEqualOperationNode() { LeftOperand = expresion, RightOperand = E() };
                return node;
            }
            else if (CompareTokenType(TokenTypes.GreaterThanOrEqualOp))
            {
                var node = new GreaterOrEqualOperationNode() { LeftOperand = expresion, RightOperand = E() };
                return node;
            }
            else if (CompareTokenType(TokenTypes.UnequalOp))
            {
                var node = new InequalityOperationNode() { LeftOperand = expresion, RightOperand = E() };
                return node;
            }
            else if (CompareTokenType(TokenTypes.EqualOp))
            {
                var node = new EqualityOperationNode() { LeftOperand = expresion, RightOperand = E() };
                return node;
            }

            return null;

        }

        private bool CompareTokenType(TokenTypes type)
        {
           if(currentToken.Type == type)
                return true;
            return false;
        }
    }

    internal class GreaterOperationNode : BinaryOperatorNode
    {
    }

    internal class LesserOrEqualOperationNode : BinaryOperatorNode
    {
    }

    internal class GreaterOrEqualOperationNode : BinaryOperatorNode
    {
    }

    internal class InequalityOperationNode : BinaryOperatorNode
    {
    }

    internal class EqualityOperationNode : BinaryOperatorNode
    {
    }

    internal class LesserOperationNode : BinaryOperatorNode
    {
    }

    internal class IdNode : BinaryOperatorNode
    {
        public string Value { get; set; }
    }

    internal class DivNode : BinaryOperatorNode
    {
    }

    internal class NumberNode : BinaryOperatorNode
    {
        
        public float Value { get; set; }
    }

    internal class MultNode : BinaryOperatorNode
    {
    }

    internal class SubNode : BinaryOperatorNode
    {
    }

    internal class AddNode : BinaryOperatorNode
    {
    }

    internal class ExpressionNode
    {
    }

    public class SyntaticException : Exception
    {
       
        public SyntaticException(string message) : base(message) {
        }

        
        public SyntaticException(string message, Exception innerException)   : base(message, innerException) {
        }

        public SyntaticException(string message, int row, int column) : base (message)
        {
            message = message + "Column :" + column.ToString() + "Row :" + row.ToString();
            
        }
        
            
        
        
        }
}
