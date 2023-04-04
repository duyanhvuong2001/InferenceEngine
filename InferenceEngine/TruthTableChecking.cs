using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2
{
    public class TruthTableChecking : InferenceMethod
    {
        private int _num_models;
        public TruthTableChecking():base("TT","Truth Table")
        {
            _num_models = 0;
        }
        public int NumModels
        {
            get
            {
                return _num_models;
            }
        }
        public override string Result
        {
            get
            {
                return _num_models.ToString();
            }
        }
        public override bool Entails(KnowledgeBase kb, Sentence alpha)
        {
            List<PropositionalSymbol> symbols = kb.GetSymbols();
            foreach(PropositionalSymbol s in alpha.GetSymbols())
            {
                if (!symbols.Contains(s)) //KB does not contain the symbol?
                {
                    symbols.Add(s);
                }
                
            }
            return TTCheckAll(kb, alpha, symbols, new Model());
        }
        private bool TTCheckAll(KnowledgeBase kb, Sentence alpha, List<PropositionalSymbol> symbols, Model model)
        {
            if (symbols.Count == 0) //all symbols have been assigned a value?
            {
                if (model.IsTrue(kb))
                {
                    _num_models++;
                    return model.IsTrue(alpha); //if model is true to KB, check if model is true to alpha
                }
                else
                {
                    //always return true when KB is false
                    return true;
                }
            }
            PropositionalSymbol p = symbols[0];
            List<PropositionalSymbol> rest = new List<PropositionalSymbol>(symbols);
            rest.Remove(p);
            return TTCheckAll(kb, alpha, rest, model.Union(p, true)) 
            && TTCheckAll(kb, alpha, rest, model.Union(p, false)); //recursively assign boolean values to symbols
        }
    }
}
