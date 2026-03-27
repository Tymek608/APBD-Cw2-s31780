namespace APBD1;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new List<User>();

    public void Add(User user)
    {
        _users.Add(user);
    }

    public List<User> GetAll()
    {
        return  _users;
    }
}