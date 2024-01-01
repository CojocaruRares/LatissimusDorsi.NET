
namespace LatissimusDorsi.Server.Data
{
    public class TrainerDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;      
        public string description { get; set; } = string.Empty;
        public int age { get; set; }     
        public string motto { get; set; } = string.Empty;     
        public string gym { get; set; } = string.Empty;       
        public string specialization { get; set; } = string.Empty;
        public IFormFile? profileImage { get; set; }
    }
}
