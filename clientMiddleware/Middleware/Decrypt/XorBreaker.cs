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

        public List<string> Keys { get; } = new List<string>();

        public XorBreaker()
        {
            foreach(var key in CryptoTools.GetPermutationsWithRept("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 4))
            {
                Keys.Add(string.Concat(key.ToArray()));
            }  
        }

        /// <summary>
        /// Will break a cipher code by using transposition and Coincidence index
        /// IC : ~0.06 = english ; ~0.07 = French
        /// </summary>
        /// <param name="cipher">the text uncrypted</param>
        /// <param name="sizeChunk">size of you key</param>
        /// <param name="ic">threshold</param>
        /// <returns>Tuple (string, string) of key and plaintext</returns>
        public IEnumerable<KeyValuePair<string, string>> breakXor(string cipher, int sizeChunk, CancellationToken token, double ic = 0.07)
        {
            if (cipher == string.Empty || cipher == null || sizeChunk <= 0 || cipher.Length > 4000) yield break;

            var blocks = CryptoTools.DivideText(cipher, sizeChunk).ToList();
            var trans = CryptoTools.Transposed(blocks);

            //will contains the potential key used to encrypt the message
            string keys = "";

            foreach (var block in trans)
            {
                List<char> blockKeys = new List<char>();
                foreach (char ch in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                {
                    string text = CryptoTools.Xor(block, char.ToString(ch));

                    if (!blockKeys.Contains(ch) && text.IsPrintable() && text.Ic() > ic - 0.01)//If the block is printable and its index is more than ic-0.01 we add the key into blockKeys
                    {
                        blockKeys.Add(ch);
                    }
                }
                //We order the letter to make the permutation process easier.
                keys += string.Concat(blockKeys.OrderBy(c => c).Select(a => a).ToArray());
            }
            keys = string.Concat(keys.GroupBy(c => c).Select(c => char.ToString(c.Key)).ToArray());

            //Console.WriteLine("Keys : {0} -  {1}", keys, keys.Length);

            if (keys.Length == 0)
                yield break;


            //For each permutation of keys
            foreach (var elem in CryptoTools.GetPermutationsWithRept(keys.ToList(), sizeChunk))
            {
                if (token.IsCancellationRequested)
                {
                    yield break;
                }
                //Merge char eachothers
                string key = string.Concat(elem.ToArray());

                string plain = CryptoTools.Xor(cipher, key);
                var index = plain.Ic();

                if (plain.IsPrintable() && index >= ic)
                {
                    yield return new KeyValuePair<string, string>(key, plain);

                }
            }
        }
    }
}
