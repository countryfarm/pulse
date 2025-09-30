using Vogen;

namespace Marap.Pulse.Domain.ValueObjects;

[ValueObject(typeof(decimal))]
public partial struct ChangeAmount
{
    private static Validation Validate(decimal value) =>
        value == 0
            ? Validation.Invalid("Change Amount cannot be zero.")
            : Validation.Ok;

    public bool IsIncrease => Value > 0;
    public bool IsDecrease => Value < 0;
}
