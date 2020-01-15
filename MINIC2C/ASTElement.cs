using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_C
{
    public enum nodeType
    {
        NA = -1,
        NT_COMPILEUNIT=0,
        NT_FUNCTIOΝDEFINITION=2,
        NT_STATEMENT=5,
        NT_IFSTATEMENT=6,
        NT_WHILESTATEMENT=9,
        NT_COMPOUNDSTATEMENT=11,
        NT_STATEMENTLIST=12,
        NT_EXPRESSION_NUMBER=13,
        NT_EXPRESSION_IDENTIFIER=14,
        NT_EXPRESSION_DIVISION = 15,
        NT_EXPRESSION_MULTIPLICATION =17,
        NT_EXPRESSION_ADDITION =19,
        NT_EXPRESSION_SUBSTRACTION = 21,
        NT_EXPRESSION_PLUS =23,
        NT_EXPRESSION_MINUS=24,
        NT_EXPRESSION_ASSIGN=25,
        NT_EXPRESSION_NOT=27,
        NT_EXPRESSION_AND=28,
        NT_EXPRESSION_OR=30,
        NT_EXPRESSION_GT=32,
        NT_EXPRESSION_GTE=34,
        NT_EXPRESSION_LT=36,
        NT_EXPRESSION_LTE=38,
        NT_EXPRESSION_EQUAL=40,
        NT_EXPRESSION_NEQUAL=42,
    };

    public enum contextType
    {
        NA = -1,
        CT_COMPILEUNIT_MAINBODY,
        CT_COMPILEUNIT_FUNCTIONDEFINITIONS,
        CT_FUNCTIONDEFINITION_IDENTIFIER,
        CT_FUNCTIONDEFINITION_FARGS,
        CT_FUNCTIONDEFINITION_BODY,
        CT_STATEMENT,
        CT_IFSTATEMENT_CONDITION,
        CT_IFSTATEMENT_IFCLAUSE,
        CT_IFSTATEMENT_ELSECLAUSE,
        CT_WHILESTATEMENT_CONDITION,
        CT_WHILESTATEMENT_BODY,
        CT_COMPOUNDSTATEMENT,
        CT_STATEMENTLIST,
        CT_EXPRESSION_NUMBER,
        CT_EXPRESSION_IDENTIFIER,
        CT_EXPRESSION_DIVISION_LEFT,
        CT_EXPRESSION_DIVISION_RIGHT,
        CT_EXPRESSION_MULTIPLICATION_LEFT,
        CT_EXPRESSION_MULTIPLICATION_RIGHT,
        CT_EXPRESSION_ADDITION_LEFT,
        CT_EXPRESSION_ADDITION_RIGHT,
        CT_EXPRESSION_SUBTRACTION_LEFT,
        CT_EXPRESSION_SUBTRACTION_RIGHT,
        CT_EXPRESSION_PLUS,
        CT_EXPRESSION_MINUS,
        CT_EXPRESSION_ASSIGN_LVALUE,
        CT_EXPRESSION_ASSIGN_EXPRESSION,
        CT_EXPRESSION_NOT,
        CT_EXPRESSION_AND_LEFT,
        CT_EXPRESSION_AND_RIGHT,
        CT_EXPRESSION_OR_LEFT,
        CT_EXPRESSION_OR_RIGHT,
        CT_EXPRESSION_GT_LEFT,
        CT_EXPRESSION_GT_RIGHT,
        CT_EXPRESSION_GTE_LEFT,
        CT_EXPRESSION_GTE_RIGHT,
        CT_EXPRESSION_LT_LEFT,
        CT_EXPRESSION_LT_RIGHT,
        CT_EXPRESSION_LTE_LEFT,
        CT_EXPRESSION_LTE_RIGHT,
        CT_EXPRESSION_EQUAL_LEFT,
        CT_EXPRESSION_EQUAL_RIGHT,
        CT_EXPRESSION_NEQUAL_LEFT,
        CT_EXPRESSION_NEQUAL_RIGHT,
    }

    public abstract class ASTElement
    {
        private int m_serial;
        private static int ms_serialCounter = 0;
        private nodeType m_nodeType;
        private ASTElement m_parent;
        protected string m_nodeName;
        protected string m_text;

        public nodeType MNodeType => m_nodeType;

        public virtual string GenerateNodeName()
        {
            return "\"" + m_nodeType + "_" + m_serial + "\"";
        }

        public abstract Result Accept<Result,VParam>(ASTBaseVisitor<Result,VParam> visitor, VParam param);

        public ASTElement MParent
        {
            get { return m_parent; }
        }

        public string MNodeName => m_nodeName;
        public int MSerial => m_serial;

        protected ASTElement(string text, nodeType type, ASTElement parent)
        {
            m_nodeType = type;
            m_parent = parent;
            m_serial = ms_serialCounter++;
            m_text = text;
        }
    }
        public abstract class ASTComposite : ASTElement
    {
        private List<ASTElement>[] m_children;

        public List<ASTElement>[] MChildren => m_children;

        protected ASTComposite(string text, nodeType type, ASTElement parent, int numContexts) : base(text, type, parent)
        {
            m_children = new List<ASTElement>[numContexts];
            for (int i = 0; i < numContexts; i++)
            {
                m_children[i] = new List<ASTElement>();
            }
            m_nodeName = GenerateNodeName();
        }

        internal int GetContextIndex(contextType ct)
        {
            int index;
            index = (int)ct - (int)MNodeType;
            return index;
        }

        internal void AddChild(ASTElement child, contextType ct)
        {
            int index = GetContextIndex(ct);
            m_children[index].Add(child);
        }

        internal ASTElement GetChild(contextType ct, int index)
        {
            int i = GetContextIndex(ct);
            return m_children[i][index];
        }
    }

    public abstract class ASTTerminal : ASTElement {
        private string m_text;
        public string M_Text => base.m_text;

        protected ASTTerminal(string text, nodeType type, ASTElement parent) : base(text, type, parent) {
            m_text = text;
        }

    }
}
