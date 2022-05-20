namespace PlantControl.Models
{
    public partial class Certificate
    {
        public int Id { get; set; }
        public int PairingId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Pairing Pairing { get; set; } = null!;
    }
}