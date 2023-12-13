using System.Reflection;

namespace SourceGenerators;

internal class Program
{
    static void Main(string[] args)
    {
        var classTypes = Assembly.GetEntryAssembly()
            .GetTypes()
            .Where(c => c.IsClass && c.IsPublic)
            .Select(c => c.FullName);

        foreach (var classType in classTypes)
        {
            Console.WriteLine(classType);
        }
    }
}
