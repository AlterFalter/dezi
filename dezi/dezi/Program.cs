using dezi.UiElements;
using System;

namespace dezi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // don't quit program on [ctrl] + [c]
            // source: https://stackoverflow.com/questions/177856/how-do-i-trap-ctrl-c-sigint-in-a-c-sharp-console-app
            Console.CancelKeyPress += (sender, eventArgs) => {
                eventArgs.Cancel = true;
            };

            if (args.Length > 1)
            {
                Console.WriteLine("Please note that currently only a single file can be opened");
            }
            else
            {
                new Ui(args).Run();
            }
        }
    }
}
