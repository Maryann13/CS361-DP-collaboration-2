using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharsFrequency
{
    public class CharsFreqRemover
    {
        private bool emptySrc;

        public char Freq { get; }
        public char Rare { get; }

        public CharsFreqRemover(string source)
        {
            if (source == null)
                throw new ArgumentNullException();
            emptySrc = source.Length == 0;
            if (emptySrc)
                return;

            int maxCnt = 0;
            int minCnt = int.MaxValue;
            Freq = char.MaxValue;
            foreach (var chr in source)
            {
                int cnt = source.Count(c => c == chr);
                if (cnt > maxCnt || cnt == maxCnt && chr < Freq)
                {
                    maxCnt = cnt;
                    Freq = chr;
                }
                if (cnt < minCnt || cnt == minCnt && Rare < chr)
                {
                    minCnt = cnt;
                    Rare = chr;
                }
            }
        }

        public string Remove(string s)
        {
            if (emptySrc)
                return s;
            else
                return s.Replace(Freq.ToString(), "").Replace(Rare.ToString(), "");
        }
    }
}
