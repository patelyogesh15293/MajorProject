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
    public class Student
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public string StudentId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        
        [StringLength(50)]
        [Display(Name = "Student Phone")]
        public string StudentPhone { get; set; }

        
        [StringLength(50)]
        [Display(Name = "Student Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string StudentEmail { get; set; }


        [Display(Name = "Create Date")]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Classes")]
        [InverseProperty("Student")]
        public virtual ICollection<Signup> Classes { get; set; } = new HashSet<Signup>();

        public override string ToString()
        {
            return String.Format("{0}", StudentName);
        }
    }
}