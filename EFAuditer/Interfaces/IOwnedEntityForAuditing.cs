namespace EFAuditer
{
    public interface IOwnedEntityForAuditing
    {
        string RootEntityType { get; }
    }
}
