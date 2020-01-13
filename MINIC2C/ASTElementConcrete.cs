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
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
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
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
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
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitFunctionDefinition(this);
        }
    }

    public class CASTExpressionMultiplication : ASTComposite
    {
        public CASTExpressionMultiplication(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitMultiplication(this);
        }
    }

    public class CASTExpressionDivision : ASTComposite
    {
        public CASTExpressionDivision(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitDivision(this);
        }
    }

    public class CASTExpressionAddition : ASTComposite
    {
        public CASTExpressionAddition(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitAddition(this);
        }
    }

    public class CASTExpressionSubtraction : ASTComposite
    {
        public CASTExpressionSubtraction(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitSubtraction(this);
        }
    }
    public class CASTExpressionPlus : ASTComposite
    {
        public CASTExpressionPlus(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitPLUS(this);
        }
    }

    public class CASTExpressionMinus : ASTComposite
    {
        public CASTExpressionMinus(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitMINUS(this);
        }
    }
    
    public class CASTExpressionAssign : ASTComposite
    {
        public CASTExpressionAssign(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitASSIGN(this);
        }
    }
    public class CASTExpressionNot : ASTComposite
    {
        public CASTExpressionNot(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitNOT(this);
        }
    }
    public class CASTExpressionAnd : ASTComposite
    {
        public CASTExpressionAnd(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitAND(this);
        }
    }

    public class CASTExpressionOr : ASTComposite
    {
        public CASTExpressionOr(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitOR(this);
        }
    }
    public class CASTExpressionGt : ASTComposite
    {
        public CASTExpressionGt(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitGT(this);
        }
    }

    public class CASTExpressionGte : ASTComposite
    {
        public CASTExpressionGte(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitGTE(this);
        }
    }

    public class CASTExpressionLt : ASTComposite
    {
        public CASTExpressionLt(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitLT(this);
        }
    }

    public class CASTExpressionLte : ASTComposite
    {
        public CASTExpressionLte(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitLTE(this);
        }
    }

    public class CASTExpressionEqual : ASTComposite
    {
        public CASTExpressionEqual(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitEQUAL(this);
        }
    }

    public class CASTExpressionNequal : ASTComposite
    {
        public CASTExpressionNequal(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitNEQUAL(this);
        }
    }
    
    public class CASTStatementList : ASTComposite
    {
        public CASTStatementList(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitSTATEMENTLIST(this);
        }
    }
    public class CASTCompoundStatement : ASTComposite
    {
        public CASTCompoundStatement(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitCOMPOUNDSTATEMENT(this);
        }
    }

    public class CASTWhileStatement : ASTComposite
    {
        public CASTWhileStatement(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitWHILESTATEMENT(this);
        }
    }

    public class CASTIfStatement : ASTComposite
    {
        public CASTIfStatement(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitIFSTATEMENT(this);
        }
    }

    public class CASTStatement : ASTComposite
    {
        public CASTStatement(String text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitSTATEMENT(this);
        }
    }

   
    public class CASTCompileUnit : ASTComposite
    {
        public CASTCompileUnit(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent, numContexts) { }
        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            return visitor.VisitCOMPILEUNIT(this);
        }
    }

}
