package fr.exiaaix.backend.trustfactor;


import java.io.IOException;
import java.util.HashSet;
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
import java.text.Normalizer;
import java.util.HashSet;
import java.util.logging.Level;
import java.util.logging.Logger;


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
    
    @Inject
    private MailServiceBean mailServiceBean;
        
    public TrustFactorServiceBean(){
        
    }
    
    @Override
    public void onMessage(Message msg) {
        
       HashSet<String> listWord = new HashSet<>();

        wordManagerServiceBean.getWords(0).forEach(w -> listWord.add(w.getWord()));
        
       ServiceMessage<DecryptData> serviceMessage = convertMessage(msg);

       //Do not put it in serviceMessage.Data.PlainText
       String sanitizedPlain = sanitizePlainText(serviceMessage.Data.PlainText);

        double percentage = calculatePercentage(sanitizedPlain, listWord);

        if(percentage > 33.33){
            serviceMessage.Data.Report =  generatePdf(serviceMessage, percentage);

            
            try {
                webClient.sendResult(serviceMessage);
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        //mailServiceBean.sendMail(); ajouter secret file key



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
    
    private double calculatePercentage(String text, HashSet<String> listWord){
        double foundWords = 0.0;
        String[] textSplit = text.split(" ");

        for (String word: textSplit) {
            if(listWord.contains(word)){
                foundWords++;
            }
        }

        return foundWords / (double)textSplit.length * 100.0;
    }
    
    private byte[] generatePdf(ServiceMessage<DecryptData> serviceMessage, double percentageOfWords){
        try {
            return pdfServiceBean.createPdf(serviceMessage.Data, percentageOfWords);
        } catch (IOException ex) {
            Logger.getLogger(TrustFactorServiceBean.class.getName()).log(Level.SEVERE, null, ex);
            return null;
        }
    }


    private String sanitizePlainText(String text){

        text = Normalizer.normalize(text, Normalizer.Form.NFD);
        text = text.replaceAll("[\\p{InCombiningDiacriticalMarks}]", "");

        return text.toLowerCase();
    }


    private Boolean checkSecret(String plain){

        return plain.contains("l'information secrete est :");
    }
}
