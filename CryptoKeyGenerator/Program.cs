using System;
using System.Security.Cryptography;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace CryptoKeyGenerator
{
    class Program
    {
        static void Main(string[] args) {
            while (!ProcessInput(args)) {
                var input = Console.ReadLine();
                args = ParseInput(input);
            }
        }

        static bool ProcessInput(string[] args) {
            var options = new Options();
            if (!CommandLineParser.Default.ParseArguments(args, options)) {
                return false;
            }

            if (options.Quit) {
                return true;
            }

            Console.WriteLine("Generating key of type {0}", options.KeyType);
            GenerateKey(options.KeyType);
            return true;
        }

        static string[] ParseInput(string enteredLine) {
            return enteredLine.Split(' ');
        }

        static void GenerateKey(KeyType type) {
            var keyLength = (int)type;
            var buffer = new byte[keyLength / 2];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetBytes(buffer);
            var sb = new StringBuilder(keyLength);
            for (var index = 0; index < buffer.Length; index++) {
                sb.Append(string.Format("{0:X2}", buffer[index]));
            }
            Console.WriteLine("Key of type {0} generated:", type);
            Console.WriteLine(sb);
        }
    }

    public class Options : CommandLineOptionsBase
    {
        [Option("t", "Type", DefaultValue = KeyType.SHA1, Required = true,
            HelpText = "Type of key to generate: valid values are SHA1, AES, and TripleDES")]
        public KeyType KeyType { get; set; }

        [Option("q", "Quit", DefaultValue = false, HelpText = "Quit to exit the program")]
        public bool Quit { get; set; }

        [HelpOption]
        public string GetUsage() {
            return HelpText.AutoBuild(
                this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    public enum KeyType
    {
        SHA1 = 128,
        AES = 64,
        TripleDES = 48,
    }
}
