namespace testing.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Archetype { get; set; } = string.Empty;
        public string Weapon { get; set; } = string.Empty;
        public int BiomeId { get; set; }

        // Właściwość nawigacyjna do biomu
        public Biome Biome { get; set; } = null!;

        public ICollection<AnimalMission> AnimalMissions { get; set; }
    = new List<AnimalMission>();
    }
}
