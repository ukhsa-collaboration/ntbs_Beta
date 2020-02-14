namespace EFAuditer
{
    public interface IHasRootEntityForAuditing
    {
        string RootEntityType { get; }
        string RootId { get; }
    }
}
