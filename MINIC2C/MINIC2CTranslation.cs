

namespace Mini_C {

    public struct TranslationParameters {
        private CComboContainer m_parent;

        public CComboContainer M_Parent {
            get => m_parent;
            set => m_parent = value;
        }
    }

    class MINIC2CTranslation : ASTBaseVisitor<int,TranslationParameters> {
        private CCFile m_translatedFile;

        public CCFile M_TranslatedFile => m_translatedFile;

        public override int VisitFunctionDefinition(CASTFunctionDefinition node,TranslationParameters param) {

            //1. Create Output File
            CCFunctionDefinition fundef = new CCFunctionDefinition(param.M_Parent);
            
            //2. Add Function Definition to the File in the appropriate context
            param.M_Parent.AddCode(fundef,CodeContextType.CC_FILE_FUNDEF);



            //CT_FUNCTION_IDENTIFIER,
            //CT_FUNCTION_FARGS,
            //CT_FUNCTION_BODY,
            return 0;
        }

        public override int VisitCOMPILEUNIT(CASTCompileUnit node, TranslationParameters param) {

            //1. Create Output File
            m_translatedFile = new CCFile();

            //2. Visit CT_COMPILEUNIT_MAINBODY and create main function
            CMainFunctionDefinition mainf = new CMainFunctionDefinition(m_translatedFile);
            m_translatedFile.AddCode(mainf,CodeContextType.CC_FILE_FUNDEF);

            VisitContext(node, contextType.CT_COMPILEUNIT_MAINBODY,
                new TranslationParameters() {M_Parent = mainf });

            // 3. Visit CT_COMPILEUNIT_FUNCTIONDEFINITIONS
            VisitContext(node, contextType.CT_COMPILEUNIT_FUNCTIONDEFINITIONS,
                new TranslationParameters() { M_Parent = m_translatedFile });

            //3. Visit CT

            

            return 0;
        }
    }
}