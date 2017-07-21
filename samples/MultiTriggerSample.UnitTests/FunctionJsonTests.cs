using Moq;
using Newtonsoft.Json.Linq;
using Servernet.Generator;
using Servernet.Samples.MultiTriggerSample.Trigger;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Servernet.Samples.MultiTriggerSample.UnitTests
{
    public class FunctionJsonTests
    {
        [Theory]
        [InlineData(typeof(TableEntityProcessorHttpTrigger), "TableEntityProcessorHttpTrigger.json")]
        [InlineData(typeof(TransactionPaginationHttpTrigger), "TriggerProcessingTransactionTable.json")]
        [InlineData(typeof(TransactionPaginationQueueTrigger), "TransactionPaginationQueueTrigger.json")]
        [InlineData(typeof(TransactionPaginationTimerTrigger), "TransactionPaginationTimerTrigger.json")]
        [InlineData(typeof(TranscationProcessorHttpTrigger), "TranscationProcessorHttpTrigger.json")]
        public void Generate_FunctionJson_ExpectedOutput(Type functionType, string expectedFileName)
        {
            var assemblyLoader = new Mock<IAssemblyLoader>();
            assemblyLoader
                .Setup(x => x.LoadFrom(It.IsAny<string>()))
                .Returns(functionType.Assembly);

            var environment = new Mock<IEnvironment>();
            environment
                .Setup(x => x.CurrentDirectory)
                .Returns(@"D:\fake\root\directory");

            var fileSystem = new Mock<IFileSystem>();

            var functionMethod = functionType.GetMethods(BindingFlags.Public | BindingFlags.Static).Single();
            var options = new Options()
            {
                AssemblyPath = string.Empty,
                Function = $"{functionType.FullName}.{functionMethod.Name}",
            };

            var program = new Program(
                assemblyLoader.Object,
                environment.Object,
                fileSystem.Object,
                Mock.Of<ILogger>(),
                options);

            program.Run();

            var expectedJson = GetJObjectForResourceName($"Servernet.Samples.MultiTriggerSample.UnitTests.{expectedFileName}");

            fileSystem.Verify(x =>
                x.WriteToFile(
                    It.IsAny<string>(),
                    It.Is<string>(definition =>
                        JToken.DeepEquals(JObject.Parse(definition), expectedJson))));
        }

        private static JObject GetJObjectForResourceName(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new ArgumentException($"{resourceName} does not exist", nameof(resourceName));
                }

                using (var reader = new StreamReader(stream))
                {
                    var expectedDefinition = reader.ReadToEnd();
                    return JObject.Parse(expectedDefinition);
                }
            }
        }
    }
}