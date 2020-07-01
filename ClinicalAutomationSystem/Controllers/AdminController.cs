using ClinicalAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        // GET: Admin
        public ActionResult Home()
        {
            var id = Convert.ToInt32(Session["MemberId"]);
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var chkname = db.Admins.Where(m => m.MemberId == id).FirstOrDefault();
            if (chkname == null)
            {
                Session["name"] = null;
            }
            else
            {
                Session["name"] = chkname.FirstName;
            }

            return View();
        }

        [Authorize]
        public ActionResult EditProfile()
        {
            var id = Convert.ToInt32(Session["MemberId"]);
            DataModel dt = new DataModel();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var getdata = db.Admins.Where(m => m.MemberId == id).FirstOrDefault();
            if (getdata != null)
            {
                dt.FirstName = getdata.FirstName;
                dt.LastName = getdata.LastName;
                dt.Gender = getdata.Gender;
                dt.Address = getdata.Address;
                
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
            var id = Convert.ToInt32(Session["MemberId"]);
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            Admin obj = new Admin();
            var getdata = db.Admins.Where(m => m.MemberId == id).FirstOrDefault();
            if (getdata != null)
            {
                getdata.FirstName = dt.FirstName;
                getdata.LastName = dt.LastName;
                getdata.Address = dt.Address;
                getdata.Gender = dt.Gender;

            }
            else
            {
                obj.FirstName = dt.FirstName;
                obj.LastName = dt.LastName;
                obj.Gender = dt.Gender;

                obj.Address = dt.Address;
                obj.MemberId = Convert.ToInt32(Session["MemberId"]);
                db.Admins.Add(obj);
            }

            db.SaveChanges();
            ViewBag.text = "Data inserted successfully.";

            var chkname = db.Admins.Where(m => m.MemberId == id).FirstOrDefault();
            if (chkname != null)
            {
                Session["name"] = chkname.FirstName;
            }
            else
            {
                Session["name"] = null;
            }
            return View(dt);
        }

        [Authorize]
        public ActionResult ChangePwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePwd(PwdModel dt)
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var getdata = db.MemberLogins.Where(m => m.MemberId == Convert.ToInt32(Session["MemberId"])).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (getdata.Password == dt.OldPassword)
                {
                    getdata.Password = dt.NewPassword;
                    ViewBag.text = "Password Changed Successfully";
                }
                else
                {
                    ViewBag.text = "You entered a wrong current password";
                }
            }

            return View();
        }

        [Authorize]
        public ActionResult AddDrugDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDrugDetails(DrugModel dt)
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            Drug objdrug = new Drug();
            if (ModelState.IsValid)
            {
                objdrug.DrugName = dt.DrugName;
                objdrug.UsedFor = dt.UsedFor;
                objdrug.SideEffects = dt.SideEffects;
                objdrug.ManufactureDate = dt.ManufactureDate;
                objdrug.ExpiryDate = dt.ExpiryDate;
                objdrug.TotalQuantityAvailable = dt.TotalQuantity;

                db.Drugs.Add(objdrug);
                db.SaveChanges();
                ViewBag.text = "Added Successfully";
            }
            
            return View();
        }

        [Authorize]
        public ActionResult ViewEditDrugs()
        {
            DrugModel dm = new DrugModel();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            List<DrugModel> dmlist = new List<DrugModel>();
            var getdata = db.Drugs.ToList();
            if (getdata!=null)
            {
                foreach (var item in getdata)
                {
                    dmlist.Add(new DrugModel
                    {
                        DrugId= item.DrugId,
                        DrugName= item.DrugName,
                        ManufactureDate = item.ManufactureDate,
                        ExpiryDate = item.ExpiryDate,
                        TotalQuantity = item.TotalQuantityAvailable,
                        UsedFor=item.UsedFor,
                        SideEffects = item.SideEffects,
                        IsDeleted = item.IsDeleted
                    });

                    dm.DrugList = dmlist;
                }
            }
            else
            {
                ViewBag.text = "No drug available";
            }
            
            return View(dm);
        }

        [Authorize]
        public ActionResult DeleteDrug(string id)
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var did = Convert.ToInt32(id);
            var getdata = db.Drugs.Where(m => m.DrugId == did).FirstOrDefault();
            getdata.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("ViewEditDrugs", "Admin");
        }

        [Authorize]
        public ActionResult EditDrug(string id)
        {
            DrugModel dm = new DrugModel();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            int did = Convert.ToInt32(id);
            var getdata = db.Drugs.Where(m => m.DrugId == did).FirstOrDefault();
            dm.DrugName = getdata.DrugName;
            dm.ManufactureDate = getdata.ManufactureDate;
            dm.ExpiryDate = getdata.ExpiryDate;
            dm.SideEffects = getdata.SideEffects;
            dm.UsedFor = getdata.UsedFor;
            dm.TotalQuantity = getdata.TotalQuantityAvailable;
            dm.IsDeleted = getdata.IsDeleted;
            dm.DrugId = getdata.DrugId;
            return View(dm);
        }

        [HttpPost]
        public ActionResult EditDrug(DrugModel dm)
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var getdata = db.Drugs.Where(m => m.DrugId == dm.DrugId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                getdata.DrugName = dm.DrugName;
                getdata.UsedFor = dm.UsedFor;
                getdata.SideEffects = dm.SideEffects;
                getdata.ManufactureDate = dm.ManufactureDate;
                getdata.ExpiryDate = dm.ExpiryDate;
                getdata.IsDeleted = dm.IsDeleted;
                getdata.TotalQuantityAvailable = dm.TotalQuantity;
                db.SaveChanges();
                return RedirectToAction("ViewEditDrugs","Admin");
            }
            else
            {
                ViewBag.text = "no";
            }
            return View();
        }

        [HttpPost]
        public ActionResult ViewPatientOrders()
        {
            DrugModel dm = new DrugModel();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var getdata = from d in db.Drugs
                          join o in db.PatientOrderDetails
                          on d.DrugId equals o.DrugId
                          join p in db.Patients
                          on o.PatientId equals p.PatientId
                          select new
                          {
                              d.DrugName,
                              o.Quantity,
                              p.FirstName,
                              o.OrderDate,
                              o.OrderStatus,
                              o.OrderNumber,
                              o.OrderId
                          };
            List<DrugModel> dmlst = new List<DrugModel>();
            if(getdata!=null)
            {
                foreach (var item in getdata)
                {
                    dmlst.Add(new DrugModel
                    {
                        DrugName = item.DrugName,
                        OrderId = item.OrderId,
                        Name = item.FirstName,
                        OrderDate = item.OrderDate,
                        OrderStatus = item.OrderStatus,
                        OrderQuantity = item.Quantity,
                        OrderNumber = item.OrderNumber
                    });
                }
                dm.DrugList = dmlst; 
            }
            else
            {
                ViewBag.text = "No Orders Yet";
            }
            return View(dm);
        }
    }
}