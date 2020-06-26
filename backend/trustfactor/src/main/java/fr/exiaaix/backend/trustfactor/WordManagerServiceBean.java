package fr.exiaaix.backend.trustfactor;

import java.util.List;
import javax.enterprise.context.ApplicationScoped;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.persistence.Query;

@ApplicationScoped
public class WordManagerServiceBean {
    
    @PersistenceContext(unitName = "XE")
    private EntityManager entityManager;
    
    public List<Word> getWords(){     
        Query query = entityManager.createQuery("SELECT w FROM Word w", Word.class);
        return query.getResultList();
    }
    
    
}
