namespace Marap.Pulse.Domain.ValueObjects;

public record Quantity
{
  public decimal Value { get; }

  public Quantity(decimal value)
  {
    if (value < 0)
      throw new ArgumentException("Quantity cannot be negative.");
    Value = value;
  }

  public static Quantity operator +(Quantity a, Quantity b)
  {
    return new Quantity(a.Value + b.Value);
  }

  public static Quantity operator -(Quantity a, Quantity b)
  {
    if (a.Value < b.Value)
      throw new InvalidOperationException("Resulting quantity cannot be negative.");
    return new Quantity(a.Value - b.Value);
  }

  public override string ToString() => Value.ToString("0.##");
}