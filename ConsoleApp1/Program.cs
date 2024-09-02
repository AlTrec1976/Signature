using System.Diagnostics;
using System.Security.Cryptography;

namespace ConsolePortScanner
{
    class Program
    {
        static long blockSize = (long)Math.Pow(1024, 2) * 4;
        static void Main(string[] args)
        {

            var filePath = @"E:\Фильмы\Исторический\Спартак\Спартак 1. Боги арены 2011.zip";
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var totalBytes = fs.Length;
                    int totalBlocks = (int)Math.Ceiling((double)totalBytes / blockSize);
                    stopwatch.Start();
                    for (int i = 0; i < totalBlocks; i++)
                    {
                        int blockIndex = i;

                        Thread thread = new Thread(() => HashAndWriteToConsole(fs, blockIndex, blockSize));
                        thread.Start();
                        thread.Join();
                    }
                    stopwatch.Stop();
                    Console.WriteLine(stopwatch.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
        static void HashAndWriteToConsole(FileStream fs, int blockIndex, long blockSize)
        {
            byte[] buffer = new byte[blockSize];
            long bytesRead;

            fs.Seek(blockIndex * blockSize, SeekOrigin.Begin);

            bytesRead = fs.Read(buffer, 0, buffer.Length);

            // Если это последний блок и он меньше размера блока
            if (bytesRead < blockSize)
            {
                Array.Resize(ref buffer, (int)bytesRead);
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(buffer);
                string hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                Console.WriteLine($"Блок {blockIndex}: {hashString}");
            }
        }
    }
}