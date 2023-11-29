namespace Lab3.GeneticAlgorithm
{
    using Schedule = Dictionary<Tuple<int, int>, List<Lesson>>;
    public class GeneticAlgorithmSolver
    {
        Dictionary<string, Dictionary<string, int>> groupSubjects;
        Dictionary<string, int> teacherHours;
        Dictionary<string, List<string>> teacherSubjects;

        List<Individual> currentPopulation;

        int days;
        int maxLessonsPerDay;

        const int populationCount = 30;
        const int eliteCount = 5;
        const int crossoverCount = populationCount - eliteCount;
        const double mutationChance = .1;

        const int subDiffMult = 1;
        const int profDiffMult = 1;
        const int groupOverlayMult = 10;
        const int profOverlayMult = 10;

        Random rand;

        public GeneticAlgorithmSolver(int days, int maxLessonsPerDay, Dictionary<string, Dictionary<string, int>> groupSubjects,
            Dictionary<string, int> teacherHours, Dictionary<string, List<string>> teacherSubjects)
        {
            this.days = days;
            this.maxLessonsPerDay = maxLessonsPerDay;

            this.groupSubjects = groupSubjects;
            this.teacherHours = teacherHours;
            this.teacherSubjects = teacherSubjects;

            rand = new Random();

            currentPopulation = new List<Individual>();

            GenFirstPopulation();
        }


        public void NextPopulation()
        {
            for (int i = 0; i < currentPopulation.Count; i++)
            {
                currentPopulation[i].score = CalculateScore(currentPopulation[i].schedule);
            }
            currentPopulation.Sort();

            var nextPopulation = new List<Individual>();

            for (int i = 0; i < eliteCount; i++)
            {
                nextPopulation.Add(new Individual(currentPopulation[i]));
            }

            for (int i = 0; i < crossoverCount; i++)
            {
                var parents = ChooseParents();
                nextPopulation.Add(Crossover(parents.Item1, parents.Item2));
            }

            currentPopulation = nextPopulation;
        }

        private Tuple<int, int> ChooseParents()
        {
            int ind1 = 0;
            int ind2 = 0;

            for (int i = 0; i < populationCount; i++)
            {
                currentPopulation[i].score = populationCount - i;
            }

            double sum = populationCount * (populationCount - 1) / 2;

            double p1 = rand.NextDouble() * sum;
            double p2 = (p1 + sum / 2) % sum;

            double t = 0;

            for (int i = 0; i < populationCount; i++)
            {
                double newT = t + currentPopulation[i].score;
                if (t <= p1 && p1 <= newT)
                {
                    ind1 = i;
                }
                if (t <= p2 && p2 <= newT)
                {
                    ind2 = i;
                }
                t = newT;
            }
            return new Tuple<int, int>(ind1, ind2);
        }

        private Individual Crossover(int ind1, int ind2)
        {
            var p1 = currentPopulation[ind1].schedule;
            var p2 = currentPopulation[ind2].schedule;

            int n = days * maxLessonsPerDay;

            int pos1 = rand.Next(n);
            int pos2 = rand.Next(n);

            Schedule newSchedule = new Schedule();

            for (int i = 0; i < n; i++)
            {
                Tuple<int, int> pos = new Tuple<int, int>(p1.Keys.ElementAt(i).Item1, p1.Keys.ElementAt(i).Item2);
                if (i < Math.Min(pos1, pos2) || i > Math.Max(pos1, pos2))
                {
                    newSchedule.Add(
                        pos,
                        new List<Lesson>(p1.Values.ElementAt(i))
                    );
                }
                else
                {
                    newSchedule.Add(
                        pos,
                        new List<Lesson>(p2.Values.ElementAt(i))
                    );
                }
            }

            newSchedule = Mutate(newSchedule);

            var ans = new Individual();
            ans.schedule = newSchedule;
            ans.score = CalculateScore(newSchedule);

            return ans;
        }

        private Schedule Mutate(Schedule schedule)
        {
            foreach (var time in schedule)
            {
                if (rand.NextDouble() < mutationChance)
                {
                    if (time.Value.Count == 0)
                    {
                        schedule[time.Key].Add(ChooseGroup());
                    }
                    else
                    {
                        int ind = rand.Next(time.Value.Count);
                        var lesson = schedule[time.Key][ind];

                        double r = rand.NextDouble();
                        if (r < 0.25)
                        {
                            schedule[time.Key][ind] = ChooseTeacher(lesson);
                        }
                        else if (r < 0.5)
                        {
                            schedule[time.Key][ind] = ChooseSubject(lesson);
                        }
                        else if (r < 0.75)
                        {
                            schedule[time.Key][ind] = ChooseGroup();
                        }
                        else
                        {
                            schedule[time.Key].RemoveAt(ind);
                        }
                    }
                }
            }

            return schedule;
        }

        private int CalculateScore(Schedule schedule)
        {
            var cur_groupSubjects = new Dictionary<string, Dictionary<string, int>>();
            var cur_teacherHours = new Dictionary<string, int>();
            int overlaysProf = 0;
            int overlaysGroup = 0;

            foreach (var item in schedule)
            {
                var lessons = item.Value;
                var usedTeachers = new HashSet<string>();
                var usedGroups = new HashSet<string>();
                lessons.ForEach(lesson =>
                {
                    if (usedGroups.Contains(lesson.group))
                    {
                        overlaysGroup++;
                    }
                    else
                    {
                        cur_groupSubjects.TryAdd(lesson.group, new Dictionary<string, int>());
                        cur_groupSubjects[lesson.group].TryAdd(lesson.subject, 0);
                        cur_groupSubjects[lesson.group][lesson.subject]++;
                        usedGroups.Add(lesson.group);
                    }

                    if (usedTeachers.Contains(lesson.teacher))
                    {
                        overlaysProf++;
                    }
                    else
                    {
                        cur_teacherHours.TryAdd(lesson.teacher, 0);
                        cur_teacherHours[lesson.teacher]++;
                        usedTeachers.Add(lesson.teacher);
                    }
                });
            }

            int differenceSubjectTime = 0;
            int differenceTeacherTime = 0;

            foreach (var item in groupSubjects)
            {
                foreach (var subject in item.Value)
                {
                    cur_groupSubjects.TryAdd(item.Key, new Dictionary<string, int>());
                    cur_groupSubjects[item.Key].TryAdd(subject.Key, 0);

                    differenceSubjectTime += Math.Abs(cur_groupSubjects[item.Key][subject.Key] - subject.Value);
                }
            }

            foreach (var teacher in teacherHours)
            {
                cur_teacherHours.TryAdd(teacher.Key, 0);
                differenceTeacherTime += Math.Abs(cur_teacherHours[teacher.Key] - teacher.Value);
            }

            return profDiffMult * differenceTeacherTime
                    + subDiffMult * differenceSubjectTime
                    + profOverlayMult * overlaysProf
                    + groupOverlayMult * overlaysGroup;
        }


        // generating schedule and its parts
        private void GenFirstPopulation()
        {
            for (int i = 0; i < populationCount; i++)
            {
                var ind = new Individual();
                ind.schedule = GenSchedule();
                ind.score = CalculateScore(ind.schedule);
                currentPopulation.Add(ind);
            }
        }

        private Schedule GenSchedule()
        {
            Schedule schedule = new Schedule();

            for (int day = 0; day < days; day++)
            {
                for (int hour = 0; hour < maxLessonsPerDay; hour++)
                {
                    var time = new Tuple<int, int>(day, hour);

                    int n = groupSubjects.Count;
                    int chooseN = rand.Next(n + 1);

                    schedule[time] = new List<Lesson>();

                    for (int i = 0; i < n; i++)
                    {
                        if (rand.Next(n - i) < chooseN)
                        {
                            chooseN--;

                            Lesson les = new Lesson();
                            les.group = groupSubjects.Keys.ElementAt(i);

                            schedule[time].Add(ChooseSubject(les));
                        }

                    }
                }
            }
            return schedule;
        }

        private Lesson ChooseTeacher(Lesson lesson)
        {
            int ind = rand.Next(teacherSubjects[lesson.subject].Count);
            lesson.teacher = teacherSubjects[lesson.subject][ind];
            return lesson;
        }

        private Lesson ChooseSubject(Lesson lesson)
        {
            int ind = rand.Next(groupSubjects[lesson.group].Count);
            lesson.subject = groupSubjects[lesson.group].Keys.ElementAt(ind);
            return ChooseTeacher(lesson);
        }

        private Lesson ChooseGroup()
        {
            Lesson lesson = new Lesson();
            int ind = rand.Next(groupSubjects.Count);
            lesson.group = groupSubjects.Keys.ElementAt(ind);
            return ChooseSubject(lesson);
        }

        public void WritePopulation()
        {
            for (int i = 0; i < currentPopulation.Count; i++)
            {
                currentPopulation[i].score = CalculateScore(currentPopulation[i].schedule);
            }
            currentPopulation.Sort();

            foreach (var individual in currentPopulation)
            {
                WriteSchedule(individual);
            }
        }

        public void WriteSchedule(Individual individual)
        {
            Console.WriteLine("");
            Console.WriteLine("Score: {0}", individual.score);
            foreach (var lesson in individual.schedule)
            {
                Console.WriteLine("{0}.{1} : ", lesson.Key.Item1 + 1, lesson.Key.Item2 + 1);
                lesson.Value.ForEach(x => Console.WriteLine("{0} - {1} - {2}", x.group, x.subject, x.teacher));
            }
            Console.WriteLine("");

        }

        public void WriteSummary(Individual ind)
        {
            Console.WriteLine("----------------");
            Console.WriteLine("Score: {0}", ind.score);
            var cur_groupSubjects = new SortedDictionary<string, SortedDictionary<string, int>>();
            var cur_teacherHours = new SortedDictionary<string, int>();
            int overlaysProf = 0;
            int overlaysGroup = 0;
            foreach (var item in ind.schedule)
            {
                var lessons = item.Value;
                var usedTeachers = new HashSet<string>();
                var usedGroups = new HashSet<string>();
                lessons.ForEach(lesson =>
                {
                    // check for every group hours for their subjects
                    if (usedGroups.Contains(lesson.group))
                    {
                        overlaysGroup++;
                    }
                    else
                    {
                        cur_groupSubjects.TryAdd(lesson.group, new SortedDictionary<string, int>());
                        cur_groupSubjects[lesson.group].TryAdd(lesson.subject, 0);
                        cur_groupSubjects[lesson.group][lesson.subject]++;
                        usedGroups.Add(lesson.group);
                    }

                    // check for teachers hours and if they have two subjects at one time
                    if (usedTeachers.Contains(lesson.teacher))
                    {
                        overlaysProf++;
                    }
                    else
                    {
                        cur_teacherHours.TryAdd(lesson.teacher, 0);
                        cur_teacherHours[lesson.teacher]++;
                        usedTeachers.Add(lesson.teacher);
                    }
                });
            }

            foreach (var req in cur_groupSubjects)
            {
                Console.WriteLine("{0}:", req.Key);
                foreach (var x in req.Value)
                    Console.WriteLine("{0} : {1}", x.Key, x.Value);
                Console.WriteLine();
            }

            foreach (var req in cur_teacherHours)
            {
                Console.WriteLine("{0} - {1}", req.Key, req.Value);
            }
            Console.WriteLine("Overlays for teachers: {0}", overlaysProf);
            Console.WriteLine("Overlays for groups: {0}", overlaysGroup);
            Console.WriteLine();
        }


        public Individual ChooseBest()
        {
            for (int i = 0; i < currentPopulation.Count; i++)
            {
                currentPopulation[i].score = CalculateScore(currentPopulation[i].schedule);
            }
            currentPopulation.Sort();
            return currentPopulation[0];
        }
    }
}