namespace ParallelCount
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class Program
    {
        public static void Main()
        {
            var outputFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "output.txt");
            var fileStream = new FileStream(outputFilePath, FileMode.Create);
            var writerStream = new StreamWriter(fileStream);
            Console.SetOut(writerStream);

            var r1 = new Task(Read);
            var r2 = new Task(Read);
            var r3 = new Task(Read);
            var w1 = new Task(Write);
            var w2 = new Task(Write);
            var tasks = new[] {r1, r2, r3, w1, w2};

            r1.Start();
            r2.Start();
            r3.Start();
            Thread.Sleep(20);
            w1.Start();
            w2.Start();

            Task.WaitAll(tasks);
        }

        private static void Read()
        {
            Parallel.For(0, 100, (i, state) =>
            {
                if (Thread.CurrentThread.Name == null)
                {
                    var name = new string(Guid.NewGuid().ToString().Where(char.IsLetterOrDigit).ToArray());
                    Thread.CurrentThread.Name = name;
                }

                CountServer.GetCount();
            });
        }

        private static void Write()
        {
            Parallel.For(0, 50, (i, state) =>
            {
                if (Thread.CurrentThread.Name == null)
                {
                    var name = new string(Guid.NewGuid().ToString().Where(char.IsLetterOrDigit).ToArray());
                    Thread.CurrentThread.Name = name;
                }

                var digit = int.Parse(Guid.NewGuid().ToString().Where(char.IsDigit).First(ch => ch > '0').ToString());
                CountServer.AddToCount(digit);
            });
        }
    }
}