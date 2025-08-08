using AccountAPI.Tests.Framework.Helpers;
using NUnit.Framework;

namespace AccountAPI.Tests.Framework.Implements
{
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    internal class FrameworkTestFixtureBase
    {
        public UserHelper UserHelper { get;set; }
        public SessionHelper SessionHelper { get; set; }

        public FrameworkTestFixtureBase()
        {
            UserHelper = new UserHelper();
            SessionHelper = new SessionHelper();
        }

        protected void Log(string message)
        {
            Console.WriteLine($"[LOG] {DateTime.Now}: {message}");
        }

                public virtual void SetupTest()
        {
            
        }

        [TearDown]
        public virtual void TearDownTest()
        {

        }
    }
}
