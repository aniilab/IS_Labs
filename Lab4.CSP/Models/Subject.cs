namespace Lab4.CSP.Models
{
    public class Subject
    {
        public string Name { get; }

        public Subject(string name)
        {
            Name = name;
        }

        public override string ToString() => Name.Split()[1];
    }
}
