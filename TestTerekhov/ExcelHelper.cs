using ClosedXML.Excel;
using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Task3
{
    public class ExcelHelper (ExcelDbContext excelDb, string filePath, ExcelController excelController) 
    {
        private readonly ExcelDbContext _db = excelDb;
        private readonly string _filePath = filePath;
        private readonly ExcelController _excelController = excelController;
        public bool PasreExcelFileToDB(string _filePath)
        {

            if (_filePath != null)
            {
                try
                {
                    using (var workbook = new XLWorkbook(_filePath))
                    {
                        var products = workbook.Worksheet("Товары");
                        var productsRows = products.RangeUsed().RowsUsed().ToList();


                      
                            for (int i = 1; i < productsRows.Count; i++)
                            {
                                var product = new Product();
                            product.Id =  Guid.NewGuid();
                                product.IdProductFromExcel = (int)productsRows[i].Cell(1).Value;
                                product.Name = productsRows[i].Cell(2).Value.ToString();
                                product.Unit = productsRows[i].Cell(3).Value.ToString();
                                product.Price = (int)productsRows[i].Cell(4).Value;

                                _db.Products.Add(product);

                              //  Console.WriteLine($"{product.IdProductFromExcel}  {product.Name} {productsRows[i].Cell(3).Value} {productsRows[i].Cell(4).Value}");
                            }
                       

                      
                      


                        var clients = workbook.Worksheet("Клиенты");
                        var clientsRows = clients.RangeUsed().RowsUsed().ToList();

                       

                            for (int i = 1; i < clientsRows.Count; i++)
                            {
                                var client = new Client();
                                client.Id = Guid.NewGuid();
                                client.IdClientFromExcel = (int)clientsRows[i].Cell(1).Value;
                                client.CompanyName = clientsRows[i].Cell(2).Value.ToString();
                                client.CompanyAdress = clientsRows[i].Cell(3).Value.ToString();
                                client.ContactName = clientsRows[i].Cell(4).Value.ToString();

                                _db.Clients.Add(client);

                            }

                        var orders = workbook.Worksheet("Заявки");
                        var ordersRows = orders.RangeUsed().RowsUsed().ToList();


                        for (int i = 1; i < ordersRows.Count; i++)
                        {
                            var order = new Order();
                            order.Id = Guid.NewGuid();
                            order.IdOrderFromExcel = (int)ordersRows[i].Cell(1).Value;
                            order.IdProductFromExcel = (int)ordersRows[i].Cell(2).Value;
                            order.IdClientFromExcel = (int)ordersRows[i].Cell(3).Value;
                            order.OrderNumber = (int)ordersRows[i].Cell(4).Value;
                            order.AmountProduct = (int)ordersRows[i].Cell(5).Value;
                            order.CreatedDate = ordersRows[i].Cell(6).Value;

                            _db.Orders.Add(order);
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message);
                    return false;
                }

                    _db.SaveChanges();

                Console.WriteLine();
                Console.WriteLine("Файл прочитан и загружен в базу данных");
               
                }
            return true;

        }

        public void UpdateContactNameClient()
        {
            Console.WriteLine();
            Console.WriteLine("Введите имя компании, контактное лицо в которой хотите изменить:");
            var companyName = Console.ReadLine();
            Console.WriteLine("Введите новое контактное лицо:");
            var newContactName = Console.ReadLine();


            var result = _excelController.UpdateContactNameClient(companyName, newContactName);
          
            if (!result)
            {
                Console.WriteLine("Не удалось обновить данные");
                return;
            }

            if (string.IsNullOrEmpty(newContactName) || string.IsNullOrEmpty(companyName))
            {
                Console.WriteLine("Введите данные корректно");
                return;
            }
            using (var workbook = new XLWorkbook(_filePath))
            {
                var clients = workbook.Worksheet("Клиенты");
                var clientsRows = clients.RangeUsed().RowsUsed().ToList();

                for (int i = 1; i < clientsRows.Count; i++)
                {


                    if (clientsRows[i].Cell(2).Value.ToString().ToLower() == companyName.ToLower())
                    {
                        clientsRows[i].Cell(4).Value = newContactName;
                    }

                }
                workbook.SaveAs(_filePath);
            }

            Console.WriteLine($"Вы изменили контактное лицо в компании {companyName} на {newContactName}");
        }
    }
}
