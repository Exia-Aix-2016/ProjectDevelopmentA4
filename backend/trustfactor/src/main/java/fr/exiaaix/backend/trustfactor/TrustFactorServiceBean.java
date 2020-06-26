package fr.exiaaix.backend.trustfactor;

import javax.ejb.ActivationConfigProperty;
import javax.ejb.MessageDriven;
import javax.inject.Inject;
import javax.jms.Message;
import javax.jms.MessageListener;

@MessageDriven(mappedName = "jms/messagingQueue", activationConfig =  {  
        @ActivationConfigProperty(propertyName = "acknowledgeMode", propertyValue = "Auto-acknowledge"),  
        @ActivationConfigProperty(propertyName = "destinationType", propertyValue = "javax.jms.Queue")  
    })  
public class TrustFactorServiceBean implements MessageListener {
    
    @Inject
    private WordManagerServiceBean worldManagerServiceBean;
    
    public TrustFactorServiceBean(){
        
    }
    
    @Override
    public void onMessage(Message msg) {
        System.out.print(worldManagerServiceBean.getWords(0));
    }
    
    
}
