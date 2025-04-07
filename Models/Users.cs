// Models/User.cs

using System;
using System.Collections.Generic;

namespace TempusNexum.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Roles { get; set; } // Comma-separated, e.g., "Teacher,Manager"
    }
}

// Models/Teacher.cs
namespace TempusNexum.Models
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public int UserID { get; set; }
        public int ObligatoryHours { get; set; }
        public string Name { get; set; }
        public List<Schedule> Schedules { get; set; }
        public List<TeacherGroup> TeacherGroups { get; set; }
    }
}

// Models/Group.cs
namespace TempusNexum.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<TeacherGroup> TeacherGroups { get; set; }
    }
}

// Models/TeacherGroup.cs
namespace TempusNexum.Models
{
    public class TeacherGroup
    {
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }
        public int GroupID { get; set; }
        public Group Group { get; set; }
    }
}

// Models/Schedule.cs
namespace TempusNexum.Models
{
    public class Schedule
    {
        public int ScheduleID { get; set; }
        public int TeacherID { get; set; }
        public DateTime TimeSlot { get; set; }
        public string Status { get; set; } // "InPerson", "Online", "Unavailable"
        public Teacher Teacher { get; set; }
    }
}

// Models/Course.cs
namespace TempusNexum.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string Name { get; set; }
        public int RequiredHours { get; set; }
        public int GroupID { get; set; }
        public Group Group { get; set; }
        public int StudyProgramID { get; set; }
        public StudyProgram StudyProgram { get; set; }
    }
}

// Models/StudyProgram.cs
namespace TempusNexum.Models
{
    public class StudyProgram
    {
        public int StudyProgramID { get; set; }
        public string Name { get; set; }
        public string SubjectStatus { get; set; } // "Pending", "Complete", "InProcess"
        public List<Course> Courses { get; set; }
    }
}

// Models/Facility.cs
namespace TempusNexum.Models
{
    public class Facility
    {
        public int FacilityID { get; set; }
        public string Name { get; set; }
        public DateTime TimeSlot { get; set; }
        public bool IsAvailable { get; set; }
    }
}

// Models/Timetable.cs
namespace TempusNexum.Models
{
    public class Timetable
    {
        public int TimetableID { get; set; }
        public DateTime TimeSlot { get; set; }
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public int FacilityID { get; set; }
        public Facility Facility { get; set; }
        public int CreatedBy { get; set; }
        public User Creator { get; set; }
    }
}