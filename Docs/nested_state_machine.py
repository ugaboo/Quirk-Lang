def power(parser):
	def _0():
		expr = atom_expr(parser)
		if expr:
			return _1(expr)
		else:
			return None
	def _1(expr):
		if lexeme(parser.scanner) == Lexeme.POWER:
			next(parser.scanner)
			return _2(expr)
		else:
			return expr
	def _2(expr):
		right = factor(parser)
		if right:
			return _end(ast.FuncCall('__pow__', expr, right))
		else:
			raise CompilationError(ErrorType.INVALID_SYNTAX)
	return _0()
	
	
def arith_expr(parser):
	def _0():
		expr = term(parser)
		if expr:
			return _2(expr)
		else:
			return None
	def _1(name, left):
		right = term(parser)
		if right:
			return _2(ast.FuncCall(name, left, right))
		else:
			raise CompilationError(ErrorType.INVALID_SYNTAX)
	def _2(expr):
		if lexeme(parser.scanner) == Lexeme.PLUS:
			next(parser.scanner)
			return _1("__add__", expr)
		elif lexeme(parser.scanner) == Lexeme.MINUS:
			next(parser.scanner)
			return _1("__sub__", expr)
		else:
			return expr
	return _0()	

def term(parser):
	def _0():
		left = factor(parser)
		if left:
			return _2(left)
		else:
			return None
	def _1(left, name):
		right = factor(parser)
		if right:
			return _2(ast.FuncCall(name, left, right))
		else:
			raise CompilationError(ErrorType.INVALID_SYNTAX)
	def _2(left):
		if lexeme(parser.scanner) == Lexeme.STAR:
			next(parser.scanner)
			return _1(left, "__mul__")
		elif lexeme(parser.scanner) == Lexeme.SLASH:
			next(parser.scanner)
			return _1(left, "__truediv__")
		elif lexeme(parser.scanner) == Lexeme.PERCENT:
			next(parser.scanner)
			return _1(left, "__mod__")
		elif lexeme(parser.scanner) == Lexeme.DOUBLE_SLASH:
			next(parser.scanner)
			return _1(left, "__floordiv__")
		else:
			return left
	return _0()
