using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Versioning;
using System.Security.Policy;
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
           Sentence();
            
        }

        
        private void Sentence()
        {
            if (currentToken.Type == TokenTypes.Eof )
             {
                return;
            }
            ListSentence();
            Sentence();
        }
        private void ListSentence()
        {
            if (CompareTokenType(TokenTypes.Html))
            {         
                ConsumeNextToken();
                     
              
            }       
        else if (CompareTokenType(TokenTypes.RwConst))
        {
            Const();
            if (CompareTokenType(TokenTypes.Eos))
            {
                ConsumeNextToken();
            }
            else
            {
                throw new SyntaticException("Expeceted ;", currentToken.Row, currentToken.Column);
            }
        } 
        else  if (CompareTokenType(TokenTypes.RwVar))
            {
              Declaration();

            }
        else if (CompareTokenType(TokenTypes.RwIf))
         {
               IfPascal();
              
            }
        else if (CompareTokenType(TokenTypes.RwType))
        {
            DeclarationType();
            if (CompareTokenType(TokenTypes.RwEnd))
            {
                ConsumeNextToken();
            }
            
            if (CompareTokenType(TokenTypes.Eos))
            {
                ConsumeNextToken();
            }
            else
            {
                throw new SyntaticException("Expected ;",currentToken.Row,currentToken.Column);
            }
            }
        else if (CompareTokenType(TokenTypes.RwWhile))
        {
            While();
        }
        else if(CompareTokenType(TokenTypes.RwRepeat))
        {
            Repeat();
        }
        else if (CompareTokenType(TokenTypes.RwCase))
        {
            Case();
        }
        else if (CompareTokenType(TokenTypes.Id))
        {
            PreId();
        if (CompareTokenType(TokenTypes.Eos))
                    ConsumeNextToken();
                
            }

        else
        {
            throw new Exception("Expected a sentences");
        }

        }

        private void ConstDeclaretion()
        {
           
      
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.EqualOp))
            {
                E();
            }
            else if (CompareTokenType(TokenTypes.Declaretion))
            {
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.Id))
                {
                    ConsumeNextToken();
                    if (CompareTokenType(TokenTypes.EqualOp))
                    {
                        E();
                    }
                }


            }
            else
            {
                throw new SyntaticException("Unexpected symbom", currentToken.Row, currentToken.Column);
            }

        }

        private void Const()
        {
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))
            {
                ConstDeclaretion();
            }
            else
            {
                throw new SyntaticException("Expected id", currentToken.Row, currentToken.Column);
            }

        }

        private void DeclarationType()
        {
             ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))
            {
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.EqualOp))
                {
                    TypeF();
                }
            }
            else
            {
                throw new SyntaxException("Expected Id");
            }
            
        }

        private void TypeF()
        {
              ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))
            {
                ConsumeNextToken();
              
            }

         else if (CompareTokenType(TokenTypes.RwArray))
            {
                ArrayDeclaretion();
              
                if (CompareTokenType(TokenTypes.Eos))
                {
                    return;
                }
                else
                {
                    throw  new SyntaticException("Expect Eos",currentToken.Row,currentToken.Column);
                }
            }


         else if (CompareTokenType(TokenTypes.SbLeftParent))
         {
                ConsumeNextToken();
                IdList();
              
             if (CompareTokenType(TokenTypes.SbRightParent))
             {
                 ConsumeNextToken();
              
             }
             else
             {
                 throw new SyntaticException("Expected LeftParent", currentToken.Row, currentToken.Column);
             }
            
         }else if (CompareTokenType(TokenTypes.RwRecord))
         {
             PropertyList();
             if (CompareTokenType(TokenTypes.Eos))
             {
                 ConsumeNextToken();
             }
             
         }
        }

        private void ArrayDeclaretion()
        {
            
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.OpenBracketOperator))
            {
                RangeList();
               
                if (CompareTokenType(TokenTypes.CloseBracketOperator))
                {
                    ConsumeNextToken();
                    if (CompareTokenType(TokenTypes.RwOf))
                    {
                        ArrayTypes();
                    }
                    else
                    {
                        throw new SyntaticException("Expected Reserverd Word Of",currentToken.Row,currentToken.Column);
                    }
                }
                else
                {
                    throw new SyntaticException("Expected CloseBracker",currentToken.Row,currentToken.Column);
                }

            }
        }

        private void ArrayTypes()
        {
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))
                
            {
                ConsumeNextToken();
                return;
            }
        else if (CompareTokenType(TokenTypes.RwArray))
        {
            ArrayDeclaretion();
        }
        else
        {
            SubRange();
        }


        }

        private void RangeList()
        {
            RangeF();
            
            

        }

        private void RangeF()
        {
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))
            {
                ConsumeNextToken();



            }
            else
            {
                SubRange();
            }

        }

        private void PropertyList()
        {

            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.RwEnd) == false)
            {


                IdList();

                if (CompareTokenType(TokenTypes.Declaretion))
                {
                    RecordType();
                    if (CompareTokenType(TokenTypes.Eos)==false)
                    {
                        throw new SyntaticException("Expected EOS", currentToken.Row, currentToken.Column);

                    }
                    
                }
                else
                {
                    throw new SyntaticException("Expected Declaretion", currentToken.Row, currentToken.Column);
                }



                PropertyList();

            }


        }

        private void RecordType()
        {
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))
            {
                ConsumeNextToken();
                return;
            }
            else if (CompareTokenType(TokenTypes.RwRecord))
            {
                PropertyList();
            }


            else if (CompareTokenType(TokenTypes.RwArray))
            {
                ArrayDeclaretion();
           
                if (CompareTokenType(TokenTypes.Eos)==false)
                {
                    throw  new SyntaticException("Expected ;",currentToken.Row,currentToken.Column);
                }
            }
            
        }

        private void PreId()
        {
             ConsumeNextToken();
            if (CompareTokenType(TokenTypes.AsiggnationOp))
            {
                E();
            }
           else if (CompareTokenType(TokenTypes.SbLeftParent))
            {

                ExpressionList();
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.SbRightParent))
                {
                    ConsumeNextToken();
                    if (CompareTokenType(TokenTypes.Eos))
                    {
                        ConsumeNextToken();
                        return;
                    }
                    else
                    {

                        ListSentence();
                    }
                }
                


            }

        }

        private void ExpressionList()
        {
            E();
            ExpressionListOp();
            
            
        }

        private void ExpressionListOp()
        {
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.CommaOperator))
            {
                ExpressionList();
            }

            return;
        }

        private void Case()
        {
            
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))
            {
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.RwOf))
                {
                    CaseList();
                }
            }
            
           
        }

        private void ListChar()
        {

            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.CommaOperator))
            {
                CharOptional();
            }
        }

        private void CharOptional()
        {
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))
            {
                ListChar();
            }
            else
            {
                throw new SyntaticException("Expected id", currentToken.Row, currentToken.Column);
            }
            

        }

        private void CaseList()
        {

            if (CompareTokenType(TokenTypes.RwElse)) 
                {
                  Else();  
                } 
            CaseLiteral();
        }

        private void CaseLiteral()
        {
            ConsumeNextToken();
            
             if (CompareTokenType(TokenTypes.char_literal))
            {
                 ListChar();
            }
            else if (CompareTokenType(TokenTypes.Id))
            {
                IdList();
            }
            else
            {
                SubRange();
            }
          
        }

        private void SubRange()
        {
            E();
           
            if (CompareTokenType(TokenTypes.RangeOp))
            {
                E();
            }
            else
            {
                throw new SyntaticException("Expected RangeOp", currentToken.Row, currentToken.Column);
            }
         
            if (CompareTokenType(TokenTypes.Eos))
            {
                return;
            }

            if(CompareTokenType(TokenTypes.CommaOperator))
            {
                RangeF();
                
            }
            


        }

        private void ListNum()
        {
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.CommaOperator))
            {
                NumOptional();
            }

        }

        private void NumOptional()
        {
            
                 ConsumeNextToken();
            if (CompareTokenType(TokenTypes.NumericLiteral))
            {
                ListNum();
            }
            else
            {
                throw new SyntaxException("Expected Literal");
            }
        }

        private void Repeat()
        {
            LS_LOOP();
        }

        private void LS_LOOP()
        {
            
            LoopS();
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Eos))
            {
                return;
            }
            else
            {
                LoopS();
            }
        }

        private void While()
        {
            E();
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.RwDo))
            {
                LoopS();
            }

        }

        private void LoopS()
        {
            if (CompareTokenType(TokenTypes.RwContinnue))
            {
                return;
            }
            if (CompareTokenType(TokenTypes.RwWhile))
            {
                return;
            }
            else
            {
                ListSentence();
            }

        }

        private void IfPascal()
        {
            E();
            if (currentToken.Type == TokenTypes.RwThen)
            {

                Block();

                if (CompareTokenType(TokenTypes.Eos))
                {
                    ConsumeNextToken();


                }
                if (CompareTokenType(TokenTypes.RwElse))
                {
                    Else();

                    if (CompareTokenType(TokenTypes.Eos))
                        ConsumeNextToken();
                }
                else
                {
                    throw new SyntaxException("Expected  EOS");
                }
               
               
                
            }
        }

        private void Else()
        {
            Block();
        }

        private void Block()
        {
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.RwBegin))
            {
                Parse();
                if (CompareTokenType(TokenTypes.RwEnd))
                {
                    return;
                }

            }
            ListSentence();
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
                if (currentToken.Type == TokenTypes.Declaretion)
                {
                    ConsumeNextToken();
                    if (currentToken.Type == TokenTypes.Id)
                    {
                        ConsumeNextToken();
                        if (currentToken.Type == TokenTypes.Eos)
                        {
                            ConsumeNextToken();
                           
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
                else if (currentToken.Type == TokenTypes.Html)
                {
                    ConsumeNextToken();
                 
                }
                else
                {
                   throw new SyntaticException("Expected AsiggnationOp", currentToken.Row, currentToken.Column);
                }
            }
            else if (currentToken.Type == TokenTypes.Declaretion)
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
                throw new SyntaticException("Expected ID", currentToken.Row, currentToken.Column);
            }
             
               
        }
        
        private void IdList()
        {
            ConsumeNextToken();
            if (currentToken.Type == TokenTypes.CommaOperator)
            {
                OptonialId();
            }
            else
            {
                return;
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
            ConsumeNextToken();


            
            if (currentToken.Type == TokenTypes.StringLiteral)
            {
               
                currentToken = lexer.GetNextToken();

                return new ExpressionNode();//To do
            }
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
                return new IdNode();
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

        public SyntaticException(string message, int row, int column)
            : base( message + "Column :" + column.ToString() + "Row :" + row.ToString())
        {
            
            
        }
        
            
        
        
        }
}
