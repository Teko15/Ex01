
namespace DataExporter
{
    class DataExporter
    {

        static List<string> allActiveStudies = new List<string>();

        public static void Main(string[] args)
        {
            string log = string.Empty;
            if (Directory.Exists("log.txt"))
                log = File.ReadAllText("log.txt");
            /*
            if (args.Length != 3)
            {
                log += "Invalid number of arguments. " + DateTime.Now + '\n';
                SaveLogs(log);
                throw new ArgumentNullException("Invalid number of arguments.");
            }

            if (!Directory.Exists(args[1]))
            {
                log += "Podana sciezka jest niepoprawna. " + DateTime.Now + '\n';
                SaveLogs(log);
                throw new ArgumentException("Podana sciezka jest niepoprawna");
            }

            if (!Directory.Exists(args[0]))
            {
                log += "Output file doesn't exist. " + DateTime.Now + '\n';
                SaveLogs(log);
                throw new FileNotFoundException("Plik {} nie istnieje", args[0]);

            }*/

            var inputPath = args[1];
            var outputPath = args[0] + '.' + args[2];

            FileInfo fileInfo = new(inputPath);

            var fileContent = new List<string>();

            using (var streamReader = new StreamReader(fileInfo.OpenRead()))
            {
                string line = string.Empty;
                while ((line = streamReader.ReadLine()) != null)
                    fileContent.Add(line + '\n');
            }

            var correctData = new List<string>();

            foreach (var item in fileContent)
            {
                if (item.Count(e => e == ',') != 8 || item.IndexOf(",,") != -1)
                    log += item + '\n';
                else
                    correctData.Add(item);
            }
            string tab = (char) 9 + string.Empty;
            string tabs = (char)9 + string.Empty + (char)9;
            string output = "{\n" + 
                tab + "uczelnia: {\n" + 
                tab + tab + "createdAt: \"" + DateTime.UtcNow.ToString("dd.MM.yyyy") + "\",\n" +
                tab + tab + "author: \"Eryk Aftowicz\",\n" +
                tab + tab + "studenci: [\n";
            StudentData(correctData[0]);

            for (int i = 0; i < correctData.Count; i++)
            {
                output += StudentData(correctData[i]);
                if (i < correctData.Count - 1)
                    output += ',';
                output += '\n';
            }

            output += (tabs + "],\n" +
                tabs + "activeStudies: [\n");

            List<String> activeStudies = allActiveStudies.Distinct().ToList();
            for (int i = 0; i < activeStudies.Count; i++)
            {
                output += (tabs + tab + "{\n" +
                    tabs + tabs + "name: \"" + activeStudies[i] + "\",\n" +
                    tabs + tabs + "numberOfStudents: " + allActiveStudies.Count(e => e == activeStudies[i]) + "\"\n" +
                    tabs + tab + '}');
                if (i < activeStudies.Count - 1)
                    output += ',';
                output += '\n';
            }
            output += (tabs + "]\n" + tab + "}\n}");
            Console.WriteLine("Pomyslnie zapisano plik!");
            File.WriteAllText(outputPath, output);
        }

        static void SaveLogs(string log)
        {
            File.WriteAllText("log.txt", log);
        }
        static string StudentData(string line)
        {
            string tab = (char)9 + string.Empty;
            string tabs = (char)9 + string.Empty + (char)9 + string.Empty + (char)9;
            string name = DataFromLine(line);
            line = CutLine(line);
            string surname = DataFromLine(line);
            line = CutLine(line);
            string studiesName = DataFromLine(line);
            line = CutLine(line);
            string studiesMode = DataFromLine(line);
            line = CutLine(line);
            string indexNumber = 's' + DataFromLine(line);
            line = CutLine(line);
            string birthdate = DataFromLine(line);
            birthdate = GetGoodDate(birthdate);
            line = CutLine(line);
            string email = DataFromLine(line);
            line = CutLine(line);
            string mothersName = DataFromLine(line);
            line = CutLine(line);
            string fathersName = line[0..^1];
            allActiveStudies.Add(studiesName);
            string output = tabs + "{\n" +
                tabs + tab + "indexNumber: \"" + indexNumber + "\",\n" +
                tabs + tab + "fname: \"" + name + "\",\n" +
                tabs + tab + "lname: \"" + surname + "\",\n" +
                tabs + tab + "birthdate: \"" + birthdate + "\",\n" +
                tabs + tab + "email: \"" + email + "\",\n" +
                tabs + tab + "mothersName: \"" + mothersName + "\",\n" +
                tabs + tab + "fathersName: \"" + fathersName + "\",\n" +
                tabs + tab + "studies: {\n" +
                tabs + tab + tab + "name: \"" + studiesName + "\",\n" +
                tabs + tab + tab + "mode: \"" + studiesMode + "\"\n" +
                tabs + tab + "}\n" +
                tabs + '}';
            return output;
        }
        static string DataFromLine(string line)
        {
            return line.Substring(0, line.IndexOf(','));
        }
        static string CutLine(string line)
        {
            return line.Substring(line.IndexOf(',') + 1);
        }

        static string GetGoodDate(string wrongDate)
        {
            string day = wrongDate.Substring(8);
            string month = wrongDate.Substring(5, 2);
            string year = wrongDate.Substring(0, 4);
            string goodDate = day + '.' + month + '.' + year;
            return goodDate;
        }
    }
}