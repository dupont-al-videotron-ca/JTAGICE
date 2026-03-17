


static class Program
{
    static void Main(string[] args)
    {

        ApplicationFW.IApplication app = new CandleConsoleApp.CandleApp();
        app.Run(args);
    }
}