using Middleware.Decrypt;
using Middleware.Models;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Services
{
    /// <summary>
    /// Decrypt Service
    /// </summary>
    public class DecryptService : IDecryptService
    {
        private ConcurrentQueue<DecryptMsgNamed> filesQueue;
        private CancellationTokenSource globalCancellationSource;
        private ConcurrentDictionary<string, CancellationTokenSource> userCancellationSource;



        public DecryptService()
        {
            filesQueue = new ConcurrentQueue<DecryptMsgNamed>();
            globalCancellationSource = new CancellationTokenSource();
            userCancellationSource = new ConcurrentDictionary<string, CancellationTokenSource>();

          

            for (int i = 0; i < Environment.ProcessorCount - 1; i++)// -1 to let a thread for our lovely Endpoint
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
                    var result = xorBreaker.BreakXor(decryptMsg.DecryptMsg.CipherText, 4, byUserToken);
                    //Send Nudes !
                    Console.WriteLine(result.timeToBreak);
                    sendResult(result.keyPlains);


                    if (byUserToken.IsCancellationRequested)
                    {
                        CancellationTokenSource cts;
                        if(!userCancellationSource.TryRemove(TokenCancellatioName, out cts))
                        {
                            throw new RemoveUserCancellationSourceException();
                        }
                    }
                }

                Thread.Sleep(10);//To not overload the thread.
            }
               
            Console.WriteLine("DecryptService Thread : " + Thread.CurrentThread.ManagedThreadId + " stopped");
        }

        /// <summary>
        /// Send result To Backend
        /// </summary>
        /// <param name="keyPlain"></param>
        private void sendResult(Dictionary<string,string> keyPlain)
        {
            //TODO: Send Result to Backend (JEE) via Http.
        }


        /// <summary>
        /// Will add Job into queue.
        /// </summary>
        /// <param name="message"></param>
        public void ServiceAction(Message message)
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
            Console.WriteLine(userCancellationSource.Count());
            globalCancellationSource.Cancel();
        }
    }
}
