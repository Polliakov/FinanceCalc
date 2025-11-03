using System.Runtime.CompilerServices;

namespace FinanceCalc.Domain.Models.Primitives
{
    public class ComplexPercent(decimal value, double period)
    {
        public decimal Value { get; protected set; } = value;
        public double Period { get; protected set; } = period;

        public ComplexPercent SetValue(decimal value)
        {
            return new(value, Period);
        }

        public ComplexPercent SetPeriod(double period)
        {
            return new(Value, period);
        }

        public virtual ComplexPercent WithPeriod(double period)
        {
            return new(Scale(Value, period / Period), period);
        }

        /// <summary>
        /// Multiplicative scaling of percent. Like * operator.
        /// </summary>
        public static decimal Scale(decimal percent, double scale)
        {
            return (decimal)Math.Pow(1.0 + (double)percent, scale) - 1;
        }

        protected decimal ScaleTo(decimal percent, double period)
        {
            return Scale(percent, period / Period);
        }

        private static readonly ComplexPercent _empty = new(0, 1);
        public static ComplexPercent Zero => _empty;

        public static ComplexPercent operator +(ComplexPercent first, ComplexPercent second)
        {
            return new ComplexPercent(first.Value + second.WithPeriod(first.Period).Value, first.Period);
        }

        public static ComplexPercent operator -(ComplexPercent first, ComplexPercent second)
        {
            return new ComplexPercent(first.Value - second.WithPeriod(first.Period).Value, first.Period);
        }

        public static implicit operator decimal(ComplexPercent percent)
        {
            if (percent is null)
                return default;
            return percent.Value;
        }

        public override string ToString()
        {
            return Value.ToString("P2");
        }
    }
}
