using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsRunner.Interfaces;

namespace ToolsRunner.Tests.Tests
{
    [TestClass]
    public class CommandRunnerTests : BaseToolsTest
    {
        private const int TimeoutMs = 1000;

        [TestMethod]
        public async Task SimpleRunTest()
        {
            using (var runner = Container.Resolve<ICommandRunner>())
            {
                runner.Start("echo", new string[] { "123" });
                await runner.WaitForEndAsync(TimeoutMs);
            }
        }

        [TestMethod]
        public async Task SendCommandTest()
        {
            var expected = "exit";

            using (var runner = Container.Resolve<ICommandRunner>())
            {
                runner.Start("cmd", new string[] { });
                await runner.SendCommandAsync("echo off");
                await runner.SendCommandAsync("exit");
                await runner.WaitForEndAsync(TimeoutMs);

                var actual = runner.ReadAllOutputLines().Last();

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public async Task ReadLineTest()
        {
            using (var runner = Container.Resolve<ICommandRunner>())
            {
                var expected = "test";

                runner.Start("echo", new[] { expected });
                await Task.Delay(TimeoutMs);
                var actual = runner.ReadOutputLine();

                Assert.IsFalse(actual.IsNone);
                Assert.AreEqual(expected, actual.Value);
            }
        }
    }
}
