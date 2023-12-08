namespace Lab4.CSP.Models
{
    public class Teacher
    {
        public string Name { get; }

        public Teacher(string name)
        {
            Name = name;
        }

        public override string ToString() => Name.Split()[1];
    }
}
