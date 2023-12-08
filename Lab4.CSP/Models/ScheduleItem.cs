namespace Lab4.CSP.Models
{
    public class ScheduleItem
    {
        public DayOfWeek Day { get; set; }
        public string Time { get; set; }
        public Classroom Room { get; set; }

        public ScheduleItem(DayOfWeek day, string time, Classroom room)
        {
            Day = day;
            Time = time;
            Room = room;
        }
    }
}
