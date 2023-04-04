using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2
{
    public class PropositionalSymbol : Sentence
    {
        private string _symbol;
        public PropositionalSymbol(string symbol)
        {
            _symbol = symbol;
        }
        public string Symbol
        {
            get
            {
                return _symbol;
            }
        }
        public override List<PropositionalSymbol> GetSymbols()
        {
            List<PropositionalSymbol> symbols = new List<PropositionalSymbol>();
            symbols.Add(this);
            return symbols;
        }
        public override string toString()
        {
            return _symbol;
        }
    }
}
