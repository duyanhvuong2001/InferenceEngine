using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2
{
    public abstract class InferenceMethod
    {
        private string _code;
        private string _fullName;
        public InferenceMethod(string code, string fullName)
        {
            _code = code;
            _fullName = fullName;
        }
        public string FullName
        {
            get
            {
                return _fullName;
            }
        }
        public string Code
        {
            get
            {
                return _code;
            }
        }
        public abstract bool Entails(KnowledgeBase kb, Sentence alpha);
        protected Dictionary<Sentence, int> MakeCountTable(KnowledgeBase kb)
        {
            Dictionary<Sentence, int> count = new Dictionary<Sentence, int>();
            foreach (Sentence s in kb.Sentences)
            {
                if (!s.IsInHornForm())
                {
                    Console.WriteLine("KB must be formed by Horn clauses!");
                    Environment.Exit(1);
                }
                else
                {
                    count.Add(s, s.GetSymbols().Count - 1);
                }
            }
            return count;
        }
        public Dictionary<PropositionalSymbol, bool> MakeInferredTable(KnowledgeBase kb)
        {
            Dictionary<PropositionalSymbol, bool> inferred = new Dictionary<PropositionalSymbol, bool>();
            foreach (PropositionalSymbol p in kb.GetSymbols())
            {
                inferred.Add(p, false);
            }
            return inferred;
        }
        protected List<PropositionalSymbol> InitAgenda(Dictionary<Sentence,int> count)
        {
            List<PropositionalSymbol> agd = new List<PropositionalSymbol>();
            foreach(Sentence s in count.Keys)
            {
                //if that sentence is a propositional symbol, add it to the agenda list
                if(count[s]==0)
                {
                    agd.Add(s as PropositionalSymbol);
                }
            }
            return agd;
        }
        public abstract string Result
        {
            get;
        }
    }
}
