using System.Runtime.Serialization;

namespace Middleware
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "StatusOp")]
        public bool StatusOp { get; set; }

        [DataMember(Name = "Info")]
        public string Info { get; set; }

        [DataMember(Name = "Data")]
        public object[] Data { get; set; }

        [DataMember(Name = "OperationName")]
        public string OperationName { get; set; }

        [DataMember(Name = "TokenApp")]
        public string TokenApp { get; set; }

        [DataMember(Name = "TokenUser")]
        public string TokenUser { get; set; }

        [DataMember(Name = "AppVersion")]
        public string AppVersion { get; set; }

        [DataMember(Name = "OperationVersion")]
        public string OperationVersion { get; set; }
    }
}