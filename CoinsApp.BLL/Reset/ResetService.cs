namespace CoinsApp.BLL.Reset;

public sealed class ResetService : IResetService
{
    private readonly IDatabaseResetter _databaseResetter;

    public ResetService(IDatabaseResetter databaseResetter)
    {
        _databaseResetter = databaseResetter ?? throw new ArgumentNullException(nameof(databaseResetter));
    }

    public ResetResult Reset()
    {
        try
        {
            _databaseResetter.Reset();

            return ResetResult.Succeeded("Demo data reset completed successfully.");
        }
        catch (Exception ex)
        {
            return ResetResult.Failed($"Demo data reset failed: {ex.Message}");
        }
    }
}