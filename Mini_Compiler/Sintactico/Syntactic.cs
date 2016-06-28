using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Mini_Compiler.Lexer;
using Mini_Compiler.Semantic;
using Mini_Compiler.Semantic.Types;
using Mini_Compiler.Tree;
using Mini_Compiler.Tree.BooleanNode;
using Mini_Compiler.Tree.CaseNode;
using Mini_Compiler.Tree.Declaretion;


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
               else if (CompareTokenType(TokenTypes.RwExit))
               {
                   return null;
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
                        return Const();
                   
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
                   return DeclarationType();
                    

                    
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
                ConsumeNextToken();
               return Case();
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
                       
                        functioN.BlockFunction = FunctionBlock(functioN);
                        
                        if(CompareTokenType(TokenTypes.Eos))
                            ConsumeNextToken();

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
                    var sentencesList =   FunctionBlock(null);



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

        private List<SentencesNode> FunctionBlock(FunctionNode function)
        {
            List<SentencesNode> listSentencesOfBlock = new List<SentencesNode>();
            if (CompareTokenType(TokenTypes.RwBegin))
            {
                ConsumeNextToken();
                while (CompareTokenType(TokenTypes.RwEnd) == false)
                {
                    var sentencesNode = ListSentence();
                    if (sentencesNode != null) 
                    listSentencesOfBlock.Add(sentencesNode);
                    if (CompareTokenType(TokenTypes.RwExit))
                    {
                        ConsumeNextToken();
                        function.ReturParameter= E();

                        if (CompareTokenType(TokenTypes.Eos))
                            ConsumeNextToken();
                        else
                        {
                            throw new SyntaticException("Expected;", currentToken.Row, currentToken.Column);
                        }


                    }
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
                IdList(param.Variables);
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
                
                
                if (CompareTokenType(TokenTypes.Eos))
                {
                    ConsumeNextToken();
                
                  return  DeclarationParam(paramsOfProcedure);
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

        private ConstNode ConstDeclaretion()
        {
           
            ConstNode currentConst = new ConstNode();
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))
            {
             
                currentConst.ConstName.Value = currentToken.Lexeme;
            }
            else
            {
                throw new SyntaticException("Expected id", currentToken.Row, currentToken.Column);
            }

            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.EqualOp))
            {
                ConsumeNextToken();
                currentConst.ExpressionConst= E();
                currentConst.declare = false;
                return currentConst;

            }
            else if (CompareTokenType(TokenTypes.Declaretion))
            {
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.Id))
                {
                    currentConst.TypeOfConst.Value = currentToken.Lexeme;
                    ConsumeNextToken();

                    if (CompareTokenType(TokenTypes.EqualOp))
                    {
                        ConsumeNextToken();
                        currentConst.ExpressionConst = E();
                        currentConst.declare = true;
                        return currentConst;
                    }
                    else
                    {
                        throw new SyntaticException("Unexpected symbom", currentToken.Row, currentToken.Column);
                    }
                }
                else
                {
                    throw new SyntaticException("Unexpected symbom", currentToken.Row, currentToken.Column);
                }

            }
            else
            {
                throw new SyntaticException("Unexpected symbom", currentToken.Row, currentToken.Column);
            }

          
        }

        private SentencesNode Const()
        {
           var Cdeclaretion =  ConstDeclaretion();
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Eos))
            {
                ConsumeNextToken();
            }
            return Cdeclaretion;


        }

        private SentencesNode DeclarationType()
        {
             ConsumeNextToken();
            
            if (CompareTokenType(TokenTypes.Id))
            {
                string NameOfType = currentToken.Lexeme;
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.EqualOp))
                {
                    ConsumeNextToken();
                    var kindOfType = TypeF();
                    
                    kindOfType.Name = NameOfType;
                    //if (CompareTokenType(TokenTypes.RwEnd))
                    //{
                        //ConsumeNextToken();
                        if (CompareTokenType(TokenTypes.Eos))
                        {
                            ConsumeNextToken();
                            return kindOfType;
                        }
                        else
                        {
                            throw new SyntaticException("Expeceted ;", currentToken.Row, currentToken.Column);
                        }
                    //}
                    //else
                    //{
                    //    throw new SyntaticException("Expeceted End", currentToken.Row, currentToken.Column);
                    //}
                    
                }
                else
                {
                    throw new SyntaticException("Expected =",currentToken.Row, currentToken.Column);
                }
            }
            else
            {
                throw new SyntaxException("Expected Id");
            }

            
          
        }

        private TypeDeclaretionNode TypeF()
        {
            if (CompareTokenType(TokenTypes.Id))
            {
                string typeValue = currentToken.Lexeme;
                ConsumeNextToken();

          
                return new TypeIdNode {TypeId = typeValue};
            }
           


        else if (CompareTokenType(TokenTypes.RwArray))
            {
               return ArrayDeclaretion();
              
               
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


                return new EnumerateNode{list = idList};
             }
             else
             {
                 throw new SyntaticException("Expected LeftParent", currentToken.Row, currentToken.Column);
             }
            
         }else if (CompareTokenType(TokenTypes.RwRecord))
         {
                ConsumeNextToken();
                List<RecordProperties> sentencesRecord = new List<RecordProperties>();

             while (CompareTokenType(TokenTypes.RwEnd)==false)
             {
                   var sentences =  PropertyList();
                   sentencesRecord.Add(sentences);
                }


             if (CompareTokenType(TokenTypes.RwEnd))
             {
                 ConsumeNextToken();
             }
             else
             {
                 throw new SyntaticException("Expected End",currentToken.Row,currentToken.Row);
             }


             return new RecordNode {RecordProperties = sentencesRecord };
             
         }
            return null;
        }

        private TypeArrayNode ArrayDeclaretion()
        {
            
            ConsumeNextToken();
            List<Range> rangeList = new List<Range>();
            if (CompareTokenType(TokenTypes.OpenBracketOperator))
            {
       


                while (CompareTokenType(TokenTypes.CloseBracketOperator) == false)
                {
                    ConsumeNextToken();
                    var range = RangeF(rangeList);
                    rangeList.Add(range);
                }
                 
                    ConsumeNextToken();
                    if (CompareTokenType(TokenTypes.RwOf))
                    {
                       var typeArray =  ArrayTypes();

                        typeArray.ListRange = rangeList;
                        return typeArray;

                    }
                    else
                    {
                        throw new SyntaticException("Expected Reserverd Word Of",currentToken.Row,currentToken.Column);
                    }
                
                

            }
            else
            {
                throw new SyntaticException("UnExpected token", currentToken.Row, currentToken.Column);
            }
            
        }

        private TypeArrayNode ArrayTypes()
        {
            ConsumeNextToken();
            if (CompareTokenType(TokenTypes.Id))

            {
                string TypeName = currentToken.Lexeme;
                ConsumeNextToken();
                return new TypeArrayNode { TypeArray = TypeName };
            }
            else
            {
                throw new SyntaticException("Expected id", currentToken.Row, currentToken.Column);
            }
        }

       

        private Range RangeF(List<Range> list)
        {
           
             if (CompareTokenType(TokenTypes.NumericLiteral) || CompareTokenType(TokenTypes.char_literal))
                return SubRange();

            else
            {
                throw new SyntaticException("UnExpceted symbol", currentToken.Column, currentToken.Row);
            }
            return null;
        }

        private RecordProperties PropertyList( )
        {
            List<IdNode> idList = new List<IdNode>();
            if (CompareTokenType(TokenTypes.RwEnd) == false)
            {


               
                idList.Add(new IdNode { Value = currentToken.Lexeme});
                    IdList(idList);

               
                if (CompareTokenType(TokenTypes.Declaretion))
                {
                   var sentences = RecordType();

                    if (CompareTokenType(TokenTypes.Eos))
                    {
                        ConsumeNextToken();
                       


                    }
                    else
                    {
                        throw new SyntaticException("Expected ;", currentToken.Row, currentToken.Column);
                    }
                    
                  
                     return new RecordProperties {ListId = idList, Type = sentences};

                    
                }
                else
                {
                    throw new SyntaticException("Expected Declaretion", currentToken.Row, currentToken.Column);
                }



        

            }


            return null;
        }

        private TypeDeclaretionNode RecordType()
        {
            ConsumeNextToken();
            string typeName = " ";
            List<RecordProperties> sentencesRecord;
            if (CompareTokenType(TokenTypes.Id))
            {
                typeName = currentToken.Lexeme;
                ConsumeNextToken();

                return new TypeIdNode {TypeId = typeName};
            }
            else if (CompareTokenType(TokenTypes.RwRecord))
            {
                ConsumeNextToken();
                sentencesRecord = new List<RecordProperties>();

                while (CompareTokenType(TokenTypes.RwEnd) == false)
                {
                    var sentences = PropertyList();
                    sentencesRecord.Add(sentences);
                }

                ConsumeNextToken();
                return new RecordNode
                {
                     RecordProperties = sentencesRecord
                        
                };
            }


            else if (CompareTokenType(TokenTypes.RwArray))
            {
               return ArrayDeclaretion();
           
            }
            else if (CompareTokenType(TokenTypes.SbLeftParent))
            {
                ConsumeNextToken();
                List<IdNode> idList = new List<IdNode>();
                idList.Add(new IdNode { Value = currentToken.Lexeme });
                IdList(idList);

                if (CompareTokenType(TokenTypes.SbRightParent))
                {
                    ConsumeNextToken();


                    return new EnumerateNode { list = idList };
                }
                else
                {
                    throw new SyntaticException("Expected LeftParent", currentToken.Row, currentToken.Column);
                }

            }

            return null;
        }


        public ExpressionNode CallFunction(string name)
        {
            ConsumeNextToken();
            List<ExpressionNode> listOfExpression = new List<ExpressionNode>(); 
                ExpressionList(listOfExpression);

            if (CompareTokenType(TokenTypes.SbRightParent))
            {
                ConsumeNextToken();
               
                    return new CallFunctionNode
                    {
                        List = listOfExpression,
                        Name = name

                    };
                
                
            }
            else
            {
                throw new SyntaticException("Expeced )",currentToken.Row,currentToken.Column);
            }
        }

        private PreIdNode PreId()
        {

            List<ExpressionNode> listOfExpression = new List<ExpressionNode>();
            var accesList = new List<AccesorNode>();
            IdNode Id = new IdNode {Value = currentToken.Lexeme};
            ConsumeNextToken();


            if (CompareTokenType(TokenTypes.AccesOp) ||  CompareTokenType(TokenTypes.OpenBracketOperator))
            {
             
                IndexingAndAccess(accesList);
                Id.Accesors = accesList;
            }

            
            
            if (CompareTokenType(TokenTypes.AsiggnationOp))
            {

                ConsumeNextToken();
                var expression =  E();
             

                
                listOfExpression.Add(expression);
                ConsumeNextToken();
                return new PreIdNode {ListExpressionNodes = listOfExpression, Variable =Id, IsAProcedure = false, ExpressionAssigned  = expression};
            }
           else if (CompareTokenType(TokenTypes.SbLeftParent))
           {

               ConsumeNextToken();
                ExpressionList(listOfExpression);

               if (CompareTokenType(TokenTypes.SbRightParent))
               {
                   ConsumeNextToken();
                   if (CompareTokenType(TokenTypes.Eos))
                   {
                       ConsumeNextToken();
                       return new PreIdNode
                       {
                           ListExpressionNodes = listOfExpression,
                           Variable = Id,
                           IsAProcedure = true
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

        private CaseNode Case()
        {

            if (CompareTokenType(TokenTypes.Id))
            {
                var caseLexeme = currentToken.Lexeme;
                ConsumeNextToken();
                var accesList = new List<AccesorNode>();
                IndexingAndAccess(accesList);
                if (CompareTokenType(TokenTypes.RwOf))
                {
                    ConsumeNextToken();
                    var caseList = new List<CaseStatement>();
                    caseList = CaseList(caseList);
                    if (CompareTokenType(TokenTypes.RwEnd))
                    {
                        ConsumeNextToken();
                        if (CompareTokenType(TokenTypes.Eos))
                        {
                            ConsumeNextToken();
                        }
                        else
                        {
                            throw new SyntaticException("Unexpected Token", currentToken.Row, currentToken.Column);
                        }
                        return new CaseNode {CaseName = new IdNode {Value = caseLexeme,Accesors = accesList}, CaseStatements = caseList};
                    }

                  

                }
            }
            else
            {
                throw new SyntaticException("Unexpected Token",currentToken.Row,currentToken.Column);
            }
      
        

            return null;
        }

        private List<AccesorNode> IndexingAndAccess(List<AccesorNode> aList)
        {
            if (CompareTokenType(TokenTypes.OpenBracketOperator))
            {
                 ConsumeNextToken();
                var expression = E();
                if (CompareTokenType(TokenTypes.CloseBracketOperator))
                {
                    ConsumeNextToken();
                    aList.Insert(0, new IndexAccesorNode {IndexExpression = expression});//PropertyAccesorNode()
                    aList = IndexingAndAccess(aList);
                    return aList;


                }
                else
                {
                    throw  new SyntaticException("Unexpected Token",currentToken.Row,currentToken.Row);

                }
            }else if (CompareTokenType(TokenTypes.AccesOp))
            {
                ConsumeNextToken();
                if (CompareTokenType(TokenTypes.Id))
                {
                    var idName = new IdNode {Value = currentToken.Lexeme};
                    ConsumeNextToken();
                    aList.Insert(0, new PropertyAccesorNode() {IdNode = idName});
                  aList=   IndexingAndAccess(aList);
                    return aList;
                }
                else
                {
                    throw new SyntaticException("Unexpected Token", currentToken.Row,currentToken.Column);

                }
                    
            }




            return aList;
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

        private List<CaseStatement> CaseList(List<CaseStatement> caseList )
        {

            if (CompareTokenType(TokenTypes.RwElse))
            {
                ConsumeNextToken();
              var sentesList =  Block();
                caseList.Insert(0,new CaseDefaultStatement {Statements = sentesList});
                return caseList;
            }

          else if (CompareTokenType(TokenTypes.NumericLiteral))
          {
              var caseLiteral = CaseLiteral();
              if (CompareTokenType(TokenTypes.Declaretion))
              {
                  ConsumeNextToken();
                  var sentencia = Block();
                  caseList = CaseList(caseList);
                  caseList.Insert(0, new CaseNonDefualtStatement {Statements = sentencia, Literals= caseLiteral});

                  return caseList;
              }
          }
            return caseList;


          
        }

        private CaseLiteral CaseLiteral()
        {
            var option = new CaseLiteralList
            {
                LiteralList = new List<IntNode> {new IntNode {Value = int.Parse(currentToken.Lexeme)}}
            };
            var numberLiteral = currentToken.Lexeme;
            ConsumeNextToken();

            if (CompareTokenType(TokenTypes.RangeOp))
            {
                var rl = new List<Range>();
                RangeF(rl);
                return new CaseLiteralRange {LiteralRanges = rl};
            }

           else if (CompareTokenType(TokenTypes.CommaOperator))
            {
                var numberLiteralList = new List<IntNode> { new IntNode { Value = int.Parse(numberLiteral) } };
                var expressionList = new List<ExpressionNode>();
                ExpressionList(expressionList);
                foreach (var expressionNode in expressionList) numberLiteralList.Add((IntNode)expressionNode);

                return new CaseLiteralList { LiteralList = numberLiteralList };
            }

            return option;


        }

        

        private Range SubRange()
        {
           var firstExpression =  E();

            ExpressionNode secondeExpression;
            if (CompareTokenType(TokenTypes.RangeOp))
            {
                ConsumeNextToken();
                secondeExpression =   E();
                return new Range {Infe = firstExpression, Super = secondeExpression};
            }
            else
            {
                throw new SyntaticException("Expected RangeOp", currentToken.Row, currentToken.Column);
            }
         
            if (CompareTokenType(TokenTypes.CloseBracketOperator))
            {
                return new Range {Infe = secondeExpression};
            }

            if(CompareTokenType(TokenTypes.CommaOperator))
            {
               // RangeF();
                
            }


            return null;
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
         
        }

        private DeclarationNode Declaration()
        {

            return FactorComunIdPrime(); ;
        }

      
        //Y en mi Gramatica

        
        private DeclarationNode FactorComunIdPrime()
        {
            //IdOpcional
            ConsumeNextToken();
            var listId = new List<IdNode>();
          
            if (currentToken.Type == TokenTypes.Id)
            {
                listId.Add(new IdNode { Value = currentToken.Lexeme });
                // return FactorComunIdPrime();
            }
            else
            {
                throw new SyntaticException("Se esperaba un Id", currentToken.Row, currentToken.Column);
            }

           

            ConsumeNextToken();
            if (currentToken.Type == TokenTypes.Declaretion)
            {
                ConsumeNextToken();
                if (currentToken.Type == TokenTypes.Id)
                {
                    IdNode typeId = new IdNode { Value = currentToken.Lexeme };
                    ConsumeNextToken();




                    if (CompareTokenType(TokenTypes.Eos))
                    {
                        return new DeclarationNode {Expression = false, TypeId = typeId, LIstIdNode = listId};
                    }
                    else
                    {
                        var expression = AssignValue();
                        return new DeclarationNode { Expression = true, LIstIdNode = listId, TypeId = typeId, ExpressionType = expression };
                    }






                }



                else
                {
                    throw new SyntaticException("Expected Id", currentToken.Row, currentToken.Column);
                }
            }
            if (currentToken.Type == TokenTypes.CommaOperator)
            {
                OptonialId(listId);
            
                if (currentToken.Type == TokenTypes.Declaretion)
                {
                    ConsumeNextToken();
                    if (currentToken.Type == TokenTypes.Id)
                    {
                       
                        IdNode typeId = new IdNode {Value = currentToken.Lexeme};
                        ConsumeNextToken();


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
            if (currentToken.Type == TokenTypes.RwTrue)
            {
                var value = currentToken.Lexeme;
                currentToken = lexer.GetNextToken();
                return new BooleanNode { Value = true };//To do

            }


            if (currentToken.Type == TokenTypes.RwFalse)
            {
                var value = currentToken.Lexeme;
                currentToken = lexer.GetNextToken();
                return new BooleanNode { Value = false };//To do
            }
            if (currentToken.Type == TokenTypes.Hexa)
            {
                var value = currentToken.Lexeme;
                currentToken = lexer.GetNextToken();
                return new HexaNode {Value = value};
            }
            if (currentToken.Type == TokenTypes.Binary)
            {
                var value = currentToken.Lexeme;
                currentToken = lexer.GetNextToken();
                return new BinaNode { Value = value };
            }

            
            if (currentToken.Type == TokenTypes.RealLiteral)
            {
                var value = currentToken.Lexeme;
                currentToken = lexer.GetNextToken();
                return new RealNode { Value = double.Parse(value, CultureInfo.InvariantCulture) };
            }
            if (currentToken.Type == TokenTypes.NumericLiteral)
            {
                var value = float.Parse(currentToken.Lexeme);
                currentToken = lexer.GetNextToken();
                return new IntNode { Value = value };
            }
            else if (currentToken.Type == TokenTypes.Id)
            {
                string id = currentToken.Lexeme;
                currentToken = lexer.GetNextToken();
                if (CompareTokenType(TokenTypes.SbLeftParent))
                {
                    var callFunction = CallFunction(id);
                    return callFunction;
                }
                else
                {
                    var listAccesor = new List<AccesorNode>();
                    var alist = IndexingAndAccess(listAccesor);
                    return new IdNode
                    {
                        Value = id,
                        Accesors = alist
                    };
                }

                
        
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
            return null;
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
                var node = new NotNode {};
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

    internal class BinaNode : ExpressionNode
    {
        public string Value;
        public override BaseType ValidateSemantic()
        {

            return null;
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }

    internal class HexaNode : ExpressionNode
    {
        public string Value;
        

        public override BaseType ValidateSemantic()
        {
            throw new NotImplementedException();
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }

    internal class RealNode : ExpressionNode
    {
        public double Value { get; set; }
        public override BaseType ValidateSemantic()
        {
            return TypesTable.Instance.GetType("real");
        }

        public override string GenerateCode()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }

    internal class NotNode : ExpressionNode
    {
       
        
        public override BaseType ValidateSemantic()
        {
         BaseType type =   this.ValidateSemantic();

            if (type is BooleanType)
            {
                return type;
            }
            else
            {
                throw  new Exception("UnExptected symbol");
            }
        }

        public override string GenerateCode()
        {
            return "!";
        }
    }


    internal class ListSentencesNode
    {
    }

    internal class GreaterOperationNode : BinaryOperatorNode
    {
        public GreaterOperationNode()
        {
            Validation =
                new Dictionary<Tuple<BaseType, BaseType>, BaseType>
                {
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("Boolean")
                    }
                };
        }

        public override string GenerateCode()
        {
            return this.LeftOperand.GenerateCode() + ">" +this.RightOperand.GenerateCode();
        }
    }

    internal class LesserOrEqualOperationNode : BinaryOperatorNode
    {
        public LesserOrEqualOperationNode()
        {
           this.Validation =  new Dictionary<Tuple<BaseType, BaseType>, BaseType>
                {
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("Boolean")
                    }, {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("real"),
                            TypesTable.Instance.GetType("real")),
                        TypesTable.Instance.GetType("Boolean")
                    },
                };
        }

        public override string GenerateCode()
        {
            return this.LeftOperand.GenerateCode() + "<=" + this.RightOperand.GenerateCode();
        }
    }

    internal class GreaterOrEqualOperationNode : BinaryOperatorNode
    {
        public GreaterOrEqualOperationNode()
        {
            this.Validation = new Dictionary<Tuple<BaseType, BaseType>, BaseType>
                {
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("Boolean")
                    }, {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("real"),
                            TypesTable.Instance.GetType("real")),
                        TypesTable.Instance.GetType("Boolean")
                    },
                };
        }

        public override string GenerateCode()
        {
            return this.LeftOperand.GenerateCode() + ">=" + this.RightOperand.GenerateCode();
        }
    }

    internal class InequalityOperationNode : BinaryOperatorNode
    {
        public InequalityOperationNode()
        {
            this.Validation = new Dictionary<Tuple<BaseType, BaseType>, BaseType>
                {
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("Boolean")
                    }, {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("real"),
                            TypesTable.Instance.GetType("real")),
                        TypesTable.Instance.GetType("real")
                    },
                };
        }

        public override string GenerateCode()
        {
            return this.LeftOperand.GenerateCode() + "!=" + this.RightOperand.GenerateCode();
        }
    }

    internal class EqualityOperationNode : BinaryOperatorNode
    {
        public EqualityOperationNode()
        {
            this.Validation = new Dictionary<Tuple<BaseType, BaseType>, BaseType>
                {
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("Boolean")
                    }, {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("real"),
                            TypesTable.Instance.GetType("real")),
                        TypesTable.Instance.GetType("real")
                    },
                };
        }

        public override string GenerateCode()
        {
            return this.LeftOperand.GenerateCode() + "==" + this.RightOperand.GenerateCode();
        }
    }

    internal class LesserOperationNode : BinaryOperatorNode
    {
        public LesserOperationNode()
        {
            this.Validation = new Dictionary<Tuple<BaseType, BaseType>, BaseType>
                {
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("Boolean")
                    }, {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("real"),
                            TypesTable.Instance.GetType("real")),
                        TypesTable.Instance.GetType("real")
                    },
                };
        }


        public override string GenerateCode()
        {
            return this.LeftOperand.GenerateCode() + "<" + this.RightOperand.GenerateCode();
        }
    }

    internal class DivNode : BinaryOperatorNode
    {
      

        public DivNode()
        {
            Validation =
                new Dictionary<Tuple<BaseType, BaseType>, BaseType>
                {
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("integer")
                    },
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("real"),
                            TypesTable.Instance.GetType("real")),
                        TypesTable.Instance.GetType("real")
                    },
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("real"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("real")
                    },
                    
                };

        }

        public override string GenerateCode()
        {
            return this.LeftOperand.GenerateCode() + "/" + this.RightOperand.GenerateCode();
        }
    }

    public class IntNode : ExpressionNode
    {
       
        public float Value { get; set; }
        public override BaseType ValidateSemantic()
        {
            return TypesTable.Instance.GetType("integer");
        }

        public override string GenerateCode()
        {
            return $"{Value}";
        }
    }

    internal class MultNode : BinaryOperatorNode
    {
       
        public MultNode()
        {
             Validation =
                new Dictionary<Tuple<BaseType, BaseType>, BaseType>
                { 
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("integer")
                    },
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("real"),
                            TypesTable.Instance.GetType("real")),
                        TypesTable.Instance.GetType("real")
                    },
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("real"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("real")
                    },
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("real")),
                        TypesTable.Instance.GetType("real")
                    }
                };


        }


        public override string GenerateCode()
        {
            return this.LeftOperand.GenerateCode() + "*" + this.RightOperand.GenerateCode();
        }
    }
      
    internal class SubNode : BinaryOperatorNode
    {

        public SubNode()
        {

            Validation = new Dictionary<Tuple<BaseType, BaseType>, BaseType>
            {
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                        TypesTable.Instance.GetType("integer")),
                    TypesTable.Instance.GetType("integer")
                },
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("real"),
                        TypesTable.Instance.GetType("real")),
                    TypesTable.Instance.GetType("real")
                },

            };

        }


        public override string GenerateCode()
        {
            return this.LeftOperand.GenerateCode() + "-" + this.RightOperand.GenerateCode();
        }
    }

    internal class AddNode : BinaryOperatorNode
    {
       
        public AddNode()
        {
            
            Validation = new Dictionary<Tuple<BaseType, BaseType>, BaseType>
               {
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("integer")
                    },
                    {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("real"),
                            TypesTable.Instance.GetType("real")),
                        TypesTable.Instance.GetType("real")
                    }, {
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("string"),
                            TypesTable.Instance.GetType("char")),
                        TypesTable.Instance.GetType("string")
                    },{
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("char"),
                            TypesTable.Instance.GetType("string")),
                        TypesTable.Instance.GetType("string")
                    },{
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("char")),
                        TypesTable.Instance.GetType("string")
                    },{
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("char"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("string")
                    },{
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("integer"),
                            TypesTable.Instance.GetType("string")),
                        TypesTable.Instance.GetType("string")
                    },{
                        new Tuple<BaseType, BaseType>(TypesTable.Instance.GetType("string"),
                            TypesTable.Instance.GetType("integer")),
                        TypesTable.Instance.GetType("string")
                    },






               };
        }

        public override string GenerateCode()
        {
            return this.LeftOperand.GenerateCode() + "+" + this.RightOperand.GenerateCode();
        }
    }

    internal class StringNode : ExpressionNode
    {
        public string Value { get; set; }
        public override BaseType ValidateSemantic()
        {
            return TypesTable.Instance.GetType("string");
        }

        public override string GenerateCode()
        {
            var javaString = Value.Remove(0, 1);
            javaString = javaString.Remove(javaString.Length - 1, 1);
            return $"\"{javaString}\"";
;        }
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
