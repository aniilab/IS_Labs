namespace Lab4.CSP.Models
{
    public class Schedule
    {
        public List<Lesson> Lessons { get; }
        public List<Classroom> Classrooms { get; }
        public List<LessonTime> Times { get; }

        public Schedule(List<Lesson> lessons, List<Classroom> classrooms, List<LessonTime> times)
        {
            Lessons = lessons;
            Classrooms = classrooms;
            Times = times;
        }

        public Schedule(Schedule copy)
        {
            Lessons = copy.Lessons;
            Classrooms = copy.Classrooms;
            Times = copy.Times;
        }

        public override string ToString()
        {
            string output = "";
            for (int i = 0; i < Lessons.Count; i++)
            {
                output += $"{Lessons[i]},   {Classrooms[i]},   {Times[i]}\n";
            }
            return output;
        }
    }
}
