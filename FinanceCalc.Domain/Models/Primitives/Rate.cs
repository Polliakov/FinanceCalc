namespace FinanceCalc.Domain.Models.Primitives
{
    public class Rate : ComplexPercent
    {
        public decimal RealValue { get; protected set; }
        public CashKind ValueKind { get; protected set; }
        public ComplexPercent Inflation { get; protected set; }

        public Rate(
            decimal value,
            CashKind kind,
            ComplexPercent? inflation = null,
            double? period = 1)
            : base(value, period!.Value)
        {
            Inflation = (inflation ?? Zero).WithPeriod(period.Value);
            RealValue = value;
            ValueKind = kind;
            Value = kind switch
            {
                CashKind.Nominal => Value,
                CashKind.Real => (1 + Value) / (1 + Inflation) - 1,
                _ => throw new NotSupportedException(),
            };
        }

        public override Rate WithPeriod(double period)
        {
            return new Rate(ScaleTo(RealValue, period), ValueKind, Inflation, period);
        }
    }
}
