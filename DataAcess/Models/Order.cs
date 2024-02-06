using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Order
    {

        public Guid Id { get; set; }
        public int IdOrderFromExcel { get; set; }
        public int IdProductFromExcel { get; set; }

        public int OrderNumber { get; set; }
        public int IdClientFromExcel { get; set; }

        public int? AmountProduct { get; set; }
        public DateTime? CreatedDate { get; set; }

        public List<Product> Products { get; set; } = [];
    }
}
