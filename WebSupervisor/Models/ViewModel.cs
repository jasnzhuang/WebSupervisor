﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupervisor.Models
{

    /// <summary>
    /// 手动填补显示
    /// </summary>
    public class HandSpareTime
    {
     public string Tid { set; get; }
     public List<Freetime> FreeTimel { set; get; }
    }
    public class Freetime
    {
        public int Week { set; get;}
        public List<DayClassesNumber> DayClist { set; get; }
    }
    public class DayClassesNumber
    {
        public int Day { set; get; }
        public List<int> Classnumberl { set; get; }
    }
    /// <summary>
    /// 首选督导
    /// </summary>
    public class FirstSupervisorModel
    {
        public string Tid { set; get; }
        public string TeacherName { set; get; }
        public string IsArrage { set; get; }
    }

    /// <summary>
    /// 备选督导
    /// </summary>
    public class SecondSupervisorModel
    {
        public string Tid { set; get; }
        public string TeacherName { set; get; }
        public int Total { set; get; }
    }
    /// <summary>
    /// 添加安排视图模型
    /// </summary>
    public class ArrageAddModel
    {
        public List<ClassesModel> classeslist { set; get; }
        public List<FirstSupervisorModel> FirstSupervisorList { set; get; }
        public List<SecondSupervisorModel> SecondSupervisorList { set; get; }
    }
    /// <summary>
    /// 安排视图模型
    /// </summary>
    public class ConfirmModel
    {
        public string Pid { set; get; }
        public string Cid { set; get; }
        public string ClassName { set; get; }
        public string ClassContent { set; get; }
        public string ClassType { set; get; }
        public string Major { set; get; }
        public string Address { set; get; }
        public string TeacherName { set; get; }
        public int Week { get; set; }
        public int ClassNumber { set; get; }
        public string SuperVisors { set; get; }
        public int Day { get; set; }
        public int Total { set; get; }
    }
    /// <summary>
    /// 导出课程模型
    /// </summary>
    class ExportClassModel
    {
        private string teachername;//教师姓名
        private int week;//周
                         /// <summary>
                         /// 上课周
                         /// </summary>
        public int Week
        {
            get { return week; }
            set { week = value; }
        }
        /// <summary>
        /// 星期
        /// </summary>
        private int day;//星期

        public int Day
        {
            get { return day; }
            set { day = value; }
        }
        /// <summary>
        /// 教师姓名
        /// </summary>
        public string Teachername
        {
            get { return teachername; }
            set { teachername = value; }
        }
        /// <summary>
        /// 课程类型
        /// </summary>
        private string classtype;//课程类型

        public string Classtype
        {
            get { return classtype; }
            set { classtype = value; }
        }
        private string classname;
        /// <summary>
        /// 课程名称
        /// </summary>
        public string Classname
        {
            get { return classname; }
            set { classname = value; }
        }
        private int start;//开始节次
                          /// <summary>
                          /// 上课开始的节次
                          /// </summary>
        public int Start
        {
            get { return start; }
            set { start = value; }
        }
        private int end;//终止节次
                        /// <summary>
                        /// 上课结束的节次，例如3-4节，则start为3 end为4.
                        /// </summary>
        public int End
        {
            get { return end; }
            set { end = value; }
        }
        private bool isOverTop;//是否超过2节课
                               /// <summary>
                               /// 判断是否只有2节课
                               /// </summary>
        public bool IsOverTop
        {
            get { return isOverTop; }
            set { isOverTop = value; }
        }
        public ExportClassModel()
        {

        }
        public ExportClassModel(string teachername, string classtype, int week, int day, int start, int end, bool isOverTop, string classname)
        {
            this.classtype = classtype;
            this.end = end;
            this.isOverTop = isOverTop;
            this.teachername = teachername;
            this.start = start;
            this.week = week;
            this.day = day;
            this.classname = classname;
        }
    }

    /// <summary>
    /// 督导信息模型
    /// </summary>
    public class SupervisorViewModel
    {
        public string Tid { set; get; }
        public string TeacherName { set; get; }
        public string SpareTime { set; get; }
        public string Phone { set; get; }
        public string Password { set; get; }
    }
    public class ReferenceModel
    {
        public int Id { set; get; }
        public string Cid { set; get; }
        public string time { set; get; }
        public string TeacherName { set; get; }
        public string Address { set; get; }
        public string Major { set; get; }
        public string ClassType { set; get; }
        public int SupervisorsSum { set; get; }
    }
    /// <summary>
    /// 导入文件
    /// </summary>
    public class ReportFileStatusModel{
        public int code { set; get; }
        public string msg { set; get; }
    }
    /// <summary>
    /// 发送安排
    /// </summary>
    public class SendArrageModel
    {
        public string Pid { set; get; }
        public string Cid { set; get; }
        public string ClassName { set; get; }
        public string ClassContent { set; get; }
        public string ClassType { set; get; }
        public string Major { set; get; }
        public string Address { set; get; }
        public string TeacherName { set; get; }
        public string SuperVisors { set; get; }
        public string Time { set; get; }
    }
}
