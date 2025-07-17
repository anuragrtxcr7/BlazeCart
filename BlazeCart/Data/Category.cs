using System.ComponentModel.DataAnnotations;

namespace BlazeCart.Data
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Emter your Name" )]
        public string Name { get; set; }

    }
}
