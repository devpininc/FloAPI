using TestFloApi.ConsoleApp;

var runner = new TestRunner();
await runner.RunAsync();

Console.WriteLine("Done. Press Enter to exit...");
Console.ReadLine();
