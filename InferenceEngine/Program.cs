using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
namespace Assignment2
{
    class Program
    {
        private const int NUM_METHODS = 3;
        private static InferenceMethod[] iMethods;
        private static KnowledgeBase kbase;
        private static Sentence query;
        private 
        static void Main(string[] args)
        {
            InitMethods();//initialize methods
            //Check the arguments
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: <filename> <method>");
                Environment.Exit(1);
            }
            //Construct the KB and the query based on the given file
            ConstructProblem(args[0]);
            string methodCode = args[1];
            InferenceMethod method = null;
            foreach(InferenceMethod iM in iMethods)
            {
                if (iM.Code == methodCode)
                {
                    method = iM;
                    break;
                }
            }
            //If the method in the args is not implemented
            if (method == null)
            {
                Console.WriteLine("Inference method codename " + methodCode + " not implemented!");
                Environment.Exit(1);
            }
            Console.WriteLine("Solving " + args[0] + " with " + method.FullName);
            Console.WriteLine("TELL");
            Console.WriteLine(kbase.SentencesToString());
            Console.WriteLine("ASK");
            Console.WriteLine(query.toString());
            Console.Write("RESULT: ");
            if (method.Entails(kbase, query))
            {
                
                Console.Write("YES: " + method.Result);
            }
            else
            {
                Console.Write("NO");
            }

        }
        static void InitMethods()
        {
            iMethods = new InferenceMethod[NUM_METHODS];
            iMethods[0] = new TruthTableChecking();
            iMethods[1] = new ForwardChaining();
            iMethods[2] = new BackwardChaining();
        }
        static void ConstructProblem(string filename)
        {
            try
            {
                //create a reader
                StreamReader reader = new StreamReader(filename);
                if (reader.ReadLine() != "TELL")
                {
                    Console.WriteLine("Wrong file format! The file should start with TELL");
                    Environment.Exit(1);
                }
                string kb = reader.ReadLine(); //read KB
                string nw = kb.Replace(" ", string.Empty); //get rid of spaces
                string[] stcs = nw.Split(";"); //split sentences by ";"
                kbase = new KnowledgeBase(new List<Sentence>());
                foreach (string str in stcs)
                {
                    Sentence stn = ConstructSentence(str, kbase);
                    if (stn != null) //if the sentence could be constructed
                    {
                        kbase.Sentences.Add(stn);
                    }
                }
                if (reader.ReadLine() != "ASK")
                {
                    Console.WriteLine("Wrong file format! The query should start with ASK");
                    Environment.Exit(1);
                }
                string ask = reader.ReadLine(); //read query
                query = ConstructSentence(ask,kbase); //construct query
                if (query == null)
                {
                    Console.WriteLine("Cannot form query from file");
                    Environment.Exit(1);
                }
                reader.Close();//close the stream reader once finished
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("File " + filename + " not found!");
                Environment.Exit(1);
            }
            catch (IOException)
            {
                Console.WriteLine("Something wrong with file reading");
                Environment.Exit(1);
            }
        }
        static Sentence Construct(string stc, string icon, Connective conn,KnowledgeBase kb)
        {
            Sentence stn;
            string[] parts = stc.Split(icon);
            if ((parts.Length > 2)) //if there happens to be more than 2 parts
            {
                List<string> mults = new List<string>(parts); //store the parts into the list
                string init = mults[0]; //take the first part out
                mults.Remove(init); //remove it from the list
                stn = ConstructSentenceWithSimilarConns(init, conn, mults, kb);
            }
            else
            {
                stn = ConstructComplexSentence(parts[0], conn, parts[1], kb);
            }
            return stn;
        }
        static Sentence ConstructSentence(string stc, KnowledgeBase kb)
        {
            Sentence stn;
            //Check which connective is considered first based on their order of precedence
            if (stc == "")
            {
                return null;
            }
            else if (stc.Contains("<=>"))
            {
                stn = Construct(stc, "<=>", Connective.BICONDITIONAL, kb);
            }
            else if (stc.Contains("=>"))
            {
                stn = Construct(stc, "=>", Connective.IMPLICATION, kb);
            }
            else if(stc.Contains("||"))
            {
                stn = Construct(stc, "||", Connective.OR, kb);
            }
            else if (stc.Contains("&"))
            {

                stn = Construct(stc, "&", Connective.AND, kb);
               
            }
            else if (stc.Contains("~"))
            {
                string[] parts = stc.Split("~");
                foreach (PropositionalSymbol p in kb.GetSymbols()) //if KB has already had that symbol, use that symbol as the negation sentence's PS
                {
                    if (p.Symbol == parts[1])
                    {
                        return new NegationSentence(p);
                    }
                }
                stn = new NegationSentence(ConstructSentence(parts[1],kb) as PropositionalSymbol);
            }
            else
            {
                foreach(PropositionalSymbol ps in kb.GetSymbols()) //if KB has already had that symbol, do not create anything new
                {
                    if (ps.Symbol == stc)
                    {
                        return ps;
                    }
                }
                stn = new PropositionalSymbol(stc);
                kb.AddSymbol(stn as PropositionalSymbol);
            }
            return stn;
        }
        static Sentence ConstructComplexSentence(string Lstc, Connective conn, string Rstc, KnowledgeBase kb)
        {
            return new ComplexSentence(ConstructSentence(Lstc, kb), conn, ConstructSentence(Rstc, kb));
        }
        static Sentence ConstructSentenceWithSimilarConns(string initial, Connective conn, List<string> remains, KnowledgeBase kb)
        {
            if (remains.Count == 1) //if there is only one item left in the list
            {
                return ConstructComplexSentence(initial,conn,remains[0],kb);
            }
            //take the first item out of the list, and recursively construct a sentence out of them
            List<string> newL = new List<string>(remains);
            string newI = newL[0];
            newL.Remove(newI);
            return new ComplexSentence(ConstructSentence(initial,kb),conn,ConstructSentenceWithSimilarConns(newI, conn, newL, kb));
        }
    }
}
