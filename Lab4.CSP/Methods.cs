using Lab4.CSP.Models;

namespace Lab4.CSP
{
    public static class Methods
    {
        public static Dictionary<int, string> timeSchedule = new Dictionary<int, string>();
        public static List<Classroom> classrooms = new List<Classroom>();
        public static List<LessonTime> schedule = new List<LessonTime>();
        public static List<Teacher> teachers = new List<Teacher>();
        public static List<Subject> subjects = new List<Subject>();
        public static List<Group> groups = new List<Group>();
        public static List<Lesson> lessons = new List<Lesson>();

        public static void InitializeData()
        {
            timeSchedule = new Dictionary<int, string>
        {
            { 1, "8:40-10:15" },
            { 2, "10:35-12:10" },
            { 3, "12:20-13:55" }
        };

            classrooms = new List<Classroom>
        {
            new Classroom(43, true),
            new Classroom(42, true),
            new Classroom(41, true),
            new Classroom(228, false),
            new Classroom(217, false),
            new Classroom(206, false)
        };


            schedule = new List<LessonTime>();
            for (int w = 1; w <= 5; w++)
            {
                for (int n = 1; n <= timeSchedule.Count; n++)
                {
                    schedule.Add(new LessonTime((DayOfWeek)w, timeSchedule[n]));
                }
            }

            teachers = new List<Teacher>
        {
            new Teacher("0 T1"),
            new Teacher("1 T2"),
            new Teacher("2 T3"),
            new Teacher("3 T4"),
            new Teacher("4 T5"),
            new Teacher("5 T6"),
            new Teacher("6 T7"),
            new Teacher("7 T8"),
            new Teacher("8 T9"),
            new Teacher("9 T10"),
            new Teacher("10 T11"),
            new Teacher("11 T12"),
            new Teacher("12 T13"),
            new Teacher("13 T14"),
            new Teacher("14 T15"),
            new Teacher("15 T16"),
            new Teacher("16 T17"),
            new Teacher("17 T18"),
            new Teacher("18 T19")
        };

            subjects = new List<Subject>
        {
            new Subject("0 S1"),
            new Subject("1 S2"),
            new Subject("2 S3"),
            new Subject("3 S4"),
            new Subject("4 S5"),
            new Subject("5 S6"),
            new Subject("6 S7"),
            new Subject("7 S8"),
            new Subject("8 S9"),
            new Subject("9 S10"),
            new Subject("10 S11"),
            new Subject("11 S12"),
            new Subject("12 S13"),
            new Subject("13 S14"),
            new Subject("14 S15"),
            new Subject("15 S16"),
            new Subject("16 S17"),
            new Subject("17 S18")
        };

            groups = new List<Group>
        {
            new Group("TTP41"),
            new Group("TTP42")
        };

            lessons = new List<Lesson>
        {
            new Lesson(teachers[0], subjects[0], groups[0], false, 1),
            new Lesson(teachers[1], subjects[1], groups[1], true, 1),
            new Lesson(teachers[2], subjects[2], groups[0], true, 2),
        };

            Schedule mySchedule = new Schedule(lessons, classrooms, schedule);
            Console.WriteLine(mySchedule);
        }
        public static void PrintSchedule(Schedule solution)
        {
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine("\n" + new string('=', 100));
                Console.WriteLine($"{(DayOfWeek)i}");
                foreach (var time in timeSchedule.Keys)
                {
                    Console.WriteLine($"\n\n{timeSchedule[time]}");
                    foreach (var c in classrooms)
                    {
                        Console.Write($"\n{c}\t\t");
                        for (int j = 0; j < solution.Lessons.Count; j++)
                        {
                            if (solution.Times[j].Weekday == (DayOfWeek)i &&
                                solution.Times[j].TimeSlot == timeSchedule[time] &&
                                solution.Classrooms[j].Room == c.Room)
                            {
                                Console.Write(solution.Lessons[j]);
                            }
                        }
                    }
                }
            }
        }

        public static Dictionary<int, List<ScheduleItem>> InitializeScheduleItems()
        {
            var domain = new Dictionary<int, List<ScheduleItem>>();
            var buf = new List<ScheduleItem>();
            var bufLecture = new List<ScheduleItem>();

            for (int i = 1; i <= 5; i++)
            {
                foreach (var timeSlot in timeSchedule.Keys)
                {
                    foreach (var room in classrooms)
                    {
                        buf.Add(new ScheduleItem(((DayOfWeek)i), timeSchedule[timeSlot], room));
                        if (room.IsBig)
                        {
                            bufLecture.Add(new ScheduleItem(((DayOfWeek)i), timeSchedule[timeSlot], room));
                        }
                    }
                }
            }

            for (int i = 0; i < lessons.Count; i++)
            {
                if (lessons[i].IsLecture)
                {
                    domain[i] = new List<ScheduleItem>(bufLecture);
                }
                else
                {
                    domain[i] = new List<ScheduleItem>(buf);
                }
            }

            return domain;
        }
        public static Dictionary<int, List<ScheduleItem>> UpdateScheduleItems(Dictionary<int, List<ScheduleItem>> items,
                                                      Lesson lesson, string day, string time, Classroom room)
        {
            var updatedItems = new Dictionary<int, List<ScheduleItem>>();
            foreach (var key in items.Keys)
            {
                var updatedValues = items[key].Where(d => !(d.Day.ToString() == day && d.Time == time && d.Room == room) &&
                                                            !(d.Day.ToString() == day && d.Time == time && (lesson.Teacher == lessons[key].Teacher ||
                                                                                                 IntersectGroups(lesson.Group, lessons[key].Group).Any()))).ToList();
                updatedItems[key] = updatedValues;
            }
            return updatedItems;
        }

        public static Schedule Backtrack(Func<Dictionary<int, List<ScheduleItem>>, int> heuristic,
                                   Dictionary<int, List<ScheduleItem>> scheduleItems, Schedule schedule)
        {
            if (!scheduleItems.Any())
                return schedule;

            int pos = heuristic(scheduleItems);
            if (pos == -1)
                return null;

            foreach (var d in scheduleItems[pos])
            {
                var schCopy = new Schedule(schedule);
                schCopy.Times.Add(new LessonTime(d.Day, d.Time));
                schCopy.Classrooms.Add(d.Room);
                schCopy.Lessons.Add(lessons[pos]);

                var domCopy = new Dictionary<int, List<ScheduleItem>>(scheduleItems);
                domCopy.Remove(pos);
                domCopy = UpdateScheduleItems(domCopy, lessons[pos], d.Day.ToString(), d.Time, d.Room);

                var res = Backtrack(heuristic, domCopy, schCopy);
                if (res != null)
                    return res;
            }
            return null;
        }


        public static Schedule RunMRV()
        {
            return Backtrack(MRV, InitializeScheduleItems(), new Schedule(new List<Lesson>(), new List<Classroom>(), new List<LessonTime>()));
        }

        public static Schedule RunLCV()
        {
            return Backtrack(LCV, InitializeScheduleItems(), new Schedule(new List<Lesson>(), new List<Classroom>(), new List<LessonTime>()));
        }

        public static Schedule RunDegree()
        {
            return Backtrack(Degree, InitializeScheduleItems(), new Schedule(new List<Lesson>(), new List<Classroom>(), new List<LessonTime>()));
        }

        public static Schedule RunForwardChecking()
        {
            return Backtrack(ForwardChecking, InitializeScheduleItems(), new Schedule(new List<Lesson>(), new List<Classroom>(), new List<LessonTime>()));
        }


        private static int MRV(Dictionary<int, List<ScheduleItem>> scheduleItems)
        {
            int minLen = int.MaxValue;
            int pos = -1;

            foreach (var key in scheduleItems.Keys)
            {
                if (scheduleItems[key].Count < minLen)
                {
                    minLen = scheduleItems[key].Count;
                    pos = key;
                }
            }

            return pos;
        }

        private static int Degree(Dictionary<int, List<ScheduleItem>> scheduleItems)
        {
            int maxDegree = -1;
            int pos = -1;

            foreach (var key in scheduleItems.Keys)
            {
                int degree = 0;
                foreach (var otherKey in scheduleItems.Keys)
                {
                    if (otherKey != key && lessons[key].Teacher == lessons[otherKey].Teacher)
                    {
                        degree++;
                    }
                    degree += IntersectGroups(lessons[key].Group, lessons[otherKey].Group).Count();
                }

                if (degree > maxDegree)
                {
                    maxDegree = degree;
                    pos = key;
                }
            }

            return pos;
        }


        private static int LCV(Dictionary<int, List<ScheduleItem>> scheduleItems)
        {
            var counts = new Dictionary<int, int>();

            foreach (var i in scheduleItems.Keys)
            {
                counts[i] = 0;
                foreach (var key in scheduleItems.Keys)
                {
                    if (i != key)
                    {
                        foreach (var d in scheduleItems[key])
                        {
                            if (!AreConflicting(d, scheduleItems[i][0]) && !AreConflicting(d, scheduleItems[i][0], key, i))
                            {
                                counts[i]++;
                            }
                        }
                    }
                }
            }

            return counts.OrderByDescending(c => c.Value).FirstOrDefault().Key;
        }

        private static List<Group> IntersectGroups(Group group1, Group group2)
        {
            var intersection = new List<Group>();

            if (group2.Name == group1.Name)
            {
                intersection.Add(group1);
            }

            return intersection;
        }

        private static bool AreConflicting(ScheduleItem d1, ScheduleItem d2, int key1 = -1, int key2 = -1)
        {
            return d1.Day == d2.Day && d1.Time == d2.Time && (d1.Room == d2.Room || (key1 != -1 && key2 != -1 &&
                   (lessons[key1].Teacher == lessons[key2].Teacher || IntersectGroups(lessons[key1].Group, lessons[key2].Group).Any())));
        }

        public static int ForwardChecking(Dictionary<int, List<ScheduleItem>> scheduleItems)
        {
            return scheduleItems.Keys.First();
        }
    }

}
