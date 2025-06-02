using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    const int BUFFER_SIZE = 100;
    static Queue<int> buffer = new Queue<int>();

    static SemaphoreSlim isFull = new SemaphoreSlim(BUFFER_SIZE);   // Số chỗ còn trống
    static SemaphoreSlim isEmpty = new SemaphoreSlim(1);            // Số phần tử hiện có
    static SemaphoreSlim mutex = new SemaphoreSlim(1);              // Mutex để truy cập buffer
    
    static int producedCount = 0;
    static int consumedCount = 0;
    static int minValue = int.MaxValue;
    static int maxValue = int.MinValue;

    static object statsLock = new object();
    static CancellationTokenSource cts = new CancellationTokenSource();

    static void Main(string[] args)
    {
        int numProducers = 2;
        int numConsumers = 2;

        for (int i = 0; i < numProducers; i++)
        {
            int id = i + 1;
            new Thread(() => Producer(id, cts.Token)).Start();
        }

        for (int i = 0; i < numConsumers; i++)
        {
            new Thread(() => Consumer(cts.Token)).Start();
        }

        new Thread(() => PrintStatistics(cts.Token)).Start();

        // Đợi người dùng nhấn phím bất kỳ để dừng
        Console.WriteLine("Nhan nut bat ki dung chuong trình...");
        Console.ReadKey();
        cts.Cancel(); // Gửi tín hiệu dừng
    }

    static void Producer(int id, CancellationToken token)
    {
        Random rand = new Random(Guid.NewGuid().GetHashCode());
        while (!token.IsCancellationRequested)
        {
            Thread.Sleep(rand.Next(500, 1000));
            int value = rand.Next(100, 1000);

            isFull.Wait(token);
            mutex.Wait(token);
             
            buffer.Enqueue(value);

            mutex.Release();
            isEmpty.Release();

            lock (statsLock)
            {
                producedCount++;
            }

            Console.WriteLine($"P{id}: {value} - {DateTime.Now:HH:mm:ss}");
        }
        Console.WriteLine($"🛑 Producer {id} stopped.");
    }

    static void Consumer(CancellationToken token)
    {
        Random rand = new Random(Guid.NewGuid().GetHashCode());

        while (!token.IsCancellationRequested)
        {
            Thread.Sleep(rand.Next(600, 1200));

            isEmpty.Wait(token);
            mutex.Wait(token);
            
            int value = buffer.Dequeue();

            mutex.Release();
            isFull.Release();
            
            int result = value * value;

            lock (statsLock)
            {
                consumedCount++;
                if (value > maxValue) maxValue = value;
                if (value < minValue) minValue = value;
            }

            Console.WriteLine($"C: {value} - {result} - {DateTime.Now:HH:mm:ss}");
        }
        Console.WriteLine("🛑 Consumer stopped.");
    }

    static void PrintStatistics(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Thread.Sleep(5000);

            lock (statsLock)
            {
                Console.WriteLine($"\n Thong Ke ({DateTime.Now:HH:mm:ss})");
                Console.WriteLine($"   Tong San Xuat: {producedCount}");
                Console.WriteLine($"   Tong Tieu Thu: {consumedCount}");
                Console.WriteLine($"   Lon Nhat: {maxValue}");
                Console.WriteLine($"   Nho Nhat: {minValue}\n");
            }
        }
        Console.WriteLine("🛑 In thống kê dừng.");
    }
}
