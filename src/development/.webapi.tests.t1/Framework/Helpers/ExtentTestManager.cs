/*
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.IO;
using System.Threading;

namespace AccountAPI.Tests.Framework.Helpers
{
    public class ExtentManager
    {
        private static readonly Lazy<ExtentReports> _lazy = new Lazy<ExtentReports>(() => new ExtentReports());

        public static ExtentReports Instance { get { return _lazy.Value; } }

        static ExtentManager()
        {
            var htmlReporter = new ExtentHtmlReporter(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestResults", "ExtentReport.html"));
            Instance.AttachReporter(htmlReporter);
        }

        private ExtentManager() { }
    }

    public class ExtentTestManager
    {
        private static ThreadLocal<ExtentTest> _parentTest = new ThreadLocal<ExtentTest>();
        private static ThreadLocal<ExtentTest> _childTest = new ThreadLocal<ExtentTest>();

        public static ExtentTest CreateParentTest(string name, string description = null)
        {
            _parentTest.Value = ExtentManager.Instance.CreateTest(name, description);
            return _parentTest.Value;
        }

        public static ExtentTest CreateTest(string name, string description = null)
        {
            _childTest.Value = _parentTest.Value.CreateNode(name, description);
            return _childTest.Value;
        }

        public static ExtentTest GetTest()
        {
            return _childTest.Value;
        }

        public static void EndTest()
        {
            ExtentManager.Instance.Flush();
        }
    }
}
*/