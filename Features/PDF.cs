using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using ChatClipV2.Utils;

namespace ChatClipV2.Features;


internal class createPDF

{
    public static DataTypes.SuccessReturn CreatePDF(string[] paragraphs)
    {
        try
        {
            using (var writer = new PdfWriter(MY_SETTINGS.COVER_PATH))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    using (var document = new Document(pdf))
                    {
                        var fontProgram = FontProgramFactory.CreateFont(MY_SETTINGS.FONT_PATH);
                        var font = PdfFontFactory.CreateFont(fontProgram, PdfEncodings.WINANSI);

                        document.SetMargins(72, 72, 72, 72);
                        document.SetFont(font);
                        document.SetFontSize(MY_SETTINGS.FONT_SIZE);
                        document.SetTextAlignment(TextAlignment.JUSTIFIED);

                        for (int i = 0; i < paragraphs.Length; i++)
                        {
                            string trimmedParagraph = paragraphs[i].Trim();

                            if (i == 0)
                            {
                                var para = new Paragraph(MY_SETTINGS.MY_SALUTATION +
                                    MY_SETTINGS.NEW_LINE + trimmedParagraph + MY_SETTINGS.NEW_LINE)
                                    .SetMarginTop(MY_SETTINGS.PARAGRAPH_TOP_SPACING)
                                    .SetMarginBottom(MY_SETTINGS.PARAGRAPH_BOTTOM_SPACING);

                                document.Add(para);
                            }
                            else if (i == paragraphs.Length - 1)
                            {
                                var para = new Paragraph(trimmedParagraph +
                                    MY_SETTINGS.NEW_LINE + MY_SETTINGS.MY_CLOSING)
                                    .SetMarginTop(MY_SETTINGS.PARAGRAPH_TOP_SPACING)
                                    .SetMarginBottom(MY_SETTINGS.PARAGRAPH_BOTTOM_SPACING);

                                document.Add(para);
                            }
                            else
                            {
                                var para = new Paragraph(trimmedParagraph + MY_SETTINGS.NEW_LINE)
                                    .SetMarginTop(MY_SETTINGS.PARAGRAPH_TOP_SPACING)
                                    .SetMarginBottom(MY_SETTINGS.PARAGRAPH_BOTTOM_SPACING);

                                document.Add(para);
                            }
                        }
                    }
                }
            }
            return new DataTypes.SuccessReturn { IsSuccessful = true };
        }
        catch (Exception ex)
        {
            return new DataTypes.SuccessReturn { IsSuccessful = false, ErrorMessage = ex.Message };
        }
    }

}
