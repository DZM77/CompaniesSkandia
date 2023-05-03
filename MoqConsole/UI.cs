namespace MoqConsole
{
    public class UI : IUI
    {

        public void Print(string message)
        {
            Console.WriteLine(message);
        }

        public string GetInput()
        {
            return Console.ReadLine()!;
        }
    }

    public class MockUI : IUI
    {
        public string SetInput { private get; set; } = "1";
        public string GetInput()
        {
            return SetInput;
        }

        public void Print(string message)
        {
           
        }
    }
}