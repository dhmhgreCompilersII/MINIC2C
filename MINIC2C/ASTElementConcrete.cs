using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_C
{
    public class CASTIDENTIFIER : ASTTerminal
    {
        public CASTIDENTIFIER(string idText, ASTElement parent) : base(idText, nodeType.NT_EXPRESSION_IDENTIFIER,
           parent){
            m_nodeName = GenerateNodeName();
        }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitIDENTIFIER(this);
        }
        public override string GenerateNodeName()
        {
            return "\"" + MNodeType + "_" + MSerial + "_(" + m_text + ")\"";
        }
    }

    public class CASTNUMBER : ASTTerminal
    {
        private int m_value;
        public int Value => m_value;

        public CASTNUMBER(string numberText,  ASTElement parent) : base(numberText, nodeType.NT_EXPRESSION_NUMBER,
            parent){
            m_value = Int32.Parse(numberText);
            m_nodeName = GenerateNodeName();
        }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitNUMBER(this);
        }
        public override string GenerateNodeName()
        {
            return "\"" + MNodeType + "_" + MSerial + "_(" + m_value + ")\"";
        }
    }

    public class CASTFunctionDefinition : ASTComposite
    {
        public CASTFunctionDefinition(String text, ASTElement parent, int numContexts) : base(text,nodeType.NT_FUNCTIOΝDEFINITION, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitFunctionDefinition(this,param);
        }

        public string GetFunctionName() {
            CASTIDENTIFIER id = GetChild(contextType.CT_FUNCTIONDEFINITION_IDENTIFIER, 0) as CASTIDENTIFIER;
            return id.M_Text;
        }

        public string[] GetFunctionArgs() {
            IEnumerable<CASTIDENTIFIER> args=m_children[GetContextIndex(contextType.CT_FUNCTIONDEFINITION_FARGS)].Select(a=>a as CASTIDENTIFIER);

            return args.Select(a => a.M_Text).ToArray();
        }
    }

    public class CASTExpressionMultiplication : ASTComposite
    {
        public CASTExpressionMultiplication(string text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_MULTIPLICATION, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitMultiplication(this,param);
        }
    }

    public class CASTExpressionDivision : ASTComposite
    {
        public CASTExpressionDivision(string text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_DIVISION, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitDivision(this, param);
        }
    }

    public class CASTExpressionAddition : ASTComposite
    {
        public CASTExpressionAddition(string text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_ADDITION, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitAddition(this, param);
        }
    }

    public class CASTExpressionSubtraction : ASTComposite
    {
        public CASTExpressionSubtraction(string text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_SUBSTRACTION, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitSubtraction(this, param);
        }
    }
    public class CASTExpressionPlus : ASTComposite
    {
        public CASTExpressionPlus(String text,  ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_PLUS, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitPLUS(this, param);
        }
    }

    public class CASTExpressionMinus : ASTComposite
    {
        public CASTExpressionMinus(String text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_MINUS, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitMINUS(this, param);
        }
    }
    
    public class CASTExpressionAssign : ASTComposite
    {
        public CASTExpressionAssign(string text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_ASSIGN, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitASSIGN(this, param);
        }
    }
    public class CASTExpressionNot : ASTComposite
    {
        public CASTExpressionNot(String text,  ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_NOT, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitNOT(this, param);
        }
    }
    public class CASTExpressionAnd : ASTComposite
    {
        public CASTExpressionAnd(String text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_AND, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitAND(this, param);
        }
    }

    public class CASTExpressionOr : ASTComposite
    {
        public CASTExpressionOr(String text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_OR, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitOR(this, param);
        }
    }
    public class CASTExpressionGt : ASTComposite
    {
        public CASTExpressionGt(String text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_GT, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitGT(this, param);
        }
    }

    public class CASTExpressionGte : ASTComposite
    {
        public CASTExpressionGte(String text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_GTE, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitGTE(this, param);
        }
    }

    public class CASTExpressionLt : ASTComposite
    {
        public CASTExpressionLt(String text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_LT, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitLT(this, param);
        }
    }

    public class CASTExpressionLte : ASTComposite
    {
        public CASTExpressionLte(String text,  ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_LTE, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitLTE(this, param);
        }
    }

    public class CASTExpressionEqual : ASTComposite
    {
        public CASTExpressionEqual(String text,  ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_EQUAL, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitEQUAL(this, param);
        }
    }

    public class CASTExpressionNequal : ASTComposite
    {
        public CASTExpressionNequal(String text, ASTElement parent, int numContexts) : base(text, nodeType.NT_EXPRESSION_NEQUAL, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitNEQUAL(this, param);
        }
    }
    
    public class CASTStatementList : ASTComposite
    {
        public CASTStatementList(String text, ASTElement parent, int numContexts) : base(text, nodeType.NT_STATEMENTLIST, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitSTATEMENTLIST(this, param);
        }
    }
    public class CASTCompoundStatement : ASTComposite
    {
        public CASTCompoundStatement(String text, ASTElement parent, int numContexts) : base(text, nodeType.NT_COMPOUNDSTATEMENT, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitCOMPOUNDSTATEMENT(this, param);
        }
    }

    public class CASTWhileStatement : ASTComposite
    {
        public CASTWhileStatement(String text, ASTElement parent, int numContexts) : base(text, nodeType.NT_WHILESTATEMENT, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitWHILESTATEMENT(this, param);
        }
    }

    public class CASTIfStatement : ASTComposite
    {
        public CASTIfStatement(String text, ASTElement parent, int numContexts) : base(text, nodeType.NT_IFSTATEMENT, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitIFSTATEMENT(this, param);
        }
    }

    public class CASTStatement : ASTComposite
    {
        public CASTStatement(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }

        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitSTATEMENT(this, param);
        }
    }

   
    public class CASTCompileUnit : ASTComposite
    {
        public CASTCompileUnit(string text,  ASTElement parent, int numContexts) : base(text, nodeType.NT_COMPILEUNIT, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitCOMPILEUNIT(this, param);
        }
    }

}
