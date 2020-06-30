using System.Runtime.Serialization;

namespace Middleware.Models
{

    /// <summary>
    /// Main model which used to carry all information between all layers of the solution Client <-> Middleware <-> Backend
    /// </summary>
    [DataContract]
    [KnownType(typeof(Credential))]
    [KnownType(typeof(LoginResult))]
    [KnownType(typeof(DecryptMsg))]
    public class Message
    {
        /// <summary>
        /// Used to select by which service the message will be process
        /// </summary>
        [DataMember(Name = "OperationName", EmitDefaultValue = false)]
        public string OperationName { get; set; }
        /// <summary>
        /// Used to authentificate the user by its Token
        /// </summary>
        [DataMember(Name = "TokenUser", EmitDefaultValue = false)]
        public string TokenUser { get; set; }


        [DataMember(Name = "StatusOp", EmitDefaultValue = false)]
        public bool StatusOp { get; set; }

        /// <summary>
        /// Will contains some information like Exceptions etc...
        /// </summary>
        [DataMember(Name = "Info", EmitDefaultValue = false)]
        public string Info { get; set; }

        /// <summary>
        /// Can contains a DecryptMsg, Credentials or LoginResult
        /// </summary>
        [DataMember(Name = "Data", EmitDefaultValue = false)]
        public object Data { get; set; }

        /// <summary>
        /// Token used to authorize a heavy client
        /// </summary>
        [DataMember(Name = "TokenApp", EmitDefaultValue = false)]
        public string TokenApp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "AppVersion", EmitDefaultValue = false)]
        public string AppVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "OperationVersion", EmitDefaultValue = false)]
        public string OperationVersion { get; set; }
    }
}