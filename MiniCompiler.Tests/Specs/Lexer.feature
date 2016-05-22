Feature: ReservedWords
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Input is Empty
	Given I have an input of ''
	When We Tokenize
	Then the result should be 
		| Type		| Lexeme   | Column | Row |
		| Eof	    | $	       |   0    | 0   |

Scenario: Input is an id equal to haloword
	Given I have an input of 'haloworld'
	When We Tokenize
	Then the result should be 
		| Type		| Lexeme   | Column | Row |
		| Id	    | haloworld|   0    | 0   |
		| Eof	    | $	       |   9    | 0   |
	
Scenario: Input is 2 ids with whitespaces
	Given I have an input of 'halo warudo'
	When We Tokenize
	Then the result should be 
		| Type		| Lexeme   | Column | Row |
		| Id	    | halo     |   0    | 0   |
		| Id	    | warudo   |   5    | 0   |
		| Eof	    | $	       |   11   | 0   |

Scenario: Input is an int equal to 2042
	Given I have an input of '2042'
	When We Tokenize
	Then the result should be 
		| Type		| Lexeme   | Column | Row |
		| NumericLiteral	| 2042     |   0    | 0   |
		| Eof	    | $	       |   4    | 0   |

Scenario: Input is an int and an a 2042
	Given I have an input of '2042a'
	When We Tokenize
	Then the result should be 
		| Type		| Lexeme   | Column | Row |
		| NumericLiteral	| 2042     |   0    | 0   |
		| Id		| a		   |   4    | 0   |
		| Eof	    | $	       |   5    | 0   |

Scenario: Input is an id a2042
	Given I have an input of 'a2042'
	When We Tokenize
	Then the result should be 
		| Type		| Lexeme   | Column | Row |
		| Id		| a2042	   |   0    | 0   |
		| Eof	    | $	       |   5    | 0   |

 Scenario: Input is an Addition operator
	Given I have an input of '+'
	When We Tokenize
	Then the result should be 
		| Type		| Lexeme   | Column | Row |
		| SumOp		| + 	   |   0    | 0   |
		| Eof	    | $	       |   1  | 0   |
 
 Scenario: Input is an integer reserverd Word
	Given I have an input of 'integer'
	When We Tokenize
	Then the result should be 
		| Type		| Lexeme   | Column | Row |
		| RwInteger	| integer  |   0    | 0   |
		| Eof	    |   $	   |   7    | 0  |

 Scenario: Input is an Hexadecimal opertaorSum and Id
	Given I have an input of '$AB + b'
	When We Tokenize
	Then the result should be 
		| Type		| Lexeme   | Column | Row |
		| Hexa	    | $AB  |   0    | 0   |
		| SumOp	    |   +  |   4    | 0  |
		| Id	    |   b  |   6   | 0  |
		| Eof	    |   $  |   7    | 0  |

    Scenario: Input is an Binary
	Given I have an input of '%101010'
	When We Tokenize
	Then the result should be 
		| Type		| Lexeme   | Column | Row |
		| Binary	| %101010  |   0    | 0   |
		| Eof	    |   $      |   7    | 0  |

  Scenario: Input is an Range
    Given I have an input of '[1 .. 10] .'
    When We Tokenize
    Then the result should be
        | Type                | Lexeme      | Column | Row |
        | OpenBracketOperator | [           |   0    | 0   |
        | NumericLiteral      | 1           |   1    | 0   |
        | RangeOp             | ..          |   3    | 0   |
        | NumericLiteral      | 10          |   6    | 0   |
        | CloseBracketOperator| ]           |   8    | 0   |
        | AccesOp             | .           |   10    | 0   |
        | Eof                 | $           |   11    | 0   |
   Scenario: Input is an Symbol OpGreaterThanEqual
	Given I have an input of ' >= '
	When We Tokenize
	Then the result should be 
		| Type		| Lexeme   | Column | Row |
		| GreaterThanOrEqualOp	|   >=  |   1    | 0   |

	Scenario: Input is an operation with sentence end
    Given I have an input of 'temp := 23 + variable/(5*8) -2;'
    When We Tokenize
    Then the result should be
        | Type                      | Lexeme  | Column | Row |
        | Id                        | temp    |   0    | 0   |
        | AsiggnationOp             | :=      |   5    | 0   |
        | NumericLiteral            | 23      |   8    | 0   |
        | SumOp                     | +       |   11   | 0   |
        | Id                        | variable|   13   | 0   |
        | DivOp                     | /       |   21   | 0   |
        | SbLeftParent              | (       |   22   | 0   |
        | NumericLiteral            | 5       |   23   | 0   |
        | MultOp                    | *       |   24   | 0   |
        | NumericLiteral            | 8       |   25   | 0   |
        | SbRightParent             | )       |   26   | 0   |
        | SubOp                     | -       |   28   | 0   |
        | NumericLiteral            | 2       |   29   | 0   |
        | Eos                        | ;       |   30   | 0   |
        | Eof                       | $       |   31   | 0   |

 Scenario: Input is an String Literal
    Given I have an input of '"test string"'
    When We Tokenize
    Then the result should be
        | Type            | Lexeme       | Column | Row |
        | StringLiteral   | "test string"|   0    | 0   |
        | Eof             | $            |   13   | 0   |	
Scenario: Input is many reserved words
    Given I have an input of 'or for while and integer'
    When We Tokenize
    Then the result should be
        | Type      | Lexeme  | Column | Row |
        | RwOr      | or      |   0    | 0   |
        | RwFor     | for     |   3    | 0   |
        | RwWhile   | while   |   7    | 0   |
        | RwAnd     | and     |   13   | 0   |
        | RwInteger     | integer |   17   | 0   |
        | Eof       | $       |   24   | 0   |

Scenario: Input is a char literal
    Given I have an input of 'days, age = integer;'
    When We Tokenize
    Then the result should be
        | Type               | Lexeme  | Column | Row |
        | Id                 | days    |   0    | 0   |
        | CommaOperator      | ,       |   4    | 0   |
        | Id                 | age     |   6   | 0   |
        | EqualOp            | =       |   10    | 0   |
        | RwInteger          | integer |   12    | 0   |
        | Eos                | ;       |   19   | 0   |
        | Eof                | $       |   20    | 0   |
Scenario: Input is a charPound
    Given I have an input of '#64'
    When We Tokenize
    Then the result should be
        | Type         | Lexeme  | Column | Row |
        | CharPound    | #64     |   0    | 0   |
        | Eof          | $       |   3    | 0   |
Scenario: Input is logical operators
    Given I have an input of '< <= <> >= > ='
    When We Tokenize
    Then the result should be
        | Type                | Lexeme  | Column | Row |
        | LessThanOp          | <       |   0    | 0   |
        | LessThanOrEqualOp   | <=      |   2    | 0   |
        | UnequalOp           | <>      |   5    | 0   |
        | GreaterThanOrEqualOp| >=      |   8    | 0   |
        | GreaterOp           | >       |   11   | 0   |
        | EqualOp             | =       |   13   | 0   |
        | Eof                 | $       |   14   | 0   |

Scenario: Input is id and a block comment
    Given I have an input of 'test { comentario } test2'
    When We Tokenize
    Then the result should be
        | Type  | Lexeme  | Column | Row |
        | Id    | test    |   0    | 0   |
        | Id    | test2   |   20   | 0   |
        | Eof   | $       |   25   | 0   |



Scenario: Input is id and a single line comment
    Given I have an input of 'test //comentario test2'
    When We Tokenize
    Then the result should be
        | Type  | Lexeme  | Column | Row |
        | Id    | test    |   0    | 0   |
        | Eof   | $       |   23   | 0   |

	Scenario: Input is a Double
    Given I have an input of '2.33'
    When We Tokenize
    Then the result should be
        | Type  | Lexeme  | Column | Row |
        | NumericLiteral    | 2.33    |   0    | 0   |
        | Eof   | $       |    4  | 0   |