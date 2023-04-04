using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2
{
    public abstract class Sentence
    {
        public virtual Connective GetConnective()
        {
            return Connective.NULL;
        }
        public virtual List<Connective> GetConnectives()
        {
            List<Connective> cns = new List<Connective>();
            cns.Add(Connective.NULL);
            return cns;
        }
        public virtual List<Sentence> GetUnarySentences()
        {
            return new List<Sentence>() { this };
        }
        public bool IsInHornForm()
        {
            if(this is PropositionalSymbol)
            {
                return true;
            }
            else if (GetConnective() == Connective.IMPLICATION)//if this is implication sentence
            {
                if (!((this as ComplexSentence).RSentence is PropositionalSymbol)) //right sentence must be a propositional symbol
                {
                    return false;
                }
                if ((this as ComplexSentence).LSentence is PropositionalSymbol)//left sentence is either a propositional symbol...
                {
                    return true;
                }
                else //...or conjunction of propositional symbols
                {
                    List<Connective> Lconn = (this as ComplexSentence).LSentence.GetConnectives();
                    foreach (Connective conn in Lconn)
                    {
                        if (conn != Connective.AND)
                        {
                            return false;
                        }
                    }
                    return true;

                }
            }
            else //if this is disjunction of negation sentences and exactly one propositional symbol
            {
                foreach (Connective con in GetConnectives())
                {
                    if (con != Connective.OR && con != Connective.NOT)
                    {
                        return false;
                    }
                }

                int count = 0;
                foreach (Sentence simplerS in GetUnarySentences())
                {
                    if (!(simplerS is NegationSentence))
                    {
                        count++;
                    }
                    if (count > 1) //there could only be one unary sentence that is not a negation sentence
                    {
                        //Console.WriteLine(simplerS.toString());
                        
                        return false;
                    }

                }
                if (count == 0) //if there are all negation sentences, the overall sentence is not in horn form 
                {
                    return false;
                }
                return true;
            }
        }
        public abstract List<PropositionalSymbol> GetSymbols();
        public abstract string toString();
    }
}
