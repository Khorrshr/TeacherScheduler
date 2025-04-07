// Services/Scheduler.cs
using TempusNexum.Data;
using TempusNexum.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TempusNexum.Services
{
    public class Scheduler
    {
        private readonly AppDbContext _context;
        private readonly User _currentUser;

        public Scheduler(AppDbContext context, User currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public List<Timetable> GenerateTimetable(List<Schedule> schedules, List<Course> courses, List<Facility> facilities)
        {
            var timetables = new List<Timetable>();
            var availableTeachers = schedules
                .Where(s => s.Status != "Unavailable")
                .GroupBy(s => s.TimeSlot)
                .ToList();

            foreach (var course in courses)
            {
                var compatibleTeachers = (from s in schedules
                                          join tg in _context.TeacherGroups on s.TeacherID equals tg.TeacherID
                                          where tg.GroupID == course.GroupID && s.Status != "Unavailable"
                                          select s).ToList();

                foreach (var teacherSchedule in compatibleTeachers)
                {
                    var facility = facilities.FirstOrDefault(f => f.TimeSlot == teacherSchedule.TimeSlot && f.IsAvailable);
                    if (facility != null)
                    {
                        var timetable = new Timetable
                        {
                            TimeSlot = teacherSchedule.TimeSlot,
                            TeacherID = teacherSchedule.TeacherID,
                            CourseID = course.CourseID,
                            FacilityID = facility.FacilityID,
                            CreatedBy = _currentUser.UserID
                        };
                        timetables.Add(timetable);
                        facility.IsAvailable = false; // Mark as occupied
                        break;
                    }
                }
            }

            return timetables;
        }

        public List<string> GetConflicts(List<Timetable> draft)
        {
            var conflicts = new List<string>();
            var grouped = draft.GroupBy(t => new { t.TimeSlot, t.TeacherID });
            foreach (var group in grouped)
            {
                if (group.Count() > 1)
                {
                    conflicts.Add($"Teacher {group.Key.TeacherID} is double-booked at {group.Key.TimeSlot}");
                }
            }
            return conflicts;
        }
    }
}