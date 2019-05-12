using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

