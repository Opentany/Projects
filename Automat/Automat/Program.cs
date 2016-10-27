using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automat
{

    class Program
    {
        public struct para
        {
            public int q;
            public char a;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Podaj tekst");
            string text = Console.ReadLine();
            Console.WriteLine("Podaj wzorzec");
            string pattern = Console.ReadLine();
            PatternFinder(text,pattern);
            Console.ReadKey();
        }

        static void PatternFinder(string T, string P)
        {
            FiniteAutomatonMatcher(T,ComputeTransitionFunction(P),P.Length);
        }
        static void FiniteAutomatonMatcher(string T, Dictionary<para, int> delta,int m)
        {
            int n = T.Length;
            int q = 0;
            for (int i = 0; i < n; i++)
            {
                try
                {
                    para para = new para
                    {
                        q = q,
                        a = T[i]
                    };
                    q = delta[para];
                }
                catch (Exception)
                {
                    
                    
                }

                if (q != m) continue;
                int s = i - m+1;
                Console.WriteLine("wzorzec wystepuje z przesunięciem " + s);
            }
        }

        static Dictionary<para, int> ComputeTransitionFunction(string P)
        {
            char[] Sigma = P.ToCharArray().Distinct().ToArray();
            Dictionary < para, int> delta = new Dictionary<para, int>();
                int m = P.Length;
            for (int q = 0; q <= m; q++)
            {
                foreach (var a in Sigma)
                {
                    int k = Math.Min(m + 1, q + 2);
                    do
                    {
                        k--;
                    } while (!(P.Substring(0,q)+a).EndsWith(P.Substring(0,k)));
                    para para = new para
                    {
                        q = q,
                        a = a
                    };
                    delta.Add(para,k);

                }
            }
            return delta;
        }
    }
}
