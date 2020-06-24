/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package fr.exiaaix.backend.webservice;
import javax.ws.rs.Consumes;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

@Path("/cipher")
@Consumes(MediaType.APPLICATION_JSON)
public class WebService {
    
    @POST()
    
    public Response checkCipher(CipherParam cipherParam){
        
        //start check db
        
        return Response.status(Response.Status.OK).entity("planText " + cipherParam.getPlanText()).type(MediaType.TEXT_PLAIN).build(); 
    }

    
}
