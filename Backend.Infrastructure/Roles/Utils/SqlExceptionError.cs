namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Roles.Utils;

internal class SqlExceptionError
{
    internal static readonly SqlExceptionError UniqueConstraintViolation = new(2627);

    public int Number { get; }

    private SqlExceptionError(int number)
    {
        Number = number;
    }
}