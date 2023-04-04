using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2
{
    public class KnowledgeBase
    {
        private List<Sentence> _sentences;
        private List<PropositionalSymbol> _symbols;
        public KnowledgeBase(List<Sentence> sentences)
        {
            _sentences = sentences;
            _symbols = new List<PropositionalSymbol>();
        }
        public List<Sentence> Sentences
        {
            get
            {
                return _sentences;
            }
        }
        public void AddSymbol(PropositionalSymbol p)
        {
            if (!_symbols.Contains(p))
            {
                _symbols.Add(p);
            }
        }
        public List<PropositionalSymbol> GetSymbols()
        {
          
            return _symbols;
        }
        private bool HasSymbol(string symbol)
        {
            foreach(PropositionalSymbol propositionalSymbol in _symbols)
            {
                if (Equals(symbol,propositionalSymbol.Symbol))
                {
                    return true;
                }
            }
            return false;
        }
        public string SentencesToString()
        {
            string stc = "";
            foreach(Sentence s in _sentences)
            {
                stc += s.toString() + ";";
            }
            return stc;
        }
        public string SymbolsToString()
        {
            string symbols = "";
            foreach(PropositionalSymbol s in _symbols)
            {
                symbols += s.Symbol + ";";
            }
            return symbols;
        }
    }
}
