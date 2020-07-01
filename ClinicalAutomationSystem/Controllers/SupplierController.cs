using ClinicalAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Controllers
{
    public class SupplierController : Controller
    {
        [Authorize]
        // GET: Supplier
        public ActionResult Home()
        {
            var id = Convert.ToInt32(Session["MemberId"]);

            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            DataModel dt = new DataModel();

            var chkname = db.Suppliers.Where(m => m.MemberId == id).FirstOrDefault();
            if (chkname != null)
            {
                Session["name"] = chkname.FirstName;
            }
            else
            {
                Session["name"] = null;
            }

            return View();
            
        }

        [Authorize]
        public ActionResult EditProfile()
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            DataModel dt = new DataModel();
            var id = Convert.ToInt32(Session["MemberId"]);

            var getdata = db.Suppliers.Where(m => m.MemberId == id).FirstOrDefault();
            if (getdata != null)
            {
                dt.FirstName = getdata.FirstName;
                dt.LastName = getdata.LastName;
                dt.CompanyAddress = getdata.CompanyAddress;
                dt.CompanyName = getdata.CompanyName;

            }
            else
            {
                dt.FirstName = null;
            }

            return View(dt);
        }

        [HttpPost]
        public ActionResult EditProfile(DataModel dt)
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var id = Convert.ToInt32(Session["MemberId"]);
            

            
            dt.MemberId = id;
            var getdata = db.Suppliers.Where(m => m.MemberId == id).FirstOrDefault();
            Supplier objSupplier = new Supplier();
            if (getdata != null)
            {
                getdata.FirstName = dt.FirstName;
                getdata.LastName = dt.LastName;
                getdata.CompanyName = dt.CompanyName;
                getdata.CompanyAddress = dt.CompanyAddress;

            }
            else
            {
                objSupplier.FirstName = dt.FirstName;
                objSupplier.LastName = dt.LastName;
                objSupplier.CompanyName = dt.CompanyName;
                objSupplier.CompanyAddress = dt.CompanyAddress;
                objSupplier.MemberId = dt.MemberId;
                db.Suppliers.Add(objSupplier);

            }

            db.SaveChanges();
            var chkname = db.Suppliers.Where(m => m.MemberId == id).FirstOrDefault();
            if (chkname != null)
            {
                Session["name"] = chkname.FirstName;
            }
            else
            {
                Session["name"] = null;
            }
            ViewBag.text = "data inserted";
            return View(dt);
        }
    }
}