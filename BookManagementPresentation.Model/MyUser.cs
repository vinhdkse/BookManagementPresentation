using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementPresentation.Model
{
    public class MyUser
    {
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(20)]
        public string Gender { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public string Serect { get; set; }
        [StringLength(int.MaxValue)]
        public string Email { get; set; }
        [StringLength(500)]
        public string Image { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<DonDatHang> DonDatHangs { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

    }
}