using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2
{
    public class BackwardChaining : InferenceMethod
    {
        private List<PropositionalSymbol> _chain;
        public BackwardChaining():base("BC","Backward Chaining")
        {
            _chain = new List<PropositionalSymbol>();
        }
        public override string Result
        {
            get
            {
                string result = "";
                foreach (PropositionalSymbol s in _chain)
                {
                    result += s.Symbol + ", ";
                }
                return result;
            }
        }
        private Dictionary<PropositionalSymbol, List<Sentence>> InitializeConclusionList(Dictionary<PropositionalSymbol, bool> inferred, Dictionary<Sentence, int> count)
        {
            Dictionary<PropositionalSymbol, List<Sentence>> pAsConclusion = new Dictionary<PropositionalSymbol, List<Sentence>>();
            foreach (PropositionalSymbol p in inferred.Keys) //for each propositional symbol in the inferred list
            {
                List<Sentence> sentenceWithPAsConclusion = new List<Sentence>(); //list of sentences that has "p" as conclusion
                foreach (Sentence s in count.Keys) //for each sentence
                {
                    if(count[s]>0) //if the sentence is a complex sentence
                    {
                        if (s.GetConnective() == Connective.IMPLICATION)
                        {
                            if((s as ComplexSentence).RSentence == p)
                            {
                                sentenceWithPAsConclusion.Add(s);
                            }
                        }
                        else
                        {
                            if (s.GetUnarySentences().Contains(p))
                            {
                                sentenceWithPAsConclusion.Add(s);
                            }
                        }
                        //and that sentence contains propositional symbol "p", add it to the list
                    }
                }
                //add "p" and its conclusion list to the dictionary
                pAsConclusion.Add(p, sentenceWithPAsConclusion);
                
            }
            return pAsConclusion;
        }
        public override bool Entails(KnowledgeBase kb, Sentence alpha)
        {
            if (!(alpha is PropositionalSymbol))
            {
                Console.WriteLine("Query for Backward Chaining must be a Propositional Symbol");
                Environment.Exit(1);
            }
            //count table - number of propositional symbols in a sentence's premise
            Dictionary<Sentence, int> count = MakeCountTable(kb);
            //inferred table, initially false for all symbols
            Dictionary<PropositionalSymbol, bool> inferred = MakeInferredTable(kb);
            //a dictionary with:
            //key: a Propositional Symbol
            //value: a list of sentences that has the key as its conclusion
            Dictionary<PropositionalSymbol, List<Sentence>> sentencesWithPAsConclusion = InitializeConclusionList(inferred, count);
            //agenda: a list of propositional symbols that are already known
            List<PropositionalSymbol> agenda = InitAgenda(count);
            return BC_Entails(alpha as PropositionalSymbol, inferred, sentencesWithPAsConclusion, agenda,count);
        }
        private void AddToResult(PropositionalSymbol p)
        {
            if (!_chain.Contains(p))
            {
                _chain.Add(p);
            }
        }
        private bool BC_Entails(PropositionalSymbol p, Dictionary<PropositionalSymbol, bool> inferred, Dictionary<PropositionalSymbol, List<Sentence>> sentencesWithPAsConclusion, List<PropositionalSymbol> agenda,Dictionary<Sentence, int> count)
        {
            if (agenda.Contains(p)) //if P is already known
            {
                AddToResult(p);
                return true;
            }
            if (!inferred[p]) //if p has not been considered yet
            {
                inferred[p] = true;
                foreach(Sentence s in sentencesWithPAsConclusion[p]) //for each sentence with p as conclusion
                {
                    if (s.GetConnective() == Connective.IMPLICATION) //if p is in "(conjunction of propositional symbols) => proposional symbol" form
                    {
                        foreach (PropositionalSymbol ps in (s as ComplexSentence).LSentence.GetSymbols()) //recursively search for a node included in the agenda
                        {
                            if (!BC_Entails(ps, inferred, sentencesWithPAsConclusion, agenda,count))
                            {
                                break;
                            }
                            count[s] -= 1;
                        }
                        if(count[s]==0) //if all the premises of that sentence is satisfied, add the conclusion to the agenda
                        {
                            agenda.Add(p);
                            AddToResult(p);
                            return true;
                        }
                    }
                    else //if p is in "disjunction of unary sentences that exactly one of them is positive"
                    {
                        
                        foreach (Sentence stc in s.GetUnarySentences())
                        {
                            if(stc is NegationSentence) //take only the negation sentences into account
                            {
                                if(!BC_Entails((stc as NegationSentence).PropositionalSymbol,inferred,sentencesWithPAsConclusion, agenda,count))
                                {
                                    break;
                                }
                                count[s] -= 1;
                            }
                        }
                        if (count[s] == 0) //if all the premises of that sentence is satisfied, add the conclusion to the agenda
                        {
                            agenda.Add(p);
                            AddToResult(p);
                            return true;
                        }
                    }
                   
                }
            }
            return false;
        }
    }
}
