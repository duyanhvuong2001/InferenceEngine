using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2
{
    public class Model
    {
        private Dictionary<PropositionalSymbol, bool> _assignments;
        public Model()
        {
           _assignments = new Dictionary<PropositionalSymbol, bool>();
        }
        public Model(Dictionary<PropositionalSymbol,bool> asm)
        {
            _assignments = new Dictionary<PropositionalSymbol, bool>(asm);
        }
        public Dictionary<PropositionalSymbol,bool> Assignment
        {
            get
            {
                return _assignments;
            }
        }
        public Model Union(PropositionalSymbol p, bool b)
        {
            Model m = new Model(_assignments);
            if(m.Assignment.ContainsKey(p))
            {
                m.Assignment[p] = b;
            }
            else
            {
                m.Assignment.Add(p, b);
            }
            return m;
        }
        public bool IsTrue(KnowledgeBase kb)
        {
            foreach(Sentence s in kb.Sentences)
            {
                if (!IsTrue(s))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsTrue(PropositionalSymbol p)
        {
            return Assignment[p];
        }
        public bool IsTrue(Sentence s)
        {
           
            if (s is PropositionalSymbol)
            {

                return IsTrue((s as PropositionalSymbol));
            }
            else
            {
                //check if the sentence is true or false based on its connective
                Connective conn = s.GetConnective();
                if (conn == Connective.BICONDITIONAL)
                {
                    return (IsTrue((s as ComplexSentence).LSentence) && IsTrue((s as ComplexSentence).RSentence)) || (!IsTrue((s as ComplexSentence).LSentence) && !IsTrue((s as ComplexSentence).RSentence));
                }
                if (conn == Connective.IMPLICATION)
                {
                    return !IsTrue((s as ComplexSentence).LSentence) || IsTrue((s as ComplexSentence).RSentence);
                }
                else if (conn == Connective.OR)
                {
                    return IsTrue((s as ComplexSentence).LSentence) || IsTrue((s as ComplexSentence).RSentence);
                }

                else if (conn == Connective.AND)
                {
                    return IsTrue((s as ComplexSentence).LSentence) && IsTrue((s as ComplexSentence).RSentence);
                }
                else if(conn == Connective.NOT)
                {
                    return !IsTrue((s as NegationSentence).PropositionalSymbol);
                }
                else
                {
                    throw new NotImplementedException();
                }
               

            }
        }
    }
}
