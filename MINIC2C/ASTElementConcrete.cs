using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_C
{
    public class CASTIDENTIFIER : ASTComposite
    {
        public CASTIDENTIFIER(string idText, nodeType type, ASTElement parent, int numContexts) : base(idText, type,
           parent, numContexts)
        {
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

    public class CASTNUMBER : ASTComposite
    {
        private int m_value;
        public int Value => m_value;

        public CASTNUMBER(string numberText, nodeType type, ASTElement parent, int numContexts) : base(numberText, type,
            parent, numContexts)
        {
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
        public CASTFunctionDefinition(String text, nodeType type, ASTElement parent, int numContexts) : base(text,type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitFunctionDefinition(this,param);
        }
    }

    public class CASTExpressionMultiplication : ASTComposite
    {
        public CASTExpressionMultiplication(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitMultiplication(this,param);
        }
    }

    public class CASTExpressionDivision : ASTComposite
    {
        public CASTExpressionDivision(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitDivision(this, param);
        }
    }

    public class CASTExpressionAddition : ASTComposite
    {
        public CASTExpressionAddition(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitAddition(this, param);
        }
    }

    public class CASTExpressionSubtraction : ASTComposite
    {
        public CASTExpressionSubtraction(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitSubtraction(this, param);
        }
    }
    public class CASTExpressionPlus : ASTComposite
    {
        public CASTExpressionPlus(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitPLUS(this, param);
        }
    }

    public class CASTExpressionMinus : ASTComposite
    {
        public CASTExpressionMinus(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitMINUS(this, param);
        }
    }
    
    public class CASTExpressionAssign : ASTComposite
    {
        public CASTExpressionAssign(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitASSIGN(this, param);
        }
    }
    public class CASTExpressionNot : ASTComposite
    {
        public CASTExpressionNot(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitNOT(this, param);
        }
    }
    public class CASTExpressionAnd : ASTComposite
    {
        public CASTExpressionAnd(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitAND(this, param);
        }
    }

    public class CASTExpressionOr : ASTComposite
    {
        public CASTExpressionOr(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitOR(this, param);
        }
    }
    public class CASTExpressionGt : ASTComposite
    {
        public CASTExpressionGt(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitGT(this, param);
        }
    }

    public class CASTExpressionGte : ASTComposite
    {
        public CASTExpressionGte(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitGTE(this, param);
        }
    }

    public class CASTExpressionLt : ASTComposite
    {
        public CASTExpressionLt(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitLT(this, param);
        }
    }

    public class CASTExpressionLte : ASTComposite
    {
        public CASTExpressionLte(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitLTE(this, param);
        }
    }

    public class CASTExpressionEqual : ASTComposite
    {
        public CASTExpressionEqual(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitEQUAL(this, param);
        }
    }

    public class CASTExpressionNequal : ASTComposite
    {
        public CASTExpressionNequal(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitNEQUAL(this, param);
        }
    }
    
    public class CASTStatementList : ASTComposite
    {
        public CASTStatementList(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitSTATEMENTLIST(this, param);
        }
    }
    public class CASTCompoundStatement : ASTComposite
    {
        public CASTCompoundStatement(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitCOMPOUNDSTATEMENT(this, param);
        }
    }

    public class CASTWhileStatement : ASTComposite
    {
        public CASTWhileStatement(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitWHILESTATEMENT(this, param);
        }
    }

    public class CASTIfStatement : ASTComposite
    {
        public CASTIfStatement(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
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
        public CASTCompileUnit(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor,VParam param)
        {
            return visitor.VisitCOMPILEUNIT(this, param);
        }
    }

}
