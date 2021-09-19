using System.ComponentModel.DataAnnotations;

namespace QTWithMe.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string GitHub { get; set; }
        
        public string ImageURI { get; set; }
    }
}