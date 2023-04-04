using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2
{
    public class NegationSentence : Sentence
    {
        private PropositionalSymbol _p;
        private Connective _conn;
        public NegationSentence(PropositionalSymbol p)
        {
            _p = p;
            _conn = Connective.NOT;
        }
        public PropositionalSymbol PropositionalSymbol
        {
            get
            {
                return _p;
            }
        }
        public override Connective GetConnective()
        {
            return _conn;
        }
        public override List<Connective> GetConnectives()
        {
            List<Connective> conn = new List<Connective>();
            conn.Add(Connective.NOT);
            return conn;
        }
        public override List<PropositionalSymbol> GetSymbols()
        {
            List<PropositionalSymbol> s = new List<PropositionalSymbol>();
            s.Add(_p);
            return s;
        }
        public override string toString()
        {
            return "~" + _p.Symbol;
        }
    }
}
