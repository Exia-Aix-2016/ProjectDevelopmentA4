using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Decrypt
{
    public class XorBreaker
    {

        /// <summary>
        /// Will break a cipher code by using transposition and Coincidence index
        /// IC : ~0.06 = english ; ~0.07 = French
        /// </summary>
        /// <param name="cipher">the text uncrypted</param>
        /// <param name="sizeChunk">size of you key</param>
        /// <param name="ic">threshold</param>
        /// <returns>Tuple (string, string) of key and plaintext</returns>
        public (Dictionary<string, string> keyPlains, long timeToBreak) BreakXor(string cipher, int sizeChunk, CancellationToken token, double ic = 0.07)
        {

            //Will contains all potential keys and their matching plain.
            Dictionary<string, string> keyPlains = new Dictionary<string, string>();

            //Security if cipher is empty
            if (cipher == string.Empty) return (keyPlains, 0);

            //To calculate the time to break
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //We split the text into chunk and then transposed each block to make easier to guess the key
            var blocks = CryptoTools.DivideText(cipher, sizeChunk).ToList();
            var trans = CryptoTools.Transposed(blocks);
            
            //will contains the potential key used to encrypt the message
            string keys = "";

            


            foreach (var block in trans)
            {
                Dictionary<string, int> blockKeys = new Dictionary<string, int>();
                foreach (char ch in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")//We know the key is only make with upperCase
                {
                    string text = CryptoTools.Xor(block, char.ToString(ch));

                    if (text.IsPrintable() && text.Ic() >= ic - 0.01)//If the block is printable and its index is more than ic-0.01 we add the key into blockKeys
                    {
                        var c = char.ToString(ch);
                        if (blockKeys.ContainsKey(c))
                        {
                            blockKeys[c] += 1;
                        }
                        else
                        {
                            blockKeys.Add(c, 1);
                        }

                    }
                }
                //We order the letter to make the permutation process easier.
                keys += string.Join("", blockKeys.OrderBy(c => c.Value).Select(a => a.Key).ToArray());

            }
            keys = string.Join("", keys.GroupBy(c => c).Select(c => char.ToString(c.Key)).ToArray());
            Console.WriteLine(keys);
            if (keys.Length == 0)
            {
                sw.Stop();
                return (keyPlains, sw.ElapsedMilliseconds);
            }

            //For each permutation of keys
            foreach (var elem in CryptoTools.GetPermutationsWithRept(keys.ToList(), sizeChunk))
            {
                if (token.IsCancellationRequested)
                    break;

                //Merge char eachothers
                string key = string.Join("", elem.ToArray());

                string plain = CryptoTools.Xor(cipher, key);
                var index = plain.Ic();

                if (index >= ic)
                {
                    keyPlains.Add(key, plain);
                }
            }
            sw.Stop();
            return (keyPlains, sw.ElapsedMilliseconds);
        }
    }
}
