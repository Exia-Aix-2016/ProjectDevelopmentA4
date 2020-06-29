﻿using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Middleware.Decrypt;
using Middleware.Models;
using Middleware.Services;

namespace MiddlewareUnitTests
{
    [TestClass]
    public class DecryptServiceUnitTests
    {
        private DecryptService decryptService;
        [TestInitialize]
        public void Init()
        {
            decryptService = new DecryptService(new Uri("https://enuw1kd3dqhr.x.pipedream.net"));
        }
        [TestMethod]
        public void TestDecryption()
        {

            string readText = File.ReadAllText("test.txt");


            var cipher = CryptoTools.Xor(readText, "TEST");
            Console.WriteLine(readText.Ic());
            decryptService.ServiceAction(new Message { 
                Data = new DecryptMsg { 
                    CipherText = cipher, 
                    FileName = "test.txt" }, 
                OperationName = "DECRYPT",
                TokenUser = "TEST"});


            Thread.Sleep(5000);


        }
    }
}
