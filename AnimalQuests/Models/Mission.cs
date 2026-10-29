using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AnimalQuests.Models
{
    public class Mission
    {
        public int Id { get; set; }

        public int BiomeId { get; set; }

        [ValidateNever]
        public Biome AssignedBiome { get; set; } = null!;

        public string Objective { get; set; } = string.Empty;

        public ICollection<AnimalMission> AnimalMissions { get; set; }
            = new List<AnimalMission>();

        public int Status { get; set; }
    }
}