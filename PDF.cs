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
        string path = "C:\\Users\\engrb\\Desktop\\hello.pdf";
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


    //public static void create_PDF(string str)
    //{
    //    str = "Dear Hiring Manager,\r\n\r\nI am excited to apply for the Supply Planner position at Storkcraft. With over three years of experience in Supply and Demand Planning, I have honed my skills in data analytics, attention to detail, and collaboration to ensure an optimal balance between customer service and inventory levels. I believe my experience and skills are a perfect match for this role.\r\nAs a former Business Analyst, I have a strong background in analyzing business intelligence data to identify areas for improvement and developing and executing solutions to drive growth and profitability. Moreover, my proficiency in Excel and other data visualization tools like Power Bl and Tableau has helped me create visually appealing reports to help stakeholders understand trends and make informed decisions.\r\nIn my current role as a CAD & Revit Technician, I have developed and implemented efficient workflows to reduce project completion time and automate repetitive tasks, resulting in a 20% increase in productivity. My ownership mindset and strong commitment to delivering outstanding results will enable me to work diligently to ensure that Storkcraft remains the manufacturer of choice for new and existing parents.\r\nI am excited to contribute my skills and experience to the Storkcraft team and would be grateful for the opportunity to discuss my qualifications further. Thank you for your time and consideration.\r\n\r\nSincerely,\r\n\r\nHerby Sio\r\n";
    //    QuestPDF.Settings.License = LicenseType.Community;
    //    Document.Create(container =>
    //    {
    //        container.Page(page =>
    //        {
    //            page.Size(PageSizes.Letter);
    //            page.Margin(1, Unit.Inch);
    //            page.PageColor(Colors.White);
    //            page.DefaultTextStyle(x => 
    //               x.FontSize(11)
    //                .FontFamily(Fonts.Calibri)

    //            );

    //            //page.Header()
    //            //    .Text("Hello PDF!")
    //            //    .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

    //            page.Content()
    //                .PaddingVertical(1, Unit.Centimetre)

    //                .Column(x =>
    //                {
    //                    x.Spacing(20);
    //                    x.Item().Text(text =>
    //                    {
    //                        text.Span(str);
    //                        text.ParagraphSpacing(8);
    //                    });

    //                });

    //            page.Footer()
    //                .AlignCenter()
    //                .Text(x =>
    //                {
    //                    //x.Span("Page ");
    //                    //x.CurrentPageNumber();
    //                });
    //        });
    //    })
    //    .GeneratePdf("C:\\Users\\engrb\\Desktop\\hello.pdf");
    //}

