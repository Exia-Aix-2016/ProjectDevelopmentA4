package fr.exiaaix.backend.webservice;
import javax.annotation.Resource;
import javax.inject.Inject;
import javax.jms.*;
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
    public Response checkCipher(String body) throws JMSException{
        
        TextMessage msg = context.createTextMessage(body);
        
        context.createProducer().setDeliveryMode(DeliveryMode.NON_PERSISTENT).send(messagingQueue, msg);
        
        return Response.status(Response.Status.OK).build();
    }

    
}
