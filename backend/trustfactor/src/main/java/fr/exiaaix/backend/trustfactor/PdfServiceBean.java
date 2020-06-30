package fr.exiaaix.backend.trustfactor;

import fr.exiaaix.backend.trustfactor.models.DecryptData;
import org.apache.pdfbox.pdmodel.PDDocument;
import org.apache.pdfbox.pdmodel.PDPage;
import org.apache.pdfbox.pdmodel.PDPageContentStream;
import org.apache.pdfbox.pdmodel.font.PDType1Font;

import javax.ejb.Stateless;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;

@Stateless
public class PdfServiceBean {

    public byte[] createPdf(DecryptData decryptData, double percentageOfWords) throws IOException {

        try(PDDocument doc =  new PDDocument()){
            PDPage page = new PDPage();

            doc.addPage(page);

            try(PDPageContentStream contentStream = new PDPageContentStream(doc, page)){
                contentStream.beginText();
                contentStream.setFont(PDType1Font.COURIER, 12);

                //DOCUMENT NAME
                contentStream.newLine();
                contentStream.showText("Document name : " + decryptData.FileName);

                //KEY
                contentStream.newLine();
                contentStream.showText("key : " + decryptData.Key);
                contentStream.newLine();

                //PERCENTAGE
                contentStream.newLine();
                contentStream.showText("Percentage of french words : " + percentageOfWords);
                contentStream.newLine();

                //PLAINTEXT
                contentStream.showText("PlainText : ");
                contentStream.newLine();
                contentStream.showText(decryptData.PlainText);
                contentStream.newLine();
                contentStream.endText();
            }

            //PDF To ByteArray
            try(ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream()){
                doc.save(byteArrayOutputStream);
                Logger.getLogger(PdfServiceBean.class.getName()).log(Level.INFO, "pdf created");
                return byteArrayOutputStream.toByteArray();
            }
        }
    }
}
