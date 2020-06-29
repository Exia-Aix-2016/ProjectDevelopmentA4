package fr.exiaaix.backend.trustfactor.models;

import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import javax.jms.JMSException;
import javax.json.bind.annotation.JsonbTypeDeserializer;

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
