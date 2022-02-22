using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models;

public class ServiceDirectoryItemWrapper
{
    public User User { get; }
    public PHEC Region { get; }
    public TBService TBService { get; }
    public Hospital Hospital { get; }

    public ServiceDirectoryItemWrapper(User user)
    {
        this.User = user;
    }

    public ServiceDirectoryItemWrapper(PHEC region)
    {
        this.Region = region;
    }
    public ServiceDirectoryItemWrapper(TBService tbService)
    {
        this.TBService = tbService;
    }
    public ServiceDirectoryItemWrapper(Hospital hospital)
    {
        this.Hospital = hospital;
    }
    
    public bool IsUser => User is not null;

    public bool IsRegion => Region is not null;

    public bool IsTBService => TBService is not null;

    public bool IsHospital => Hospital is not null;
}
