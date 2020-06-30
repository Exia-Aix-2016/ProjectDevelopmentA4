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
    private WordManagerServiceBean wordManagerServiceBean;

    @Inject
    WebClientServiceBean webClient;
    
    @Inject
    private PdfServiceBean pdfServiceBean;
        
    public TrustFactorServiceBean(){
        
    }
    
    @Override
    public void onMessage(Message msg) {
        
       List<Word> listWord = wordManagerServiceBean.getWords(10000);
        
       ServiceMessage<DecryptData> serviceMessage = convertMessage(msg);

        double percentage = calculatePercentage(serviceMessage.Data.PlainText, listWord);
        
        System.out.println(percentage + "%");
        
        serviceMessage.Data.Report =  generatePdf(serviceMessage);

        try {
            webClient.sendResult(serviceMessage);
        } catch (IOException e) {
            e.printStackTrace();
        }

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
    
    private double calculatePercentage(String text, List<Word> listWord){
        double foundWords = 0.0;

        for (Word word: listWord) {
            if(text.contains(word.getWord()))
                foundWords++;

        }

        return foundWords / (double)text.split(" ").length * 100.0;
    }
    
    private byte[] generatePdf(ServiceMessage<DecryptData> serviceMessage){
        try {
            return pdfServiceBean.createPdf(serviceMessage.Data);
        } catch (IOException ex) {
            Logger.getLogger(TrustFactorServiceBean.class.getName()).log(Level.SEVERE, null, ex);
            return null;
        }
    }
    
}
