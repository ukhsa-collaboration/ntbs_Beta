using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum AnimalType
    {
        [Display(Name = "Wild animal")]
        WildAnimal,
        [Display(Name = "Farm animal")]
        FarmAnimal,
        [Display(Name = "Pet")]
        Pet
    }
}
