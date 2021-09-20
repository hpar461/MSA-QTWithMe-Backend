using System;
using System.ComponentModel.DataAnnotations;

namespace QTWithMe.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = null!;
        
        [Required]
        public int QtId { get; set; }

        public QT Qt { get; set; } = null!;
        
        [Required]
        public int UserId { get; set; }

        public User User { get; set; } = null!;
        
        public DateTime Modified { get; set; }
        
        public DateTime Created { get; set; }
    }
}