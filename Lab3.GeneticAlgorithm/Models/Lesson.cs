namespace Lab3.GeneticAlgorithm.Models
{
    public class Lesson
    {
        public string Day { get; set; }
        public int LessonNumber { get; set; }
        public Group Group { get; set; }
        public string Subject { get; set; }
        public Teacher Teacher { get; set; }
    }
}
