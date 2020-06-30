using Middleware.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Middleware
{
    class Server
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Middleware configuration...");

            //AuthService test = new AuthService();



            

            //INITIATIZATION OF NetTcpBinding

            NetTcpBinding netTcpBinding = new NetTcpBinding();
            netTcpBinding.Name = "NetTcpBinding";
            netTcpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            netTcpBinding.TransactionFlow = false;
            netTcpBinding.Security.Mode = SecurityMode.Message;
            netTcpBinding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Basic128;
            netTcpBinding.MaxBufferSize = 2147483647;
            netTcpBinding.MaxReceivedMessageSize = 2147483647;
            netTcpBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
            netTcpBinding.ReaderQuotas.MaxArrayLength = 2147483647;



            Uri tcpUri = new Uri(@"net.tcp://0.0.0.0:6969");

           using(ServiceHost serviceHost = new ServiceHost(typeof(Endpoint), tcpUri) ){
                serviceHost.AddServiceEndpoint(typeof(IEndpoint), netTcpBinding, tcpUri);

                
                
                //PART TO ABLE TO GET THE WSDL
                Uri wsdlUri = new Uri(@"http://0.0.0.0:8080/mex");
                var meta = new ServiceMetadataBehavior();
                meta.HttpGetEnabled = true;
                meta.HttpGetUrl = wsdlUri;
                serviceHost.Description.Behaviors.Add(meta);
                serviceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), wsdlUri);



                //PART REST API
                Uri restApi = new Uri(@"http://0.0.0.0:8080/api");

                var httpBinding = new WebHttpBinding();

                httpBinding.MaxBufferSize = 2147483647;
                httpBinding.MaxReceivedMessageSize = 2147483647;
                httpBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
                httpBinding.ReaderQuotas.MaxArrayLength = 2147483647;
                httpBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;
                httpBinding.ReaderQuotas.MaxDepth = 2147483647;

                var endpoint = serviceHost.AddServiceEndpoint(typeof(IRestAPI), httpBinding, restApi);
                endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
   


                Console.WriteLine("Middleware is starting...");
                serviceHost.Open();

                //serviceHost.Closed += (o, a) => Endpoint.decryptService.StopService();

                Console.ReadKey();
            }
        }
    }
}
