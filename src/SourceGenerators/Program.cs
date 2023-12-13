using System.Reflection;

namespace SourceGenerators;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Via Reflection\n");
        WriteViaReflection();

        Console.WriteLine("Via Generator\n");
        WriteViaGenerator();
    }

    private static void WriteViaReflection() {
        var classTypes = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(c => c!= null && c.IsClass && c.IsPublic)
                .Select(c => c.FullName);

        foreach (var classType in classTypes)
        {
            Console.WriteLine(classType);
        }
    }

    private static void WriteViaGenerator() {
        foreach (var className in Generator.ClassNames.Names)
        {
            Console.Write(className);
        }

    }
}
