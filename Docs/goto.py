def power(parser):
	$0:
		expr = atom_expr(parser)
		if expr:
			goto $1
		return False
	$1:
		if lexeme(parser) == Lexeme.POWER:
			next_lexeme(parser)
			goto $2
		goto $end
	$2:
		right = factor(parser)
		if right:
			expr = ast.FuncCall('__pow__', expr, right)
			goto $end
		raise CompilationError(ErrorType.INVALID_SYNTAX)
	$end:
		return expr


def power(parser):
	def _0():
		expr = atom_expr(parser)
		if expr != False:
			return _1(expr)
		return False
	def _1(expr):
		if lexeme(parser.scanner) == Lexeme.POWER:
			next(parser.scanner)
			return _2(expr)
		return _end(expr)
	def _2(expr):
		right = factor(parser)
		if right != False:
			expr = ast.FuncCall('__pow__', expr, right)
			return _end(expr)
		raise CompilationError(ErrorType.INVALID_SYNTAX)
	def _end(expr):
		return expr
		
	return _0()
	
	
def arith_expr(parser):
	def _0():
		expr = term(parser)
		if left != False:
			return _2(expr)
		return False
	def _1(name, left):
		right = term(parser)
		if right != False:
			return _2(ast.FuncCall(name, left, right));
		raise CompilationError(ErrorType.INVALID_SYNTAX);
	def _2(expr):
		if lexeme(parser.scanner) == Lexeme.PLUS:
			next(parser.scanner)
			return _1("__add__", expr)
		elif lexeme(parser.scanner) == Lexeme.MINUS:
			next(parser.scanner)
			return _1("__sub__", expr);
		else:
			return expr;
	return _0()