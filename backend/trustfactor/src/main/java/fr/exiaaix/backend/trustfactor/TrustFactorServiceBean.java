package fr.exiaaix.backend.trustfactor;

import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import fr.exiaaix.backend.trustfactor.models.DecryptData;
import fr.exiaaix.backend.trustfactor.models.ServiceMessage;

import javax.ejb.ActivationConfigProperty;
import javax.ejb.MessageDriven;
import javax.inject.Inject;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageListener;
import javax.jms.TextMessage;
import java.lang.reflect.Type;

@MessageDriven(mappedName = "jms/messagingQueue", activationConfig =  {  
        @ActivationConfigProperty(propertyName = "acknowledgeMode", propertyValue = "Auto-acknowledge"),  
        @ActivationConfigProperty(propertyName = "destinationType", propertyValue = "javax.jms.Queue")  
    })  
public class TrustFactorServiceBean implements MessageListener {
    
    @Inject
    private WordManagerServiceBean worldManagerServiceBean;
    
    @Inject
    private PdfServiceBean pdfServiceBean;
    
    public TrustFactorServiceBean(){
        
    }
    
    @Override
    public void onMessage(Message msg) {


        TextMessage text = (TextMessage)msg;
        Gson gson =  new Gson();
        Type type = new TypeToken<ServiceMessage<DecryptData>>(){}.getType();
        ServiceMessage<DecryptData> serviceMessage;


        try {
            serviceMessage = gson.fromJson(text.getText(), type);
        } catch (JMSException e) {
            e.printStackTrace();
        }


        //System.out.print(worldManagerServiceBean.getWords(10000));
        try {
            //System.out.print(worldManagerServiceBean.getWords(10000));
            pdfServiceBean.createPdf("Hello world", "name.pdf");
        } catch (IOException ex) {
            Logger.getLogger(TrustFactorServiceBean.class.getName()).log(Level.SEVERE, null, ex);
        }
                
    }
    
    
}
