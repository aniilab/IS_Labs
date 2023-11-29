namespace Lab3.GeneticAlgorithm
{
    using Schedule = Dictionary<Tuple<int, int>, List<Lesson>>;
    public class Individual : IComparable<Individual>
    {
        public Schedule schedule;
        public int score;

        public Individual()
        {
            schedule = new Schedule();
        }

        public Individual(Individual other)
        {
            schedule = new Schedule();
            for (int i = 0; i < other.schedule.Count; i++)
            {
                schedule.Add(
                    new Tuple<int, int>(other.schedule.Keys.ElementAt(i).Item1,
                                        other.schedule.Keys.ElementAt(i).Item2),
                    new List<Lesson>(other.schedule.Values.ElementAt(i))
                );
            }
            score = other.score;
        }

        public int CompareTo(Individual other)
        {
            if (this.score < other.score)
            {
                return -1;
            }
            else if (this.score > other.score)
            {
                return 1;
            }
            return 0;
        }
    }

}
