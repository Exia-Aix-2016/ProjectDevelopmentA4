package fr.exiaaix.backend.trustfactor;

import com.google.gson.Gson;
import fr.exiaaix.backend.trustfactor.models.DecryptData;
import fr.exiaaix.backend.trustfactor.models.ServiceMessage;

import javax.enterprise.context.RequestScoped;
import javax.net.ssl.HttpsURLConnection;
import java.io.IOException;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.StandardCharsets;

@RequestScoped
public class WebClientServiceBean {


    public  void sendResult(ServiceMessage<DecryptData> message) throws IOException {

        URL url = new URL("localhost:8080/api/MServiceRest");

        //Convert to Json
        Gson gson =  new Gson();
        String json = gson.toJson(message);
        System.out.println("------JSON : " + json);
        //Create Connection
        HttpURLConnection con = (HttpURLConnection) url.openConnection();
        con.setRequestMethod("POST");
        con.setDoOutput(true);
        con.setRequestProperty("Content-Type", "application/json");

        //Send Data
        try(OutputStream os = con.getOutputStream()){
            os.write(json.getBytes(StandardCharsets.UTF_8));
        }

        con.disconnect();
    }

}
