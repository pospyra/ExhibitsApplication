using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ExhibitsApplication.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExhibitsApplication.Services
{
    public class MuseumExhibitParser
    {
        public List<ExhibitsModel> ParseWordDocument(string filePath)
        {
            List<ExhibitsModel> exhibitsList = new List<ExhibitsModel>();

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
            {
                var tables = wordDoc.MainDocumentPart.Document.Body.Elements<Table>();

                foreach (var table in tables)
                {
                    var rows = table.Elements<TableRow>();
                    var exhibit = new ExhibitsModel();

                    foreach (var row in rows)
                    {
                        var cells = row.Elements<TableCell>().ToList();
                        var cellTexts = cells.Select(cell => cell.InnerText.Trim()).ToList();

                        // Извлекаем данные из ячеек таблицы
                        if (cellTexts.Count > 1)
                        {

                            if (cellTexts[0].Contains("Инвентарный номер:"))
                            {
                                var splitTexts = cellTexts[0].Split(':');
                                exhibit.InventoryNumber = splitTexts[1].Trim();
                            }
                            if (cellTexts[1].Contains("Название:"))
                            {
                                var splitTexts = cellTexts[1].Split(':');
                                exhibit.Name = splitTexts[1].Trim();
                            }

                            switch (cellTexts[0])
                            {
                                case "шифр фондовой коллекции":
                                    exhibit.FundCode = cellTexts[1];
                                    break;
                                case "Место и время изготовления":
                                    exhibit.Year = cellTexts[1];
                                    break;
                                case "Количество":
                                    exhibit.Quantity = cellTexts[1];
                                    break;
                                case "Материал, техника изготовления":
                                    exhibit.Material = cellTexts[1];
                                    break;
                                case "Размер":
                                    exhibit.Size = cellTexts[1];
                                    break;
                                case "Состояние сохранности":
                                    exhibit.Condition = cellTexts[1];
                                    break;
                                case "Описание музейного предмета":
                                    exhibit.Description = cellTexts[1];
                                    break;
                                case "Источник поступления":
                                    exhibit.Source = cellTexts[1];
                                    break;
                                case "Дата регистрации":
                                    exhibit.RegistratonDate = cellTexts[1];
                                    break;
                            }
                        }
                    }

                    // Обработка фотографии, следующей за таблицей
                    var nextSibling = table.NextSibling();
                    if (nextSibling != null && nextSibling is Paragraph)
                    {
                        var run = nextSibling.GetFirstChild<Run>();
                        if (run != null && run.GetFirstChild<DocumentFormat.OpenXml.Drawing.Blip>() != null)
                        {
                            var blip = run.GetFirstChild<DocumentFormat.OpenXml.Drawing.Blip>();
                            var embed = blip.Embed;
                            if (embed != null)
                            {
                                var imageData = (wordDoc.MainDocumentPart.GetPartById(embed) as ImagePart).GetStream();
                                if (imageData != null)
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        imageData.CopyTo(ms);
                                        exhibit.Photo = ms.ToArray();
                                    }
                                }
                            }
                        }
                    }

                    exhibitsList.Add(exhibit);
                }
            }

            return exhibitsList;
        }
    }
}
