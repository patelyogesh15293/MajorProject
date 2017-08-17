using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity.Spatial;

namespace MajorProject.Models
{
    public class Class
    {
        public Class()
        {

        }

        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public string ClassId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Class Name")]
        public string ClassName { get; set; }

        [StringLength(250)]
        [Display(Name = "Class Description")]
        public string ClassDescription { get; set; }

        [Display(Name = "Create Date")]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Students")]
        [InverseProperty("Class")]
        public virtual ICollection<Signup> Students { get; set; } = new HashSet<Signup>();

        public override string ToString()
        {
            return String.Format("{0}", ClassName);
        }
    }
}