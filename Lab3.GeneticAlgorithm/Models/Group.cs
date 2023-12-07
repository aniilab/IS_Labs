namespace Lab3.GeneticAlgorithm.Models
{
    public class Group
    {
        public string GroupName { get; set; }
        public Dictionary<string, int> SubjectsHours { get; set; }

        public Group(string groupName, Dictionary<string, int> subjectsHours)
        {
            GroupName = groupName;
            SubjectsHours = subjectsHours;
        }

        public override string ToString()
        {
            return $"Group(group_name={GroupName}, subjects_hours={{ {string.Join(", ", SubjectsHours.Select(sh => $"{sh.Key}: {sh.Value}"))} }})";
        }
    }
}
