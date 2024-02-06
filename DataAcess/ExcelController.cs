using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ExcelController(ExcelDbContext excelDb)
    {
        private readonly ExcelDbContext _db = excelDb;
        public void GetClientForNameProduct()
        {
            Console.WriteLine();
            Console.WriteLine("Введите наименование товара, что бы посмотреть кто его заказал:");
            var productName = Console.ReadLine();
            //var productName = "молоко";
            var product = _db.Products.AsNoTracking()
                .Where(p => p.Name.ToLower() == productName.ToLower()).FirstOrDefault();

            if (product == null)
            {
                Console.WriteLine("Товар не найден");
                return;
            }

            var idProductFromExcel = product.IdProductFromExcel;
            var priceProduct = product.Price;

            List<Order> ordersList = _db.Orders.AsNoTracking()
                .Where(o => o.IdProductFromExcel == idProductFromExcel)
                .ToList();

            if (ordersList.Count == 0)
            {
                Console.WriteLine("Данный товар еще не заказывали");
                return;
            };

            List<Client> clients = _db.Clients.AsNoTracking().ToList();

            Console.WriteLine();
            Console.WriteLine("Наименование организации | Контактное лицо          | Кол-во  | Цена | Наименование | Дата заказа");

            foreach (var item in ordersList)
            {
                var client = clients.FirstOrDefault(c => c.IdClientFromExcel == item.IdClientFromExcel);
                if (client == null)
                {
                    Console.WriteLine("Клиент не найден");
                    break;
                }
                
                Console.WriteLine("{0,25}|{1,26}|{2,9}|{3,3}|{4,14}|{5,8}", client?.CompanyName, client?.ContactName, item?.AmountProduct, priceProduct, productName, item.CreatedDate);
            }
        }

        public void GetGoldClientForYear()
        {

            int year = 0;
            Console.WriteLine();
            Console.WriteLine("Введите год, за который хотите узнать Золотого покупателя, в виде порядкового номера:");
            try
            {
                year = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            if (year > DateTime.Now.Year)
            {
                Console.WriteLine("Вы ввели год некорректно");
                return;
            }
            var clientFromExcel = _db.Orders.AsNoTracking()
                .Where(o => o.CreatedDate.Value.Year == year)
                .GroupBy(o => o.IdClientFromExcel)
                .Select(g => new
                {
                    Year = g.Key,
                    IdClientFromExcel = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(g => g.OrderCount)
                .FirstOrDefault();

            if (clientFromExcel == null)
            {
                Console.WriteLine("За этот период заказов не было");
                return;
            }
            var client = _db.Clients.AsNoTracking()
                .Where(c => c.IdClientFromExcel == clientFromExcel.IdClientFromExcel)
                .FirstOrDefault();
            Console.WriteLine();
            Console.WriteLine($"Больше всех заказов за запрашиваемый год у {client?.CompanyName}");
        }
        public void GetGoldClientForMonth()
        {
            int month = 0;
            Console.WriteLine();
            Console.WriteLine("Введите месяц, за который хотите узнать Золотого покупателя, в виде порядкового номера:");
            try
            {
                month = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            if (month < 1 || month > 12)
            {
                Console.WriteLine("Вы ввели месяц некорректно");
                return;
            }

            var clientFromExcel = _db.Orders.AsNoTracking()
                .Where(o => o.CreatedDate.Value.Month == month)
                .GroupBy(o => o.IdClientFromExcel)
                .Select(g => new
                {
                    Month = g.Key,
                    IdClientFromExcel = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(g => g.OrderCount)
                .FirstOrDefault();

            if (clientFromExcel == null)
            {
                Console.WriteLine("За этот период заказов не было");
                return;
            }
            var client = _db.Clients.AsNoTracking()
                .Where(c => c.IdClientFromExcel == clientFromExcel.IdClientFromExcel)
                .FirstOrDefault();
            Console.WriteLine();
            Console.WriteLine($"Больше всех заказов за запрашиваемый месяц у {client.CompanyName}");

        }
        public bool UpdateContactNameClient(string companyName, string newContactName)
        {

            if (string.IsNullOrEmpty(newContactName) || string.IsNullOrEmpty(companyName))
            {
                Console.WriteLine("Введите данные корректно");
                return false;
            }

            var client = _db.Clients.Where(c => c.CompanyName.ToLower() == companyName.ToLower()).FirstOrDefault();

            if (client == null)
            {
                Console.WriteLine("Компания не найдена");
                return false;
            }

            client.ContactName = newContactName;
            _db.Clients.Update(client);

            _db.SaveChanges();

            return true;
        }
    }
}
