using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Mini_Compiler.Lexer;
using Mini_Compiler.Tree;

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



        public List<Tree.SentencesNode> Parse()
        {
          var sentencesList = Sentence();
            return sentencesList;
        }

        
        private List<Tree.SentencesNode> Sentence()
        {
          
            
            if (currentToken.Type == TokenTypes.Eof )
             {
                return new List<Tree.SentencesNode>();
            }
                      
                var sentence =  ListSentence();
               
            
           var listSentences = Sentence();
            if (sentence != null)
                listSentences.Insert(0,sentence);
            return listSentences;
        }

        public SentencesNode ListSentence()
        {
            if (CompareTokenType(TokenTypes.Html))
                {
                    ConsumeNextToken();


                }
                else if (CompareTokenType(TokenTypes.RwFunction))
                {
                    return FunctionDecla();
                }
                else if (CompareTokenType(TokenTypes.RwWhile))
                {
                   return While();
                }
                else if (CompareTokenType(TokenTypes.RwProcedure))
                {
                    ConsumeNextToken();
               return  Procedure();
                   
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
                else if (CompareTokenType(TokenTypes.RwVar))
                {
                   var declaretionList =  Declaration();

                    if (CompareTokenType(TokenTypes.Eos))
                    {
                        ConsumeNextToken();
                    }
                   else
                    {
                    throw new SyntaticException("Expeceted ;", currentToken.Row, currentToken.Column);
                    }


                return declaretionList;

                }
                else if (CompareTokenType(TokenTypes.RwIf))
                {
                    var ifNode = IfPascal();
                    return ifNode;


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
                        throw new SyntaticException("Expected ;", currentToken.Row, currentToken.Column);
                    }
                }
                else if (CompareTokenType(TokenTypes.RwWhile))
                {
                   return  While();
                }
                else if (CompareTokenType(TokenTypes.RwRepeat))
                {
                ConsumeNextToken();
                return Repeat();
                }
                else if (CompareTokenType(TokenTypes.RwCase))
                {
                    Case();
                }
                else if (CompareTokenType(TokenTypes.RwFor))
                {
                    var someFor = PreFor();
               
            
               return someFor;


                }
                else if (CompareTokenType(TokenTypes.Id))
                {
                    var somePreId = PreId();
                    
                    return somePreId;
                }
                else
                {
                    throw new SyntaticException("Unexpected token", currentToken.Row, currentToken.Column);
                }


            return null;
        }


        private FunctionNode FunctionDecla()
        {
             FunctionNode functioN = new FunctionNode();
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))
            {
                functioN.NameOfFunction.Value = currentToken.Lexeme;
                ConsumeNextToken();
                functioN.Params = Params();
            }
            else
            {
                throw new SyntaticException("Expected some Id",currentToken.Row, currentToken.Column);
            }

            if (CompareTokenType(TokenTypes.Declaretion))
            {
                ConsumeNextToken();

                if (CompareTokenType(TokenTypes.Id))
                {
                    functioN.TypeOfReturn.Value = currentToken.Lexeme;
                    ConsumeNextToken();
                    if (CompareTokenType(TokenTypes.Eos))
                    {
                        ConsumeNextToken();
                        functioN.BlockFunction = FunctionBlock();

                        return functioN;

                    }
                    else
                    {
                        throw new SyntaticException("Expected ;", currentToken.Row, currentToken.Column);
                    }
                }
                else
                {
                    throw new SyntaticException("Expected some id", currentToken.Row, currentToken.Column);
                }
              
            }
            else
            {
                throw new SyntaticException("Expected :",currentToken.Row,currentToken.Column);
            }

        }


        private SentencesNode PreFor()
        {
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))
            {
                var firstId = new IdNode {Value = currentToken.Lexeme};
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.AsiggnationOp))
                {
                    ConsumeNextToken();
                    var forNode = For(firstId);
                    
                           
                            return forNode;
                        
                    
                    
                }
                else if (CompareTokenType(TokenTypes.RwIn))
                {
                    ConsumeNextToken();
                    var forInNode = FORIN(firstId);
                                         
                            return forInNode;    
                    

                }



            }

            return null;
        }

        private SentencesNode FORIN(IdNode firstId)
        {
            if (firstId == null) throw new ArgumentNullException(nameof(firstId));
            if (CompareTokenType(TokenTypes.Id))
            {
                var secondId = new IdNode {Value = currentToken.Lexeme};
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.RwDo))
                {
                    ConsumeNextToken();
                   
                var listSentences = LoopBlock();
                    return new ForInNode {ListSentences = listSentences, FirstId = firstId, SecondId = secondId};
                }
                else
                {
                    throw new SyntaticException("Expected the reserverd word DO", currentToken.Row, currentToken.Column);
                }

            
            }
            else
            {
                throw new SyntaticException("Expected id", currentToken.Row, currentToken.Column);
            }
        }

        private ForNode For(IdNode firstId)
        {
       
               var firstExpression =  E();
            if (CompareTokenType((TokenTypes.RwTo)))
            {
                ConsumeNextToken();
                var secondExpression = E();
                if (CompareTokenType(TokenTypes.RwDo))
                {
                    ConsumeNextToken();
                    List<SentencesNode> listSentences = LoopBlock();


                    return new ForNode
                    {
                        FirstIdOfCondition = firstId,
                        FirstCondition = firstExpression,
                        SecondCondition = secondExpression,
                        Sentences = listSentences
                    };

                    //Podria controlarlo aqui

                }
            }
            else
            {
                throw new SyntaticException("Expected reserverd word To", currentToken.Row, currentToken.Column);
            }

            return null;
        }

        private List<SentencesNode> LoopBlock()
        {
            List<SentencesNode>listSentences = new List<SentencesNode>();
            if (CompareTokenType(TokenTypes.RwBegin))
            {
       
              ConsumeNextToken();
                //Podria controlarlo aqui
                while(CompareTokenType(TokenTypes.RwEnd) == false)
                {
                SentencesNode sentences = LoopS();
                    listSentences.Add(sentences);
                }
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.Eos))
                {
                    ConsumeNextToken();
                    return listSentences;
                }
                else
                {
                    throw new SyntaticException("Expected ;", currentToken.Row, currentToken.Column);
                }
                
            }

            else
            {
               SentencesNode oneSentence =  LoopS();
                listSentences.Add(oneSentence);
                return listSentences;
            }


           
        }

        private SentencesNode Procedure()
        {
            
             if (CompareTokenType(TokenTypes.Id))
             {
                 var nameOfProcedure = new IdNode {Value = currentToken.Lexeme};
                 ConsumeNextToken();
               List<ParamsOfDeclaretion> paramsOfProcedure =  Params();
                if (CompareTokenType(TokenTypes.Eos))
                {
                    ConsumeNextToken();
                    var sentencesList =   FunctionBlock();



                    return new ProcedureNode {NameOfProcedure = nameOfProcedure, Params = paramsOfProcedure,BlockProcedure = sentencesList};
                }
                else
                {
                    throw new SyntaticException("Expected ;",currentToken.Row,currentToken.Column);
                }
            }
            else
            {
                throw new SyntaticException("Expected id", currentToken.Row, currentToken.Column);
            }

            return null;
        }

        private List<SentencesNode> FunctionBlock()
        {
            List<SentencesNode> listSentencesOfBlock = new List<SentencesNode>();
            if (CompareTokenType(TokenTypes.RwBegin))
            {
                ConsumeNextToken();
                while (CompareTokenType(TokenTypes.RwEnd) == false)
                {
                    var sentencesNode = ListSentence();
                    listSentencesOfBlock.Add(sentencesNode);
                }
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.Eos))
                {
                    ConsumeNextToken();
                    return listSentencesOfBlock;
                }

               
            }
            else
            {
                throw  new SyntaticException("Expected a Begin Token",currentToken.Row, currentToken.Column);
            }


            return null;
        }

        private List<ParamsOfDeclaretion> Params()
        {
            if (CompareTokenType(TokenTypes.SbLeftParent))
            {
                ConsumeNextToken();
              var listOfParams =   DeclarationParam(new List<ParamsOfDeclaretion>());
                if (CompareTokenType(TokenTypes.SbRightParent))
                {
                    ConsumeNextToken();
                    return listOfParams;
                }
                else
                {
                    throw new SyntaticException("Expected a (", currentToken.Row, currentToken.Column);
                }
            }
                
            else
            {
                throw new SyntaticException("Expected a )",currentToken.Row,currentToken.Column);
            };
        }

        

        public List<ParamsOfDeclaretion> DeclarationParam(List<ParamsOfDeclaretion> paramsOfProcedure)
        {
            ParamsOfDeclaretion param = new ParamsOfDeclaretion();
           

            if (CompareTokenType(TokenTypes.RwVar))
            {
                param.IsDeclaretionVar = true;
                ConsumeNextToken();

                
                if (CompareTokenType(TokenTypes.Id))
                {
                    param.Variables.Add(new IdNode {Value = currentToken.Lexeme});
                    IdList(param.Variables);


                }
                else
                {
                    throw new SyntaticException("Excpted Id",currentToken.Row,currentToken.Column);
                }
                if (CompareTokenType(TokenTypes.Declaretion))
                {
                    ConsumeNextToken();
                    if (CompareTokenType(TokenTypes.Id))
                    {
                        param.TypeV.Value = currentToken.Lexeme;
                        paramsOfProcedure.Add(param);
                        ConsumeNextToken();
                    }
                    else
                    {
                        throw new SyntaticException("Excpted Id", currentToken.Row, currentToken.Column);
                    }


                }
                else
                { 
                    throw new SyntaticException("Expected :", currentToken.Row, currentToken.Column);
                    
                }
                if (CompareTokenType(TokenTypes.Eos))
                {
                    ConsumeNextToken();
                    return DeclarationParam(paramsOfProcedure);
                }
                else
                {
                    return paramsOfProcedure;
                }

            }

            else if (CompareTokenType(TokenTypes.Id))
            {
                param.IsDeclaretionVar = false;
                param.Variables.Add(new IdNode { Value = currentToken.Lexeme });
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.Declaretion))
                {
                    ConsumeNextToken();

                    if (CompareTokenType(TokenTypes.Id))
                    {
                        param.TypeV.Value = currentToken.Lexeme;
                        ConsumeNextToken();
                        paramsOfProcedure.Add(param);
                    }
                    else
                    {
                        throw new SyntaticException("Expected Id", currentToken.Row, currentToken.Row);
                    }
                }
                else
                {
                    throw new SyntaticException("Expected :", currentToken.Row, currentToken.Row);
                }
                
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.Eos))
                {
                    ConsumeNextToken();
                
                    DeclarationParam(paramsOfProcedure);
                }
                else
                {
                    
                    return paramsOfProcedure;
                }
            }
            else
            {
                throw new SyntaticException("UnExpected Token", currentToken.Row, currentToken.Row);
            }

            return null;
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
                List<IdNode> idList = new List<IdNode>();
                idList.Add(new IdNode {Value = currentToken.Lexeme});
                IdList(idList);
              
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


                List<IdNode> idList = new List<IdNode>(); 
                idList.Add(new IdNode { Value = currentToken.Lexeme});
                    IdList(idList);

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

        private PreIdNode PreId()
        {

            List<ExpressionNode> ListOfExpression= new List<ExpressionNode>();
            IdNode Id = new IdNode {Value = currentToken.Lexeme};
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.AsiggnationOp))
            {

                ConsumeNextToken();
                var expression =  E();
             

                
                ListOfExpression.Add(expression);
                ConsumeNextToken();
                return new PreIdNode {ListExpressionNodes = ListOfExpression, Variable =Id, IsAfunction = false };
            }
           else if (CompareTokenType(TokenTypes.SbLeftParent))
           {

               ConsumeNextToken();
                ExpressionList(ListOfExpression);

               if (CompareTokenType(TokenTypes.SbRightParent))
               {
                   ConsumeNextToken();
                   if (CompareTokenType(TokenTypes.Eos))
                   {
                       ConsumeNextToken();
                       return new PreIdNode
                       {
                           ListExpressionNodes = ListOfExpression,
                           Variable = Id,
                           IsAfunction = false
                       };
                   }
                   else
                   {
                       throw new SyntaticException("Expected ;", currentToken.Row, currentToken.Column);
                   }
               }



           }
           else
           {
                throw new SyntaticException("Expected ()", currentToken.Row, currentToken.Column);
              
           }

            return null;
        }

        private List<ExpressionNode> ExpressionList(List<ExpressionNode> expressionList)
        {
            //Arreglar
          
            var expression =  E();
            if (expression != null)
            {
                expressionList.Add(expression);
            }
            if (CompareTokenType(TokenTypes.SbRightParent))
            {
                return expressionList;
            }
            else
            {
               return ExpressionListOp(expressionList);
            }


           
        }

        private List<ExpressionNode> ExpressionListOp(List<ExpressionNode> Elist)
        {
           // ConsumeNextToken();
            if (CompareTokenType(TokenTypes.CommaOperator))
            {
                ConsumeNextToken();
                return ExpressionList(Elist);
            }

            else
            {
                throw new SyntaticException("Expected ;",  currentToken.Row,currentToken.Column);
            }
        }

        private void Case()
        {
        

        ConsumeNextToken();
            E();
           
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.RwOf))
                {
                    CaseList();
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

           else 
            {
                CaseLiteral();
            }
        }

        private void CaseLiteral()
        {
            ConsumeNextToken();


            if (CompareTokenType(TokenTypes.NumericLiteral))
            {
                ListNum();
            }
            else if (CompareTokenType(TokenTypes.char_literal))
            {
                 ListChar();
            }
            else if (CompareTokenType(TokenTypes.Id))
            {
                List<IdNode> idList = new List<IdNode>();
                idList.Add(new  IdNode {Value = currentToken.Lexeme});
                IdList(idList);
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

        private SentencesNode Repeat()
        {
            List<SentencesNode> sentencesList = new List<SentencesNode>();
           
            while (CompareTokenType(TokenTypes.RwUntil)==false)
            {
               var sentence =  LoopS();
                sentencesList.Add(sentence);
            }

            ConsumeNextToken();
            var expression =  E();


            if (CompareTokenType(TokenTypes.Eos))
            {
                ConsumeNextToken();
                return new RepeatNode {ListSentences = sentencesList, Condition = expression};
            }
            else
            {
                throw new SyntaticException("Se esperaba un ;", currentToken.Row, currentToken.Column);
            }
            
        }

        private void LS_LOOP()
        {
            
            LoopS();
           

         

        }

        private SentencesNode While()
        {
            ConsumeNextToken();
            var condition = E();

            if (CompareTokenType(TokenTypes.RwDo))
            {
                ConsumeNextToken();
                var sentenceList = LoopBlock();


                ConsumeNextToken();
                return new WhileNode {Condition = condition, Sentences = sentenceList};




            }
            else
            {
                throw new SyntaticException("Expected while", currentToken.Row, currentToken.Column);
            }

            
        }

        private SentencesNode LoopS()
        {
            
            if (CompareTokenType(TokenTypes.RwContinnue))
            {
                return null;
            }
            if (CompareTokenType(TokenTypes.RwBreak))
            {
                return null;
            }

            else if(CompareTokenType(TokenTypes.RwEnd))
            {
                return null;
            }
            else
            {
                var sentencesNode = ListSentence();
                return sentencesNode;
            }
        }

        private IfNode IfPascal()
        {
            var trueBlock = new List<Tree.SentencesNode>();
            var falseBlock = new List<Tree.SentencesNode>();
            ConsumeNextToken();
            
            var expression =  E();
            if (currentToken.Type == TokenTypes.RwThen)
            {

                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.RwBegin))
                {
                    var trueB = Block();
                    trueBlock.AddRange(trueB);
                }
                else
                {
                    var trueB = ListSentence();
                    trueBlock.Add(trueB);
                }
               
                

                
              if (CompareTokenType(TokenTypes.RwElse))
              {
                 falseBlock =  Else();
              }
               
               return  new IfNode { FalseBlock = falseBlock, TrueBlock = trueBlock, IfCondition = expression};
               
                
            }
            return null;
        }

        private List<Tree.SentencesNode> Else()
        {

            ConsumeNextToken();
            List<Tree.SentencesNode> elseBlock = Block();
         
            

            return elseBlock;

        }

        private List<Tree.SentencesNode> Block()
        {
           
              
            List<SentencesNode> list = new List<SentencesNode>();
            if (CompareTokenType(TokenTypes.RwBegin))
            {
                ConsumeNextToken();
                while (CompareTokenType(TokenTypes.RwEnd)==false)
                {
                    var sentences = ListSentence();
                    list.Add(sentences);
                }
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.Eos))
                {
                    ConsumeNextToken();
                }
                else
                {
                    throw new SyntaticException("Se esperaba un ;", currentToken.Row, currentToken.Column);
                }
                return list;


            }
            else
            {
                var sentencesNode = ListSentence();
                //    listSentences.Add(sentencesNode);
                // return listSentences ;

             


                if (sentencesNode != null)
                    list.Add(sentencesNode);
                return list;

            }
            return null;
        }

        private DeclarationNode Declaration()
        {

            return FactorComunId(); ;
        }

        private DeclarationNode FactorComunId()
        {
            ConsumeNextToken();
            if (currentToken.Type==TokenTypes.Id)
            {
               return  FactorComunIdPrime();
            }
            else
            {
                throw new SyntaticException("Se esperaba un Id", currentToken.Row, currentToken.Column);
            }
        }
        //Y en mi Gramatica

        
        private DeclarationNode FactorComunIdPrime()
        {
            //IdOpcional


            var listId =  new List<IdNode>();
            listId.Add(new IdNode {Value = currentToken.Lexeme});

            ConsumeNextToken();

            if (currentToken.Type == TokenTypes.CommaOperator)
            {
                OptonialId(listId);
            
                if (currentToken.Type == TokenTypes.Declaretion)
                {
                    ConsumeNextToken();
                    if (currentToken.Type == TokenTypes.Id)
                    {
                        ConsumeNextToken();
                        IdNode typeId = new IdNode {Value = currentToken.Lexeme};
                        
  
                            return new DeclarationNode {Expression = false,TypeId = typeId,LIstIdNode = listId};
                           
                       
                        
                    }

                    
                   
                    else
                    {
                        throw  new SyntaticException("Expected Id", currentToken.Row, currentToken.Column);
                    }
                }
                else if (currentToken.Type == TokenTypes.Html)
                {
                    ConsumeNextToken();
                 
                }
                else
                {
                   throw new SyntaticException("Expected Declaration", currentToken.Row, currentToken.Column);
                }
            }
            else if (currentToken.Type == TokenTypes.Declaretion)
            {

           ConsumeNextToken();

           var typeId = new IdNode {Value = currentToken.Lexeme};
                ConsumeNextToken();
           var expression =    AssignValue();
          return new DeclarationNode {Expression = true, LIstIdNode = listId,TypeId = typeId,ExpressionType = expression};

            }
            else if (currentToken.Type == TokenTypes.Eos)
            {
                var sentencesNode = ListSentence();
            }
            else
            {
                throw  new SyntaticException("Missing EOS",currentToken.Row, currentToken.Column);
            }

            return null;
        }

        private ExpressionNode AssignValue()
        {
        ConsumeNextToken();
          var exp =   E();
            if (currentToken.Type == TokenTypes.Eos)
            {
               
                return exp;
            }
            else
            {
                throw new SyntaticException("Expected Eos",currentToken.Row,currentToken.Column);
            }
        }

        private List<IdNode> OptonialId( List<IdNode> list )
        {
           ConsumeNextToken();
            if (currentToken.Type == TokenTypes.Id)
            {
                list.Add(new IdNode {Value = currentToken.Lexeme});
                return  IdList(list );


            }
            else
            {
                throw new SyntaticException("Expected ID", currentToken.Row, currentToken.Column);
            }
             
               
        }
        
        private List<IdNode> IdList(List<IdNode> list)
        {
          
            ConsumeNextToken();
            if (currentToken.Type == TokenTypes.CommaOperator)
            {

              return  OptonialId(list);
            }
            else
            {
                return list;

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

           

            return relational;
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
            


            
            if (currentToken.Type == TokenTypes.StringLiteral)
            {


                var value = currentToken.Lexeme;
                currentToken = lexer.GetNextToken();


                return new StringNode { Value= value};//To do
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
                return new IdNode {Value = id };
                
           

        
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
                ConsumeNextToken();
                var node = new LesserOperationNode() { LeftOperand = expresion, RightOperand = Expression() };
                return RelationalExpression(node);
            }
            else if (CompareTokenType(TokenTypes.GreaterOp))
            {

                ConsumeNextToken();
                var node = new GreaterOperationNode() { LeftOperand = expresion, RightOperand = Expression() };
                
                return RelationalExpression(node);
            }
            else if (CompareTokenType(TokenTypes.LessThanOrEqualOp))
            {
                ConsumeNextToken();
                var node = new LesserOrEqualOperationNode() { LeftOperand = expresion, RightOperand = Expression() };
                return RelationalExpression(node);
            }
            else if (CompareTokenType(TokenTypes.GreaterThanOrEqualOp))
            {
                ConsumeNextToken();
                var node = new GreaterOrEqualOperationNode() { LeftOperand = expresion, RightOperand = Expression() };
                return RelationalExpression(node);
            }
            else if (CompareTokenType(TokenTypes.UnequalOp))
            {
                ConsumeNextToken();
                var node = new InequalityOperationNode() { LeftOperand = expresion, RightOperand = Expression() };
                return RelationalExpression(node);
            }
            else if (CompareTokenType(TokenTypes.EqualOp))
            {
                ConsumeNextToken();
                var node = new EqualityOperationNode() { LeftOperand = expresion, RightOperand = Expression() };
                return RelationalExpression(node);
            }
            else if (CompareTokenType(TokenTypes.RwNot))
            {
                ConsumeNextToken();
                var node = new NotNode() {LeftOperand = expresion, RightOperand = Expression()};
                return RelationalExpression(node);
            }


            return expresion;

        }

        private bool CompareTokenType(TokenTypes type)
        {
           if(currentToken.Type == type)
                return true;
            return false;
        }
    }

    internal class NotNode : BinaryOperatorNode
    {
    }


    internal class ListSentencesNode
    {
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

    internal class StringNode : BinaryOperatorNode
    {
        public string Value { get; set; }
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
