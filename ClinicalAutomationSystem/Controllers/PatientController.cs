using ClinicalAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Controllers
{
    public class PatientController : Controller
    {
        [Authorize]
        // GET: Patient
        public ActionResult Home()
        {
            var id = Convert.ToInt32(Session["MemberId"]);
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var chkname = db.Patients.Where(m => m.MemberId == id).FirstOrDefault();
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
            var getdata = db.Patients.Where(m=> m.MemberId == id).FirstOrDefault();
            if(getdata!=null)
            {
                dt.FirstName = getdata.FirstName;
                dt.LastName = getdata.LastName;
                dt.Gender = getdata.Gender;
                dt.Address = getdata.Address;
                dt.DOB = getdata.DateOfBirth;
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
            Patient objPatient = new Patient();
            var getdata = db.Patients.Where(m => m.MemberId == id).FirstOrDefault();
            if (getdata != null)
            {
                getdata.FirstName = dt.FirstName;
                getdata.LastName = dt.LastName;
                getdata.DateOfBirth = dt.DOB;
                getdata.Address = dt.Address;
                getdata.Gender = dt.Gender;
                
            }
            else
            {
                objPatient.FirstName = dt.FirstName;
                objPatient.LastName = dt.LastName;
                objPatient.Gender = dt.Gender;
                objPatient.DateOfBirth = dt.DOB;
                objPatient.Address = dt.Address;
                objPatient.MemberId = Convert.ToInt32(Session["MemberId"]);
                db.Patients.Add(objPatient);
            }
            
            db.SaveChanges();
            ViewBag.text = "Data inserted successfully.";

            var chkname = db.Patients.Where(m => m.MemberId == id).FirstOrDefault();
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
        public ActionResult TakeAppointment()
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var getdata = db.Doctors.ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            if (getdata!= null)
            {
                foreach (var item in getdata)
                {
                    string name = String.Concat(item.FirstName," ",item.LastName);
                    lst.Add(new SelectListItem
                    {
                        Text = name,
                        Value = item.DoctorId.ToString()
                    }
                    ); ;
                }
                
            }
            DataModel dm = new DataModel();
            dm.ListD = lst;
            return View(dm);
        }

        [HttpPost]
        public ActionResult TakeAppointment(DataModel dm,string datet)
        {
            //to save the date value entered by the patient  pass the id of the input in the action result
            //and then assign that value to dt
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var id = Convert.ToInt32(Session["MemberId"]);

            DateTime date = Convert.ToDateTime(datet);
            var chk = date.Hour;
            if (chk < 9 || chk > 17)
            {
                ViewBag.text = "Time should be chosen between 9 AM to 5 PM";
            }
            else
            {
                var sysdate = DateTime.Now;
                var getpid = db.Patients.Where(m => m.MemberId == id).FirstOrDefault();
                dm.PatientId = getpid.PatientId;

                if (date > sysdate)
                {
                    dm.AppointmentDate = Convert.ToDateTime(date);
                    //call procedure AddAppointment() to insert values in the db
                    db.AddAppointment(dm.PatientId, dm.DoctorId, dm.Subject, dm.Description, dm.AppointmentDate);
                    ViewBag.text = "Appointment recieved by the doctor";
                }
                else
                {
                    ViewBag.text = "Date incorrect";
                }

            }


            var getdata = db.Doctors.ToList();
            List<SelectListItem> lst = new List<SelectListItem>();
            if (getdata != null)
            {
                foreach (var item in getdata)
                {
                    string name = String.Concat(item.FirstName," ", item.LastName);
                    lst.Add(new SelectListItem
                    {
                        Text = name,
                        Value = item.DoctorId.ToString()
                    }
                    ); ;
                }

            }
            
            dm.ListD = lst;
            return View(dm);
        }

        [Authorize]
        public ActionResult ViewAppointments()
        {
            DataModel dm = new DataModel();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();

            var id = Convert.ToInt32(Session["MemberId"]);
            var getpid = db.Patients.Where(m => m.MemberId == id).FirstOrDefault();
            
            var getdata = from a in db.DoctorAppointments
                          join d in db.Doctors
                          on a.DoctorId equals d.DoctorId
                          select new
                          {
                              a.AppointmentDate,
                              a.AppointmentRecieveDate,
                              a.AppointmentStatus,
                              a.Subject,
                              a.Description,
                              a.PatientId,
                              d.FirstName
                          }
                         ;
            List<DataModel> dmlist = new List<DataModel>();
            if (getdata != null)
            {
                foreach (var item in getdata)
                {
                    if (item.PatientId==getpid.PatientId)
                    {
                        dmlist.Add(new DataModel
                        {
                            FirstName = item.FirstName,
                            AppointmentDate = item.AppointmentDate,
                            AppointmentRecieveDate = item.AppointmentRecieveDate,
                            AppointmentStatus = item.AppointmentStatus,
                            Subject = item.Subject,
                            Description = item.Description

                        });
                    }
                    
                }
                
                dm.AppointmentList = dmlist;
            }
            else
            {
                ViewBag.text = "No Appointments yet";
            }
            return View(dm);
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
        public ActionResult SendMessage()
        {
            DataModel dt = new DataModel();
            List<DataModel> dlst = new List<DataModel>();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var id = Convert.ToInt32(Session["MemberId"]);
            var getpid = db.Patients.Where(m => m.MemberId == id).FirstOrDefault();
            var status = db.DoctorAppointments.Where(m=>m.PatientId== id);
            var getdata = from a in db.DoctorAppointments
                          join d in db.Doctors
                          on a.DoctorId equals d.DoctorId
                          where a.PatientId== getpid.PatientId
                          select new
                          {
                              a.AppointmentStatus,
                              a.AppointmentDate,
                              a.DoctorId,
                              a.PatientId,
                              d.FirstName
                          };

            if (getdata != null)
            {
                
                foreach (var item in getdata)
                {
                    if (item.AppointmentStatus=="Accepted")
                    {
                        dlst.Add(new DataModel
                        {
                            FirstName = item.FirstName,
                            AppointmentDate = item.AppointmentDate,
                            PatientId = item.PatientId,
                            DoctorId = item.DoctorId
                        });
                    }
                    
                    
                }

                dt.MsgList = dlst;
            }
            else
            {
                ViewBag.text = "No Accepted Appoitments yet";
            }
            return View(dt);
        }

        public ActionResult Message(string fromid,string toid)
        {
            DataModel dt = new DataModel();
            dt.PatientId = Convert.ToInt32(fromid);
            dt.DoctorId = Convert.ToInt32(toid);
            return View(dt);
        }

        [HttpPost]
        public ActionResult Message(DataModel dt)
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var id = Convert.ToInt32(Session["MemberId"]);
            var getfromemail = db.MemberLogins.Where(m => m.MemberId == id).FirstOrDefault();
            
            var docid = db.Doctors.Where(m=>m.DoctorId==dt.DoctorId).FirstOrDefault();
            var gettoemail = db.MemberLogins.Where(m => m.MemberId == docid.MemberId).FirstOrDefault();
            
            Inbox obj = new Inbox();
            obj.FromEmailId = getfromemail.Email;
            obj.ToEmailId = gettoemail.Email;
            obj.Subject = dt.Subject;
            obj.MessageDetail = dt.MessageDetails;
            obj.MessageDate = DateTime.Now;
            obj.ReplyId = 0;
            obj.IsRead = false;
            db.Inboxes.Add(obj);
            db.SaveChanges();
            ViewBag.text = "Message Sent";
            return View();
        }

        [Authorize]
        public ActionResult ViewMessage()
        {
            DataModel dt = new DataModel();
            List<DataModel> dlst = new List<DataModel>();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var id = Convert.ToInt32(Session["MemberId"]);
            var fromemail = db.MemberLogins.Where(m => m.MemberId == id).FirstOrDefault();
            var getdata = (from i in db.Inboxes
                              join M in db.MemberLogins
                              on i.ToEmailId equals M.Email
                              join D in db.Doctors
                              on M.MemberId equals D.MemberId
                             where i.FromEmailId == fromemail.Email && i.ReplyId==0
                             select new 
                             {
                              D.FirstName,
                              i.FromEmailId,
                              i.ToEmailId,
                              i.MessageDate,
                              i.MessageDetail,
                              i.MessageId,
                              i.Subject
                             }).ToList();
            if (getdata != null)
            {
                foreach (var item in getdata)
                {
                    dlst.Add(new DataModel
                    {
                        MessageDetails = item.MessageDetail,
                        Subject = item.Subject,
                        FromEmail = item.FromEmailId,
                        ToEmail = item.ToEmailId,
                        MsgDate = item.MessageDate,
                        FirstName = item.FirstName
                    });
                }
            }
            else
            {
                ViewBag.text = "No Messages to View";
            }

            dt.MsgList = dlst;
            return View(dt);
        }

        public ActionResult ViewAllMessage(string pmail, string docmail, string subject)
        {
            DataModel dt = new DataModel();
            dt.DMail = docmail;
            dt.PMail = pmail;
            dt.Subject = subject;
            List<DataModel> dlst = new List<DataModel>();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var id = Convert.ToInt32(Session["MemberId"]);
            var fromemail = db.MemberLogins.Where(m => m.MemberId == id).FirstOrDefault();
            var getdata = db.Inboxes.Where(m => (m.FromEmailId == pmail && m.ToEmailId == docmail) || (m.FromEmailId == docmail && m.ToEmailId == pmail)).ToList();
            if (getdata!=null)
            {
                foreach (var item in getdata)
                {
                    if(item.ReplyId!=0)
                    {
                        dlst.Add(new DataModel { 
                        From = item.FromEmailId,
                        MessageDetails = item.MessageDetail,
                        MsgDate = item.MessageDate
                        });
                    }
                    dt.MsgViewList = dlst;
                }
            }
            else
            {
                ViewBag.text = "No new messages";
            }
            return View(dt);
        }

        [HttpPost]
        public ActionResult ViewAllMessage(DataModel dt)
        {
            List<DataModel> dlst = new List<DataModel>();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            Inbox obj = new Inbox();
            obj.FromEmailId = dt.PMail;
            obj.ToEmailId = dt.DMail;
            obj.Subject = dt.Subject;
            obj.MessageDetail = dt.MessageDetails;
            obj.MessageDate = DateTime.Now;
            obj.ReplyId += 1;
            db.Inboxes.Add(obj);
            db.SaveChanges();
            var getdata = db.Inboxes.Where(m => (m.FromEmailId == dt.DMail && m.ToEmailId == dt.PMail) || (m.FromEmailId == dt.PMail && m.ToEmailId == dt.DMail)).ToList();
            foreach (var item in getdata)
            {
                dlst.Add(new DataModel
                {
                    From = item.FromEmailId,
                    MessageDetails = item.MessageDetail,
                    MsgDate = item.MessageDate
                });

            }

            dt.MsgViewList = dlst;
            return View(dt);
        }

        [Authorize]
        public ActionResult OrderDrug()
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var getdata = db.Drugs.Where(m=>m.IsDeleted==false).ToList();
            DrugModel dm = new DrugModel();
            List<SelectListItem> lst = new List<SelectListItem>();
            if(getdata!=null)
            {
                foreach (var item in getdata)
                {
                    lst.Add(new SelectListItem
                    {
                        Text = item.DrugName,
                        Value = item.DrugId.ToString()
                    });
                }
                dm.DrugNameList = lst;
            }
            else
            {
                ViewBag.text = "No Drugs Available";
            }
            return View(dm);
        }

        public JsonResult GetQuantity(int id)
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var getdata = db.Drugs.Where(m => m.DrugId == id).FirstOrDefault();
            var qty = getdata.TotalQuantityAvailable;
            return Json(qty, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OrderDrug(DrugModel dm)
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var getqty = db.Drugs.Where(m => m.DrugId == dm.DrugId).FirstOrDefault();
            int id = Convert.ToInt32(Session["MemberId"]);
            var getpid = db.Patients.Where(m => m.MemberId == id).FirstOrDefault();
            int month = DateTime.Now.Month;
            PatientOrderDetail obj = new PatientOrderDetail();
            if(dm.OrderQuantity<getqty.TotalQuantityAvailable)
            {
                obj.PatientId = getpid.PatientId;
                obj.DrugId = dm.DrugId;
                obj.OrderDate = DateTime.Now;
                obj.Quantity = dm.OrderQuantity;
                obj.OrderStatus="Requested";
                db.PatientOrderDetails.Add(obj);
                getqty.TotalQuantityAvailable = getqty.TotalQuantityAvailable - dm.OrderQuantity;
                db.SaveChanges();
                ViewBag.text = "Order placed successfully";
            }
            else
            {
                ViewBag.text = "Order Quantity cannot exceed Total Quantity";
            }

            var getdata = db.Drugs.Where(m => m.IsDeleted == false).ToList();
            
            List<SelectListItem> lst = new List<SelectListItem>();
            if (getdata != null)
            {
                foreach (var item in getdata)
                {
                    lst.Add(new SelectListItem
                    {
                        Text = item.DrugName,
                        Value = item.DrugId.ToString()
                    });
                }
                dm.DrugNameList = lst;
            }
            return View(dm);
        }

        [Authorize]
        public ActionResult ViewOrders()
        {
            DrugModel dm = new DrugModel();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            int id = Convert.ToInt32(Session["MemberId"]);
            var getpid = db.Patients.Where(m => m.MemberId == id).FirstOrDefault();
            var getdata = from d in db.Drugs
                          join o in db.PatientOrderDetails
                          on d.DrugId equals o.DrugId
                          where o.PatientId == getpid.PatientId
                          select new {
                          o.OrderDate,
                          o.OrderStatus,
                          o.Quantity,
                          d.DrugName
                          };
            List<DrugModel> dmlst = new List<DrugModel>();
            if(getdata!=null)
            {
                foreach (var item in getdata)
                {
                    dmlst.Add(new DrugModel
                    {
                        DrugName = item.DrugName,
                        OrderQuantity = item.Quantity,
                        OrderStatus = item.OrderStatus,
                        OrderDate = item.OrderDate
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