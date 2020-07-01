using Middleware.Decrypt;
using Middleware.Models;
using Middleware.Services.Authentification;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Services.Uncryption
{
    /// <summary>
    /// Decrypt Service
    /// </summary>
    public class DecryptService : IDecryptService
    {
        private CancellationTokenSource globalCancellationSource { get; set; } = new CancellationTokenSource();
        private ConcurrentDictionary<string, CancellationTokenSource> userCancellationTokens { get; set; } = new ConcurrentDictionary<string, CancellationTokenSource>();
        private XorBreaker xorBreaker = new XorBreaker();

        /// <summary>
        /// used to break a text by index of coincidence, Smart attack but not work with  plaintext that sucks!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private void Decrypt(Message message, CancellationToken token)
        {
            var request = new RequestHttp();
            
            var decryptMessage = (DecryptMsg)message.Data;

            int i = 0;
            foreach (var kv in xorBreaker.breakXor(decryptMessage.CipherText, 4, token, 0.06))
            {
                if (i > 1) break;
                var decryptMsg = new DecryptMsg
                {
                    FileName = decryptMessage.FileName,
                    Key = kv.Key,
                    PlainText = kv.Value
                };

                Console.WriteLine(decryptMsg.FileName + " : " + decryptMsg.Key + " - " + decryptMsg.PlainText.Length);
                var msg = new Message
                {
                    OperationName = "DECRYPT",
                    TokenApp = message.TokenApp,
                    TokenUser = message.TokenUser,
                    Data = decryptMsg,
                    Info = message.Info,
                    AppVersion = message.AppVersion,
                    StatusOp = message.StatusOp,
                    OperationVersion = message.OperationVersion
                };
                i++;
                request.sendJson(msg);
            }
        }

        /// <summary>
        /// Will send Files into Backend cache
        /// </summary>
        /// <param name="message">message</param>
        private void cacheFile(Message message)
        {
            var request = new RequestHttp();
            message.OperationName = "CACHE";
            request.sendJson(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Message ServiceAction(Message message)
        {
            
            //Cache before decrypt
            cacheFile(message);

            //Token preparation
            var decryptMessage = (DecryptMsg)message.Data;

            var userCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(globalCancellationSource.Token);
            var tokenId = message.TokenUser + decryptMessage.FileName;

            //Add user token to Dictionary (to stop it after)
            if(userCancellationTokens.AddOrUpdate(tokenId, userCancellationToken, (k, v) => userCancellationToken) == null)
            {
                Console.WriteLine("Error userTokenCancellation for : " + tokenId);
            }

            Task.Run(() => Decrypt(message, userCancellationToken.Token), userCancellationToken.Token);
 
            #region bruteforce
            /*try
            {
                Parallel.ForEach(xorBreaker.Keys, new ParallelOptions { CancellationToken = userCancellationToken.Token }, (key) =>
                {
                    var request = new RequestHttp();

                    var decryptMsg = new DecryptMsg
                    {
                        FileName = decryptMessage.FileName,
                        Key = key,
                        CipherText = null,
                        PlainText = null
                    };

                    var msg = new Message
                    {
                        OperationName = "DECRYPT",
                        TokenApp = message.TokenApp,
                        TokenUser = message.TokenUser,
                        Data = decryptMsg,
                        Info = message.Info,
                        AppVersion = message.AppVersion,
                        StatusOp = message.StatusOp,
                        OperationVersion = message.OperationVersion
                    };
                    request.sendJson(msg);

                });
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancelled by Backend");

            }*/

            #endregion

            return null;
        }
        
        public void StopOperation(Message message)
        {
            
            var decryptMessage = (DecryptMsg)message.Data;
            var tokenId = message.TokenUser + decryptMessage.FileName;

            if (userCancellationTokens.ContainsKey(tokenId))
            {
                userCancellationTokens[tokenId].Cancel();
                Console.WriteLine("STOPPED OPERATION FOR : " + tokenId);
            }
            else
            {
                Console.WriteLine("userCancellationTokens does not exist : " + tokenId);
            }
        }

        public void StopService()
        {
            globalCancellationSource.Cancel();
        }
    }
}
