namespace Lab4.CSP.Models
{
    public class Lesson
    {
        public Teacher Teacher { get; }
        public Subject Subject { get; }
        public Group Group { get; }
        public bool IsLecture { get; }
        public int PerWeek { get; }

        public Lesson(Teacher teacher, Subject subject, Group group, bool isLecture, int perWeek)
        {
            Teacher = teacher;
            Subject = subject;
            Group = group;
            IsLecture = isLecture;
            PerWeek = perWeek;
        }

        public override string ToString() =>
            $"{Teacher} | {Subject} | {Group} | {(IsLecture ? "Lecture" : "Seminar")} {PerWeek}/week";

    }
}
