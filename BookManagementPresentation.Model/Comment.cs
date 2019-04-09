using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementPresentation.Model
{
    public class Comment
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public string Content { get; set; }
        public DateTime ContentDate { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        [ForeignKey("UserId")]
        public virtual MyUser MyUser { get; set; }
        
    }
}
