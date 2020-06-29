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
        public IEnumerable<KeyValuePair<string, string>> breakXor(string cipher, int sizeChunk, CancellationToken token,  double ic = 0.07)
        {
            IEnumerable<KeyValuePair<string, string>> keyPlains = new List<KeyValuePair<string, string>>();


            if (cipher == string.Empty || cipher == null) yield break;

            //Used to bypass the break by index of coincidence.
            bool bypass = false;

            if (cipher.Length >= 1000000)
            {

                bypass = true;
            }
            //will contains the potential key used to encrypt the message
            string keys = "";


            if (!bypass)
            {
                var blocks = CryptoTools.DivideText(cipher, sizeChunk).ToList();
                var trans = CryptoTools.Transposed(blocks);

                

                foreach (var block in trans)
                {
                    Dictionary<string, int> blockKeys = new Dictionary<string, int>();
                    foreach (char ch in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
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

            }


            if (keys.Length == 0)
            {
                bypass = true;
                keys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }


            //For each permutation of keys
            foreach (var elem in CryptoTools.GetPermutationsWithRept(keys.ToList(), sizeChunk))
            {
                if (token.IsCancellationRequested)
                {
                    yield break;
                }
                //Merge char eachothers
                string key = string.Join("", elem.ToArray());
                string plain = CryptoTools.Xor(cipher, key);
                double index = 0.0;

                if (!bypass)
                {
                    index = plain.Ic();
                }


                if (index >= ic || bypass)
                {
                    yield return new KeyValuePair<string, string>(key, plain);
                }
            }
        }
    }
}
