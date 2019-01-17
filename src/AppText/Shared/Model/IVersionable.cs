namespace AppText.Shared.Model
{
    public interface IVersionable : IWithIdentifier
    {
        int Version { get; set; }
    }
}
