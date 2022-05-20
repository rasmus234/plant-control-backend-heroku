namespace PlantControl.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public int PairingId { get; set; }
        public DateTime Time { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Moisture { get; set; }

        public virtual Pairing Pairing { get; set; } = null!;
    }
}