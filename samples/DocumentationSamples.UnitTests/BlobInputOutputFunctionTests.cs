using Moq;
using Servernet.Generator;
using Xunit;

namespace Servernet.Samples.DocumentationSamples.UnitTests
{
    public class BlobInputOutputFunctionTests
    {
        [Fact]
        public void Generate_FunctionJson_ExpectedOutput()
        {
            var assemblyLoader = new Mock<IAssemblyLoader>();
            assemblyLoader
                .Setup(x => x.LoadFrom(It.IsAny<string>()))
                .Returns(typeof(BlobInputOutputFunction).Assembly);

            var environment = new Mock<IEnvironment>();
            environment
                .Setup(x => x.CurrentDirectory)
                .Returns(@"D:\fake\root\directory");

            var fileSystem = new Mock<IFileSystem>();

            var options = new Options()
            {
                AssemblyPath = string.Empty,
                Function = "Servernet.Samples.DocumentationSamples.BlobInputOutputFunction.Run",
            };

            var program = new Program(
                assemblyLoader.Object,
                environment.Object,
                fileSystem.Object,
                Mock.Of<ILogger>(),
                options);

            program.Run();

            fileSystem.Verify(x =>
                x.WriteToFile(
                    It.IsAny<string>(),
                    It.Is<string>(definition =>
                        definition.Contains("BlobInputOutputFunction"))));
        }
    }
}
