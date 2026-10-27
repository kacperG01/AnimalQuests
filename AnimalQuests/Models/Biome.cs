namespace testing.Models
{
    public class Biome
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Kolekcja zwierząt należących do biomu
        public ICollection<Animal> Animals { get; set; }
            = new List<Animal>();
    }
}
