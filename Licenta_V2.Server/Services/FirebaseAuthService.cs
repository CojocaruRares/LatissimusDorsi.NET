using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace LatissimusDorsi.Server.Services
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

    
        public async Task<string> GetRoleForUser(string token)
        {
            var decoded = await _auth.VerifyIdTokenAsync(token);
            object role;
            if (decoded.Claims.TryGetValue("role", out role))
            {
                return (string)role;
            }
            else
            {
                return "No role assigned";
            }
        }
       
    }
}
