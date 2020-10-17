namespace ParallelCount
{
    using System;
    using System.Threading;

    /// <summary>
    ///     Сервер счетчик
    /// </summary>
    public static class CountServer
    {
        /// <summary>
        ///     Локер
        /// </summary>
        private static readonly ReaderWriterLockSlim RwLockSlim = new ReaderWriterLockSlim();

        /// <summary>
        ///     Счетчик
        /// </summary>
        private static int count;

        /// <summary>
        ///     Прибавляет целочисленное значение к счетчику
        /// </summary>
        /// <param name="value"> целочисленное значение </param>
        public static void AddToCount(int value)
        {
            RwLockSlim.EnterWriteLock();
            try
            {
                Console.WriteLine($"[Writing] [Thread Id: {Thread.CurrentThread.Name}] Add: {value}");
                count += value;

                // Имитация работы
                var rnd = new Random();
                Thread.Sleep(rnd.Next(5, 30));
            }
            finally
            {
                RwLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Возвращает значение счетчика
        /// </summary>
        /// <returns> Значение счетчика </returns>
        public static int GetCount()
        {
            RwLockSlim.EnterReadLock();
            try
            {
                Console.WriteLine($"[Reading] [Thread Id: {Thread.CurrentThread.Name}] Current Value: {count}");

                // Имитация работы
                var rnd = new Random();
                Thread.Sleep(rnd.Next(5, 30));

                return count;
            }
            finally
            {
                RwLockSlim.ExitReadLock();
            }
        }
    }
}