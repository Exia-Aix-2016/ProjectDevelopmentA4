/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package fr.exiaaix.backend.webservice;
import javax.annotation.Resource;
import javax.inject.Inject;
import javax.jms.ConnectionFactory;
import javax.jms.JMSConnectionFactory;
import javax.jms.JMSConsumer;
import javax.jms.JMSContext;
import javax.jms.JMSException;
import javax.jms.Queue;
import javax.jms.TextMessage;
import javax.ws.rs.Consumes;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

@Path("/cipher")
@Consumes(MediaType.APPLICATION_JSON)
public class WebService {
    
    @Inject
    @JMSConnectionFactory("jms/__defaultConnectionFactory")
    private JMSContext context;
    
    @Resource(lookup = "jms/messagingQueue")
    private Queue messagingQueue;

    @POST()
    public Response checkCipher(CipherParam cipherParam) throws JMSException{
        
        //start check db
        
        TextMessage textMessage = context.createTextMessage("tralala");
        context.createProducer().send(messagingQueue, textMessage);
        JMSConsumer consumer = context.createConsumer(messagingQueue);
        
        String mesageBody = consumer.receive().getBody(String.class);
        
        
        return Response.status(Response.Status.OK).entity("messageContain " + mesageBody).type(MediaType.TEXT_PLAIN).build(); 
    }

    
}
