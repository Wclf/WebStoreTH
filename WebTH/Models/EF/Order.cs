using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebTH.Models.EF
{
    [Table("tb_Order")]
    public class Order:CommonAbstract
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>(); 
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Adress { get; set; }

        public decimal TotalAmount { get; set; }

        public int Quantity { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}