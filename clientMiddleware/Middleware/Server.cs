using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
    class Server
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Middleware configuration...");

            //INITIATIZATION OF NetTcpBinding
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            netTcpBinding.Name = "NetTcpBinding";
            netTcpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            netTcpBinding.TransactionFlow = false;
            netTcpBinding.Security.Mode = SecurityMode.Message;
            netTcpBinding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Basic128Sha256;

            Uri tcpUri = new Uri(@"net.tcp://0.0.0.0:6969");
           
            ServiceHost serviceHost = new ServiceHost(typeof(Endpoint), tcpUri);
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
            var endpoint = serviceHost.AddServiceEndpoint(typeof(IRestAPI), new WebHttpBinding(), restApi);
            endpoint.EndpointBehaviors.Add(new WebHttpBehavior());


            

            Console.WriteLine("Middleware is starting...");
            serviceHost.Open();

            Console.ReadLine();

            serviceHost.Close();


        }
    }
}
