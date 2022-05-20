namespace PlantControl.Models
{
    public partial class Pairing
    {
        public Pairing()
        {
            Certificates = new HashSet<Certificate>();
            Logs = new HashSet<Log>();
        }

        public int Id { get; set; }
        public int LoggerId { get; set; }
        public int PlantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; } = null!;

        public virtual Logger Logger { get; set; } = null!;
        public virtual Plant Plant { get; set; } = null!;
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}