using Marap.Pulse.Domain.Common;

namespace Marap.Pulse.Domain.ValueObjects;

public sealed class PurchaseOrderStatus : ValueObject
{
  public static readonly PurchaseOrderStatus Draft = new("Draft");
  public static readonly PurchaseOrderStatus Submitted = new("Submitted");
  public static readonly PurchaseOrderStatus Approved = new("Approved");
  public static readonly PurchaseOrderStatus Received = new("Received");
  public static readonly PurchaseOrderStatus Cancelled = new("Cancelled");

  private static readonly Dictionary<PurchaseOrderStatus, PurchaseOrderStatus[]> AllowedTransitions =
    new()
    {
      { Draft, new[] { Submitted, Cancelled } },
      { Submitted, new[] { Approved, Cancelled } },
      { Approved, new[] { Received } }
    };

  public string Value { get; }

  private PurchaseOrderStatus(string value) => Value = value;

  protected override IEnumerable<object?> GetEqualityComponents()
  {
    yield return Value;
  }

  public override string ToString() => Value;

  public static PurchaseOrderStatus From(string value) =>
    value switch
    {
      "Draft" => Draft,
      "Submitted" => Submitted,
      "Approved" => Approved,
      "Received" => Received,
      "Cancelled" => Cancelled,
      _ => throw new ArgumentException($"Invalid status: {value}")
    };

  public bool CanTransitionTo(PurchaseOrderStatus next)
  {
    if (this == next) return true;
    if (this == Cancelled || this == Received) return false;

    return AllowedTransitions.TryGetValue(this, out var allowed) && allowed.Contains(next);
  }
}