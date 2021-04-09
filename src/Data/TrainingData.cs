using Conesoft.Files;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Davepermen.Website.Fitness.Data
{
    public class TrainingData
    {
        private readonly Dictionary<DateTime, int> entries = new();

        public int Sum { get; private set; } = 0;
        public int Goal { get; private set; } = 0;
        public int[] AmountPerDay { get; private set; } = Array.Empty<int>();
        public int[] AccumulatedEveryDay { get; private set; } = Array.Empty<int>();
        public string User { get; }
        public string Training { get; }
        public int Year { get; }

        public TrainingData(string user, string training, int year)
        {
            this.User = user;
            this.Training = training;
            this.Year = year;

            var _ = LoadDataAsync(user, training, year);
        }

        public async Task AddAsync(int amount)
        {
            var file = Conesoft.Hosting.Host.GlobalStorage / "Fitness" / $"{Year}" / User / Training / Filename.From($"{DateTime.UtcNow:yyyy-MM-dd}", "txt");

            var current = file.Exists ? int.Parse(await file.ReadText()) : 0;

            await file.WriteText($"{current + amount}");

            await LoadDataAsync(User, Training, Year);
        }

        private async Task LoadDataAsync(string user, string training, int year)
        {
            var path = Conesoft.Hosting.Host.GlobalStorage / "Fitness" / $"{Year}" / User / Training;
            if (path.Exists)
            {
                await LoadEntriesAsync(path);

                AmountPerDay = new int[DaysInYear(year)];
                AccumulatedEveryDay = new int[DaysInYear(year)];

                foreach (var date in entries.Keys)
                {
                    AmountPerDay[date.DayOfYear - 1] = entries[date];
                }

                var accumulated = 0;
                for (var i = 0; i < AmountPerDay.Length; i++)
                {
                    accumulated += AmountPerDay[i];
                    AccumulatedEveryDay[i] = accumulated;
                }
            }
        }

        private async Task LoadEntriesAsync(Directory path)
        {
            foreach (var file in path.Filtered($"{Year}-*.txt", allDirectories: false))
            {
                var date = DateTime.Parse(file.NameWithoutExtension);
                var amount = int.Parse(await file.ReadText());

                entries[date] = entries.TryGetValue(date, out int value) ? value + amount : amount;

                Sum += amount;
            }

            var goal = path / Filename.From("goal", "txt");

            if (goal.Exists)
            {
                Goal = int.Parse(await goal.ReadText());
            }
        }

        private static int DaysInYear(int year)
        {
            var firstDay = new DateTime(year, 1, 1);
            var firstDayOfNextYear = new DateTime(year + 1, 1, 1);
            return (firstDayOfNextYear - firstDay).Days;
        }
    }
}
