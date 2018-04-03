using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SpinnerDemo
{
    class Program
    {
        static private bool _exit = false;
        static private string _dir = @"[local directory path goes here]";
        static private int _delay = 9000;

        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            double seconds = 0;
            ConsoleSpinner spinner = new ConsoleSpinner
            {
                Delay = 300
            };

            Console.Title = "Async & Await Demo";
            Console.WriteLine("Will search a directory in your harddrive and show the total number of files found.");
            Console.CursorVisible = false;

            stopwatch.Start();

            GetFilesAsync();
            //GetFiles();

            while (!_exit)
            {
                spinner.Turn(displayMsg: "Loading ", sequenceCode: 0);
            }
            Console.CursorVisible = true;
            Console.WriteLine("Search completed!");

            seconds = stopwatch.ElapsedMilliseconds / 1000;

            Console.WriteLine($"It took {seconds} seconds.");
            Console.WriteLine("Press [Enter] to exit.");

            Console.ReadLine();
        }
        static async Task GetFilesAsync()
        {
            await Task.Run(() => Thread.Sleep(_delay)); // blocking process
            var files = await Task.Run(() => Directory.GetFiles(_dir, "*", SearchOption.AllDirectories)); // blocking process
            Console.WriteLine($"Total files found: {files.Count():N0}");
            _exit = true;
        }

        static void GetFiles()
        {
            Thread.Sleep(_delay); // blocking process
            var files = Directory.GetFiles(@"C:\Users\navarroaburr\Dropbox", "*", SearchOption.AllDirectories); // blocking process
            Console.WriteLine($"Total files found: {files.Count():N0}");
            _exit = true;
        }
    }

    class ConsoleSpinner
    {
        string[,] sequence = null;

        public int Delay { get; set; } = 200;

        int totalSequences = 0;
        int counter;

        public ConsoleSpinner()
        {
            counter = 0;
            sequence = new string[,] {
            { "/", "-", "\\", "|" },
            { ".", "o", "0", "o" },
            { "+", "x","+","x" },
            { "V", "<", "^", ">" },
            { ".   ", "..  ", "... ", "...." },
            { "=>   ", "==>  ", "===> ", "====>" },
           // ADD YOUR OWN CREATIVE SEQUENCE HERE IF YOU LIKE
        };

            totalSequences = sequence.GetLength(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequenceCode"> 0 | 1 | 2 |3 | 4 | 5 </param>
        public void Turn(string displayMsg = "", int sequenceCode = 0)
        {
            counter++;

            Thread.Sleep(Delay);

            sequenceCode = sequenceCode > totalSequences - 1 ? 0 : sequenceCode;

            int counterValue = counter % 4;

            string fullMessage = displayMsg + sequence[sequenceCode, counterValue];
            int msglength = fullMessage.Length;

            Console.Write(fullMessage);

            Console.SetCursorPosition(Console.CursorLeft - msglength, Console.CursorTop);
        }
    }
}
