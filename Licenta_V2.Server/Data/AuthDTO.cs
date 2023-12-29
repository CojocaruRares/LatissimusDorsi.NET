

namespace Licenta_V2.Server.Data
{
    public class AuthDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public int age { get; set; }
        public int height { get; set; }
        public int weight { get; set; }
        public string Objective { get; set; } = string.Empty;
        public byte Gender { get; set; }
        public int BodyFatPercentage { get; set; }

        public IFormFile? profileImage { get; set; }
    }
}