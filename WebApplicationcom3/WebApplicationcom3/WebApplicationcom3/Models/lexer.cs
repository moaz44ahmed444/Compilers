namespace WebApplicationcom3.Models
{
    class lexer
    {
        private readonly string text;
        private int position;
        public List<token> Tokens = new List<token>();
        private int line;
        private char currentChar
        {
            get
            {
                if (position >= text.Length)
                {
                    return '\0';
                }
                return text[position];
            }
        }
        private void Next()
        {
            position++;
        }
        public lexer(string text)
        {
            this.text = text;
            this.line = 1;
        }
        public List<token> lex()
        {
            while (currentChar != '\0')
            {
                while (currentChar == ' ' || currentChar == '\r')
                    Next();
                if (currentChar == '/')
                {
                    string TokenText = currentChar+"";
                    Next();
                    if(currentChar == '$')
                    {
                        TokenText += currentChar;
                        Next();
                        while (currentChar != '\0')
                        {
                            TokenText += currentChar;
                            if(currentChar == '\n')
                                line++;
                            if (currentChar == '$')
                            {
                                Next();
                                if (currentChar == '/')
                                {
                                    TokenText += currentChar;
                                    break;
                                }
                            }
                            Next();
                        }
                        if(currentChar == '\0')
                        {
                            Tokens.Add(new token(tokenType.BadToken, line, TokenText, null));
                        }

                    }
                }
                else if (currentChar == '$')
                {
                    string TokenText = currentChar + "";
                    Next();
                    if(currentChar == '$')
                    {
                        TokenText += currentChar;
                        Next();
                        if(currentChar == '$')
                        {
                            TokenText += currentChar;
                                while (currentChar != '\n' && currentChar != '\0')
                                {
                                    Next();
                                }
                                line++;
                        }
                        else
                            Tokens.Add(new token(tokenType.BadToken, line, TokenText, null));
                    }
                    else
                        Tokens.Add(new token(tokenType.BadToken, line, TokenText+currentChar, null));
                }
                else if (currentChar == '\n')
                {
                    line++;
                }
                else if(currentChar == ';')
                {
                    Next();
                    continue;
                }
                else
                {
                    NextToken();
                }
                Next();
            }
            return Tokens;
        }
        private bool isLetter(char c)
        {
            if((c >= 'a' && c <='z') ||(c >= 'A' && c <='Z') || c == '_')
                return true;
            return false;
        }
        private bool isDigit(char c)
        {
            if(c >= '0' && c <= '9')
                return true;
            return false;
        }
        private bool isSymbol(char c)
        {
            string symbol = "[]{}()+-/*<>!~&|,=";
            foreach(char ch in symbol)
            {
                if(ch == c)
                    return true;
            }
            return false;
        }
        public void NextToken()
        {
            if (isDigit(currentChar))
            {
                ReadDigitToken();


            }
            else if (isLetter(currentChar))
            {
                RealIdentifierToken();

            }
            else if (isSymbol(currentChar))
            {

                ReadSymbolToken();

            }
            else
                Tokens.Add(new token(tokenType.BadToken, line, currentChar+"", null));
        }

        private token checkID(string tokenText)
        {
            token t;
            switch (tokenText)
            {
                case "Iow":
                    t =  new token(tokenType.Integer, line, tokenText, null);
                    break;
                case "SIow":
                    t = new token(tokenType.SInteger, line, tokenText, null);
                    break;
                case "Iowf":
                    t = new token(tokenType.Float, line, tokenText, null);
                    break;
                case "SIowf":
                    t = new token(tokenType.SFloat, line, tokenText, null);
                    break;
                case "If":
                    t = new token(tokenType.Condition, line, tokenText, null);
                    break;
                case "Else":
                    t = new token(tokenType.Condition, line, tokenText, null);
                    break;
                case "Chlo":
                    t = new token(tokenType.Character, line, tokenText, null);
                    break;
                case "Chain":
                    t = new token(tokenType.String, line, tokenText, null);
                    break;
                case "Worthless":
                    t = new token(tokenType.Void, line, tokenText, null);
                    break;
                case "Loopwhen":
                    t = new token(tokenType.Loop, line, tokenText, null);
                    break;
                case "Iteratewhen":
                    t = new token(tokenType.Loop, line, tokenText, null);
                    break;
                case "TurnBack":
                    t = new token(tokenType.Return, line, tokenText, null);
                    break;
                case "Stop":
                    t = new token(tokenType.Break, line, tokenText, null);
                    break;
                case "Loli":
                    t = new token(tokenType.Struct, line, tokenText, null);
                    break;
                case "Include":
                    t = new token(tokenType.Inclusion, line, tokenText, null);
                    break;
                    default:
                    t = new token(tokenType.Identifier, line, tokenText, null);
                    break;
            }
            return t;
        }
        
        private token SymbolToken(string tokenText)
        {
            int state = 0;
            token t = null;
            bool done = false;
            while (!done)
            {
                switch (state)
                {
                    case 0:
                        if (tokenText == "+" || tokenText == "*" || tokenText == "/")
                            state = 1;
                        else if (tokenText == "{" || tokenText == "}" || tokenText == "[" || tokenText == "]" || tokenText == "(" || tokenText == ")")
                            state = 2;
                        else if (tokenText == "<" || tokenText == ">")
                            state = 3;
                        else if (tokenText == "!")
                            state = 5;
                        else if (tokenText == "~")
                            state = 6;
                        else if (tokenText == "&")
                            state = 7;
                        else if (tokenText == "|")
                            state = 9;
                        else if (tokenText == "-")
                        {
                            Next();
                            if (currentChar == '>')
                            {
                                tokenText += '>';
                                state = 11;
                            }
                            if (isDigit(currentChar) || isLetter(currentChar) || currentChar == ' ' || currentChar == '\0')
                            {
                                position--;
                                state = 1;
                            }
                        }
                        else if (tokenText == ",")
                            state = 12;
                        else if (tokenText == "=")
                        {
                            Next();
                            if (currentChar == '=')
                            {
                                tokenText += '=';
                                state = 4;
                            }
                            else if (isDigit(currentChar) || isLetter(currentChar) || currentChar == ' ' || currentChar == '\0')
                            {
                                position--;
                                state = 13;
                            }
                            else
                                state = 14;
                        }
                        else
                            state = 14;
                        break;
                    case 1:
                        done = true;
                        t = new token(tokenType.AtritmerticOperation, line, tokenText + "", null);
                        break;
                    case 2:
                        done = true;
                        t = new token(tokenType.Braces, line, tokenText + "", null);
                        break;
                    case 3:
                        Next();
                        if (currentChar == '=')
                        {
                            state = 4;
                            tokenText += '=';
                            continue;
                        }
                        else if (isDigit(currentChar) || isLetter(currentChar) || currentChar == ' ' || currentChar == '\0')
                            position--;
                        else
                            state = 14;
                        done = true;
                        t = new token(tokenType.relationOperation, line, tokenText + "", null);
                        break;
                    case 4:
                        done = true;
                        t = new token(tokenType.relationOperation, line, tokenText + "", null);
                        break;
                    case 5:
                        Next();
                        if (currentChar == '=')
                        {
                            state = 4;
                            tokenText += '=';
                        }
                        else if (isDigit(currentChar) || isLetter(currentChar) || currentChar == ' ' || currentChar == '\0')
                            position--;
                        else
                            state = 14;
                        break;
                    case 6:
                        done = true;
                        t = new token(tokenType.LogicOperation, line, tokenText + "", null);
                        break;
                    case 7:
                        Next();
                        if (currentChar == '&')
                        {
                            state = 8;
                            tokenText += '&';
                        }
                        else if (isDigit(currentChar) || isLetter(currentChar) || currentChar == ' ' || currentChar == '\0')
                            position--;
                        else
                            state = 14;
                        break;
                    case 8:
                        done = true;
                        t = new token(tokenType.LogicOperation, line, tokenText + "", null);
                        break;
                    case 9:
                        Next();
                        if (currentChar == '|')
                        {
                            state = 10;
                            tokenText += '|';
                        }
                        else if (isDigit(currentChar) || isLetter(currentChar) || currentChar == ' ' || currentChar == '\0')
                            position--;
                        else
                            state = 14;
                        break;
                    case 10:
                        done = true;
                        t = new token(tokenType.LogicOperation, line, tokenText + "", null);
                        break;
                    case 11:
                        done = true;
                        t = new token(tokenType.AcessOperator, line, tokenText + "", null);
                        break;
                    case 12:
                        done = true;
                        t = new token(tokenType.QuotationMark, line, tokenText + "", null);
                        break;
                    case 13:
                        done = true;
                        t = new token(tokenType.AssignmentOperator, line, tokenText + "", null);
                        break;
                    case 14:
                        done = true;
                        t = new token(tokenType.BadToken, line, tokenText + "", null);
                        break;
                }
            }
            return t;
        }
        private void ReadSymbolToken()
        {

              Tokens.Add(SymbolToken(currentChar+""));
        }
        private void RealIdentifierToken()
        {
            bool invalid = false;
            string TokenText = "";
            token t = null;
            while (currentChar != ' ')
            {
                if (currentChar == '\0')
                {
                    break;
                }
                if (currentChar == ';' || currentChar == '\r')
                {
                    position--;
                    break;
                }
                if (isSymbol(currentChar))
                {
                    t = SymbolToken(currentChar+"");
                    position -= t.Text.Length;
                    break;
                }
                TokenText += currentChar;
                if (!isLetter(currentChar) && !isDigit(currentChar))
                    invalid = true;
                Next();
            }
            if (invalid)
                Tokens.Add(new token(tokenType.BadToken, line, TokenText, null));
            else
                Tokens.Add(checkID(TokenText));
        }
        private void ReadDigitToken()
        {
            bool invalid = false;
            string TokenText = "";
            token t = null;
            while (currentChar != ' ')
            {
                if (currentChar == '\0')
                {
                    break;
                }
                if(currentChar == ';' || currentChar == '\r')
                {
                    position--;
                    break;
                }
                if (isSymbol(currentChar))
                {
                    t = SymbolToken(currentChar+"");
                    position -= t.Text.Length;
                    break;
                }
                TokenText += currentChar;
                if (!isDigit(currentChar))
                    invalid = true;
                Next();
            }
            if (invalid)
                Tokens.Add(new token(tokenType.BadToken, line, TokenText, null));
            else
                Tokens.Add(new token(tokenType.Constant, line, TokenText, null));
        }
    }
}
