using learn_realearn.Services;

namespace learn_realearn
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameManager = new GameManager();
            gameManager.Run();
        }
    }
}