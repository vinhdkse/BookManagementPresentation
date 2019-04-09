using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementPresentation.Model
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Publisher { get; set; }

        public DateTime PublishDate { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        [StringLength(500)]
        public string Image { get; set; }

        [StringLength(int.MaxValue)]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public bool isDelete { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
