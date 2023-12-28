partial class Program
{
    private static void Main(string[] args)
    {
        welcome7947();
        welcome6663();
        Console.ReadKey();
    }

    private static void welcome7947()
    {
        Console.WriteLine("Enter your name: ");
        string name = Console.ReadLine();
        Console.WriteLine("{0},welcome to my first console application", name);
    }
    static partial void welcome6663();
}