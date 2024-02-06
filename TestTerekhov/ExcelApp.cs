using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3;

namespace TestTerekhov
{
    public class ExcelApp
    {
        public void Run()
        {
            
            Console.WriteLine("Введите полный адрес до файла Excel. Пример ввода: " +
                    "D:\\projects\\TestTerekhov\\TestTerekhov\\new.xlsx");
            var filePath = Console.ReadLine();
            var excelDb = new ExcelDbContext();
            var excelController = new ExcelController(excelDb);
            var excelHelper = new ExcelHelper(excelDb, filePath, excelController);
            var result = excelHelper.PasreExcelFileToDB(filePath);
             var action = 0;
            while (true)
            {
                if (result)
                {
                    while (true)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Выберите действие, введя соответсвующий ему номер: ");
                        Console.WriteLine("1. По наименованию товара вывести информацию о клиентах, заказавших этот товар");
                        Console.WriteLine("2. Изменить контактное лицо компании");
                        Console.WriteLine("3. Узнать Зологото клиента за год");
                        Console.WriteLine("4. Узнать Зологото клиента за месяц");
                         try
                            {
                                 action = Convert.ToInt32(Console.ReadLine());
                            }
                         catch(Exception ex) { Console.WriteLine(ex.Message); }

                        switch (action)
                        {
                            case 1:
                                excelController.GetClientForNameProduct();
                                break;

                            case 2:
                                excelHelper.UpdateContactNameClient();
                                break;
                            case 3:
                                excelController.GetGoldClientForYear();
                                break;
                            case 4:
                                excelController.GetGoldClientForMonth();
                                break;
                            default:
                                Console.WriteLine("Введите номер действия из списка");
                                break;
                        }
                    }
                }
                else
                {
                    while (!result)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Введите полный адрес до файла Excel. Пример ввода: " +
                       "D:\\projects\\TestTerekhov\\TestTerekhov\\new.xlsx");
                        filePath = Console.ReadLine();
                        result = excelHelper.PasreExcelFileToDB(filePath);
                    }

                }
            }
        }
    }

}
