using OnlineStore.Models;
using X.PagedList;

namespace OnlineStore.Repositories.Interfaces
{
    public interface ICollaboratorRepository
    {
        Collaborator Login(string email, string password);

        void Register(Collaborator collaborator);
        void Update(Collaborator collaborator);
        void Delete(int id);
        Collaborator GetCollaborator(int id);
        IPagedList<Collaborator> GetAllCollaborators(int? page, int pageSize);
    }
}