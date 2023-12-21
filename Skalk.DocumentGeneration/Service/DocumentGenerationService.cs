using ConvertApiDotNet;
using Skalk.Common.DTO.Order;
using Skalk.DocumentGeneration.Interfaces;
using Skalk.DocumentGeneration.Service.Helpers;
using Xceed.Document.NET;
using Xceed.Words.NET;


namespace Skalk.DocumentGeneration.Service
{
    public class DocumentGenerationService : IDocumentGenerationService
    {
        const decimal vatRate = 20;
        public async Task<byte[]> CreateOrderAsync(OrderContractDTO newOrderDTO)
        {
            byte[] bytes = null;

            string projectDirectory = @$"{Directory.GetCurrentDirectory()}\Data\Files";
            string templateFilePath = Path.Combine(projectDirectory, "sample-contract.docx");
            string pdfFilePath = Path.Combine(projectDirectory, "sample-contract.pdf");

            // Создаем временный путь для копии
            string copiedFilePath = Path.Combine(projectDirectory, "sample-contract-copy.docx");
            try
            {
                // Копируем файл
                File.Copy(templateFilePath, copiedFilePath, true);
                using (DocX doc = DocX.Load(copiedFilePath))
                {
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

                    ReplaceTagWithText(doc, "<Itogo>", totalSum.ToString());
                    ReplaceTagWithText(doc, "<NDS>", nds.ToString());
                    ReplaceTagWithText(doc, "<TotalPrice>", totalSum.ToString());
                    ReplaceTagWithText(doc, "<sum>", totalSum.ToString());
                    ReplaceTagWithText(doc, "<sumText>", totalSumText);

                    Table table = doc.Tables[4];


                    int rowIndex = 1;
                    foreach (var item in newOrderDTO.ItemsOrder)
                    {
                        Row newRow = table.InsertRow();
                        // Добавление ячеек в строку и заполнение данными
                        newRow.Cells[0].Paragraphs.First().Append(rowIndex.ToString());
                        newRow.Cells[1].Paragraphs.First().Append(item.Mpn);
                        newRow.Cells[2].Paragraphs.First().Append(item.Quantity.ToString());
                        newRow.Cells[3].Paragraphs.First().Append("шт");
                        newRow.Cells[4].Paragraphs.First().Append(item.Price.ToString());
                        newRow.Cells[5].Paragraphs.First().Append(item.TotalAmount.ToString());

                        rowIndex++;
                    }

                    doc.SaveAs(copiedFilePath);

                    var convertApi = new ConvertApi("2DIawtXjTea2agVg");
                    var result = await convertApi.ConvertAsync("docx", "pdf", new[]
                    {
                        new ConvertApiFileParam(copiedFilePath)
                    });

                    // Save the PDF file to the same directory
                    await result.SaveFileAsync(pdfFilePath);

                    // Read the PDF file into a byte array
                    bytes = await File.ReadAllBytesAsync(pdfFilePath);
                    return bytes;
                }
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            finally
            {
                if (File.Exists(copiedFilePath))
                {
                    File.Delete(copiedFilePath);
                }

                if (File.Exists(pdfFilePath))
                {
                    File.Delete(pdfFilePath);
                }
            }
        }

        private void ReplaceTagWithText(DocX document, string tag, string replacement)
        {
            // Используем метод ReplaceText для замены меток на данные
            document.ReplaceText(tag, replacement ?? string.Empty);
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
            decimal roundedvatAmount = Math.Round(vatAmount, 2, MidpointRounding.AwayFromZero);

            return roundedvatAmount;
        }
    }
}
