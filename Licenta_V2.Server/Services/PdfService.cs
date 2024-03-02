using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using LatissimusDorsi.Server.Models;


namespace LatissimusDorsi.Server.Services
{
    public class PdfService
    {     
        public void GenerateWorkoutPDF(Workout workout, string outputPath)
        {
            PdfWriter writer = new PdfWriter(outputPath);
            PdfDocument pdfDoc = new PdfDocument(writer);
            Document doc = new Document(pdfDoc);

            Paragraph title = new Paragraph(workout.title)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFontSize(22)
                .SetBold();
            doc.Add(title);

            foreach(var day in workout.exercises)
            {
                if (day.Value.Count == 1 && day.Value[0].name == "RestDay")
                    continue;

                Paragraph dayTitle = new Paragraph(day.Key)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT) 
                    .SetFontSize(18)
                    .SetBold();
                doc.Add(dayTitle);

                Table table = new Table(5).UseAllAvailableWidth();
                DeviceRgb headerColor = new DeviceRgb(51, 51, 51);
                DeviceRgb cellColor = new DeviceRgb(233, 242, 252);
           
                Cell exerciseHeaderCell = new Cell().SetBackgroundColor(headerColor)
                    .Add(new Paragraph("Exercise").SetFontColor(ColorConstants.WHITE));
                Cell setsHeaderCell = new Cell().SetBackgroundColor(headerColor)
                    .Add(new Paragraph("Sets").SetFontColor(ColorConstants.WHITE));
                Cell repsHeaderCell = new Cell().SetBackgroundColor(headerColor)
                    .Add(new Paragraph("Reps").SetFontColor(ColorConstants.WHITE));
                Cell rpeHeaderCell = new Cell().SetBackgroundColor(headerColor)
                    .Add(new Paragraph("RPE").SetFontColor(ColorConstants.WHITE));
                Cell descriptionHeaderCell = new Cell().SetBackgroundColor(headerColor)
                    .Add(new Paragraph("Description").SetFontColor(ColorConstants.WHITE));
                    
                table.AddHeaderCell(exerciseHeaderCell);
                table.AddHeaderCell(setsHeaderCell);
                table.AddHeaderCell(repsHeaderCell);
                table.AddHeaderCell(rpeHeaderCell);
                table.AddHeaderCell(descriptionHeaderCell);

                foreach (var exercise in day.Value)
                {
                    Cell exerciseCell = new Cell().SetBackgroundColor(cellColor).Add(new Paragraph(exercise.name));
                    Cell setsCell = new Cell().SetBackgroundColor(cellColor).Add(new Paragraph(exercise.sets.ToString() ?? ""));
                    Cell repsCell = new Cell().SetBackgroundColor(cellColor).Add(new Paragraph(exercise.reps.ToString() ?? ""));
                    Cell rpeCell = new Cell().SetBackgroundColor(cellColor).Add(new Paragraph(exercise.rpe.ToString() ?? ""));
                    Cell descriptionCell = new Cell().SetBackgroundColor(cellColor).Add(new Paragraph(exercise.description ?? ""));

                    table.AddCell(exerciseCell);
                    table.AddCell(setsCell);
                    table.AddCell(repsCell);
                    table.AddCell(rpeCell);
                    table.AddCell(descriptionCell);
                }

                doc.Add(table);
                
            }
            doc.Close();
        }
        
    }
}
