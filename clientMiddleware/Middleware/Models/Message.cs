using System.Runtime.Serialization;

namespace Middleware.Models
{
    [DataContract]
    [KnownType(typeof(Credential))]
    [KnownType(typeof(LoginResult))]
    [KnownType(typeof(DecryptMsg))]
    public class Message
    {
        [DataMember(Name = "OperationName", EmitDefaultValue = false)]
        public string OperationName { get; set; }
        [DataMember(Name = "TokenUser", EmitDefaultValue = false)]
        public string TokenUser { get; set; }

        [DataMember(Name = "StatusOp", EmitDefaultValue = false)]
        public bool StatusOp { get; set; }

        [DataMember(Name = "Info", EmitDefaultValue = false)]
        public string Info { get; set; }

        [DataMember(Name = "Data", EmitDefaultValue = false)]
        public object Data { get; set; }

        [DataMember(Name = "TokenApp", EmitDefaultValue = false)]
        public string TokenApp { get; set; }

        [DataMember(Name = "AppVersion", EmitDefaultValue = false)]
        public string AppVersion { get; set; }

        [DataMember(Name = "OperationVersion", EmitDefaultValue = false)]
        public string OperationVersion { get; set; }
    }
}