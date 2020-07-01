using ClinicalAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Controllers
{
    public class DoctorController : Controller
    {
        [Authorize]
        // GET: Doctor
        public ActionResult Home()
        {
            var id = Convert.ToInt32(Session["MemberId"]);

            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            DataModel dt = new DataModel();

            var chkname = db.Doctors.Where(m => m.MemberId == id).FirstOrDefault();
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
            
            var getdata = db.Doctors.Where(m => m.MemberId == id).FirstOrDefault();
            if (getdata != null)
            {
                dt.FirstName = getdata.FirstName;
                dt.LastName = getdata.LastName;
                dt.Gender = getdata.Gender;
                dt.SpecializationId = getdata.SpecializedId;
                dt.TotalExperience = getdata.TotalExperience;
                
            }
            else
            {
                dt.FirstName = null;
            }


            List<SelectListItem> lst = new List<SelectListItem>();// LOCAL LIST
            var getspec = db.SpecializedDetails.ToList();
            foreach (var item in getspec)
            {
                lst.Add(new SelectListItem
                {

                    Text = item.SpecializedName,
                    Value = item.SpecializedId.ToString()

                });

            }
            dt.SpecList = lst;

            return View(dt);
        }

        [HttpPost]
        public ActionResult EditProfile(DataModel dt)
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var id = Convert.ToInt32(Session["MemberId"]);
            dt.MemberId = id;

            Doctor objDoctor = new Doctor();

            var getdata = db.Doctors.Where(m => m.MemberId == id).FirstOrDefault();
            if (getdata != null)
            {
                getdata.FirstName = dt.FirstName;
                getdata.LastName = dt.LastName;
                getdata.Gender = dt.Gender;
                getdata.SpecializedId = dt.SpecializationId;
                getdata.TotalExperience = dt.TotalExperience;

            }
            else
            {
                objDoctor.FirstName = dt.FirstName;
                objDoctor.LastName = dt.LastName;
                objDoctor.TotalExperience = dt.TotalExperience;
                objDoctor.Gender = dt.Gender;
                objDoctor.SpecializedId = dt.SpecializationId;
                objDoctor.MemberId = dt.MemberId;
                db.Doctors.Add(objDoctor);
            }

            
                db.SaveChanges();
            ViewBag.text = "data inserted";
            List<SelectListItem> lst = new List<SelectListItem>();// LOCAL LIST
            var getSpec = db.SpecializedDetails.ToList();
            foreach (var item in getSpec)
            {
                lst.Add(new SelectListItem
                {
                    Text = item.SpecializedName,
                    Value = item.SpecializedId.ToString()

                });
                dt.SpecList = lst;
            }

            var chkname = db.Doctors.Where(m => m.MemberId == id).FirstOrDefault();
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
        public ActionResult ViewAppointment()
        {
           
                DataModel dm = new DataModel();
                Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();

                var id = Convert.ToInt32(Session["MemberId"]);
                var getpid = db.Doctors.Where(m => m.MemberId == id).FirstOrDefault();

                var getdata = from a in db.DoctorAppointments
                              join p in db.Patients
                              on a.PatientId equals p.PatientId
                              select new
                              {
                                  a.AppointmentDate,
                                  a.AppointmentStatus,
                                  a.Subject,
                                  a.Description,
                                  a.AppointmentId,
                                  a.DoctorId,
                                  p.FirstName,
                                  p.Gender
                              }
                             ;
                List<DataModel> dmlist = new List<DataModel>();
                if (getdata != null)
                {
                    foreach (var item in getdata)
                    {
                        if (item.DoctorId == getpid.DoctorId)
                        {
                            dmlist.Add(new DataModel
                            {
                                FirstName = item.FirstName,
                                AppointmentDate = item.AppointmentDate,
                                Gender = item.Gender,
                                AppointmentStatus = item.AppointmentStatus,
                                Subject = item.Subject,
                                Description = item.Description,
                                AppointmentId = item.AppointmentId

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

       
        public ActionResult UpdateAppointments(int id, string str)
        {
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var obj = db.DoctorAppointments.Where(m=>m.AppointmentId==(id)).FirstOrDefault();
             obj.AppointmentStatus = str;
            
              db.SaveChanges();
            return RedirectToAction("ViewAppointment","Doctor");
        }

        [Authorize]
        public ActionResult ChangePwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePwd(PwdModel dt)
        {
            var id = Convert.ToInt32(Session["MemberId"]);
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var getdata = db.MemberLogins.Where(m => m.MemberId == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (getdata.Password == dt.OldPassword)
                {
                    getdata.Password = dt.NewPassword;
                    db.SaveChanges();
                    ViewBag.text = "Password Changed Successfully";
                }
                else
                {
                    ViewBag.text = "You entered a wrong current password";
                }
                
            }
            
            return View();
        }

        public ActionResult ViewMessage()
        {
            DataModel dt = new DataModel();
            List<DataModel> dlst = new List<DataModel>();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var id = Convert.ToInt32(Session["MemberId"]);
            var docemail = db.MemberLogins.Where(m => m.MemberId == id).FirstOrDefault();
            var getdata = (from i in db.Inboxes
                           join M in db.MemberLogins
                           on i.FromEmailId equals M.Email
                           join P in db.Patients
                           on M.MemberId equals P.MemberId
                           where i.ToEmailId == docemail.Email && i.ReplyId == 0
                           select new
                           {
                               P.FirstName,
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

        public ActionResult ViewAllMessage(string docmail, string pmail,string subject)
        {

            DataModel dt = new DataModel();
            dt.DMail = docmail;
            dt.PMail = pmail;
            dt.Subject = subject;


            List<DataModel> dlst = new List<DataModel>();
            Clinic_automation_systemEntities db = new Clinic_automation_systemEntities();
            var id = Convert.ToInt32(Session["MemberId"]);
            var getdata = db.Inboxes.Where(m => (m.FromEmailId == pmail && m.ToEmailId ==docmail )||(m.FromEmailId==docmail && m.ToEmailId==pmail)).ToList();
            if (getdata != null)
            {
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
            obj.FromEmailId = dt.DMail;
            obj.ToEmailId = dt.PMail;
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
    }
}