namespace LatissimusDorsi.Server.Models
{
    public class Exercise
    {
        public string name { get; set; } = string.Empty;
        public int? sets { get; set; }
        public int? reps { get; set; }
        public int? rpe { get; set; }
        public string? description { get; set; } = string.Empty;
    }
}
