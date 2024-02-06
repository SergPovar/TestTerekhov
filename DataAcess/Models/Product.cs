using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Product
    {

        public Guid Id { get; set; }
        public int IdProductFromExcel { get; set; }
        public string? Name { get; set; }
        public string? Unit { get; set; }
        public decimal? Price { get; set; }
       
    }
}
