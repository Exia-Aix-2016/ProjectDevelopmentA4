package fr.exiaaix.backend.trustfactor;

import com.itextpdf.text.Document;
import com.itextpdf.text.DocumentException;
import com.itextpdf.text.Paragraph;
import com.itextpdf.text.pdf.PdfWriter;
import fr.exiaaix.backend.trustfactor.models.DecryptData;
import javax.ejb.Stateless;
import java.io.ByteArrayOutputStream;
import java.io.IOException;

@Stateless
public class PdfServiceBean {

    public byte[] createPdf(DecryptData decryptData, double percentageOfWords) throws IOException, DocumentException {


        //Text to write
        String information = "Information : \n " +
                "File name : " + decryptData.FileName + "\n" +
                "Key : " + decryptData.FileName + "\n" +
                "Percentage of French words : " + percentageOfWords;

        String plaintext = "Plaintext : \n" + decryptData.PlainText;

        //Create Pdf document
        Document document = new Document();

        try(ByteArrayOutputStream os = new ByteArrayOutputStream()){
            PdfWriter.getInstance(document, os);
            document.open();
            document.add(new Paragraph(information));
            document.add(new Paragraph(plaintext));
            document.close();
            return os.toByteArray();
        }
    }
}
