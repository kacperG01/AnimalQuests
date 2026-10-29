using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AnimalQuests.Models
{
    public class AnimalMission
    {
        public int AnimalId { get; set; }

        [ValidateNever]
        public Animal Animal { get; set; } = null!;

        public int MissionId { get; set; }

        [ValidateNever]
        public Mission Mission { get; set; } = null!;
    }
}