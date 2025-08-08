/*
using AventStack.ExtentReports;
using System;

namespace AccountAPI.Tests.Framework.Helpers
{
    public class Logger
    {
        public static void WriteLine(string message)
        {
            ExtentTestManager.GetTest().Info($"<pre>{message}</pre>");
            Console.WriteLine(message);
        }

        public static void LogTestStep(Status status, string message)
        {
            ExtentTestManager.GetTest().Log(status, message);
            Console.WriteLine($"[{status.ToString().ToUpper()}] {message}");
        }
    }
}
*/