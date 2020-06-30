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

    public void sendMail()/*add parametre secret key file*/ {

      Message simpleMail = new MimeMessage(mailSession);

      try {
        simpleMail.setSubject("Info Secrete");
        simpleMail.setRecipient(Message.RecipientType.TO, new InternetAddress("lucvdv83@hotmail.fr"));

        MimeMultipart mailContent = new MimeMultipart();

        MimeBodyPart mailMessage = new MimeBodyPart();
        mailMessage.setContent("<p>L'info secret est :Elvis is the King.<br /> Dans le fichier: file_074.txt.<br />Avec la Clé: .", "text/html; charset=utf-8");
        mailContent.addBodyPart(mailMessage);

        simpleMail.setContent(mailContent);

        Transport.send(simpleMail);

        System.out.println("Message successfully send to: " + "lucvdv83@hotmail.fr");
      } catch (MessagingException e) {
        e.printStackTrace();
      }
    }
}
