using dezi.UiElements;
using System;

namespace dezi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // TODO: use args for opening files

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
