package fr.exiaaix.backend.trustfactor;

import javax.ejb.Singleton;
import java.util.concurrent.ConcurrentHashMap;

@Singleton
public class CacheServiceBean {

    private ConcurrentHashMap<String, String> filesCaches = new ConcurrentHashMap<>();

    public void addFile(String filename, String text) {
        filesCaches.put(filename, text);
    }

    public String getFile(String filename) {
        return filesCaches.get(filename);
    }
}
