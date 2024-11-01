﻿using System.ComponentModel.DataAnnotations;

namespace DemoPL.Models
{
    public class ForgetPasswordViewModel
    {

        [Required (ErrorMessage = "Email is Required")]
        [EmailAddress (ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
