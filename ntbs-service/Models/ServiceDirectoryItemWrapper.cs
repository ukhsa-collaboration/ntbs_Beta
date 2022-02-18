using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models;

public class ServiceDirectoryItemWrapper
{
    public User User { get; }
    public PHEC Region { get; }
    public TBService TBService { get; }
    public Hospital Hospital { get; }

    public ServiceDirectoryItemWrapper(User user = null, PHEC region = null, TBService tbService = null, Hospital hospital = null)
    {
        this.User = user;
        this.Region = region;
        this.TBService = tbService;
        this.Hospital = hospital;
    }

    public bool IsUser()
    {
        return this.User is not null;
    }

    public bool IsRegion()
    {
        return this.Region is not null;
    }

    public bool IsTBService()
    {
        return this.TBService is not null;
    }

    public bool IsHospital()
    {
        return this.Hospital is not null;
    }
}
