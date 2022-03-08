namespace ntbs_service;

/// <summary>
/// This class exists so that the WebHostFactory in the integration tests can use a type inside the entry assembly.
/// Previously this would have been Startup, but since .NET 6 we have no Startup class to use.
/// </summary>
public class EntryPoint
{
}
