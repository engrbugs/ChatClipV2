//using Microsoft.Office.Interop.Word;
//using QuestPDF.Fluent;
//using QuestPDF.Helpers;
//using QuestPDF.Infrastructure;

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using static iText.IO.Util.IntHashtable;
using Windows.System;
using iText.IO.Font;

//using Word = Microsoft.Office.Interop.Word;

namespace PDFIO;

internal class createPDF

{
    public static void create_PDF(string[] paragraphs)
    {
        string head = "Dear Hiring Manager,\r\n";
        string newLine = "\r\n";
        string bottom = "\r\nSincerely,\r\n\r\nHerby Sio\r\n";
        string path = "C:\\Users\\engrb\\Desktop\\Herby_Sio-Cover.pdf";
        int font_size = 11;
        int para_botttom_spacing = 8;
        int para_top_spacing = 0;

        PdfWriter writer = new PdfWriter(path);
        PdfDocument pdf = new PdfDocument(writer);
        Document document = new Document(pdf);

        FontProgram fontProgram =
        FontProgramFactory.CreateFont(@"C:\Windows\Fonts\calibri.ttf");
        PdfFont calibri = PdfFontFactory.CreateFont(fontProgram, PdfEncodings.WINANSI);


        document.SetMargins(72, 72, 72, 72);
        document.SetFont(calibri);
        document.SetFontSize(11);
        document.SetTextAlignment(TextAlignment.JUSTIFIED);

        for (int i = 0; i < paragraphs.Length; i++)
        {
            paragraphs[i] = paragraphs[i].Trim(); 
            if (i == 0)
            {
                Paragraph para = new Paragraph(head + newLine + paragraphs[i] + newLine)
                    .SetMarginBottom(8)
                    .SetMarginTop(0);
                document.Add(para);
            }
            else if (i == paragraphs.Length - 1)
            {
                Paragraph para4 = new Paragraph(paragraphs[i] + newLine + bottom)
                    .SetMarginBottom(8)
                    .SetMarginTop(0);

                document.Add(para4);
            }
            else
            {
                Paragraph para2 = new Paragraph(paragraphs[i] + newLine)
                    .SetMarginBottom(8)
                    .SetMarginTop(0);

                document.Add(para2);
            }
        }

        document.Close();

    }
}
