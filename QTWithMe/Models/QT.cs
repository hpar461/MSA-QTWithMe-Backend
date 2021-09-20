using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QTWithMe.Models
{
    public class QT
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [Required]
        public string Passage { get; set; } = null!;

        public string PassageText { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime Modified { get; set; }
        
        public DateTime Created { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}