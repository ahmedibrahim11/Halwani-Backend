using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Halwani.Core.ViewModels.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
