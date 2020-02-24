using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum ExposureSetting
    {
        [Display(Name = "Detention - Immigration")]
        DetentionImmigration,
        [Display(Name = "Detention - Prison")]
        DetentionPrison,
        [Display(Name = "Education - Nursery")]
        EducationNursery,
        [Display(Name = "Education - Primary")]
        EducationPrimary,
        [Display(Name = "Education - Secondary")]
        EducationSecondary,
        [Display(Name = "Education - Tertiary")]
        EducationTertiary,
        [Display(Name = "Elderly Residential")]
        ElderlyResidential,
        [Display(Name = "Healthcare - Hospital")]
        HealthcareHospital,
        [Display(Name = "Healthcare - Other")]
        HealthcareOther,
        [Display(Name = "Homeless - Night shelter")]
        HomelessNightShelter,
        [Display(Name = "Homeless - Residential hostel")]
        HomelessResidentialHostel,
        [Display(Name = "Homeless - Other")]
        HomelessOther,
        [Display(Name = "Household")]
        Household,
        [Display(Name = "Not known")]
        NotKnown,
        [Display(Name = "Nursing Home")]
        NursingHome,
        [Display(Name = "Other")]
        Other,
        [Display(Name = "Pub")]
        Pub,
        [Display(Name = "Travel - Air")]
        TravelAir,
        [Display(Name = "Travel - Bus")]
        TravelBus,
        [Display(Name = "Travel - Ship")]
        TravelShip,
        [Display(Name = "Travel - Train")]
        TravelTrain,
        [Display(Name = "Workplace")]
        Workplace
    }
}
