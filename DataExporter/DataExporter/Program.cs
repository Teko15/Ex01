using DataExporter.Models;

namespace DataExporter
{
    class DataExporter
    {
        public static void Main(string[] args)
        {
            var inputPath = args[0];
            //var outputPath = args[1];

            //var fileInfo = new FileInfo(inputPath);
            FileInfo fileInfo = new(inputPath);

            var fileContent = new List<string>();

            using (var streamReader = new StreamReader(fileInfo.OpenRead()))
            {
                string line = string.Empty;
                while ((line = streamReader.ReadLine()) != null)
                {
                    fileContent.Add(line);
                }
            }
            //streamReader.Dispose();
            foreach (var item in fileContent)
            {
                Console.Write(item);

            }


            /*
             * foreach(var line in File.ReadLines(path))
             * {
             *  Console.Writeline(line);
             * }
             */

            var hashSet = new HashSet<Student>(new MyComparer());
        }
    }
}