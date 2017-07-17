using System;
using System.IO;
using Moq;
using Xunit;

namespace Servernet.Generator.UnitTests
{
    public class ProgramTests
    {
        [Fact]
        public void Run_AssemblyNotFound_ThrowsFileNotFoundException()
        {
            var assemblyLoader = new Mock<IAssemblyLoader>();
            assemblyLoader
                .Setup(x => x.LoadFrom(It.IsAny<string>()))
                .Throws<FileNotFoundException>();

            var environment = new Mock<IEnvironment>();
            environment
                .Setup(x => x.CurrentDirectory)
                .Returns(@"\\fake\root\directory");

            var options = new Options()
            {
                Assembly = string.Empty,
            };

            var program = new Program(
                assemblyLoader.Object,
                environment.Object,
                Mock.Of<ILogger>(),
                options);

            Assert.Throws<ArgumentException>(() =>
                program.Run());
        }

        [Fact]
        public void Run_NoFunctionParameter_NoFunctionFound_ReturnsEmpty()
        {
            var assemblyLoader = new Mock<IAssemblyLoader>();
            assemblyLoader
                .Setup(x => x.LoadFrom(It.IsAny<string>()))
                .Returns(typeof(Object).Assembly);

            var environment = new Mock<IEnvironment>();
            environment
                .Setup(x => x.CurrentDirectory)
                .Returns(@"\\fake\root\directory");

            var options = new Options()
            {
                Assembly = string.Empty,
            };

            var program = new Program(
                assemblyLoader.Object,
                environment.Object,
                Mock.Of<ILogger>(),
                options);

            program.Run();
        }
    }
}
