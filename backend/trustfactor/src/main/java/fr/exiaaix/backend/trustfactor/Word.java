package fr.exiaaix.backend.trustfactor;

import java.io.Serializable;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

@Entity
@Table(name="FrenchWord")
public class Word implements Serializable {
    
    @Id
    private String word;
    private double frequency;
    
    public String getWord(){
        return this.word;
    }
    
    public double getFrequency(){
        return this.frequency;
    }
    
}
