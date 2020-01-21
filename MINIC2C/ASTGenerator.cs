using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANTLR_Startup_Project;
using Antlr4.Runtime.Tree;
using MINIC2C;

namespace Mini_C
{
    class ASTGenerator : MINICBaseVisitor<int>
    {
        private ASTComposite m_root;

        Stack<ASTComposite> m_parents = new Stack<ASTComposite>();

        Stack<contextType> m_parentContext = new Stack<contextType>();

        public ASTComposite M_Root => m_root;


        public override int VisitCompileUnit(MINICParser.CompileUnitContext context)
        {
            CASTCompileUnit newnode = new CASTCompileUnit(context.GetText(),  null, 2);
            m_root = newnode;
            m_parents.Push(newnode);
            this.VisitElementsInContext(context.statement(), m_parentContext, contextType.CT_COMPILEUNIT_MAINBODY);
            this.VisitElementsInContext(context.functionDefinition(), m_parentContext, contextType.CT_COMPILEUNIT_FUNCTIONDEFINITIONS);
            m_parents.Pop();
            return 0;
        }

        public override int VisitFunctionDefinition(MINICParser.FunctionDefinitionContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTFunctionDefinition newnode = new CASTFunctionDefinition(context.GetText(), m_parents.Peek(), 3);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);
            this.VisitTerminalInContext(context, context.IDENTIFIER().Symbol, m_parentContext,
                contextType.CT_FUNCTIONDEFINITION_IDENTIFIER);
            if (context.fargs() != null)
                this.VisitElementInContext(context.fargs(), m_parentContext, contextType.CT_FUNCTIONDEFINITION_FARGS);
            this.VisitElementInContext(context.compoundStatement(), m_parentContext, contextType.CT_FUNCTIONDEFINITION_BODY);
            m_parents.Pop();
            return 0;
        }

        
        public override int VisitExpr_DIVMULT(MINICParser.Expr_DIVMULTContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            if (context.op.Type == MINICParser.MULT)
            {
                CASTExpressionMultiplication newnode = new CASTExpressionMultiplication(context.GetText(), m_parents.Peek(), 2);
                m_parent.AddChild(newnode, m_parentContext.Peek());
                m_parents.Push(newnode);
                this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_MULTIPLICATION_LEFT);
                this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_MULTIPLICATION_RIGHT);
            }
            else if (context.op.Type == MINICParser.DIV)
            {
                CASTExpressionDivision newnode = new CASTExpressionDivision(context.GetText(), m_parents.Peek(), 2);
                m_parent.AddChild(newnode, m_parentContext.Peek());
                m_parents.Push(newnode);
                this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_DIVISION_LEFT);
                this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_DIVISION_RIGHT);
            }

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_PARENTHESIS(MINICParser.Expr_PARENTHESISContext context) {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionInParenthesis newnode = new CASTExpressionInParenthesis(context.GetText(), m_parents.Peek(), 1);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(), m_parentContext, contextType.CT_EXPRESSION_PARENTHESIS);

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_PLUSMINUS(MINICParser.Expr_PLUSMINUSContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            if (context.op.Type == MINICParser.MINUS)
            {
                CASTExpressionSubtraction newnode = new CASTExpressionSubtraction(context.GetText(), m_parents.Peek(), 2);
                m_parent.AddChild(newnode, m_parentContext.Peek());
                m_parents.Push(newnode);

                this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_SUBTRACTION_LEFT);
                this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_SUBTRACTION_RIGHT);
            }
            else if (context.op.Type == MINICParser.PLUS)
            {
                CASTExpressionAddition newnode = new CASTExpressionAddition(context.GetText(), m_parents.Peek(), 2);
                m_parent.AddChild(newnode, m_parentContext.Peek());
                m_parents.Push(newnode);

                this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_ADDITION_LEFT);
                this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_ADDITION_RIGHT);
            }



            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_PLUS(MINICParser.Expr_PLUSContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionPlus newnode = new CASTExpressionPlus(context.GetText(), m_parents.Peek(), 1);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);
            this.VisitElementInContext(context.expression(), m_parentContext, contextType.CT_EXPRESSION_PLUS);

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_MINUS(MINICParser.Expr_MINUSContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionMinus newnode = new CASTExpressionMinus(context.GetText(), m_parents.Peek(), 1);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(), m_parentContext, contextType.CT_EXPRESSION_MINUS);

            m_parents.Pop();
            return 0;
        }
        
        public override int VisitExpr_ASSIGN(MINICParser.Expr_ASSIGNContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionAssign newnode = new CASTExpressionAssign(context.GetText(), m_parents.Peek(), 2);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitTerminalInContext(context, context.IDENTIFIER().Symbol, m_parentContext, contextType.CT_EXPRESSION_ASSIGN_LVALUE);
            this.VisitElementInContext(context.expression(), m_parentContext, contextType.CT_EXPRESSION_ASSIGN_EXPRESSION);
            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_NOT(MINICParser.Expr_NOTContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionNot newnode = new CASTExpressionNot(context.GetText(), m_parents.Peek(), 1);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(), m_parentContext, contextType.CT_EXPRESSION_NOT);

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_AND(MINICParser.Expr_ANDContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionAnd newnode = new CASTExpressionAnd(context.GetText(), m_parents.Peek(), 2);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_AND_LEFT);
            this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_AND_RIGHT);

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_OR(MINICParser.Expr_ORContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionOr newnode = new CASTExpressionOr(context.GetText(), m_parents.Peek(), 2);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_OR_LEFT);
            this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_OR_RIGHT);

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_GT(MINICParser.Expr_GTContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionGt newnode = new CASTExpressionGt(context.GetText(), m_parents.Peek(), 2);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_GT_LEFT);
            this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_GT_RIGHT);

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_GTE(MINICParser.Expr_GTEContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionGte newnode = new CASTExpressionGte(context.GetText(), m_parents.Peek(), 2);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_GTE_LEFT);
            this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_GTE_RIGHT);

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_LT(MINICParser.Expr_LTContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionLt newnode = new CASTExpressionLt(context.GetText(), m_parents.Peek(), 2);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_LT_LEFT);
            this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_LT_RIGHT);

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_LTE(MINICParser.Expr_LTEContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionLte newnode = new CASTExpressionLte(context.GetText(), m_parents.Peek(), 2);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_LTE_LEFT);
            this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_LTE_RIGHT);

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_EQUAL(MINICParser.Expr_EQUALContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionEqual newnode = new CASTExpressionEqual(context.GetText(), m_parents.Peek(), 2);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_EQUAL_LEFT);
            this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_EQUAL_RIGHT);

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_NEQUAL(MINICParser.Expr_NEQUALContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTExpressionNequal newnode = new CASTExpressionNequal(context.GetText(), m_parents.Peek(), 2);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(0), m_parentContext, contextType.CT_EXPRESSION_NEQUAL_LEFT);
            this.VisitElementInContext(context.expression(1), m_parentContext, contextType.CT_EXPRESSION_NEQUAL_RIGHT);

            m_parents.Pop();
            return 0;
        }
        
        public override int VisitCompoundStatement(MINICParser.CompoundStatementContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTCompoundStatement newnode = new CASTCompoundStatement(context.GetText(), m_parents.Peek(), 1);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.statementList(), m_parentContext, contextType.CT_COMPOUNDSTATEMENT);

            m_parents.Pop();
            return 0;
        }

        public override int VisitWhilestatement(MINICParser.WhilestatementContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTWhileStatement newnode = new CASTWhileStatement(context.GetText(), m_parents.Peek(), 2);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);
            this.VisitElementInContext(context.expression(), m_parentContext, contextType.CT_WHILESTATEMENT_CONDITION);
            this.VisitElementInContext(context.statement(), m_parentContext, contextType.CT_WHILESTATEMENT_BODY);

            m_parents.Pop();
            return 0;
        }

        public override int VisitIfstatement(MINICParser.IfstatementContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTIfStatement newnode = new CASTIfStatement(context.GetText(), m_parents.Peek(), 3);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);
                        
            this.VisitElementInContext(context.expression(), m_parentContext, contextType.CT_IFSTATEMENT_CONDITION);
            this.VisitElementInContext(context.statement(0), m_parentContext, contextType.CT_IFSTATEMENT_IFCLAUSE);
            if(context.statement(1)!=null)
            this.VisitElementInContext(context.statement(1), m_parentContext, contextType.CT_IFSTATEMENT_ELSECLAUSE);
            m_parents.Pop();
            return 0;
        }

       

        public override int VisitStatement_ReturnStatement(MINICParser.Statement_ReturnStatementContext context)
        {
            ASTComposite m_parent = m_parents.Peek();
            CASTReturnStatement newnode = new CASTReturnStatement(context.GetText(), m_parents.Peek(), 1);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(), m_parentContext, contextType.CT_STATEMENT_RETURN);

            m_parents.Pop();
            return 0;
        }

       
        public override int VisitStatement_ExpressionStatement(MINICParser.Statement_ExpressionStatementContext context){

            ASTComposite m_parent = m_parents.Peek();
            CASTEpxressionStatement newnode = new CASTEpxressionStatement(context.GetText(), m_parents.Peek(), 1);
            m_parent.AddChild(newnode, m_parentContext.Peek());
            m_parents.Push(newnode);

            this.VisitElementInContext(context.expression(), m_parentContext, contextType.CT_STATEMENT_EXPRESSION);
            
            m_parents.Pop();
            return 0;
            return base.VisitStatement_ExpressionStatement(context);
        }

       
        public override int VisitTerminal(ITerminalNode node)
        {
            ASTComposite m_parent = m_parents.Peek();
            switch (node.Symbol.Type)
            {
                case MINICLexer.NUMBER:
                    CASTNUMBER newnode1 = new CASTNUMBER(node.Symbol.Text,m_parents.Peek());
                    m_parent.AddChild(newnode1, m_parentContext.Peek());
                    break;
                case MINICLexer.IDENTIFIER:
                    CASTIDENTIFIER newnode2 = new CASTIDENTIFIER(node.Symbol.Text, m_parents.Peek());
                    m_parent.AddChild(newnode2, m_parentContext.Peek());
                    break;
                default:
                    break;
            }
            return base.VisitTerminal(node);
        }

    }

}
