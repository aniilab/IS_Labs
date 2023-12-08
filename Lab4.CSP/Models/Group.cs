namespace Lab4.CSP.Models
{
    public class Group
    {
        public string Name { get; }

        public Group(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}
