using lab7v9;

IRepository<Project> repository = new InMemoryRepository<Project>();

repository.Add(new Project(1, "Website", new DateTime(2026, 9, 1), new DateTime(2026, 11, 30)));
repository.Add(new Project(2, "Mobile app", new DateTime(2026, 10, 1), new DateTime(2027, 2, 15)));
repository.Add(new Project(3, "Database", new DateTime(2026, 8, 15), new DateTime(2026, 10, 20)));

Console.WriteLine("All projects:");
foreach (Project project in repository.GetAll())
{
    Console.WriteLine(project);
}

Console.WriteLine();
Console.WriteLine("Get by id 2:");
Console.WriteLine(repository.GetById(2));

Console.WriteLine();
Console.WriteLine("Get by id 10:");
Console.WriteLine(repository.GetById(10) == null ? "Not found" : "Found");

Console.WriteLine();
Console.WriteLine("Update project 2:");
repository.Update(new Project(2, "Mobile app v2", new DateTime(2026, 10, 1), new DateTime(2027, 3, 1)));
Console.WriteLine(repository.GetById(2));

Console.WriteLine();
Console.WriteLine("Delete project 1:");
repository.Delete(1);
foreach (Project project in repository.GetAll())
{
    Console.WriteLine(project);
}

Console.WriteLine();
Console.WriteLine("Delete project 10:");
try
{
    repository.Delete(10);
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
