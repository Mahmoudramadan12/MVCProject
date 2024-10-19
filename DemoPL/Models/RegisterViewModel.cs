using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoPL.Models
{
    public class RegisterViewModel
    {


        [Required (ErrorMessage = "Frist Name is Required")]
        public string FName { get; set; }

        [Required (ErrorMessage = "Last Name is Required")]
        public string LName { get; set; }
        [Required (ErrorMessage = "Email is Required")]
        [EmailAddress (ErrorMessage = "Invalid Email")]
        public string  Email { get; set; }


        [Required (ErrorMessage = "Password is Required") ]
        [DataType (DataType.Password)]
        public string  Password { get; set; }


        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [DataType(DataType.Password)]
        [Compare (nameof(Password), ErrorMessage ="Password Dosen't Match")]
        public string ConfirmPassword { get; set; }


        public bool IsAgree { get; set; }
    }
}
