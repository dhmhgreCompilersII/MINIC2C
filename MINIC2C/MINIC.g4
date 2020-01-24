grammar MINIC;

/*
 * Parser Rules
 */

compileUnit : (statement|functionDefinition)+
			;

functionDefinition : FUNCTION IDENTIFIER LP fargs? RP compoundStatement					
				   ;

statement : expression QM		#statement_ExpressionStatement
		  | ifstatement			#statement_IfStatement
		  | whilestatement		#statement_WhileStatement
		  | compoundStatement	#statement_CompoundStatement
		  | RETURN expression QM #statement_ReturnStatement
		  | BREAK QM			 #statement_BreakStatement
		  ;

ifstatement : IF LP expression RP statement (ELSE statement)?
			;

whilestatement : WHILE LP expression RP statement
			   ;

compoundStatement : LB RB
				  | LB statementList RB
				  ;

statementList : (statement)+ 
			  ;

expression : NUMBER											#expr_NUMBER	
		   | IDENTIFIER										#expr_IDENTIFIER
		   | expression op=(DIV|MULT) expression 			#expr_DIVMULT   
		   | expression op=(PLUS|MINUS) expression		    #expr_PLUSMINUS
		   | PLUS expression								#expr_PLUS
		   | MINUS expression								#expr_MINUS
		   | LP expression RP								#expr_PARENTHESIS
		   | IDENTIFIER ASSIGN expression					#expr_ASSIGN
		   | NOT expression									#expr_NOT
	       | expression AND expression						#expr_AND
		   | expression OR expression						#expr_OR
		   | expression GT expression						#expr_GT
		   | expression GTE expression						#expr_GTE
		   | expression LT expression						#expr_LT
		   | expression LTE expression						#expr_LTE
		   | expression EQUAL expression					#expr_EQUAL
		   | expression NEQUAL expression					#expr_NEQUAL
		   ;

args : (expression (COMMA)?)+
	 ;

fargs : (IDENTIFIER (COMMA)?)+
	  ;

/*
 * Lexer Rules
 */

// Reserved words
FUNCTION :'function';
RETURN :'return'; 
IF:'if';
ELSE:'else';
WHILE:'while';
BREAK: 'break';

// Operators
PLUS:'+'; 
MINUS:'-';
DIV:'/'; 
MULT:'*';
OR:'||';
AND:'&&';
NOT:'!';
EQUAL:'==';
NEQUAL:'!='; 
GT:'>';
LT:'<';
GTE:'>=';
LTE:'<=';
QM:';';
LP:'(';
RP:')';
LB:'{';
RB:'}'; 
COMMA:',';
ASSIGN:'=';

// Identifiers - Numbers
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;
NUMBER: '0'|[1-9][0-9]*;

// Whitespace
WS: [ \r\n\t]-> skip;
