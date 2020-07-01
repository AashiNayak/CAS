using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem.Models
{
    public class PwdModel
    {
        [Required(ErrorMessage = "*Required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "*Required")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Password Not match")]
        public string CNPassword { get; set; }


    }
}