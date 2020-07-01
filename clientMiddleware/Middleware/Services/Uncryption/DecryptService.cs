using Middleware.Decrypt;
using Middleware.Models;
using Middleware.Services.Authentification;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private XorBreaker xorBreaker = new XorBreaker();

        private CancellationTokenSource globalCancellationSource { get; set; } = new CancellationTokenSource();


        /// <summary>
        /// used to break a text by index of coincidence, Smart attack but not work with  plaintext that sucks!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool byIndex(Message message)
        {
            var decryptMessage = (DecryptMsg)message.Data;

            var results = xorBreaker.breakXor(decryptMessage.CipherText, 4, globalCancellationSource.Token, 0.06);

            if (results.Count() == 0) return false;

            Parallel.ForEach(results, new ParallelOptions { CancellationToken = globalCancellationSource.Token }, (kv) =>
            {
                var request = new RequestHttp();
                var decryptMsg = new DecryptMsg
                {
                    FileName = decryptMessage.FileName,
                    Key = kv.Key,
                    PlainText = kv.Value
                };

                var msg = new Message
                {
                    OperationName = message.OperationName,
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
            return true;
        }


        public Message ServiceAction(Message message)
        {
            var decryptMessage = (DecryptMsg)message.Data;

            Console.WriteLine("COUNT KEYS : " + xorBreaker.Keys.Count);


            //We first attack by index of coincidence
            if (byIndex(message)) return null;


            //If the first attack didn't work, well... go bruteforce...
            Parallel.ForEach(xorBreaker.Keys, new  ParallelOptions{ CancellationToken = globalCancellationSource.Token }, (key) =>
            {
                var request = new RequestHttp();
                var plain = CryptoTools.Xor(decryptMessage.CipherText, key);
                
                //We filter all the key below a low Index (to take a maximum keys but not too much)
                if (plain.Ic2() < 0.05)
                    return;
                
                var decryptMsg = new DecryptMsg
                {
                    FileName = decryptMessage.FileName,
                    Key = key,
                    PlainText = plain
                };

                var msg = new Message
                {
                    OperationName = message.OperationName,
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
            return null;
        }
        
        public void StopOperation(Message message)
        {
            Console.WriteLine("CANCEL");
            globalCancellationSource.Cancel();
        }

        public void StopService()
        {
            globalCancellationSource.Cancel();
        }






































        /*
        private ConcurrentQueue<DecryptMsgNamed> filesQueue { get; set; }
        private CancellationTokenSource globalCancellationSource { get; set; }
        private ConcurrentDictionary<string, CancellationTokenSource> userCancellationSource { get; set; }

        private RequestHttp request { get; set; }



        public DecryptService(Uri uri)
        {
            request = new RequestHttp(uri);
            filesQueue = new ConcurrentQueue<DecryptMsgNamed>();
            globalCancellationSource = new CancellationTokenSource();
            userCancellationSource = new ConcurrentDictionary<string, CancellationTokenSource>();

          

            for (int i = 0; i < Environment.ProcessorCount - 2; i++)// -1 to let a thread for our lovely Endpoint
            {
                ThreadPool.QueueUserWorkItem(Decrypt, globalCancellationSource.Token);

            }
        }

        /// <summary>
        /// Worker
        /// </summary>
        /// <param name="obj"></param>
        private void Decrypt(object obj)
        {
            var token = (CancellationToken)obj;//Get the main cancellation token
            XorBreaker xorBreaker = new XorBreaker();
            
            while (!token.IsCancellationRequested)
            {
                DecryptMsgNamed decryptMsg;
                
                if(filesQueue.TryDequeue(out decryptMsg))
                {
                    var TokenCancellatioName = decryptMsg.UserToken + decryptMsg.DecryptMsg.FileName;
                    //Check by users if a cancellation has been requested.
                    var byUserToken = userCancellationSource[TokenCancellatioName].Token;
                    
                    //Break it !
                    foreach(var kv in xorBreaker.breakXor(decryptMsg.DecryptMsg.CipherText, 4, byUserToken, 0.06))
                    {
                        sendResult(decryptMsg.DecryptMsg.FileName, decryptMsg.UserToken, kv);
                    }
  
                    
                    if (byUserToken.IsCancellationRequested)
                    {
                        CancellationTokenSource cts;
                        if(!userCancellationSource.TryRemove(TokenCancellatioName, out cts))
                        {
                            throw new RemoveUserCancellationSourceException();
                        }
                    }
                }

                Thread.Sleep(10);//To not overload the CPU.
            }
               
            Console.WriteLine("DecryptService Thread : " + Thread.CurrentThread.ManagedThreadId + " stopped");
        }

        /// <summary>
        /// Send result To Backend
        /// </summary>
        /// <param name="keyPlain"></param>
        private void sendResult(string filename, string userToken, KeyValuePair<string, string> keyPlain)
        {
            //TODO: Send Result to Backend (JEE) via Http.

            //KeyPlain : [CLEF : Texte en clair]
            DecryptMsg decryDecryptMsg = new DecryptMsg
            {
                FileName = filename,
                CipherText = "",
                PlainText = keyPlain.Value,
                Key = keyPlain.Key
            };

           

            Message message = new Message
            {
                OperationName = "CHECK",
                Data = decryDecryptMsg,
                TokenUser = userToken,
                TokenApp = AuthService.APP_TOKEN
            };

            request.sendJson(message);    
        }


        /// <summary>
        /// Will add Job into queue.
        /// </summary>
        /// <param name="message"></param>
        public Message ServiceAction(Message message)
        {
            var uct = CancellationTokenSource.CreateLinkedTokenSource(globalCancellationSource.Token);
            if (message != null && message.TokenUser != string.Empty)
            {
                var decryptMessage = (DecryptMsg)message.Data;
                if(userCancellationSource.TryAdd(message.TokenUser+decryptMessage.FileName, uct))
                {
                    filesQueue.Enqueue(new DecryptMsgNamed { UserToken = message.TokenUser, DecryptMsg = decryptMessage });
                }  
            }

            return null;
        }

        /// <summary>
        /// Stop Process Just on userFile
        /// </summary>
        /// <param name="message"></param>
        public void StopOperation(Message message)
        {
            if(message != null && message.TokenUser != string.Empty)
            {

                var cipher = (DecryptMsg)message.Data;
                if(userCancellationSource.ContainsKey(message.TokenUser + cipher.FileName))
                {
                    userCancellationSource[message.TokenUser + cipher.FileName].Cancel();
                }
                
            }

                
        }

        /// <summary>
        /// Stop all the service.
        /// </summary>
        public void StopService()
        {
            Console.WriteLine("Count Token Cancellation : " + userCancellationSource.Count());
            globalCancellationSource.Cancel();

            Console.WriteLine("DecryptService is Closed");
        }
        
         */



    }
}
