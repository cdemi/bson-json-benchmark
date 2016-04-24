using System.IO;

namespace BSON_JSON_Benchmark
{
    class Program
    {
        static string BigJSON;
        static string SmallJSON;

        static MemoryStream BigBSON = new MemoryStream();
        static MemoryStream SmallBSON = new MemoryStream();
        static void Main(string[] args)
        {
            BigJSON = File.ReadAllText("App_Data\\JSON\\Big.json");
            SmallJSON = File.ReadAllText("App_Data\\JSON\\Small.json");

            using (var bigBsonFile = File.OpenRead("App_Data\\BSON\\Big.bson"))
            {
                bigBsonFile.CopyTo(BigBSON);
            }
            using (var smallBsonFile = File.OpenRead("App_Data\\BSON\\small.bson"))
            {
                smallBsonFile.CopyTo(SmallBSON);
            }
        }
    }
}
