package fr.exiaaix.backend.trustfactor;

import javax.annotation.Resource;
import javax.ejb.Stateless;
import javax.mail.Message;
import javax.mail.MessagingException;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeBodyPart;
import javax.mail.internet.MimeMessage;
import javax.mail.internet.MimeMultipart;

@Stateless
public class MailServiceBean {

    @Resource(name = "mailNotifier")
    private Session mailSession;

    public void sendMail(String secret, String FileName, String Key) {

      Message simpleMail = new MimeMessage(mailSession);

      try {
          
        simpleMail.setSubject("Info Secrete");
        simpleMail.setRecipient(Message.RecipientType.TO, new InternetAddress("Exia-Aix-Promo-20162017@viacesi.fr"));

        MimeMultipart mailContent = new MimeMultipart();

        MimeBodyPart mailMessage = new MimeBodyPart();
        mailMessage.setContent("<p>L'info secret est : "+secret+"<br /> Dans le fichier: "+FileName+"<br />Avec la Clé: "+Key+ ".", "text/html; charset=utf-8");
        mailContent.addBodyPart(mailMessage);

        simpleMail.setContent(mailContent);

        Transport.send(simpleMail);

        System.out.println("Message successfully send to: " + "Exia-Aix-Promo-20162017@viacesi.fr");
      } catch (MessagingException e) {
        e.printStackTrace();
      }
    }
}
