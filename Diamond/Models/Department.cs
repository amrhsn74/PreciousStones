using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Diamond.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You have to provide a valid name.")]
        [MinLength(2, ErrorMessage = "Name mustn't be less than 2 characters.")]
        [MaxLength(20, ErrorMessage = "Name mustn't exceed 20 characters.")]
        public string Name { get; set; }

        public string Description { get; set; }

        //Navigation Property
        [ValidateNever]
        public List<Employee> Employees { get; set; }
    }
}
