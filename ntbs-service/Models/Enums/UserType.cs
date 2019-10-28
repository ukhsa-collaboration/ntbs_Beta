namespace ntbs_service.Models.Enums
{
    public enum UserType
    {
        /** Members of the national team with access to all regions */
        NationalTeam,
        /** Members of PHECs, regions associated with multiple TB services */
        PheUser,
        /** Clinical staff, members of particular TB services */
        NhsUser
    }
}