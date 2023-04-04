using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2
{
    public class ForwardChaining : InferenceMethod
    {
        private List<PropositionalSymbol> _found;
        public ForwardChaining() : base("FC","Forward Chaining")
        {
            _found = new List<PropositionalSymbol>();
        }
        public override string Result
        {
            get
            {
                string result = "";
                foreach(PropositionalSymbol s in _found)
                {
                    result += s.Symbol + ", ";
                }
                return result;
            }
        }
        private Dictionary<PropositionalSymbol,List<Sentence>> InitializeInPremiseList(Dictionary<PropositionalSymbol,bool> inferred, Dictionary<Sentence,int> count)
        {
            Dictionary<PropositionalSymbol, List<Sentence>> pInPremise = new Dictionary<PropositionalSymbol, List<Sentence>>();
            foreach(PropositionalSymbol p in inferred.Keys) //for each propositional symbol in the knowledge base
            {
                List<Sentence> sentenceWithPInPremise = new List<Sentence>();
                foreach(Sentence s in count.Keys) //for each sentence in the knowledge base
                {
                    if (count[s] > 0)
                    {
                        if(s.GetConnective() == Connective.IMPLICATION) //if it is an implication sentence
                        {
                            if ((s as ComplexSentence).LSentence.GetSymbols().Contains(p)) //if the left side contains p
                            {
                                sentenceWithPInPremise.Add(s);
                            }
                        }
                        else //if it is a disjunction of literals
                        {
                           foreach(Sentence ss in s.GetUnarySentences())
                            {
                                if(ss is NegationSentence)
                                {
                                    if((ss as NegationSentence).PropositionalSymbol == p) //if one of negative literals in this sentence has p as its propositional symbol
                                    {
                                        sentenceWithPInPremise.Add(s);
                                    }
                                }
                            }
                        }
                    }
                }
                pInPremise.Add(p, sentenceWithPInPremise);
            }
            return pInPremise;
        }

        private PropositionalSymbol PopAgenda(List<PropositionalSymbol> agenda)
        {
            PropositionalSymbol p = agenda[0];
            agenda.Remove(p);
            _found.Add(p);
            return p;
        }
        public override bool Entails(KnowledgeBase kb, Sentence alpha)
        {
            if(!(alpha is PropositionalSymbol))
            {
                Console.WriteLine("Query for Forward Chaining must be a Propositional Symbol");
                Environment.Exit(1);
            }
            //count table - indicating how many propositional symbols are in a sentence's premise
            Dictionary<Sentence, int> count = MakeCountTable(kb);
            //inferred table, initially false for all symbols
            Dictionary<PropositionalSymbol, bool> inferred = MakeInferredTable(kb);
            //agenda - a list of propositional symbols that are already known in the KB
            List<PropositionalSymbol> agenda = InitAgenda(count);
            //a dictionary with:
            // key: a propositional symbol
            // value: a list of sentences that have that symbol in their premise
            Dictionary<PropositionalSymbol, List<Sentence>> pInPremise = InitializeInPremiseList(inferred, count);

            while (agenda.Count>0) //if the agenda list is not empty
            {
                PropositionalSymbol p = PopAgenda(agenda); //take a propositional symbol out of the agenda
                if(p==alpha) //is alpha? return true
                {
                    return true;
                }
                if (!inferred[p]) //if the symbol is not inferred
                {
                    inferred[p] = true;
                    foreach(Sentence s in pInPremise[p]) //for each sentence that has this symbol in premise
                    {
                        count[s] -= 1;
                        if (count[s] == 0) //if the premise is satisfied
                        {
                            //Add the conclusion of this sentence to the agenda
                            if (s.GetConnective() == Connective.IMPLICATION)
                            {
                                agenda.Add((s as ComplexSentence).RSentence as PropositionalSymbol);
                            }
                            else
                            {
                                foreach(Sentence simplS in s.GetUnarySentences())
                                {
                                    if(simplS is PropositionalSymbol)
                                    {
                                        agenda.Add(simplS as PropositionalSymbol);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
