using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ExhibitsApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExhibitsApplication.Services
{
    public class MuseumExhibitParser
    {
        private ExhibitsStorage storage;
        public MuseumExhibitParser()
        {
            storage = ExhibitsStorage.GetInstance();
            storage.Clear();
        }

        public void ParseWordDocument(string filePath)
        {
            ExhibitsStorage storage = ExhibitsStorage.GetInstance();
            storage.Clear();

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
            {
                var tables = wordDoc.MainDocumentPart.Document.Body.Elements<Table>();

                foreach (var table in tables)
                {
                    var rows = table.Elements<TableRow>();
                    var exhibit = new ExhibitsModel();
                    byte[] imageData = null;

                    foreach (var row in rows)
                    {
                        var cells = row.Elements<TableCell>().ToList();
                        var cellTexts = cells.Select(cell => cell.InnerText.Trim()).ToList();

                        // Извлекаем данные из ячеек таблицы
                        if (cellTexts.Count > 1)
                        {
                            // Парсим информацию об выставке
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

                            // Парсим информацию об выставке
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

                    // Сохранение данных о выставке вместе с изображением
                    exhibit.Photo = imageData;
                    storage.AddExhibit(exhibit);
                }
            }
        }


        public List<ExhibitsModel> GetExhibitsByFilter(string searchTerm)
        {
            List<ExhibitsModel> exhibits = storage.GetAllExhibits();

            if (String.IsNullOrEmpty(searchTerm))
            {
                return exhibits;
            }

            string lowerSearchTerm = searchTerm.ToLower();

            var filteredExhibits = exhibits
                .Where(exhibit => exhibit.Name.ToLower()
                .Contains(lowerSearchTerm)).ToList();

            return filteredExhibits;
        }

    }
}
