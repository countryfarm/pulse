using Vogen;

namespace Marap.Pulse.Domain.ValueObjects;

[ValueObject(typeof(decimal))]
public partial struct Quantity
{
  private static Validation Validate(decimal value) =>
    value < 0
      ? Validation.Invalid("Quantity cannot be negative.")
      : Validation.Ok;

  public override string ToString() => Value.ToString("0.##");

  // Domain arithmetic operators
  public static Quantity operator +(Quantity a, Quantity b) =>
    From(a.Value + b.Value);

  public static Quantity operator -(Quantity a, Quantity b)
  {
    if (a.Value < b.Value)
      throw new InvalidOperationException("Resulting quantity cannot be negative.");
    return From(a.Value - b.Value);
  }

  // Comparison operators (only if your Vogen version does NOT generate them)
  public static bool operator <(Quantity a, Quantity b) => a.Value < b.Value;
  public static bool operator >(Quantity a, Quantity b) => a.Value > b.Value;
  public static bool operator <=(Quantity a, Quantity b) => a.Value <= b.Value;
  public static bool operator >=(Quantity a, Quantity b) => a.Value >= b.Value;
}
