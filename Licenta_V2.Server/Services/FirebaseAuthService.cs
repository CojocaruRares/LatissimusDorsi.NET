using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Licenta_V2.Server.Services
{
    public class FirebaseAuthService
    {

        private readonly FirebaseAuth _auth;

        public FirebaseAuthService()
        {
            FirebaseApp app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("./FirebaseConfig/licenta-10cc3-firebase-adminsdk-qctr7-e80c15d661.json"),
            });

            _auth = FirebaseAuth.GetAuth(app);
        }

        public async Task<string> CreateUserWithClaim(string email, string password, string id, string role)
        {
            var user = await _auth.CreateUserAsync(new UserRecordArgs()
            {
                Uid = id,
                Email = email,
                Password = password,
            });

            await _auth.SetCustomUserClaimsAsync(user.Uid, new Dictionary<string, object>
            {
                { "role", role }
            });

            return user.Uid;
        }

      
        public async Task<string> GetRoleForUser(string userId)
        {
            UserRecord user = await FirebaseAuth.DefaultInstance.GetUserAsync(userId);

            if (user.CustomClaims != null && user.CustomClaims.ContainsKey("role"))
            {
                return user.CustomClaims["role"].ToString();
            }
            else
            {
                return "No role assigned";
            }
        }
       
    }
}
