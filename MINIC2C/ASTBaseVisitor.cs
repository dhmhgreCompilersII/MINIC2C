using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_C
{
    public abstract class ASTBaseVisitor<T>
    {
        public T Visit(ASTComposite node)
        {
            return node.Accept(this);
        }
        public T VisitChildren(ASTComposite node)
        {
            for (int i = 0; i < node.MChildren.Length; i++)
            {
                foreach (ASTElement item in node.MChildren[i])
                {
                    item.Accept(this);
                }
            }
            return default(T);
        }
       /* public virtual T VisitCompileUnit(CASTCompileUnit node)
        {
            VisitChildren(node);
            return default(T);
        }*/
        public virtual T VisitIDENTIFIER(CASTIDENTIFIER node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitNUMBER(CASTNUMBER node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitFunctionDefinition(CASTFunctionDefinition node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitMultiplication(CASTExpressionMultiplication node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitDivision(CASTExpressionDivision node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitAddition(CASTExpressionAddition node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitSubtraction(CASTExpressionSubtraction node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitPLUS(CASTExpressionPlus node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitMINUS(CASTExpressionMinus node)
        {
            VisitChildren(node);
            return default(T);
        }
        
        public virtual T VisitASSIGN(CASTExpressionAssign node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitNOT(CASTExpressionNot node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitAND(CASTExpressionAnd node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitOR(CASTExpressionOr node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitGT(CASTExpressionGt node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitGTE(CASTExpressionGte node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitLT(CASTExpressionLt node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitLTE(CASTExpressionLte node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitEQUAL(CASTExpressionEqual node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitNEQUAL(CASTExpressionNequal node)
        {
            VisitChildren(node);
            return default(T);
        }
       
        public virtual T VisitSTATEMENTLIST(CASTStatementList node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitCOMPOUNDSTATEMENT(CASTCompoundStatement node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitIFSTATEMENT(CASTIfStatement node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitWHILESTATEMENT(CASTWhileStatement node)
        {
            VisitChildren(node);
            return default(T);
        }
        public virtual T VisitSTATEMENT(CASTStatement node)
        {
            VisitChildren(node);
            return default(T);
        }
                                           
        public virtual T VisitCOMPILEUNIT(CASTCompileUnit node)
        {
            VisitChildren(node);
            return default(T);
        }

            }
}
