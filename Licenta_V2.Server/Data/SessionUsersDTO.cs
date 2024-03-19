namespace LatissimusDorsi.Server.Data
{
    public class SessionUsersDTO
    {
        public string profileImage { get; set; } = String.Empty;
        public string name { get; set; } = String.Empty;
        public string objective { get; set; } = String.Empty;
        public int age { get; set; }
        public string gender { get; set; } = String.Empty;

        public SessionUsersDTO(string img, string name, string obj, int age, string gender) { 
            this.name = name;
            this.objective = obj;
            this.profileImage = img;
            this.age = age;
            this.gender = gender;
        }
    }
}
