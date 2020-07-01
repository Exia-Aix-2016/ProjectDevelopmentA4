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
using Middleware.Models;
using System.Security.Authentication;

namespace Middleware.Services.Authentification
{
    public class AuthService : IService, IToken
    {
        private readonly string secret = "rjehke456zer21ZAdazdas5";
        public static readonly string APP_TOKEN = "e2lOmEf7z2YcWNOsMgwxrytjcOftPwpi";

        public LoginResult UserLogin(string login, string pass)
        {
            userContext db = new userContext();
            bool passBool = db.Users.Where(x => x.Login == login).Any(); 

            if (!passBool)
            {
                throw new InvalidCredentialException("Login Invalid");
            }

            var userBool = db.Users.Where(x => (x.Login == login) && (x.Password == pass)).Any();

            if (!userBool)
            {
                throw new InvalidCredentialException("Password Invalid");
            }

            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(secret)
                .AddClaim("validation", "yes")
                .AddClaim("user", login)
                .Encode();

            LoginResult result = new LoginResult();
            result.TokenUser = token;
            result.IsValid = true;

            return result;
        }
        public bool IsValidToken(string token)
        {
            if (token == null || token == string.Empty) return false;
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
            catch (Exception)
            {
                Console.WriteLine("bad user token");
                return false;
            }
        }

        public Message ServiceAction(Message message)
        {
            if(message.Data == null)
            {
                message.Info = "Data Null";
                message.OperationName = "DROP";
                return message;
            }

            Credential credential = (Credential)message.Data;
            

            try
            {
                message.Data = UserLogin(credential.Username, credential.Password);
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

        public bool IsValidAppToken(string token)
        {
            if (token == null || token == string.Empty) return false;

            return token == APP_TOKEN;
        }
    }
}