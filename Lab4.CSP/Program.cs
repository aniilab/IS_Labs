// using System;
using Lab4.CSP;

class Program
{
    static void Main()
    {
        Methods.InitializeData();
        var solution = Methods.RunMRV();
        Methods.PrintSchedule(solution);

        var start_time = DateTime.Now;
        Methods.RunMRV();
        Console.WriteLine($"MRV: {(DateTime.Now - start_time).TotalSeconds}");

        start_time = DateTime.Now;
        Methods.RunLCV();
        Console.WriteLine($"LCV: {(DateTime.Now - start_time).TotalSeconds}");

        start_time = DateTime.Now;
        Methods.RunDegree();
        Console.WriteLine($"Degree: {(DateTime.Now - start_time).TotalSeconds}");

        start_time = DateTime.Now;
        Methods.RunForwardChecking();
        Console.WriteLine($"Forward checking: {(DateTime.Now - start_time).TotalSeconds}");
    }

}
