using Companies.TestDemo;

namespace MoqConsole
{
    public class Main
    {
        private IUI ui;
        private Calculator calculator;

        public Main(IUI ui)
        {
            this.ui = ui;
            calculator = new Calculator(); 
        }

        public int Run()
        {
            var res = AddNumbers();
            return res;
        }

        private int AddNumbers()
        {
            var num1 = Util.AskForInt("Enter first nr", ui);
            var num2 = Util.AskForInt("Enter second nr", ui);

            return calculator.Add(num1, num2);
        }
    }
}