using System.Security.Cryptography;

internal static class Program
{
    static long blockSize = (long)Math.Pow(1024, 2) * 4;
    static void Main(string[] args)
    {
        var fileName = @"E:\Фильмы\Исторический\Спартак\Спартак 1. Боги арены 2011.zip";
        var bloks = 0;
        var dict = new Dictionary<string, byte[]>();


        //4194304

        // 11130297091
        using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            Console.WriteLine("Start...");
            var sizeFile = fileStream.Length;
            //количество деления без остатка + 1 = итого блоков

            bloks = (int)Math.Ceiling(((double)sizeFile / blockSize));

            for (int i = 0; i < bloks; i++)
            {
                //var i1 = i;
                byte[] result;
                var thread = new Thread(() => {
                    result = CalculateHash(fileStream, i);
                    //dict.Add(i.ToString(), result);

                    Console.WriteLine($"Хэш  {BitConverter.ToString(result)} блок номер {i + 1}");
                });

                thread.Start();
                thread.Join();

            }
            Console.WriteLine("End...");
        }

    }

    static byte[] CalculateHash(FileStream stream, int blockIndex)
    {
        using (var hasher = SHA256.Create())
        {
            stream.Seek(blockIndex * blockSize, SeekOrigin.Begin);
            var buffer = new byte[blockSize];
            var actualLength = stream.Read(buffer, 0, buffer.Length);

            return hasher.ComputeHash(buffer, 0, actualLength);
        }
    }
}