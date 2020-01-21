

using System.Linq;

namespace Mini_C {

    public class TranslationParameters {
        private CComboContainer m_parent=null;
        private CodeContextType m_contextType=CodeContextType.CC_NA;
        private CCFunctionDefinition m_containerFunction=null;

        public CComboContainer M_Parent {
            get => m_parent;
            set => m_parent = value;
        }

        public CCFunctionDefinition M_ContainerFunction {
            get => m_containerFunction;
            set => m_containerFunction = value;
        }

        public CodeContextType M_ContextType {
            get => m_contextType;
            set => m_contextType = value;
        }
    }

    class MINIC2CTranslation : ASTBaseVisitor<CEmmitableCodeContainer,TranslationParameters> {
        private CCFile m_translatedFile;

        public CCFile M_TranslatedFile => m_translatedFile;

        public override CEmmitableCodeContainer VisitFunctionDefinition(CASTFunctionDefinition node,TranslationParameters param) {

            //1. Create Output File
            CCFunctionDefinition fundef = new CCFunctionDefinition(param.M_Parent);
            
            //2. Add Function Definition to the File in the appropriate context
            param.M_Parent.AddCode(fundef,param.M_ContextType);

            //3. Assemble the function header
            CASTIDENTIFIER id = node.GetChild(contextType.CT_FUNCTIONDEFINITION_IDENTIFIER, 0) as CASTIDENTIFIER;

            CEmmitableCodeContainer header = fundef.GetHeader();
            header.AddCode("float "+id.M_Text +"(");
            string last = node.GetFunctionArgs().Last();
            foreach (string s in node.GetFunctionArgs()) {
                header.AddCode("float "+s);
                if (!s.Equals(last)) {
                    header.AddCode(", ");
                }
                fundef.AddVariableToLocalSymbolTable(s);
            }
            header.AddCode(")");

            VisitContext(node, contextType.CT_FUNCTIONDEFINITION_FARGS, new TranslationParameters() {
                M_Parent = fundef,
                M_ContextType = CodeContextType.CC_FUNCTIONDEFINITION_HEADER,
                M_ContainerFunction = fundef
            });

            VisitContext(node, contextType.CT_FUNCTIONDEFINITION_BODY, new TranslationParameters() {
                M_Parent = fundef,
                M_ContextType = CodeContextType.CC_FUNCTIONDEFINITION_BODY,
                M_ContainerFunction = fundef
            });

            return fundef;
        }

        public override CEmmitableCodeContainer VisitCOMPILEUNIT(CASTCompileUnit node, TranslationParameters param) {

            //1. Create Output File
            m_translatedFile = new CCFile();

            //2. Visit CT_COMPILEUNIT_MAINBODY and create main function
            CMainFunctionDefinition mainf = new CMainFunctionDefinition(m_translatedFile);
            m_translatedFile.AddCode(mainf,CodeContextType.CC_FILE_FUNDEF);

            VisitContext(node, contextType.CT_COMPILEUNIT_MAINBODY,
                new TranslationParameters() {
                    M_Parent = mainf ,
                    M_ContextType = CodeContextType.CC_FUNCTIONDEFINITION_BODY,
                    M_ContainerFunction = mainf

                });

            // 3. Visit CT_COMPILEUNIT_FUNCTIONDEFINITIONS
            VisitContext(node, contextType.CT_COMPILEUNIT_FUNCTIONDEFINITIONS,
                new TranslationParameters() {
                    M_Parent = m_translatedFile,
                    M_ContextType = CodeContextType.CC_FILE_FUNDEF
                });

            //3. Visit CT

            return m_translatedFile;
        }

        public override CEmmitableCodeContainer VisitASSIGN(CASTExpressionAssign node, TranslationParameters param = default(TranslationParameters)) {
            CCFunctionDefinition fun = param.M_Parent as CCFunctionDefinition;

            CEmmitableCodeContainer rep = fun.GetBody();

            CASTIDENTIFIER id = node.GetChild(contextType.CT_EXPRESSION_ASSIGN_LVALUE, 0) as CASTIDENTIFIER;
            fun.DeclareVariable(id.M_Text);
            rep.AddCode(id.M_Text);
            rep.AddCode("=");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_ASSIGN_EXPRESSION,0),param).AssemblyCodeContainer());
            rep.AddCode(";");
            rep.AddNewLine();
            return rep;
        }

        public override CEmmitableCodeContainer VisitAddition(CASTExpressionAddition node,
            TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY,param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_ADDITION_LEFT, 0),param).AssemblyCodeContainer());
            rep.AddCode("+");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_ADDITION_RIGHT, 0),param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitMultiplication(CASTExpressionMultiplication node,
            TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_MULTIPLICATION_LEFT, 0), param).AssemblyCodeContainer());
            rep.AddCode("*");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_MULTIPLICATION_RIGHT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitDivision(CASTExpressionDivision node,
            TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_DIVISION_LEFT, 0), param).AssemblyCodeContainer());
            rep.AddCode("/");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_DIVISION_RIGHT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitSubtraction(CASTExpressionSubtraction node,
            TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_SUBTRACTION_LEFT, 0), param).AssemblyCodeContainer());
            rep.AddCode("-");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_SUBTRACTION_RIGHT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitParenthesis(CASTExpressionInParenthesis node,
            TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode("(");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_PARENTHESIS, 0), param).AssemblyCodeContainer());
            rep.AddCode(")");
            return rep;
        }

        public override CEmmitableCodeContainer VisitPLUS(CASTExpressionPlus node,
            TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode("+");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_PLUS, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitMINUS(CASTExpressionMinus node,
            TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode("-");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_MINUS, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitNOT(CASTExpressionNot node, TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode("!");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_NOT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitAND(CASTExpressionAnd node, TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_AND_LEFT, 0), param).AssemblyCodeContainer());
            rep.AddCode("&&");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_AND_RIGHT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitOR(CASTExpressionOr node, TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_OR_LEFT, 0), param).AssemblyCodeContainer());
            rep.AddCode("||");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_OR_RIGHT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitGT(CASTExpressionGt node, TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_GT_LEFT, 0), param).AssemblyCodeContainer());
            rep.AddCode(">");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_GT_RIGHT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitGTE(CASTExpressionGte node, TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_GTE_LEFT, 0), param).AssemblyCodeContainer());
            rep.AddCode(">=");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_GTE_RIGHT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitLT(CASTExpressionLt node, TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_LT_LEFT, 0), param).AssemblyCodeContainer());
            rep.AddCode("<");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_LT_RIGHT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitLTE(CASTExpressionLte node, TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_LTE_LEFT, 0), param).AssemblyCodeContainer());
            rep.AddCode("<=");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_LTE_RIGHT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitEQUAL(CASTExpressionEqual node,
            TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_EQUAL_LEFT, 0), param).AssemblyCodeContainer());
            rep.AddCode("==");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_EQUAL_RIGHT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitNEQUAL(CASTExpressionNequal node,
            TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, param.M_Parent);
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_NEQUAL_LEFT, 0), param).AssemblyCodeContainer());
            rep.AddCode("!=");
            rep.AddCode(Visit(node.GetChild(contextType.CT_EXPRESSION_NEQUAL_RIGHT, 0), param).AssemblyCodeContainer());
            return rep;
        }

        public override CEmmitableCodeContainer VisitIDENTIFIER(CASTIDENTIFIER node,
            TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, null);
            param.M_ContainerFunction.DeclareVariable(node.M_Text);
            rep.AddCode(node.M_Text);
            return rep;
        }

        public override CEmmitableCodeContainer VisitNUMBER(CASTNUMBER node, TranslationParameters param = default(TranslationParameters)) {
            CodeContainer rep = new CodeContainer(CodeBlockType.CB_CODEREPOSITORY, null);
            rep.AddCode(node.M_Text);
            return rep;
        }
    }
}