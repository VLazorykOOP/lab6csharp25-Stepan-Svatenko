using Lab6Ships;
using Lab6Persona;
using Lab6Money;

namespace Lab6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = 0;
            do
            {
                Console.WriteLine("Enter number of task: ");
                n = Convert.ToInt32(Console.ReadLine());
                switch (n)
                {
                    case 1:
                        Moneytest.moneytest();
                        break;
                    case 2:
                        SHIPS.shiptest();
                        break;
                    case 3:
                        PersonaTest.persontest();
                        break;
                    case 4:
                        break;
                }
            } while (n != 0);



        }
    }
}