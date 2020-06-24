﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.EndpointServiceRef {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Message", Namespace="http://schemas.datacontract.org/2004/07/Middleware")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(object[]))]
    public partial class Message : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AppVersionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private object[] DataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string InfoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OperationNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OperationVersionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool StatusOpField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TokenAppField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TokenUserField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AppVersion {
            get {
                return this.AppVersionField;
            }
            set {
                if ((object.ReferenceEquals(this.AppVersionField, value) != true)) {
                    this.AppVersionField = value;
                    this.RaisePropertyChanged("AppVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public object[] Data {
            get {
                return this.DataField;
            }
            set {
                if ((object.ReferenceEquals(this.DataField, value) != true)) {
                    this.DataField = value;
                    this.RaisePropertyChanged("Data");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Info {
            get {
                return this.InfoField;
            }
            set {
                if ((object.ReferenceEquals(this.InfoField, value) != true)) {
                    this.InfoField = value;
                    this.RaisePropertyChanged("Info");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OperationName {
            get {
                return this.OperationNameField;
            }
            set {
                if ((object.ReferenceEquals(this.OperationNameField, value) != true)) {
                    this.OperationNameField = value;
                    this.RaisePropertyChanged("OperationName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OperationVersion {
            get {
                return this.OperationVersionField;
            }
            set {
                if ((object.ReferenceEquals(this.OperationVersionField, value) != true)) {
                    this.OperationVersionField = value;
                    this.RaisePropertyChanged("OperationVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool StatusOp {
            get {
                return this.StatusOpField;
            }
            set {
                if ((this.StatusOpField.Equals(value) != true)) {
                    this.StatusOpField = value;
                    this.RaisePropertyChanged("StatusOp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TokenApp {
            get {
                return this.TokenAppField;
            }
            set {
                if ((object.ReferenceEquals(this.TokenAppField, value) != true)) {
                    this.TokenAppField = value;
                    this.RaisePropertyChanged("TokenApp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TokenUser {
            get {
                return this.TokenUserField;
            }
            set {
                if ((object.ReferenceEquals(this.TokenUserField, value) != true)) {
                    this.TokenUserField = value;
                    this.RaisePropertyChanged("TokenUser");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EndpointServiceRef.IEndpoint", CallbackContract=typeof(Client.EndpointServiceRef.IEndpointCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IEndpoint {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IEndpoint/MService")]
        void MService(Client.EndpointServiceRef.Message message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IEndpoint/MService")]
        System.Threading.Tasks.Task MServiceAsync(Client.EndpointServiceRef.Message message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEndpointCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IEndpoint/MServiceCallback")]
        void MServiceCallback(Client.EndpointServiceRef.Message message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEndpointChannel : Client.EndpointServiceRef.IEndpoint, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EndpointClient : System.ServiceModel.DuplexClientBase<Client.EndpointServiceRef.IEndpoint>, Client.EndpointServiceRef.IEndpoint {
        
        public EndpointClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public EndpointClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public EndpointClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public EndpointClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public EndpointClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void MService(Client.EndpointServiceRef.Message message) {
            base.Channel.MService(message);
        }
        
        public System.Threading.Tasks.Task MServiceAsync(Client.EndpointServiceRef.Message message) {
            return base.Channel.MServiceAsync(message);
        }
    }
}
