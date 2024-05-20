using Dronee_Chan_2.Discord_Bot;

namespace Dronee_Chan_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}