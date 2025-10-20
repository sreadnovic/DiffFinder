public class Repo : IRepo
{
    private readonly Storage _storage;

    public Repo(Storage storage)
    {
        _storage = storage;
    }

    public string Get(string id)
    {
        return _storage.InMemoryStorage.ContainsKey(id) ? _storage.InMemoryStorage[id] : null;
    }

    public void Add(string id, string content)
    {
        _storage.InMemoryStorage[id] = content;
    }
}