using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.WebRequestMethods;

namespace BE.Models
{
    [Table("Category")]
    [Index(nameof(CategoryId), Name = "IX_Category_CategoryId", IsUnique = true)]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        //navigation
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
