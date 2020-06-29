package fr.exiaaix.backend.webservice;
import javax.annotation.Resource;
import javax.inject.Inject;
import javax.jms.JMSConnectionFactory;
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
    public Response checkCipher(String x) throws JMSException{

        TextMessage textMessage = context.createTextMessage(x);

        context.createProducer().send(messagingQueue, textMessage);

        return Response.status(Response.Status.OK).build();
    }

    
}
