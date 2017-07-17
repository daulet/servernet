using Moq;
using Xunit;

namespace Servernet.Generator.UnitTests
{
    public class ProgramTests
    {
        [Fact]
        public void Test()
        {
            var program = new Program(
                Mock.Of<ILogger>(),
                Mock.Of<Options>());
        }
    }
}
