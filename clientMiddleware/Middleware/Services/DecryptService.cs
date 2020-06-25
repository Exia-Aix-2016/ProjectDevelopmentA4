using Middleware.Decrypt;
using Middleware.Models;
using Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware.Services
{
    public class DecryptService : IDecryptService
    {
        private BlockingCollection<DecryptMsgNamed> filesQueue;
        private CancellationTokenSource cancellationTokenSource;

        private ConcurrentDictionary<string, CancellationTokenSource> cts;



        public DecryptService()
        {
            filesQueue = new BlockingCollection<DecryptMsgNamed>();
            cancellationTokenSource = new CancellationTokenSource();


            for (int i = 0; i < Environment.ProcessorCount - 1; i++)
            {
                ThreadPool.QueueUserWorkItem(Decrypt, cancellationTokenSource.Token);

            }
        }


        private void Decrypt(object obj)
        {
            var token = (CancellationToken)obj;
            XorBreaker xorBreaker = new XorBreaker();
            while (!token.IsCancellationRequested)
            {
                DecryptMsgNamed decryptMsg = filesQueue.Take(token);

                var byUserToken = cts[decryptMsg.UserToken].Token;

                if (byUserToken.IsCancellationRequested)
                {
                    break;
                }

                var result = xorBreaker.BreakXor(decryptMsg.DecryptMsg.CipherText, 4);

                sendResult(result.keyPlains);
            }
        }

        private void sendResult(Dictionary<string,string> keyPlain)
        {
            //TODO SEND 
        }



        public void ServiceAction(Message message)
        {
            if(message != null && message.TokenUser != string.Empty && cts.TryAdd(message.TokenUser, new CancellationTokenSource()))
            {
                var decryptMessage = (DecryptMsg) message.Data;

                var dmsgN = new DecryptMsgNamed { UserToken = message.TokenUser, DecryptMsg = decryptMessage };

                filesQueue.Add(dmsgN);
            }
        }

        /// <summary>
        /// Stop Process Just on userFile
        /// </summary>
        /// <param name="message"></param>
        public void StopOperation(Message message)
        {
            if(message != null && message.TokenUser != string.Empty && cts.ContainsKey(message.TokenUser))
                cts[message.TokenUser].Cancel();
        }

        /// <summary>
        /// Stop all the service.
        /// </summary>
        public void StopService()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
