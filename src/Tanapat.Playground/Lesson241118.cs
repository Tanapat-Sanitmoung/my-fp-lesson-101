namespace Tanapat.Playground.Lesson241118;

public static class Practices
{
    public static void Practice1()
    {
        // Map  : (Option<T>, (T -> R))         -> Option<R>)
        // Bind : (Option<T>, (T -> Option<R>)) -> Option<R>)
        // Bind : (Option<T>, (T -> R))         -> R)

        // ** I can say that Map is lift "R" into "Option<R>"
        // ** but Bind will not lift the result
    }

    public readonly record struct Age
    {
        public int Value { get; }
        private Age(int value)
            => Value = value;

        public static Option<Age> Of(int value)
            => IsValid(value) ? new Age(value) : None;

        private static bool IsValid(int value) => value.Between(0, 120);

        public bool IsAdult => Value > 18;
    }

    public readonly record struct Risk
    {
        public RiskRange Value { get; }

        private Risk(RiskRange value)
            => Value = value;

        public static Option<Risk> Of(Age age)
            => age.IsAdult ? new Risk(RiskRange.High) : new Risk(RiskRange.Low);
    }

    public enum RiskRange { Low, High }
}

public static class Ex
{
    public static bool Between(this int value, int min, int max)
        => value >= min && value <= max;
}