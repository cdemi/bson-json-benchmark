using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace BSON_JSON_Benchmark
{
    class Program
    {
        const int iterations = 1000;

        static MemoryStream BigJSON = new MemoryStream();
        static MemoryStream SmallJSON = new MemoryStream();

        static MemoryStream BigBSON = new MemoryStream();
        static MemoryStream SmallBSON = new MemoryStream();

        static JsonSerializer JsonSerializer = new JsonSerializer();
        static void Main(string[] args)
        {
            using (var bigJsonFile = File.OpenRead("App_Data\\JSON\\Big.json"))
            {
                bigJsonFile.CopyTo(BigJSON);
                resetStream(BigJSON);
            }
            using (var smallJsonFile = File.OpenRead("App_Data\\JSON\\Small.json"))
            {
                smallJsonFile.CopyTo(SmallJSON);
                resetStream(SmallJSON);
            }

            using (var bigBsonFile = File.OpenRead("App_Data\\BSON\\Big.bson"))
            {
                bigBsonFile.CopyTo(BigBSON);
                resetStream(BigBSON);
            }
            using (var smallBsonFile = File.OpenRead("App_Data\\BSON\\small.bson"))
            {
                smallBsonFile.CopyTo(SmallBSON);
                resetStream(SmallBSON);
            }


            benchmarker("Big JSON", () =>
            {
                JsonTextReader jsonReader = new JsonTextReader(new StreamReader(BigJSON));
                JsonSerializer.Deserialize<List<User>>(jsonReader);
                resetStream(BigJSON);
            });

            benchmarker("Small JSON", () =>
            {
                JsonTextReader jsonReader = new JsonTextReader(new StreamReader(SmallJSON));
                JsonSerializer.Deserialize<List<User>>(jsonReader);
                resetStream(SmallJSON);
            });

            benchmarker("Big BSON", () =>
            {
                BsonReader bsonReader = new BsonReader(BigBSON);
                bsonReader.ReadRootValueAsArray = true;
                JsonSerializer.Deserialize<List<User>>(bsonReader);
                resetStream(BigBSON);
            });

            benchmarker("Small BSON", () =>
            {
                BsonReader bsonReader = new BsonReader(SmallBSON);
                bsonReader.ReadRootValueAsArray = true;
                JsonSerializer.Deserialize<List<User>>(bsonReader);
                resetStream(SmallBSON);
            });


            Console.ReadKey();
        }

        static void resetStream(MemoryStream ms)
        {
            ms.Position = 0;
            ms.Seek(0, SeekOrigin.Begin);
        }

        static void benchmarker(string name, Action test)
        {
            Console.WriteLine($"Starting Test: {name}");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                test();
            }
            stopWatch.Stop();
            Console.WriteLine($"Time to {iterations} iterations: {stopWatch.Elapsed.ToString()}");
            Console.WriteLine($"Time to 1 iteration: {stopWatch.ElapsedMilliseconds / iterations}ms");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
