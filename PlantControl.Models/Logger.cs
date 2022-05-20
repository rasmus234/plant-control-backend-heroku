namespace PlantControl.Models
{
    public partial class Logger
    {
        public Logger()
        {
            Pairings = new HashSet<Pairing>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool? IsPaired { get; set; }

        public virtual ICollection<Pairing> Pairings { get; set; }
    }
}