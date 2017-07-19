using System.IO;
using System.Reflection;
using Moq;
using Newtonsoft.Json.Linq;
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

            JObject expectedJson = null;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Servernet.Samples.DocumentationSamples.UnitTests.BlobInputOutputFunction.json";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var expectedDefinition = reader.ReadToEnd();
                    expectedJson = JObject.Parse(expectedDefinition);
                }
            }

            fileSystem.Verify(x =>
                x.WriteToFile(
                    It.IsAny<string>(),
                    It.Is<string>(definition =>
                        JToken.DeepEquals(JObject.Parse(definition), expectedJson))));
        }
    }
}
