using Dronee_Chan_2.Discord_Bot;

namespace Dronee_Chan_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var bot = new Bot();
                bot.RunAsync().GetAwaiter().GetResult();
            }catch(Exception e)
            {
                Console.WriteLine("Message: " + e.Message + "\nStackTrace: " + e.StackTrace);
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}