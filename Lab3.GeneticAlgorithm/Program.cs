using Lab3.GeneticAlgorithm;

// Initial information
int maxLessonsPerDay = 4;
int days = 5;
int maxWeekLoad = days * maxLessonsPerDay;
var groupSubjects = new Dictionary<string, Dictionary<string, int>>() { { "TTP41", new Dictionary<string, int>() { { "IS", 3 }, { "MPO", 2 }, { "IT", 1 } } },
                                                                        { "TTP42", new Dictionary<string, int>() { { "IS", 2 }, { "MPO", 2 }, { "IT", 1 } } } };

var teacherLessons = new Dictionary<string, int>() { { "Shevchenko", 10 }, { "Petrenko", 15 } };

var teacherSubjects = new Dictionary<string, List<string>>() { { "IS", new List<string>(){ "Petrenko", "Shevchenko" } },
                                                               { "MPO", new List<string>(){ "Shevchenko" } },
                                                               { "IT", new List<string>(){ "Petrenko" } }, };

foreach (var item in groupSubjects)
{
    Console.WriteLine("Group {0}:", item.Key);
    foreach (var x in item.Value)
        Console.WriteLine("Subject {0} : {1} lessons", x.Key, x.Value);
    Console.WriteLine();
}
Console.WriteLine();

foreach (var item in teacherLessons)
{
    Console.WriteLine("Teacher {0} - {1} lessons/week", item.Key, item.Value);
}
Console.WriteLine();


foreach (var item in teacherSubjects)
{
    Console.WriteLine("Subject {0}:", item.Key);
    item.Value.ForEach(s => Console.WriteLine("\t{0}", s));
    Console.WriteLine();
}

var geneticAlgorithmSolver = new GeneticAlgorithmSolver(days, maxLessonsPerDay, groupSubjects, teacherLessons, teacherSubjects);
geneticAlgorithmSolver.WritePopulation();

int i = 0;
while (geneticAlgorithmSolver.ChooseBest().score != 0)
{
    i++;
    geneticAlgorithmSolver.NextPopulation();
}
int populationCount = i;

geneticAlgorithmSolver.WritePopulation();

Console.WriteLine("Population count = {0}", populationCount);