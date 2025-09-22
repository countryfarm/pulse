namespace Marap.Pulse.Domain.ValueObjects;

public record Quantity
{
    public decimal Value { get; }

    public Quantity(decimal value)
    {
        if (value < 0) throw new ArgumentException("Quantity cannot be negative.");
        Value = value;
    }
}