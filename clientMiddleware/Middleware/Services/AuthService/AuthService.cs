using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using JWT.Algorithms;
using JWT;
using JWT.Builder;
using System.Reflection.Emit;
using JWT.Exceptions;
using JWT.Serializers;
using Middleware.Services;
using System.ServiceModel;
using Middleware.Services.AuthService;
using Middleware.Models;
using System.Security.Authentication;

namespace Middleware
{
    public class AuthService : IService, IToken
    {
        private readonly string secret = "rjehke456zer21ZAdazdas5";

        public LoginResult UserLogin(string login, string pass)
        {
            //Comment récupérer Login/Password et la connexion a la bdd

            userContext db = new userContext();
            
            bool passBool = db.Users.Where(x => x.Login == login).Any(); //Change to return bool

            if (!passBool)
            {
                //TODO Retourner au client pour user incorrect
                // throw error
                throw new InvalidCredentialException("Login Invalid");
            }


            var userBool = db.Users.Where(x => (x.Login == login) && (x.Password == pass)).Any();//SingleOrDefault

            if (!userBool)
            {
                //TODO Retourner au client pour pass incorrect
                // throw error
                throw new InvalidCredentialException("Password Invalid");
            }


            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                .WithSecret(secret)
                .AddClaim("validation", "yes")
                .Encode();

            LoginResult result = new LoginResult();
            result.TokenUser = token;
            result.IsValid = true;

            return result;
        }
        public bool IsValidToken(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                string json = decoder.Decode(token, secret, verify: true);
                return true;

            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token expiré");
                return false;
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Signature erronée");
                return false;
            }
        }

        public Message ServiceAction(Message message)
        {
            if(message.Data == null)
            {
                message.Info = "Data NullReferenceException";
                message.OperationName = "DROPMESSAGE";
                return message;
            }
            Credential credential = (Credential)message.Data;

            AuthService auth = new AuthService();


            try
            {
                message.Data = auth.UserLogin(credential.Username, credential.Password);
                message.OperationName = "TOKEN";
                return message;
            }
            catch(Exception err)
            {
                message.OperationName = "DROPMESSAGE";
                message.Info = err.ToString();
                message.Data = null;
                return message;

            }
            
        }

        public void StopService()
        {
            Console.WriteLine("AuthService is Closed");
        }
    }
}