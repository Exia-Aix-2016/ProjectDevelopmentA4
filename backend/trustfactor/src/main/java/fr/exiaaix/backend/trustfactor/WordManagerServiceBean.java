package fr.exiaaix.backend.trustfactor;

import java.util.HashSet;
import java.util.List;
import javax.annotation.PostConstruct;
import javax.enterprise.context.ApplicationScoped;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.persistence.Query;

@ApplicationScoped
public class WordManagerServiceBean {
    
    @PersistenceContext(unitName = "persistenceUnit")
    private EntityManager entityManager;

    //stock words into RAM
    private HashSet<String> words = new HashSet<>();


    @PostConstruct()
    public void init(){
        getWords(0).forEach(w -> words.add(w.getWord()));
    }
    
    private List<Word> getWords(int frequencyMin){
        Query query = entityManager.createQuery("SELECT w FROM Word w WHERE w.frequency >= :frequencyMin", Word.class).setParameter("frequencyMin", frequencyMin);
        return query.getResultList();
    }

    public HashSet<String> getWords() {
        return words;
    }
}
