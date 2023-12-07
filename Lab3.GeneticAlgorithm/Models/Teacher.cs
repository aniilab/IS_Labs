namespace Lab3.GeneticAlgorithm.Models
{
    public class Teacher
    {
        public string Name { get; set; }
        public List<string> Subjects { get; set; }
        public int MaxHours { get; set; }

        public Teacher(string name, List<string> subjects, int maxHours)
        {
            Name = name;
            Subjects = subjects;
            MaxHours = maxHours;
        }

        public override string ToString()
        {
            return $"Teacher(name={Name}, subjects=[{string.Join(", ", Subjects)}], max_hours={MaxHours})";
        }
    }
}
