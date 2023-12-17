using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;
using Skalk.Common.DTO.Order;
using Skalk.DocumentGeneration.Interfaces;
using Skalk.DocumentGeneration.Service.Helpers;
using Range = Microsoft.Office.Interop.Word.Range;

namespace Skalk.DocumentGeneration.Service
{
    public class DocumentGenerationService : IDocumentGenerationService
    {
        const decimal vatRate = 20;
        public async Task<byte[]> CreateOrderAsync(OrderContractDTO newOrderDTO)
        {
            byte[] bytes = null;

            // Создание приложения Word
            Application wordApp = new Application();
            wordApp.Visible = true;

            // Получение пути к шаблонному файлу
            string projectDirectory = @$"{Directory.GetCurrentDirectory()}\Data\Files";
            string templateFilePath = Path.Combine(projectDirectory, "sample-contract.docx");

            // Создание временного файла
            string tempFilePath = Path.GetTempFileName();

            try
            {
                // Копирование шаблона во временный файл
                File.Copy(templateFilePath, tempFilePath, true);

                // Открытие временной копии документа
                Document doc = wordApp.Documents.Open(tempFilePath);

                string formattedDate = RuDateAndMoneyConverter.DateToTextLong(newOrderDTO.CreatedAt, "г.");

                string customerInfo = $"{newOrderDTO.CompanyName}, ИНН {newOrderDTO.INN}," +
                    $" КПП {newOrderDTO.KPP}, {newOrderDTO.PostCode}, {newOrderDTO.Address} ";

                int itemsCount = newOrderDTO?.ItemsOrder?.Count ?? 0;
                var totalSum = CalculateTotalAmountOfItems(newOrderDTO);
                var totalSumText = RuDateAndMoneyConverter.CurrencyToTxt(totalSum, true);
                var nds = CalculateVAT(totalSum);
                // Нахождение и замена меток на данные
                ReplaceTagWithText(doc, "<ContractNumber>", newOrderDTO.Id.ToString());
                ReplaceTagWithText(doc, "<ContractDate>", formattedDate);
                ReplaceTagWithText(doc, "<InfoCustomer>", customerInfo);
                ReplaceTagWithText(doc, "<NameCount>", itemsCount.ToString());

                ReplaceTagWithText(doc, "<Itogo>", totalSum.ToString("F2"));
                ReplaceTagWithText(doc, "<NDS>", nds.ToString("F2"));
                ReplaceTagWithText(doc, "<TotalPrice>", totalSum.ToString("F2"));
                ReplaceTagWithText(doc, "<sum>", totalSum.ToString("F2"));
                ReplaceTagWithText(doc, "<sumText>", totalSumText);

                Table table = doc.Tables[5];

                int rowIndex = 1;
                foreach (var item in newOrderDTO.ItemsOrder)
                {
                    Row newRow = table.Rows.Add();

                    newRow.Cells[1].Range.Text = rowIndex.ToString();
                    newRow.Cells[2].Range.Text = item.Mpn;
                    newRow.Cells[3].Range.Text = item.Quantity.ToString();
                    newRow.Cells[4].Range.Text = "шт";
                    newRow.Cells[5].Range.Text = item.Price.ToString("F2");
                    newRow.Cells[6].Range.Text = item.TotalAmount.ToString("F2");

                    rowIndex++;
                }

                foreach (Row row in table.Rows)
                {
                    foreach (Cell cell in row.Cells)
                    {
                        cell.Range.Font.Name = "Arial";
                        cell.Range.Font.Size = 10;
                        cell.Range.Font.Bold = 0;
                    }
                }

                string pdfFilePath = Path.ChangeExtension(tempFilePath, ".pdf");
                doc.SaveAs2(pdfFilePath, WdSaveFormat.wdFormatPDF);


                doc.Save();
                doc.Close();
                // Закрытие приложения Word
                wordApp.Quit();
                using (FileStream fileStream = File.OpenRead(pdfFilePath))
                {
                    bytes = new byte[fileStream.Length];
                    await fileStream.ReadAsync(bytes, 0, bytes.Length);
                }

                // Очистка ресурсов
                File.Delete(pdfFilePath);
                Marshal.ReleaseComObject(doc);
                Marshal.ReleaseComObject(wordApp);

            }
            catch
            {
                wordApp.Quit();
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }

            return bytes;
        }

        private static void ReplaceTagWithText(Document doc, string tag, string text)
        {
            // Поиск и замена метки на текст в документе
            Range range = doc.Content;
            range.Find.Execute(FindText: tag, ReplaceWith: text);
        }

        private decimal CalculateTotalAmountOfItems(OrderContractDTO order)
        {
            decimal totalAmount = 0;

            if (order?.ItemsOrder != null && order.ItemsOrder.Any())
            {
                foreach (var item in order.ItemsOrder)
                {
                    totalAmount += item.TotalAmount;
                }
            }

            return totalAmount;
        }

        decimal CalculateVAT(decimal totalAmount)
        {

            decimal vatAmount = totalAmount * vatRate / (100 + vatRate);

            return vatAmount;
        }
    }
}
