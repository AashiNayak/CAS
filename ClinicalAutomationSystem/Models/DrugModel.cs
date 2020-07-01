using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Models
{
    public class DrugModel
    {
        [Range(1,50, ErrorMessage = "*Required")]
        public string DrugName { get; set; }

        [Range(1, 250, ErrorMessage = "*Required")]
        public string UsedFor { get; set; }

        [Range(1, 250, ErrorMessage = "*Required")]
        public string SideEffects { get; set; }

        [Range(1, 10, ErrorMessage = "*Required")]
        public DateTime ManufactureDate { get; set; }

        [Range(1, 10, ErrorMessage = "*Required")]
        public DateTime ExpiryDate { get; set; }

        [Range(1, 10, ErrorMessage = "*Required")]
        public int TotalQuantity { get; set; }

        public int DrugId { get; set; }

        public bool IsDeleted { get; set; }

        public string OrderStatus { get; set; }

        public string Name { get; set; }

        public DateTime OrderDate { get; set; }

        public int OrderQuantity { get; set; }

        public int OrderId { get; set; }

        public int OrderNumber { get; set; }

        public List<DrugModel> DrugList { get; set; }

        public List<SelectListItem> DrugNameList { get; set; }
    }
}