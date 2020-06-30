package fr.exiaaix.backend.trustfactor;

import com.google.gson.Gson;
import fr.exiaaix.backend.trustfactor.models.DecryptData;
import fr.exiaaix.backend.trustfactor.models.ServiceMessage;

import javax.enterprise.context.RequestScoped;
import java.io.IOException;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.logging.Level;
import java.util.logging.Logger;

@RequestScoped
public class WebClientServiceBean {


    public  void sendResult(ServiceMessage<DecryptData> message) throws IOException {

        URL url = new URL("http://192.168.20.11:8080/api/MServiceRest");//Will be different

        //Convert to Json
        Gson gson =  new Gson();
        String json = gson.toJson(message);


        //Create Connection
        HttpURLConnection con = (HttpURLConnection) url.openConnection();
        con.setRequestMethod("POST");
        con.setDoOutput(true);
        con.setRequestProperty("Content-Type", "application/json");
        con.setConnectTimeout(2000);//5sec of timeout

        //Send Data
        try(OutputStream os = con.getOutputStream()){
            os.write(json.getBytes("UTF-8"));
            os.flush();
        }


        int httpResult = con.getResponseCode();

        Logger.getLogger(WebClientServiceBean.class.getName()).log(Level.INFO, "Sended code Response : " + httpResult);
        con.disconnect();
    }

}
