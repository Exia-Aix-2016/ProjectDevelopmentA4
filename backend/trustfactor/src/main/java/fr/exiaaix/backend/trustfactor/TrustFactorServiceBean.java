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
import java.io.IOException;
import java.lang.reflect.Type;
import java.util.List;

@MessageDriven(mappedName = "jms/messagingQueue", activationConfig =  {  
        @ActivationConfigProperty(propertyName = "acknowledgeMode", propertyValue = "Auto-acknowledge"),  
        @ActivationConfigProperty(propertyName = "destinationType", propertyValue = "javax.jms.Queue")  
    })  
public class TrustFactorServiceBean implements MessageListener {
    
    @Inject
    private WordManagerServiceBean worldManagerServiceBean;

    @Inject
    WebClientServiceBean webClient;
    
    @Inject
    private PdfServiceBean pdfServiceBean;
    
    private List<Word> listWord = wordManagerServiceBean.getWords(10000);
    
    public TrustFactorServiceBean(){
        
    }
    
    @Override
    public void onMessage(Message msg) {
        
        ServiceMessage<DecryptData> serviceMessage = convertMessage(msg);

        double percentage = calculatePercentage(serviceMessage.Data.PlainText);
        
        System.out.println(percentage + "%");
        
        generatePdf(serviceMessage);     
                     
    }
    
    private ServiceMessage<DecryptData> convertMessage(Message msg){
        TextMessage text = (TextMessage)msg;
        Gson gson =  new Gson();
        Type type = new TypeToken<ServiceMessage<DecryptData>>(){}.getType();
        ServiceMessage<DecryptData> serviceMessage = null;

        try {
            serviceMessage = gson.fromJson(text.getText(), type);

        } catch (JMSException e) {
            e.printStackTrace();
        }
        return serviceMessage;
    }
    
    private double calculatePercentage(String text){
        int foundWords = 0;
        String[] plainText = text.split("\\s+");
                       
        for (int i = 0; i < plainText.length; i++) {
            if(listWord.contains(plainText[i]))
                foundWords++;
        }

        return foundWords / plainText.length;
    }
    
    private void generatePdf(ServiceMessage<DecryptData> serviceMessage){
        try {
            pdfServiceBean.createPdf(serviceMessage.Data.Report, serviceMessage.Data.FileName);
        } catch (IOException ex) {
            Logger.getLogger(TrustFactorServiceBean.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
}
