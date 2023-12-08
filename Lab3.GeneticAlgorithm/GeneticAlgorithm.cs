using Lab3.GeneticAlgorithm.Models;

public class GeneticAlgorithm
{
    private static readonly Random rand = new Random();
    private const int nIter = 2;
    private const int nPop = 2;
    private const int nClasses = 3;
    private const double rMut = 0.9;
    private static readonly string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
    private static readonly string[] subjects = { "IS", "Mobs", "IT", "MIT" };
    private static List<Teacher> teachers;
    private static List<Group> groups;

    public GeneticAlgorithm()
    {
        InitializeTeachersAndGroups();
    }

    private void InitializeTeachersAndGroups()
    {
        teachers = new List<Teacher> {
            new Teacher("Fedorus", new List<string>{ "IS"}, 16),
            new Teacher("Taranukha", new List<string>{ "IS"}, 16),
            new Teacher("Tkachenko", new List<string>{"Mobs", "IT" }, 20),
            new Teacher("Polyshchuk", new List<string>{"Mobs"}, 14),
            new Teacher("Shyshatska", new List<string>{ "MIT"}, 13)
        };

        groups = new List<Group>
        {
            new Group("TTP41", new Dictionary<string, int> { { "IT", 2 }, { "IS", 1 }, {"Mobs", 4}, {"MIT", 2} }),
            new Group("TTP42", new Dictionary<string, int> { { "IT", 2 }, { "IS", 1 }, { "Mobs", 2 }, {"MIT", 3 }}),
        };
    }

    private List<List<Lesson>> GeneratePopulation()
    {
        List<List<Lesson>> population = new List<List<Lesson>>();

        for (int i = 0; i < nPop; i++)
        {
            var schedule = GenerateSchedule();
            population.Add(schedule);
        }

        return population;
    }

    private List<Lesson> GenerateSchedule()
    {
        List<Lesson> schedule = new List<Lesson>();
        foreach (var group in groups)
        {
            foreach (var subjectHour in group.SubjectsHours)
            {
                string subject = subjectHour.Key;
                int hours = subjectHour.Value;
                for (int i = 0; i < hours; i++)
                {
                    Lesson cell = new Lesson
                    {
                        Day = days[rand.Next(days.Length)],
                        LessonNumber = rand.Next(1, nClasses + 1),
                        Group = group,
                        Subject = subject,
                        Teacher = GetRandomTeacher(subject)
                    };
                    schedule.Add(cell);
                }
            }
        }
        return schedule;
    }

    private Teacher GetRandomTeacher(string subject)
    {
        var eligibleTeachers = teachers.Where(t => t.Subjects.Contains(subject)).ToList();
        return eligibleTeachers.Count > 0 ? eligibleTeachers[rand.Next(eligibleTeachers.Count)] : null;
    }

    private List<List<Lesson>> Crossover(List<Lesson> p1, List<Lesson> p2)
    {
        List<Lesson> c1 = new List<Lesson>(p1);
        List<Lesson> c2 = new List<Lesson>(p2);
        int pt = rand.Next(1, p1.Count - 2);
        c1 = p1.Take(pt).Concat(p2.Skip(pt)).ToList();
        c2 = p2.Take(pt).Concat(p1.Skip(pt)).ToList();

        var result = new List<List<Lesson>>
        {
            c1,
            c2
        };

        return result;
    }

    private List<Lesson> Selection(List<List<Lesson>> pop, List<double> scores)
    {
        int selectionIx = rand.Next(pop.Count);
        int selectionIy = rand.Next(pop.Count);
        return scores[selectionIx] > scores[selectionIy] ? pop[selectionIx] : pop[selectionIy];
    }

    private List<Lesson> Mutation(List<Lesson> schedule)
    {
        for (int i = 0; i < schedule.Count; i++)
        {
            if (rand.NextDouble() < rMut)
            {
                schedule[i].Day = days[rand.Next(days.Length)];
                schedule[i].LessonNumber = rand.Next(1, nClasses + 1);
            }
        }
        return schedule;
    }

    private double CalculateFitness(List<Lesson> schedule)
    {
        int conflicts = 0;
        var lessonsCount = new Dictionary<(string, int, Group), int>();

        foreach (var lesson in schedule)
        {
            var key = (lesson.Day, lesson.LessonNumber, lesson.Group);
            if (lessonsCount.ContainsKey(key))
            {
                lessonsCount[key]++;
            }
            else
            {
                lessonsCount[key] = 1;
            }
        }

        foreach (var count in lessonsCount.Values)
        {
            if (count > 1)
            {
                conflicts += count - 1;
            }
        }

        return 1.0 / (conflicts + 1.0);
    }


    public void Run()
    {
        var pop = GeneratePopulation();

        List<Lesson> best = pop[0];
        double bestEval = CalculateFitness(pop[0]);

        for (int gen = 0; gen < nIter; gen++)
        {
            List<double> scores = pop.Select(c => CalculateFitness(c)).ToList();
            for (int i = 0; i < nPop; i++)
            {
                if (scores[i] > bestEval)
                {
                    best = pop[i];
                    bestEval = scores[i];
                }
            }

            if (bestEval >= 0.99) break;

            List<List<Lesson>> newPopulation = new List<List<Lesson>>();
            while (newPopulation.Count < nPop)
            {
                List<Lesson> p1 = Selection(pop, scores);
                List<Lesson> p2 = Selection(pop, scores);
                if (rand.NextDouble() <= rMut)
                {
                    newPopulation.Add(Mutation(p1));
                    newPopulation.Add(Mutation(p2));
                }
                else
                {
                    newPopulation.AddRange(Crossover(p1, p2));
                }
            }

            pop = newPopulation;
        }

        var bestSorted = best.OrderBy(l => l.LessonNumber).ToList();

        Console.WriteLine("Best Schedule with Fitness: " + bestEval);

        foreach (var day in days)
        {
            Console.WriteLine($"{day}:");

            foreach (var cell in bestSorted)
            {
                if (cell.Day == day)
                {
                    Console.WriteLine($"{cell.LessonNumber}. {cell.Subject}, group: {cell.Group.GroupName}, teacher: {cell.Teacher.Name}");
                }
            }

        }
    }
}