using System;
using System.Threading;
using Timer = System.Threading.Timer;

namespace MultiThreadingTest
{
    internal class Program
    {
        private static readonly object Lock1 = new object();
        private static Timer _timer1, _timer2;
        private static bool isDisposeTimer1 = false, isDisposeTimer2 = false;
        private static int _c = 0;

        static void Main(string[] args)
        {
            _timer1 = new Timer(Timer1Function, null, 0, 5);
            _timer2 = new Timer(Timer2Function, null, 0, 10);
            Thread.Sleep(1000000);
            Console.ReadLine();
        }

        private static void Timer2Function(object state)
        {
            if (isDisposeTimer2) return;
            lock (Lock1)
            {
                if (IsPrime(_c))
                {
                    Console.WriteLine($"{_c} is prime number");
                }
            }

        }
        static void Timer1Function(object state)
        {
            if (isDisposeTimer1) return;
            while (true)
            {
                lock (Lock1)
                {
                    Console.Write("c = ");
                    _c = int.Parse(Console.ReadLine() ?? string.Empty);
                    if (_c > 0)
                    {
                        //Console.WriteLine(IsPrime(_c) ? $"{_c} is a prime number" : $"{_c} is not a prime 2number");
                        //Console.WriteLine($"c = {_c}");
                    }
                    else
                    {
                        //Console.WriteLine($"c = {_c}");
                        Console.WriteLine("Stop timer");
                        _timer1.Dispose();
                        isDisposeTimer1 = isDisposeTimer2 = true;
                        _timer2.Dispose();
                        break;
                    }
                }
            }
        }

        static bool IsPrime(int x)
        {
            if (x < 2) return false;
            for (int i = 2; i * i <= x; i++)
            {
                if (x % i == 0) return false;
            }

            return true;
        }
    }
}