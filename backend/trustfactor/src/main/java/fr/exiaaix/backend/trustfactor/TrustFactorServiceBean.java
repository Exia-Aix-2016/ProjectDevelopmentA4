package fr.exiaaix.backend.trustfactor;


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
import java.sql.SQLOutput;
import java.text.Normalizer;
import java.util.HashSet;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

@MessageDriven(mappedName = "jms/messagingQueue", activationConfig =  {  
        @ActivationConfigProperty(propertyName = "acknowledgeMode", propertyValue = "Auto-acknowledge"),  
        @ActivationConfigProperty(propertyName = "destinationType", propertyValue = "javax.jms.Queue") ,
        @ActivationConfigProperty(propertyName = "maxSession", propertyValue = "50")
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

    /**
     * Event executed when a message is in the queue
     * @param msg JMS Message
     */
    @Override
    public void onMessage(Message msg) {

        ServiceMessage<DecryptData> serviceMessage = convertMessage(msg);
       //Do not put it in serviceMessage.Data.PlainText
        String sanitizedPlain = sanitizePlainText(serviceMessage.Data.PlainText);

       //Calculate the percentage of FrenchWords of the given Plaintext
        double percentage = calculatePercentage(sanitizedPlain, wordManagerServiceBean.getWords());

        if(percentage > 20){
            System.out.println(" -------- " + serviceMessage.Data.Key + " : " + percentage);
            //will try to get the secret from the plaintext
            String secret = checkSecret(sanitizedPlain);
            if(!secret.equals("")){
                serviceMessage.Data.Secret = secret;
                mailServiceBean.sendMail(secret,serviceMessage.Data.FileName, serviceMessage.Data.Key);
            }
            //create PDF report
            serviceMessage.Data.Report =  generatePdf(serviceMessage, percentage);

            //Send back to Middleware the Result (will stop the process on this file)
            try {
                webClient.sendResult(serviceMessage);
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

    }

    /**
     * Converts JMS Message toServiceMessage
     * @param msg JMS Message from the Queue
     * @return ServiceMessage
     */
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

    /**
     * Calculate the percentage of matching words (from listWord) in the text
     * @param text text to check
     * @param listWord List of word used to check
     * @return the percentage of matching
     */
    private double calculatePercentage(String text, HashSet<String> listWord){
        double foundWords = 0.0;
        String[] textSplit = text.split(" ");

        for (int i = 0; i < textSplit.length; i++) {
            if(listWord.contains(textSplit[i])){
                foundWords++;
            }
        }
        return foundWords / (double)textSplit.length * 100.0;
    }

    /**
     * Generate the PDF report of given DecryptData
     * @param serviceMessage Message which contains the DecryptData
     * @param percentageOfWords Percentage of matching word in plaintext
     * @return the pdf in byte array
     */
    private byte[] generatePdf(ServiceMessage<DecryptData> serviceMessage, double percentageOfWords){
        try {
            return pdfServiceBean.createPdf(serviceMessage.Data, percentageOfWords);
        } catch (IOException ex) {
            Logger.getLogger(TrustFactorServiceBean.class.getName()).log(Level.SEVERE, null, ex);
            return null;
        }
    }


    /**
     * To remove all accents of the text and put it in lowercase
     * @param text
     * @return
     */
    private String sanitizePlainText(String text){

        text = Normalizer.normalize(text, Normalizer.Form.NFD);
        text = text.replaceAll("[\\p{InCombiningDiacriticalMarks}]", "");

        return text.toLowerCase();
    }

    /**
     * To get the secret from given plaintext
     * @param plain plaintext where looking for the secret.
     * @return the secret
     */
    private String checkSecret(String plain){
        Pattern pattern = Pattern.compile("(l'information secrete est :) ([a-z A-Z]*.)");

        Matcher matcher = pattern.matcher(plain);

        if(matcher.find()){
            return matcher.group(2);
        }

        return "";
    }
}
