using System.Runtime.Serialization;

namespace Middleware
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "op", EmitDefaultValue = false)]
        public string OperationName { get; set; }
        [DataMember(Name = "token", EmitDefaultValue = false)]
        public string TokenUser { get; set; }

        /*[DataMember(Name = "StatusOp", EmitDefaultValue = false)]
        public bool StatusOp { get; set; }

        [DataMember(Name = "Info", EmitDefaultValue = false)]
        public string Info { get; set; }

        [DataMember(Name = "Data", EmitDefaultValue = false)]
        public object[] Data { get; set; }



        [DataMember(Name = "TokenApp", EmitDefaultValue = false)]
        public string TokenApp { get; set; }



        [DataMember(Name = "AppVersion", EmitDefaultValue = false)]
        public string AppVersion { get; set; }

        [DataMember(Name = "OperationVersion", EmitDefaultValue = false)]
        public string OperationVersion { get; set; }*/
    }
}