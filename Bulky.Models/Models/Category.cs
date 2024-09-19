using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

<<<<<<< HEAD
<<<<<<< HEAD
namespace BulkyBook.Models
=======
namespace Bulky.Models
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
=======
namespace Bulky.Models
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100, ErrorMessage ="Display Order must be 1 to 100")]
        public int DisplayOrder { get; set; }
    }
}
