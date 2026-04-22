using NUnit.Framework;
using PensionMCP.Data;

namespace TestPensionMCP.Data
{
    // These tests were written during investigation of why the database file could not be
    // found in the expected AppData\Local\PensionMCP folder. The root cause turned out to be
    // MSIX file system virtualisation: Claude Desktop is installed as a Windows Store package,
    // which transparently redirects LocalApplicationData writes to its sandboxed package folder:
    // C:\Users\<user>\AppData\Local\Packages\Claude_pzs8sxrjxfjjc\LocalCache\Local\
    [TestFixture]
    public class DbUtilsTests
    {
        [Test]
        public void BuildConnectionString_ShouldReturnValidConnectionString()
        {
            var result = DbUtils.BuildConnectionString();

            Assert.That(result, Does.StartWith("Data Source="));
            Assert.That(result, Does.EndWith("pensionmcp.db"));
        }

        [Test]
        public void GetDatabasePath_ShouldReturnExpectedPath()
        {
            var result = DbUtils.GetDatabasePath();

            Assert.That(result, Does.Contain("PensionMCP"));
            Assert.That(result, Does.EndWith("pensionmcp.db"));
        }
    }
}
