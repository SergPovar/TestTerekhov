using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Client
    {

        public Guid Id { get; set; }
        public int? IdClientFromExcel { get; set; }

        public string? CompanyName { get; set; }
        public string? CompanyAdress { get; set; }
        public string? ContactName { get; set; }

     
    }
}
