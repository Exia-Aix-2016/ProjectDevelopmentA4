package fr.exiaaix.backend.trustfactor.models;

public class ServiceMessage<T>
{
    public String OperationName;
    public String TokenUser;
    public Boolean StatusOp;
    public String Info;

    public T Data;
    public String TokenApp;
    public String AppVersion;
    public String OperationVersion;

}
