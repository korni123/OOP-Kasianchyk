using lab7v9;

namespace lab7v9.Tests;

public class InMemoryRepositoryTests
{
    private static Project CreateProject(int id, string name = "Test")
    {
        return new Project(id, name, new DateTime(2026, 1, 1), new DateTime(2026, 6, 1));
    }

    [Fact]
    public void Add_NewItem_ItemIsStored()
    {
        var repository = new InMemoryRepository<Project>();
        var project = CreateProject(1);

        repository.Add(project);

        Assert.Single(repository.GetAll());
        Assert.Same(project, repository.GetById(1));
    }

    [Fact]
    public void Add_Null_ThrowsArgumentNullException()
    {
        var repository = new InMemoryRepository<Project>();

        Assert.Throws<ArgumentNullException>(() => repository.Add(null!));
    }

    [Fact]
    public void Add_DuplicateId_ThrowsInvalidOperationException()
    {
        var repository = new InMemoryRepository<Project>();
        repository.Add(CreateProject(1));

        Assert.Throws<InvalidOperationException>(() => repository.Add(CreateProject(1, "Other")));
    }

    [Fact]
    public void GetById_ExistingId_ReturnsItem()
    {
        var repository = new InMemoryRepository<Project>();
        repository.Add(CreateProject(1, "First"));
        repository.Add(CreateProject(2, "Second"));

        var result = repository.GetById(2);

        Assert.NotNull(result);
        Assert.Equal("Second", result.Name);
    }

    [Fact]
    public void GetById_NonExistingId_ReturnsNull()
    {
        var repository = new InMemoryRepository<Project>();
        repository.Add(CreateProject(1));

        var result = repository.GetById(99);

        Assert.Null(result);
    }

    [Fact]
    public void GetAll_WithItems_ReturnsAllItems()
    {
        var repository = new InMemoryRepository<Project>();
        repository.Add(CreateProject(1));
        repository.Add(CreateProject(2));
        repository.Add(CreateProject(3));

        var result = repository.GetAll().ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { 1, 2, 3 }, result.Select(x => x.Id));
    }

    [Fact]
    public void GetAll_EmptyRepository_ReturnsEmptyCollection()
    {
        var repository = new InMemoryRepository<Project>();

        var result = repository.GetAll();

        Assert.Empty(result);
    }

    [Fact]
    public void Update_ExistingItem_ChangesData()
    {
        var repository = new InMemoryRepository<Project>();
        repository.Add(CreateProject(1, "Old"));

        repository.Update(CreateProject(1, "New"));

        Assert.Equal("New", repository.GetById(1)!.Name);
        Assert.Single(repository.GetAll());
    }

    [Fact]
    public void Update_NonExistingItem_ThrowsKeyNotFoundException()
    {
        var repository = new InMemoryRepository<Project>();
        repository.Add(CreateProject(1));

        Assert.Throws<KeyNotFoundException>(() => repository.Update(CreateProject(99)));
    }

    [Fact]
    public void Delete_ExistingItem_RemovesItem()
    {
        var repository = new InMemoryRepository<Project>();
        repository.Add(CreateProject(1));
        repository.Add(CreateProject(2));

        repository.Delete(1);

        Assert.Null(repository.GetById(1));
        Assert.Single(repository.GetAll());
    }

    [Fact]
    public void Delete_NonExistingItem_ThrowsKeyNotFoundException()
    {
        var repository = new InMemoryRepository<Project>();
        repository.Add(CreateProject(1));

        Assert.Throws<KeyNotFoundException>(() => repository.Delete(99));
        Assert.Single(repository.GetAll());
    }
}
