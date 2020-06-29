package fr.exiaaix.backend.trustfactor;

import java.io.ByteArrayOutputStream;
import java.io.IOException;

import javax.ejb.Stateless;

import org.apache.pdfbox.pdmodel.PDDocument;
import org.apache.pdfbox.pdmodel.PDPage;
import org.apache.pdfbox.pdmodel.PDPageContentStream;
import org.apache.pdfbox.pdmodel.font.PDType1Font;

@Stateless
public class PdfServiceBean {

    public byte[] createPdf(String message, String name) throws IOException {
        PDDocument document = new PDDocument();
        PDPage page = new PDPage();
        document.addPage(page);

        PDPageContentStream contentStream = new PDPageContentStream(document, page);

        contentStream.setFont(PDType1Font.COURIER, 12);
        contentStream.beginText();
        contentStream.showText(message);
        contentStream.endText();
        contentStream.close();

        document.close();

        ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
        document.save(byteArrayOutputStream);

        return byteArrayOutputStream.toByteArray();
    }
}
