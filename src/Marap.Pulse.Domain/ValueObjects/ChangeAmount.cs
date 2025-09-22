namespace Marap.Pulse.Domain.ValueObjects;

public record ChangeAmount
{
    public decimal Value { get; }

    public ChangeAmount(decimal value)
    {
        if (value == 0) throw new ArgumentException("Change Amount cannot be zero.");
        Value = value;
    }

    public bool IsIncrease => Value > 0;
    public bool IsDecrease => Value < 0;
}