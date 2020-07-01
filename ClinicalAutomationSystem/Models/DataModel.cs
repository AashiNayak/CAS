using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;

namespace ClinicalAutomationSystem.Models
{
    public class DataModel
    {
        [Required(ErrorMessage = "*Required")]

        [EmailAddress(ErrorMessage = "Email not in correct format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*Required")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password Not match")]
        public string CPassword { get; set; }

        public int RoleId { get; set; }

        public int MemberId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public string RoleName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime DOB { get; set; }

        public string Address { get; set; }

        public int? TotalExperience { get; set; }

        public int SpecializationId { get; set; }

        public string SpecializationName { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string MessageDetails { get; set; }

        public DateTime MsgDate { get; set; }

        public int ReplyId { get; set; }

        public string From { get; set; }

        public string DMail { get; set; }

        public string PMail { get; set; }

        public string FromEmail { get; set; }

        public string ToEmail { get; set; }

        public DateTime AppointmentRecieveDate { get; set; }

        public DateTime AppointmentDate { get; set; }

        public int AppointmentId { get; set; }

        
        public string AppointmentStatus { get; set; }

        public List<SelectListItem> SpecList { get; set; }

        public List<SelectListItem> RoleList { get; set; }

        public List<SelectListItem> ListD { get; set; }

        public List<DataModel> AppointmentList { get; set; }

        public List<DataModel> MsgList { get; set; }

        public List<DataModel> MsgViewList { get; set; }
    }
}