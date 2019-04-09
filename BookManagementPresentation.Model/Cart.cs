    using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementPresentation.Model
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int BookID { get; set; }
        public string BookName { get; set; }
        public int DDH_ID { get; set; }

        public DateTime? CartDate { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }

        [ForeignKey("DDH_ID")]
        public virtual DonDatHang DonDatHang { get; set; }

    }
}