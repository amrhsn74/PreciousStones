using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Diamond.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You have to provide a valid full name.")]
        [MinLength(8, ErrorMessage = "Full name mustn't be less than 8 characters.")]
        [MaxLength(50, ErrorMessage = "Full name mustn't exceed 50 characters.")]
        public string FullName { get; set; }

        [MinLength(14, ErrorMessage = "National Id mustn't be less than 14 characters.")]
        [MaxLength(14, ErrorMessage = "National Id mustn't exceed 14 characters.")]
        public string NationalId { get; set; }

        [RegularExpression("^01\\d{9}$", ErrorMessage = "You have to provide a valid mobile phone number.")]
        public string PhoneNo { get; set; }

        [MinLength(5, ErrorMessage = "Position mustn't be less than 5 character.")]
        public string Position { get; set; }

        [Range(6_000, 60_000, ErrorMessage = "Salary must be between 6000 EGP and 60000 EGP.")]
        public decimal Salary { get; set; }

        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime HiringDateAndTime { get; set; }

        [DataType(DataType.Time)]
        public TimeOnly AttendanceTime { get; set; }

        [DataType(DataType.Time)]
        public TimeOnly LeaveTime { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        //Foreign Key
        public int DepartmentId { get; set; }

        //Navigation Property
        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ValidateNever]
        public Department Department { get; set; }
    }
}
