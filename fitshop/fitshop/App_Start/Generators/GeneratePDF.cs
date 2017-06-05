using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fitshop.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace fitshop.App_Start.Generators
{
    public class GeneratePDF : IGenerate
    {
        public byte[] Generate(List<food> foods)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                document.AddAuthor("fitshop");
                document.AddTitle("foods");

                Font bold = new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.BOLD);
                Paragraph para = new Paragraph(foods.First().user.login + " foods", bold);

                para.Alignment = Element.ALIGN_CENTER;
                document.Add(para);

                document.Add(new Chunk(Chunk.NEWLINE));

                PdfPTable table = new PdfPTable(5);

                table.AddCell("Food name");
                table.AddCell("Fat");
                table.AddCell("Carbs");
                table.AddCell("Protein");
                table.AddCell("Calories");

                foreach (var food in foods)
                {
                    table.AddCell(food.foodName);
                    table.AddCell(food.fat.ToString());
                    table.AddCell(food.carbs.ToString());
                    table.AddCell(food.protein.ToString());
                    table.AddCell(food.calories.ToString());
                }

                document.Add(table);

                document.Close();
                byte[] bytes = memoryStream.ToArray();

                return bytes;
            }
        }
    }
}