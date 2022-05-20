namespace PlantControl.Models
{
    public partial class Plant
    {
        public Plant()
        {
            Pairings = new HashSet<Pairing>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public byte[]? Image { get; set; }

        public virtual ICollection<Pairing> Pairings { get; set; }
    }
}