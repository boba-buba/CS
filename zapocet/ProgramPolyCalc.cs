using System;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;

namespace zapocetPolyCalc
{
    class ErrorException : Exception
    {
        public const string ErrorMessage = "Error Message";
    }
    public class ProgramInputOutputState : IDisposable
    {
        public const string ArgumentErrorMessage = "Argument Error";
        public const string FileErrorMessage = "File Error";

        public StreamReader? Reader { get; private set; }

        public bool InitializeFromCommandLineArgs(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Write(ArgumentErrorMessage + "\n");
                return false;
            }

            try
            {
                Reader = new StreamReader(args[0]);
            }
            catch (IOException)
            {
                Console.Write(FileErrorMessage + "\n");
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                Console.Write(FileErrorMessage + "\n");
                return false;
            }
            catch (ArgumentException)
            {
                Console.Write(FileErrorMessage + "\n");
                return false;
            }
            return true;
        }
        public void Dispose()
        {
            Reader?.Dispose();
        }
    }
    
    class PolynomMember
    { 
        public int koef = 0;
        public int power = 0;
        public PolynomMember(int koef, int power)
        {
            this.koef = koef;
            this.power = power;
        }   
    }


    class Calculator
    {
        private Dictionary<int, int> _intermediateResult = new Dictionary<int, int> { };

        private bool IsVariable(string member)
        {
            bool xVar = false;
            bool power = false;
            for (int i = 0; i < member.Length; i++)
            {
                if (!char.IsDigit(member[i]))
                {
                    if (member[i] != 'x' && member[i] != '^')
                    {
                        return false;
                    }
                    if (member[i] == 'x') xVar = true;
                    if (member[i] == '^') power = true;
                }
                
            }
            if (power && !xVar) { return false; }
            return true;
        }
        private bool CheckIfValidPolynom(string[] tokens)
        {
            foreach (string token in tokens) 
            {
                if (!(IsVariable(token))) 
                    return false;
            }
            return true;
        }

        private Dictionary<int, int> ConvertToPolynom(string[] tokens)
        {
            var nonDigists = new char[] { 'x', '^' };
            var polynom = new Dictionary<int, int>{ };
            foreach (string token in tokens)
            {
                var koef = "0";
                var power = "0";
                int minus = 1;
                var i = 0;
                if (token[i] == '-') { minus = -1; i++; }
                var c = token[i];
                while(c != 'x')
                {
                    koef += c;
                    i++; if (i == token.Length) break;
                    c = token[i];
                }
                if (c == 'x') 
                {
                    
                    if (i == 0) koef = "1";
                }
                i += 2;
                while (i < token.Length) 
                {
                    c = token[i];
                    power += c;
                    i++;
                }
                if (token[token.Length - 1] == 'x') 
                    power = "1";
                var PowerNumber =  int.Parse(power);
                polynom[PowerNumber] = minus * int.Parse(koef);
            }
            return polynom;
        }
        private void SetPolynom(string[] tokens)
        {
            if (!CheckIfValidPolynom(tokens)) 
            { throw new ErrorException(); }

            _intermediateResult = ConvertToPolynom(tokens);
        }

        private bool ChangePolynomMember(Dictionary<int, int> polynom, int power, int koef, char op)
        {
            if (polynom.ContainsKey(power))
            {
                if (op == '+') polynom[power] += koef;
                else polynom[power] -= koef;
                return true;
            }
            return false;
        }

        private Dictionary<int, int> PlusMinus(Dictionary<int, int> first, Dictionary<int, int> second, char op)
        {
            foreach (var member in first)
            {
                if (!(ChangePolynomMember(first, member.Key, member.Value, op)))
                {
                    if (op == '-') second[member.Key] = -member.Value;
                    else second[member.Key] = member.Value;
                }
            }
            return second;
        }
        private void PlusMinusPolynom(string[] tokens, char op)
        {
            var polynomToAdd = ConvertToPolynom(tokens.Skip(1).ToArray());
            PlusMinus(polynomToAdd, _intermediateResult, op);
        }
        
        private Dictionary<int, int> Multiply(Dictionary<int, int> first, Dictionary<int, int> second)
        {
            var newPolynom = new Dictionary<int, int> { };

            foreach (var m1 in first)
            {
                int power = m1.Key;
                int koef = m1.Value;
                foreach (var m2 in second)
                {
                    int newKoef = koef * m2.Value;
                    int newPower = power + m2.Key;
                    if (newPolynom.ContainsKey(newPower))
                        newPolynom[newPower] += newKoef;
                    else
                    {
                        newPolynom[newPower] = newKoef;
                    }
                }
            }
            return newPolynom;
        }
        private void MultiplyPolynom(string[] tokens)
        {
            var polynomToMult = ConvertToPolynom(tokens.Skip(1).ToArray());
            var newPolynom = Multiply(polynomToMult, _intermediateResult);
            _intermediateResult = newPolynom;   
        }

        private void EvaluatePolynom(string[] tokens)
        {
            int number;
            if (tokens.Length != 2 || !(int.TryParse(tokens[1], out number))) throw new ErrorException();

            int result = 0;
            foreach (var member in _intermediateResult)
            {
                if (member.Key == 0) result += member.Value;
                else
                {
                    result += member.Value * (int)Math.Pow(number, member.Key);
                }
            }
            Console.WriteLine(result);
        }

        private void DerivatePolynom(string[] tokens)
        {
            var newPolynom = new Dictionary<int, int> { };
            foreach (var member in _intermediateResult)
            {

                if (member.Key != 0)
                {
                    var newKoef = member.Key * member.Value;
                    var newPower = member.Key - 1;
                    newPolynom[newPower] = newKoef;

                }
            }
            _intermediateResult = newPolynom;
        }

        private void SubstitutePolynom(string[] tokens)
        {
            var polynom = ConvertToPolynom(tokens.Skip(1).ToArray());
            var result = new Dictionary<int, int>();
            foreach (var member in _intermediateResult)
            {
                var interResult = polynom;
                for (int i = 0; i < member.Key - 1; i++)
                {
                    var multResult = Multiply(polynom, interResult);
                    interResult = multResult;
                }
                if (member.Key != 0)
                {
                    var koefResult = Multiply(interResult, new Dictionary<int, int> { { 0, member.Value } });
                    interResult = koefResult;
                }
                
                var r = PlusMinus(interResult, result, '+');
                result = r;
            }
            _intermediateResult = result;
        }

        private void PrintCurrent()
        {
            foreach( var pair in _intermediateResult.OrderBy(key => key.Key))
            {
                if (pair.Key == 0)
                { if (pair.Value != 0) Console.Write(pair.Value.ToString()); }
                else if (pair.Key == 1)
                {
                    if (pair.Value != 1) Console.Write(pair.Value.ToString());
                    Console.Write('x');
                }
                else
                {
                    if (pair.Value != 1) Console.Write(pair.Value.ToString());
                    Console.Write("x^" + pair.Key);
                }
                Console.Write(' ');
            }
            Console.WriteLine();
        }
        
        public void ProcessRequest(string[] tokens)
        {
            if (tokens.Length < 1) 
            { throw new ErrorException(); }
            switch (tokens[0])
            {
                case "+":
                    PlusMinusPolynom(tokens, '+'); 
                    break;
                case "-":
                    PlusMinusPolynom(tokens, '-'); 
                    break;
                case "*":
                    MultiplyPolynom(tokens); break;
                case "e":
                    EvaluatePolynom(tokens); break;
                case "d":
                    DerivatePolynom(tokens); break;
                case "s":
                    SubstitutePolynom(tokens); break;
                default:
                    SetPolynom(tokens);
                    break;
            }
            PrintCurrent();
        }
    }

    

    class Parser
    {
        private StreamReader _reader;
        private Calculator _calculator;
        public Parser(StreamReader reader)
        {
            _reader = reader;
            _calculator = new Calculator();
        }
        private string[] GetTokens(string line)
        {
            char[] whitespaces = { ' ', '\t' };
            string[] tokens = line.Split(whitespaces, StringSplitOptions.RemoveEmptyEntries);
            return tokens;
        }

        public void ReadLines()
        {

            string line = _reader.ReadLine();
            while (line != null)
            {
                try
                {
                    _calculator.ProcessRequest(GetTokens(line));
                }
                catch (ErrorException)
                {
                    Console.WriteLine(ErrorException.ErrorMessage);
                }
                line = _reader.ReadLine();
            }

        }
    }
    internal class Program2
    {
        static void Main2(string[] args)
        {

            ProgramInputOutputState state = new ProgramInputOutputState();
            if (!state.InitializeFromCommandLineArgs(args))
            {
                return;
            }

            Parser parser = new Parser(state.Reader);
            parser.ReadLines();
            

        }
    }
}
