using Lab3.GeneticAlgorithm;

// Initial information
int maxHoursPerDay = 8;
int days = 5;
int maxWeekLoad = days * maxHoursPerDay;
var groupSubjects = new Dictionary<string, Dictionary<string, int>>() { { "TTP41", new Dictionary<string, int>() { { "IS", 10 }, { "MPO", 12 }, { "IT", 6 } } },
                                                                        { "TTP42", new Dictionary<string, int>() { { "IS", 12 }, { "MPO", 8 }, { "IT", 6 } } } };

var teacherHours = new Dictionary<string, int>() { { "Shevchenko", 30 }, { "Petrenko", 40 } };

var teacherSubjects = new Dictionary<string, List<string>>() { { "IS", new List<string>(){ "Petrenko", "Shevchenko" } },
                                                               { "MPO", new List<string>(){ "Shevchenko" } },
                                                               { "IT", new List<string>(){ "Petrenko" } }, };

foreach (var item in groupSubjects)
{
    Console.WriteLine("Group {0}:", item.Key);
    foreach (var x in item.Value)
        Console.WriteLine("Subject {0} : {1} hours", x.Key, x.Value);
    Console.WriteLine();
}
Console.WriteLine();

foreach (var item in teacherHours)
{
    Console.WriteLine("Teacher {0} - {1} hours/week", item.Key, item.Value);
}
Console.WriteLine();


foreach (var item in teacherSubjects)
{
    Console.WriteLine("Teacher {0}:", item.Key);
    item.Value.ForEach(s => Console.WriteLine("\t{0}", s));
    Console.WriteLine();
}

var geneticAlgorithmSolver = new GeneticAlgorithmSolver(days, maxHoursPerDay, groupSubjects, teacherHours, teacherSubjects);
geneticAlgorithmSolver.WritePopulation();

int i = 0;
while (geneticAlgorithmSolver.ChooseBest().score != 0)
{
    i++;
    geneticAlgorithmSolver.NextPopulation();
    geneticAlgorithmSolver.WritePopulation();
}
int populationCount = i;


geneticAlgorithmSolver.WriteSummary(geneticAlgorithmSolver.ChooseBest());
Console.WriteLine("Population count = {0}", populationCount);