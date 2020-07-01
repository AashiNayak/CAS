using ClinicalAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace ClinicalAutomationSystem.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(DataModel dm)
        {
            int status = -1;
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var getdata = db.MemberLogins.ToList();
            foreach (var item in getdata)
            {
                if (item.Email == dm.Email && item.Password == dm.Password)
                {
                    FormsAuthentication.SetAuthCookie(dm.Email, false);
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        item.Email,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(20),
                        false, item.Email);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);

                    dm.MemberId = item.MemberId;
                    Session["Email"] = dm.Email;
                    Session["MemberId"] = dm.MemberId;

                    dm.RoleId = item.RoleId;
                    status = 0;
                    break;
                }

            }

            if (status == 0)
            {
                switch (dm.RoleId)
                {
                    case 1:
                        return RedirectToAction("Home", "Patient"); // home view action method of Patient Controller
                        break;
                    case 2:
                        return RedirectToAction("Home", "Doctor");
                        break;
                    case 3:
                        return RedirectToAction("Home", "Supplier");
                        break;
                    case 4:
                        return RedirectToAction("Home", "Admin");
                        break;
                    default:
                        break;
                }

                
            }
            else
            {
                ViewBag.text = "Email or Password Incorrect";
            }
            return View();
        }

        public ActionResult Register()
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            List<SelectListItem> lst = new List<SelectListItem>();
            DataModel dm = new DataModel();
            var getdata = db.RoleDetails.ToList();
            for (int i=0; i<3; i++)
            {
                lst.Add(new SelectListItem
                {
                    Text = getdata[i].RoleName,
                    Value = getdata[i].RoleId.ToString()
                });

            }
            dm.RoleList = lst;
            return View(dm);
        }

        [HttpPost]
        public ActionResult Register(DataModel dm) 
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            System.Data.Entity.Core.Objects.ObjectParameter Memberid = new System.Data.Entity.Core.Objects.ObjectParameter("id", typeof(int));

            if (ModelState.IsValid)
            {
                    db.AddMember(dm.Email, dm.Password, dm.RoleId, Memberid);
                    db.SaveChanges();
                    dm.MemberId = Convert.ToInt32(Memberid.Value);
                    Session["MemberId"] = dm.MemberId;

                    ViewBag.text = "Registerd Successfuly";
            }
            List<SelectListItem> lst = new List<SelectListItem>();
            var getdata = db.RoleDetails.ToList();
            for (int i = 0; i < 3; i++)
            {
                lst.Add(new SelectListItem
                {
                    Text = getdata[i].RoleName,
                    Value = getdata[i].RoleId.ToString()
                });

            }
            dm.RoleList = lst;


            return View(dm);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            ModelState.Clear();

            return RedirectToAction("Login", "Home");
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }
    }
}