namespace APBD1;

public interface IUserRepository
{
    void Add(User user);
    List<User>GetAll();
    User GetById(string id);
}