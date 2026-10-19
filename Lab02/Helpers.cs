using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Lab02
{
    internal static class AppPaths
    {
        public static string BaseDir => AppDomain.CurrentDomain.BaseDirectory;
        public static string PathFor(string name) => Path.Combine(BaseDir, name);
    }

    // Recursive-descent parser for +, -, *, /, parentheses and unary signs.
    internal sealed class ExpressionParser
    {
        private readonly string _text;
        private int _pos;
        public ExpressionParser(string text) { _text = text ?? ""; }

        public double Parse()
        {
            double value = ParseExpression();
            SkipSpaces();
            if (_pos != _text.Length) throw new FormatException("Ký tự không hợp lệ tại vị trí " + (_pos + 1));
            return value;
        }

        private double ParseExpression()
        {
            double value = ParseTerm();
            while (true)
            {
                SkipSpaces();
                if (Match('+')) value += ParseTerm();
                else if (Match('-')) value -= ParseTerm();
                else return value;
            }
        }

        private double ParseTerm()
        {
            double value = ParseUnary();
            while (true)
            {
                SkipSpaces();
                if (Match('*')) value *= ParseUnary();
                else if (Match('/'))
                {
                    double divisor = ParseUnary();
                    if (Math.Abs(divisor) < 1e-12) throw new DivideByZeroException("Không thể chia cho 0.");
                    value /= divisor;
                }
                else return value;
            }
        }

        private double ParseUnary()
        {
            SkipSpaces();
            if (Match('+')) return ParseUnary();
            if (Match('-')) return -ParseUnary();
            return ParsePrimary();
        }

        private double ParsePrimary()
        {
            SkipSpaces();
            if (Match('('))
            {
                double value = ParseExpression();
                SkipSpaces();
                if (!Match(')')) throw new FormatException("Thiếu dấu đóng ngoặc ')'.");
                return value;
            }

            int start = _pos;
            bool seenDigit = false;
            while (_pos < _text.Length && char.IsDigit(_text[_pos])) { _pos++; seenDigit = true; }
            if (_pos < _text.Length && (_text[_pos] == '.' || _text[_pos] == ','))
            {
                _pos++;
                while (_pos < _text.Length && char.IsDigit(_text[_pos])) { _pos++; seenDigit = true; }
            }
            if (!seenDigit) throw new FormatException("Thiếu số tại vị trí " + (_pos + 1));
            string token = _text.Substring(start, _pos - start).Replace(',', '.');
            return double.Parse(token, CultureInfo.InvariantCulture);
        }

        private void SkipSpaces() { while (_pos < _text.Length && char.IsWhiteSpace(_text[_pos])) _pos++; }
        private bool Match(char c) { if (_pos < _text.Length && _text[_pos] == c) { _pos++; return true; } return false; }
    }
}
