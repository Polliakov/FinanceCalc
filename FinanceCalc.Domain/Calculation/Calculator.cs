using FinanceCalc.Domain.Calculation.Checkers.Models;
using FinanceCalc.Domain.Calculation.Core;
using FinanceCalc.Domain.Calculation.Profiles;
using FinanceCalc.Domain.Calculation.View;
using FinanceCalc.Domain.Models.Primitives;

namespace FinanceCalc.Domain.Calculation
{
    public class Calculator
    {
        public static List<Report> Main(CalculatorProfile profile)
        {
            var needInMonth = 50_000m;
            var needInYear = needInMonth * 12;

            var inflationPeerYear = new ComplexPercent(0.075m, 12);
            var inflationPeerMonth = inflationPeerYear.WithPeriod(1);
            var inflationPeerDay = inflationPeerYear.WithPeriod(12.0 / 365);

            var mainYearRate = new Rate(0.17m, CashKind.Nominal, inflationPeerYear, 12);
            var mainMonthRate = mainYearRate.WithPeriod(1);

            var periodDays = (profile.EndDate - profile.StartDate).TotalDays;

            var inflation = new SimpleMultiplicative(inflationPeerDay, new IntervalChecker(1, true, profile.StartDate))
            {
                Name = "Inflation",
            };
            var mainRateIncrease = new SimpleMultiplicative(mainMonthRate, new MonthDayChecker(1, profile.StartDate))
            {
                Name = "Main Rate Increase",
            };

            var worthAdditives = new List<TimeBasedFunction>(profile.WorthAdditives.Count);
            foreach (var (name, item) in profile.WorthAdditives)
            {
                var adding = new SimpleAdditive(_ => item.InMonth, new MonthDayChecker(1, profile.StartDate))
                {
                    Name = name
                };
                worthAdditives.Add(adding);
            }

            var worth = 100_000m;
            var worthAdding = 0m;
            var oldWorth = worth;
            var oldWorthAdding = worthAdding;

            var needInMonthWI = needInMonth;
            var targetBalanceWI = needInYear / mainYearRate;

            var incomeWorthMonth = 0m;
            var incomeWorthAddingMonth = 0m;
            var currentTime = profile.StartDate;
            var reports = new List<Report>();
            for (var daysLeft = 0; daysLeft < periodDays; daysLeft++)
            {
                needInMonthWI = inflation.Activate(currentTime, needInMonthWI);
                worth = mainRateIncrease.Activate(currentTime, worth);
                worth += mainRateIncrease.Activate(currentTime, worthAdding) - worthAdding;

                worthAdding += worthAdditives.Aggregate(0m, (r, f) => f.Activate(currentTime, r));
                var addingPercent = worthAdding / (worthAdding + worth);

                targetBalanceWI = needInMonthWI * 12 / mainYearRate;

                if (currentTime.ToString("dd") == "01")
                {
                    incomeWorthMonth = worth - oldWorth;
                    incomeWorthAddingMonth = worthAdding - oldWorthAdding;

                    oldWorthAdding = worthAdding;
                    oldWorth = worth;

                    var worthSum = worth + worthAdding;

                    var incomeWorthSum = incomeWorthMonth + incomeWorthAddingMonth;
                    var addingIncomePercent = incomeWorthAddingMonth / incomeWorthSum;

                    var actualInflation = needInMonthWI / needInMonth - 1;
                    reports.Add(new Report(
                        currentTime,
                        actualInflation,
                        worthSum,
                        incomeWorthSum,
                        addingPercent,
                        addingIncomePercent));
                }

                currentTime = currentTime.AddDays(1);
            }

            return reports;
        }
    }

    public class SimpleAdditive(Func<decimal, decimal> addGetter, ICanActivateChecker checker)
       : TimeBasedFunction(checker)
    {
        public Func<decimal, decimal> AddGetter { get; private set; } = addGetter;

        protected override decimal ActivationAction(decimal sum)
        {
            return sum + AddGetter(sum);
        }
    }

    public class SimpleMultiplicative(decimal rate, ICanActivateChecker checker)
        : TimeBasedFunction(checker)
    {
        public decimal Rate { get; private set; } = rate;

        protected override decimal ActivationAction(decimal sum)
        {
            return sum + sum * Rate;
        }
    }
}
