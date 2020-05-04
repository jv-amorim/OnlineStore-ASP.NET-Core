using OnlineStore.Models;
using Newtonsoft.Json;

namespace OnlineStore.Libraries.Session
{
    public class CollaboratorSession
    {
        private const string Key = "CollaboratorLogin";
        private SessionManager sessionManager;

        public CollaboratorSession(SessionManager sessionManager) => this.sessionManager = sessionManager;

        public void Login(Collaborator collaborator) => sessionManager.SetValue(Key, JsonConvert.SerializeObject(collaborator));

        public Collaborator GetLoggedInCollaborator()
        {
            string collaborator = sessionManager.GetValue(Key);

            if (collaborator == null)
                return null;
            
            return JsonConvert.DeserializeObject<Collaborator>(collaborator);
        }

        public void Logout() => sessionManager.RemoveAllValues();
    }
}