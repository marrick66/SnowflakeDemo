namespace Demo.API.Models
{
    /// <summary>
    /// Simple fake record of user data.
    /// </summary>
    public readonly record struct UserRecord(
        Guid Id,
        string FirstName,
        string LastName);
}
