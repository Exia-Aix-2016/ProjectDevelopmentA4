using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Middleware.Decrypt;
using Middleware.Models;
using Middleware.Services.Uncryption;

namespace MiddlewareUnitTests
{
    [TestClass]
    public class DecryptServiceUnitTests
    {
        private DecryptService decryptService;
        [TestInitialize]
        public void Init()
        {
            decryptService = new DecryptService(new Uri("http://192.168.20.10:8282/webservice/resources/cipher"));
        }
        [TestMethod]
        public void TestDecryption()
        {

            string readText = File.ReadAllText("test.txt");


            var cipher = CryptoTools.Xor(readText, "TEST");


            decryptService.ServiceAction(new Message { 
                Data = new DecryptMsg { 
                    CipherText = cipher, 
                    FileName = "test.txt" }, 
                OperationName = "DECRYPT",
                TokenUser = "TEST"});

            decryptService.ServiceAction(new Message
            {
                Data = new DecryptMsg
                {
                    CipherText = File.ReadAllText("test2.txt"),
                    FileName = "test2.txt"
                },
                OperationName = "DECRYPT",
                TokenUser = "TEST"
            });

            decryptService.ServiceAction(new Message
            {
                Data = new DecryptMsg
                {
                    CipherText = File.ReadAllText("test3.txt"),
                    FileName = "test3.txt"
                },
                OperationName = "DECRYPT",
                TokenUser = "TEST"
            });



            Thread.Sleep(5000);


            decryptService.StopService();


        }
    }
}
