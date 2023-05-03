namespace MoqConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IUI ui = new UI();
            var main = new Main(ui);
            var res = main.Run();
        }
    }
}