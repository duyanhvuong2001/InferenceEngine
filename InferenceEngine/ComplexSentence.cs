using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2
{
    public class ComplexSentence : Sentence
    {
        private Connective _connective;
        private Sentence _Lsentence;
        private Sentence _Rsentence;
        public override Connective GetConnective()
        {
            return _connective;
        }
        public override List<Connective> GetConnectives()
        {
            List<Connective> connectives = new List<Connective>();
            connectives.Add(_connective);
            foreach(Connective conn in _Lsentence.GetConnectives())
            {
                if(conn!= Connective.NULL)
                {
                    connectives.Add(conn);
                }
               
            }
            foreach(Connective conn in _Rsentence.GetConnectives())
            {
                if (conn != Connective.NULL)
                {
                    connectives.Add(conn);
                }
            }
            return connectives;
        }
        public override List<Sentence> GetUnarySentences()
        {
            List<Sentence> stcs = new List<Sentence>();
            stcs.AddRange(LSentence.GetUnarySentences());
            stcs.AddRange(RSentence.GetUnarySentences());
            return stcs;
        }
        public Sentence LSentence
        {
            get
            {
                return _Lsentence;
            }
        }
        public Sentence RSentence
        {
            get
            {
                return _Rsentence;
            }
        }
        public ComplexSentence(Sentence Ls, Connective conn, Sentence Rs)
        {
            _Lsentence = Ls;
            _Rsentence = Rs;
            _connective = conn;
        }
        public override List<PropositionalSymbol> GetSymbols()
        {
            List<PropositionalSymbol> symbols = new List<PropositionalSymbol>();
            foreach(PropositionalSymbol LpS in _Lsentence.GetSymbols())
            {
                symbols.Add(LpS);
            }
            foreach(PropositionalSymbol RpS in _Rsentence.GetSymbols())
            {
                symbols.Add(RpS);
            }
            return symbols;
        }
        public override string toString()
        {
            string conn;
            if (_connective == Connective.AND)
            {
                conn = "&";
            }
            else if (_connective==Connective.OR)
            {
                conn = "|";
            }
            else if(_connective==Connective.NOT)
            {
                conn = "-";
            }
            else if(_connective==Connective.IMPLICATION)
            {
                conn = "=>";
            }
            else
            {
                conn = "<=>";
            }
            return _Lsentence.toString() + conn + _Rsentence.toString();
        }
    }
}
