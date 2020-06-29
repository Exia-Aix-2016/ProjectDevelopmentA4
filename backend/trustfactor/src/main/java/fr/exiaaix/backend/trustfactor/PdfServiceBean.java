package fr.exiaaix.backend.trustfactor;

import java.io.BufferedReader;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.logging.Level;
import java.util.logging.Logger;

import javax.ejb.Stateless;
import javax.swing.text.Document;

import fr.exiaaix.backend.trustfactor.models.DecryptData;
import org.apache.pdfbox.pdmodel.PDDocument;
import org.apache.pdfbox.pdmodel.PDPage;
import org.apache.pdfbox.pdmodel.PDPageContentStream;
import org.apache.pdfbox.pdmodel.font.PDType1Font;

@Stateless
public class PdfServiceBean {

    public byte[] createPdf(DecryptData decryptData) throws IOException {

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
