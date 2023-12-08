namespace Lab4.CSP.Models
{
    public class LessonTime
    {
        public DayOfWeek Weekday { get; }
        public string TimeSlot { get; }

        public LessonTime(DayOfWeek weekday, string timeSlot)
        {
            Weekday = weekday;
            TimeSlot = timeSlot;
        }
    }
}
