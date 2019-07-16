using System.IO;
using System.Threading.Tasks;

namespace Clear
{
    public interface IFileManager
    {
        string ReadFile(string filename);
        Task<string> ReadFileAsync(string filename);
        void WriteToFile(string filename, string text);
        Task WriteToFileAsync(string filename, string text);
    }
    public class FileManager : IFileManager
    {
        public void WriteToFile(string filename, string text)
        {
            //Pass the filepath and filename to the StreamWriter Constructor
            using (StreamWriter sw = new StreamWriter(filename))
            {
                //Write a line of text
                sw.WriteLine(text);

                //Close the file
                sw.Close();
            }
        }

        public string ReadFile(string filename)
        {
            string line = "";

            //Pass the file path and file name to the StreamReader constructor
            using (StreamReader sr = new StreamReader(filename))
            {
                //Read the first line of text
                line = sr.ReadToEnd();

                //close the file
                sr.Close();
            }

            return line;
        }
        public async Task WriteToFileAsync(string filename, string text)
        {
            //Pass the filepath and filename to the StreamWriter Constructor
            using (StreamWriter sw = new StreamWriter(filename))
            {
                //Write a line of text
                await sw.WriteLineAsync(text);

                //Close the file
                sw.Close();
            }
        }

        public async Task<string> ReadFileAsync(string filename)
        {
            string line = "";

            //Pass the file path and file name to the StreamReader constructor
            using (StreamReader sr = new StreamReader(filename))
            {
                //Read the first line of text
                line = await sr.ReadToEndAsync();

                //close the file
                sr.Close();
            }

            return line;
        }
    }
}