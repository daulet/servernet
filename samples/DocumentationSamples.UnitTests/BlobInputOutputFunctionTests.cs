using System;
using System.IO;
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
            using (var memoryStream = new MemoryStream())
            {
                using (var textWriter = new StreamWriter(memoryStream))
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
                    fileSystem
                        .Setup(x => x.CreateFileWriter(It.IsAny<string>()))
                        .Returns(textWriter);

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

                    fileSystem.VerifyAll();

                    memoryStream.Flush();
                    memoryStream.Position = 0;
                    using (var streamReader = new StreamReader(memoryStream))
                    {
                        var functionDef = streamReader.ReadToEnd();

                        Console.WriteLine(functionDef);
                    }
                }
            }
        }
    }
}
