namespace AnimalQuests.Models
{
    public class Biome
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Animal> Animals { get; set; }
            = new List<Animal>();
    }
}
