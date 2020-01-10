namespace EFAuditer
{
    public interface IHasRootEntity
    {
        string RootEntityType { get; }
        string RootId { get; }
    }
}
