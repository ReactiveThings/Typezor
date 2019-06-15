using System;

namespace Typewriter
{
    class Log
    {
        internal static void Warn(string v, string id, string message)
        {
            Console.WriteLine($"Warn: {v} {id} {message}");
        }

        internal static void Warn(string v)
        {
            Console.WriteLine($"Warn: {v}");
        }

        internal static void Error(string logMessage)
        {
            Console.WriteLine($"Error: {logMessage}");
        }

        internal static void Debug(string v, long elapsedMilliseconds)
        {
            Console.WriteLine($"Debug: {v} {elapsedMilliseconds}");
        }

        internal static void Debug(string v)
        {
            Console.WriteLine($"Debug: {v}");
        }
    }
}
