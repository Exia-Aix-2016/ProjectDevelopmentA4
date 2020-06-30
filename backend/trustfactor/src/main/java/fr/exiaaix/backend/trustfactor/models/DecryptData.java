package fr.exiaaix.backend.trustfactor.models;

public class DecryptData
{
    //For WCF Compatibilities
    public final String __type = "DecryptMsg:#Middleware.Models";
    public String FileName;

    public String CipherText;

    public String PlainText;

    public byte[] Report;

    public String Key;

    public String Secret;
}
