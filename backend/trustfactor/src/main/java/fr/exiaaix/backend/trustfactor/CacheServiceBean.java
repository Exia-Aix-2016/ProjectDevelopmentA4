package fr.exiaaix.backend.trustfactor;

import javax.enterprise.context.ApplicationScoped;
import java.util.Dictionary;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;

@ApplicationScoped
public class CacheServiceBean {

    private Map<String, String> filesCaches =  new HashMap<>();


    public void addFile(String filename, String text){

        filesCaches.put(filename, text);
    }

    public String getFile(String filename){
        return filesCaches.get(filename);
    }
}
