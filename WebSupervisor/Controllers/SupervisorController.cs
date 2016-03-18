﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using WebSupervisor.Models;
using WebDAL;
using PagedList;
using WebSupervisor.Code.Placement;
using WebSupervisor.Controllers.CheckUser;
using WebSupervisor.Code.Classes;

namespace WebSupervisor.Controllers
{
    [AuthenAdmin]
    public class SupervisorController : Controller
    {
        List<TeachersModel> teacherlist = DBHelper.ExecuteList<TeachersModel>("select * from teachers where indentify=1", CommandType.Text, null);
        List<SpareTimeModel> splist = DBHelper.ExecuteList<SpareTimeModel>("select * from sparetime", CommandType.Text, null);
        List<ClassesModel> classlist = DBHelper.ExecuteList<ClassesModel>("select * from classes", CommandType.Text, null);
        // GET: SupervisorPage
        public ActionResult CheifSupervisor()
        {
            return View();
        }
        public ActionResult NormalSupervisor()
        {
            return View();
        }
        public ActionResult Reference(string year = "", string month = "", string day = "", int page = 1)
        {
            int i = 1;
            Common com = new Common();
            List<ReferenceModel> referencelist = new List<ReferenceModel>();
            if (year != "" && month != "" && day != "")
            {
                int thisday = CalendarTools.weekdays(CalendarTools.CaculateWeekDay(int.Parse(year), int.Parse(month), int.Parse(day)));
                int thisweek = CalendarTools.WeekOfYear(int.Parse(year), int.Parse(month), int.Parse(day)) - CalendarTools.WeekOfYear(Common.Year, Common.Month, Common.Day) + 1;
                if (Session["Power"].ToString() == "管理员")
                {
                    referencelist = (from t in teacherlist
                                     join c in classlist on t.TeacherName equals c.TeacherName
                                     where t.College == Session["College"].ToString() && c.Week == thisweek && c.Day == thisday
                                     select new ReferenceModel
                                     {
                                         Id = i++,
                                         time = CalendarTools.getdata(Common.Year, c.Day, c.Week - CalendarTools.weekdays(CalendarTools.CaculateWeekDay(Common.Year, Common.Month, Common.Day)), Common.Month, Common.Day).ToLongDateString() + "" + com.AddSeparator(c.ClassNumber),
                                         TeacherName = c.TeacherName,
                                         Address = c.Address,
                                         Major = c.Major,
                                         ClassType = c.ClassType,
                                         SupervisorsSum = numbersupervisor(c.Week, c.Day, c.ClassNumber)
                                     }).ToList();
                }
                else
                {
                    referencelist = (from c in classlist
                                     where c.Week == thisweek && c.Day == thisday
                                     select new ReferenceModel
                                     {
                                         Id = i++,
                                         Cid = c.Cid,
                                         time = CalendarTools.getdata(Common.Year, c.Day, c.Week - CalendarTools.weekdays(CalendarTools.CaculateWeekDay(Common.Year, Common.Month, Common.Day)), Common.Month, Common.Day).ToLongDateString() + "" + com.AddSeparator(c.ClassNumber),
                                         TeacherName = c.TeacherName,
                                         Address = c.Address,
                                         Major = c.Major,
                                         ClassType = c.ClassType,
                                         SupervisorsSum = numbersupervisor(c.Week, c.Day, c.ClassNumber)
                                     }).ToList();
                }
            }
            else
            {
                if (Session["Power"].ToString() == "管理员")
                {
                    referencelist = (from t in teacherlist
                                     join c in classlist on t.TeacherName equals c.TeacherName
                                     where t.College == Session["College"].ToString() 
                                     select new ReferenceModel
                                     {
                                         Id = i++,
                                         time = CalendarTools.getdata(Common.Year, c.Day, c.Week - CalendarTools.weekdays(CalendarTools.CaculateWeekDay(Common.Year, Common.Month, Common.Day)), Common.Month, Common.Day).ToLongDateString() + "" + com.AddSeparator(c.ClassNumber),
                                         TeacherName = c.TeacherName,
                                         Address = c.Address,
                                         Major = c.Major,
                                         ClassType = c.ClassType,
                                         SupervisorsSum = numbersupervisor(c.Week, c.Day, c.ClassNumber)
                                     }).ToList();
                }
                else
                {
                    referencelist = (from c in classlist
                                     select new ReferenceModel
                                     {
                                         Id = i++,
                                         Cid = c.Cid,
                                         time = CalendarTools.getdata(Common.Year, c.Day, c.Week - CalendarTools.weekdays(CalendarTools.CaculateWeekDay(Common.Year, Common.Month, Common.Day)), Common.Month, Common.Day).ToLongDateString() + "" + com.AddSeparator(c.ClassNumber)+"节",
                                         TeacherName = c.TeacherName,
                                         Address = c.Address,
                                         Major = c.Major,
                                         ClassType = c.ClassType,
                                         SupervisorsSum = numbersupervisor(c.Week, c.Day, c.ClassNumber)
                                     }).ToList();
                }
            }
            IPagedList<ReferenceModel> iplist = referencelist.ToPagedList(page, 10);
            return PartialView(iplist);
        }
        public ActionResult ReferenceSure(string cid)
        {
            var classesl =( from c in classlist
                           where c.Cid == cid
                           select new
                           {
                               c.Week,
                               c.Day,
                               c.ClassNumber,
                               c.TeacherName,
                               c.ClassType
                           }).First();
            return RedirectToAction("ArrageAddallselect",new {week=classesl.Week,day=classesl.Day,classnumber=classesl.ClassNumber,teachername=classesl.TeacherName,classtype=classesl.ClassType });
        }
        public ActionResult Supervisor()
        {
            return PartialView();
        }
        public PartialViewResult SupervisorList(int page = 1)
        {
            List<SupervisorViewModel> spvlist = new List<SupervisorViewModel>();
            foreach (TeachersModel teacher in teacherlist)
            {
                SupervisorViewModel m = new SupervisorViewModel();
                m.Tid = teacher.Tid;
                m.TeacherName = teacher.TeacherName;
                m.Phone = teacher.Phone;
                m.Password = teacher.Password;
                m.SpareTime = "";
                var sptlist = (from s in splist
                               where s.Tid == teacher.Tid
                               select s).ToList();
                if (sptlist.Count > 0)
                {
                    List<SpareTimeModel> sptlist1 = new List<SpareTimeModel>();
                    sptlist1 = sptlist.GroupBy(a => a.Week).Select(b => b.First()).ToList();
                    foreach (SpareTimeModel spt in sptlist1)
                    {
                        m.SpareTime = m.SpareTime + " " + spt.Week.ToString();
                    }
                }
                else
                {
                    m.SpareTime = "未填写";
                }
                spvlist.Add(m);
            }

            IPagedList<SupervisorViewModel> Iteachers = spvlist.ToPagedList(page, 10);
            return PartialView(Iteachers);
        }
        //自动填补空闲时间
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AutoSpare(FormCollection fc)
        {
            var cherkbox = from x in fc.AllKeys
                               //where fc[x] == "on"
                           select x;
            foreach (var cherkname in cherkbox)
            {
                var t =( from te in teacherlist
                        where te.Tid == cherkname
                        select te.TeacherName).First();
                MakeSpareTime.AutoSelectSpareTime(t);
                //string i=list[index].TeacherName;
                //string a;
            }
            return Redirect("/#!/Supervisor/Supervisor");
        }
        public ActionResult ArrageAddallselect(string week, string day, string classnumber, string teachername, string classtype)
        {
            int[] select = new int[] { int.Parse(week), int.Parse(day), int.Parse(classnumber) };
            List<CheckClassModel> checkclasslist = DBHelper.ExecuteList<CheckClassModel>("select * from checkclass", CommandType.Text, null);
            ArrageAddModel arrageadd = new ArrageAddModel();
            if (Session["Power"].ToString() == "管理员")
            {
                arrageadd.classeslist = (from c in classlist
                                         join t in teacherlist on c.TeacherName equals t.TeacherName
                                         where c.Week == @select[0] && c.Day == @select[1] && c.ClassNumber == @select[2] && c.TeacherName == teachername
                                         && c.ClassType == classtype && t.College == Session["College"].ToString()
                                         select c).ToList();
                arrageadd.FirstSupervisorList = (from s in splist
                                                 join t in teacherlist on s.Tid equals t.Tid
                                                 where t.College == Session["College"].ToString()&& s.Week == @select[0] && s.Day == @select[1] && s.ClassNumber == @select[2]
                                                 select new FirstSupervisorModel
                                                 {
                                                     TeacherName = t.TeacherName,
                                                     IsArrage = Trueflase(s.Assign)
                                                 }).ToList();
                arrageadd.SecondSupervisorList = (from ch in checkclasslist
                                                  join t in teacherlist on ch.Tid equals t.Tid
                                                  where t.College == Session["College"].ToString()
                                                  select new SecondSupervisorModel
                                                  {
                                                      TeacherName = t.TeacherName,
                                                      Total = ch.total
                                                  }).ToList();
            }
            else
            {
                arrageadd.classeslist = (from c in classlist
                                         where c.Week == @select[0] && c.Day == @select[1] && c.ClassNumber == @select[2] && c.TeacherName == teachername
                                         && c.ClassType == classtype
                                         select c).ToList();
                arrageadd.FirstSupervisorList = (from s in splist
                                                 join t in teacherlist on s.Tid equals t.Tid
                                                 where s.Week == @select[0] && s.Day == @select[1] && s.ClassNumber == @select[2]
                                                 select new FirstSupervisorModel
                                                 {
                                                     TeacherName = t.TeacherName,
                                                     IsArrage = Trueflase(s.Assign)
                                                 }).ToList();
                arrageadd.SecondSupervisorList = (from ch in checkclasslist
                                                  join t in teacherlist on ch.Tid equals t.Tid
                                                  select new SecondSupervisorModel
                                                  {
                                                      TeacherName = t.TeacherName,
                                                      Total = ch.total
                                                  }).ToList();
            }
            return Json(arrageadd,JsonRequestBehavior.AllowGet);
        }
        private string Trueflase(int i)
        {
            string chiness_tureorflase;
            if (i == 1)
            {
                chiness_tureorflase = "已安排";
                return chiness_tureorflase;
            }
            else
            {
                chiness_tureorflase = "未安排";
                return chiness_tureorflase;
            }

        }
        private int numbersupervisor(int week, int day, int classnumber)
        {
            var count = from sp in splist
                        where sp.Week == week && sp.Day == day && sp.ClassNumber == classnumber
                        group sp.Tid by sp into spl
                        select spl;
            return count.Count();

        }
    }
}