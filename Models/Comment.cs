﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_major_project.Models
{
    public class Comment
    {
        [Key]
        [Required]
        [DisplayName("Id mã giảm giá")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Id mã giảm giá")]
        public string Content { get; set; }
        [Required]
        public DateTime DateComment { get; set; }

        [Required]
        public int BlogId { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        [ForeignKey("BlogId")]
        public virtual Blog? Blog { get; set; }
    }
}
