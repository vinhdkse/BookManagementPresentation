using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementPresentation.Model
{
    public class DonDatHang
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DDH_ID { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool? isDelivered { get; set; }
        public DateTime? DeliverDate { get; set; }
        public bool? isPaid { get; set; }
        public string CusUsername { get; set; }
        public double? Total { get; set; }
        public bool? isCanceled { get; set; }
        public bool? isDeleted { get; set; }
        [StringLength(int.MaxValue)]
        public string Address { get; set; }

        public string Phone { get; set; }
        public int PaymentId { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }

        [ForeignKey("CusUsername")]
        public virtual MyUser MyUser { get; set; }

        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }
    }
}
