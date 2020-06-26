package fr.exiaaix.backend.trustfactor;

import java.util.List;
import javax.enterprise.context.ApplicationScoped;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.persistence.Query;

@ApplicationScoped
public class WordManagerServiceBean {
    
    @PersistenceContext(unitName = "persistenceUnit")
    private EntityManager entityManager;
    
    public List<Word> getWords(int frequencyMin){     
        Query query = entityManager.createQuery("SELECT w FROM Word w WHERE w.frequency >= :frequencyMin", Word.class).setParameter("frequencyMin", frequencyMin);
        return query.getResultList();
    }
    
}
