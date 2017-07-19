using System;
using System.IO;
using Moq;
using Xunit;

namespace Servernet.Generator.UnitTests
{
    public class ProgramTests
    {
        [Fact]
        public void Run_AssemblyNotFound_ThrowsArgumentException()
        {
            var assemblyLoader = new Mock<IAssemblyLoader>();
            assemblyLoader
                .Setup(x => x.LoadFrom(It.IsAny<string>()))
                .Throws<FileNotFoundException>();

            var environment = new Mock<IEnvironment>();
            environment
                .Setup(x => x.CurrentDirectory)
                .Returns(@"D:\fake\root\directory");

            var options = new Options()
            {
                AssemblyPath = string.Empty,
            };

            var program = new Program(
                assemblyLoader.Object,
                environment.Object,
                Mock.Of<IFileSystem>(),
                Mock.Of<ILogger>(),
                options);

            try
            {
                program.Run();
                throw new NotSupportedException();
            }
            catch (ArgumentException e)
            {
                Assert.StartsWith("Failed to load assembly at location:", e.Message);
            }
        }

        [Fact]
        public void Run_AssemblyExists_CorrectPathQueried()
        {
            var assemblyLoader = new Mock<IAssemblyLoader>();
            assemblyLoader
                .Setup(x => x.LoadFrom(It.IsAny<string>()))
                .Returns(typeof(Object).Assembly);

            var environment = new Mock<IEnvironment>();
            environment
                .Setup(x => x.CurrentDirectory)
                .Returns(@"D:\fake\root\directory");

            var options = new Options()
            {
                AssemblyPath = "AssemblyName.dll",
            };

            var program = new Program(
                assemblyLoader.Object,
                environment.Object,
                Mock.Of<IFileSystem>(),
                Mock.Of<ILogger>(),
                options);

            program.Run();

            assemblyLoader.Verify(x => x.LoadFrom(
                It.Is<string>(path => path.Equals(@"D:\fake\root\directory\AssemblyName.dll"))));
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
                .Returns(@"D:\fake\root\directory");

            var options = new Options()
            {
                AssemblyPath = string.Empty,
            };

            var program = new Program(
                assemblyLoader.Object,
                environment.Object,
                Mock.Of<IFileSystem>(),
                Mock.Of<ILogger>(),
                options);

            program.Run();

            // @TODO not really asserting anything
        }

        [Fact]
        public void Run_NotFullyQualifiedFunctionName_ThrowsArgumentException()
        {
            var assemblyLoader = new Mock<IAssemblyLoader>();
            assemblyLoader
                .Setup(x => x.LoadFrom(It.IsAny<string>()))
                .Returns(typeof(Object).Assembly);

            var environment = new Mock<IEnvironment>();
            environment
                .Setup(x => x.CurrentDirectory)
                .Returns(@"D:\fake\root\directory");

            var options = new Options()
            {
                AssemblyPath = string.Empty,
                Function = "NonExistingMethod",
            };

            var program = new Program(
                assemblyLoader.Object,
                environment.Object,
                Mock.Of<IFileSystem>(),
                Mock.Of<ILogger>(),
                options);

            try
            {
                program.Run();
                throw new NotSupportedException();
            }
            catch (ArgumentException e)
            {
                Assert.StartsWith("Invalid fully qualified method name:", e.Message);
            }
        }

        [Fact]
        public void Run_FunctionNotFound_ThrowsArgumentException()
        {
            var assemblyLoader = new Mock<IAssemblyLoader>();
            assemblyLoader
                .Setup(x => x.LoadFrom(It.IsAny<string>()))
                .Returns(typeof(ProgramTests).Assembly);

            var environment = new Mock<IEnvironment>();
            environment
                .Setup(x => x.CurrentDirectory)
                .Returns(@"D:\fake\root\directory");

            var options = new Options()
            {
                AssemblyPath = string.Empty,
                Function = "Servernet.Generator.UnitTests.ProgramTests.NonExistingMethod",
            };

            var program = new Program(
                assemblyLoader.Object,
                environment.Object,
                Mock.Of<IFileSystem>(),
                Mock.Of<ILogger>(),
                options);

            try
            {
                program.Run();
                throw new NotSupportedException();
            }
            catch (ArgumentException e)
            {
                Assert.StartsWith("Failed to find method with name:", e.Message);
            }
        }

        [Fact]
        public void Run_NoOutputPath_ReleasedToDefaultPath()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var textWriter = new StreamWriter(memoryStream))
                {
                    var assemblyLoader = new Mock<IAssemblyLoader>();
                    assemblyLoader
                        .Setup(x => x.LoadFrom(It.IsAny<string>()))
                        .Returns(typeof(AFunction).Assembly);

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
                        Function = "Servernet.Generator.UnitTests.AFunction.Run"
                    };

                    var program = new Program(
                        assemblyLoader.Object,
                        environment.Object,
                        fileSystem.Object,
                        Mock.Of<ILogger>(),
                        options);

                    program.Run();

                    // @TODO shouldn't this be CurrentDirectory + AFunction?

                    fileSystem.Verify(x => x.CreateDirectory(
                        It.Is<string>(path => path.Equals("AFunction"))));
                }
            }
        }

        [Fact]
        public void Run_OutputPathProvided_ReleasedToProvidedPath()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var textWriter = new StreamWriter(memoryStream))
                {
                    var assemblyLoader = new Mock<IAssemblyLoader>();
                    assemblyLoader
                        .Setup(x => x.LoadFrom(It.IsAny<string>()))
                        .Returns(typeof(AFunction).Assembly);

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
                        Function = "Servernet.Generator.UnitTests.AFunction.Run",
                        OutputDirectory = @"E:\fake\output\directory",
                    };

                    var program = new Program(
                        assemblyLoader.Object,
                        environment.Object,
                        fileSystem.Object,  
                        Mock.Of<ILogger>(),
                        options);

                    program.Run();

                    fileSystem.Verify(x => x.CreateDirectory(
                        It.Is<string>(path => path.Equals(@"E:\fake\output\directory\AFunction"))));
                }
            }
        }

        [Fact]
        public void Run_NoOutputPathProvided_GeneratedFunctionJsonUnderDefaultPath()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var textWriter = new StreamWriter(memoryStream))
                {
                    var assemblyLoader = new Mock<IAssemblyLoader>();
                    assemblyLoader
                        .Setup(x => x.LoadFrom(It.IsAny<string>()))
                        .Returns(typeof(AFunction).Assembly);

                    var environment = new Mock<IEnvironment>();
                    environment
                        .Setup(x => x.CurrentDirectory)
                        .Returns(@"D:\fake\root\directory");

                    var fileSystem = new Mock<IFileSystem>();
                    fileSystem
                        .Setup(x =>
                            x.CreateFileWriter(It.Is<string>(path =>
                                PathExtensions.IsEqual(path, @"AFunction/function.json"))))
                        .Returns(textWriter);

                    var options = new Options()
                    {
                        AssemblyPath = string.Empty,
                        Function = "Servernet.Generator.UnitTests.AFunction.Run",
                    };

                    var program = new Program(
                        assemblyLoader.Object,
                        environment.Object,
                        fileSystem.Object,
                        Mock.Of<ILogger>(),
                        options);

                    program.Run();

                    fileSystem.VerifyAll();
                }
            }
        }

        [Fact]
        public void Run_OutputPathProvided_GeneratedFunctionJsonUnderOutputPath()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var textWriter = new StreamWriter(memoryStream))
                {
                    var assemblyLoader = new Mock<IAssemblyLoader>();
                    assemblyLoader
                        .Setup(x => x.LoadFrom(It.IsAny<string>()))
                        .Returns(typeof(AFunction).Assembly);

                    var environment = new Mock<IEnvironment>();
                    environment
                        .Setup(x => x.CurrentDirectory)
                        .Returns(@"D:\fake\root\directory");

                    var fileSystem = new Mock<IFileSystem>();
                    fileSystem
                        .Setup(x =>
                            x.CreateFileWriter(It.Is<string>(path =>
                                PathExtensions.IsEqual(path, @"E:\fake\output\directory\AFunction\function.json"))))
                        .Returns(textWriter);

                    var options = new Options()
                    {
                        AssemblyPath = string.Empty,
                        Function = "Servernet.Generator.UnitTests.AFunction.Run",
                        OutputDirectory = @"E:\fake\output\directory",
                    };

                    var program = new Program(
                        assemblyLoader.Object,
                        environment.Object,
                        fileSystem.Object,
                        Mock.Of<ILogger>(),
                        options);

                    program.Run();

                    fileSystem.VerifyAll();
                }
            }
        }

        [Fact]
        public void Run_AssemblyPathDoesNotIncludeDirectories_DependenciesCopiedFromAssemblyDirectory()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var textWriter = new StreamWriter(memoryStream))
                {
                    var assemblyLoader = new Mock<IAssemblyLoader>();
                    assemblyLoader
                        .Setup(x => x.LoadFrom(It.IsAny<string>()))
                        .Returns(typeof(AFunction).Assembly);

                    var environment = new Mock<IEnvironment>();
                    environment
                        .Setup(x => x.CurrentDirectory)
                        .Returns(@"D:\fake\root\directory");

                    var fileSystem = new Mock<IFileSystem>();
                    fileSystem
                        .Setup(x => x.CreateFileWriter(It.IsAny<string>()))
                        .Returns(textWriter);
                    fileSystem
                        .Setup(x => x.GetFiles(It.Is<string>(path => path.Equals(@"D:\fake\root\directory"))))
                        .Returns(new FileInfo[]
                        {
                            new FileInfo(@"D:\fake\root\directory\dependency1.dll"),
                            new FileInfo(@"D:\fake\root\directory\dependency432.dll"),
                        });

                    var options = new Options()
                    {
                        AssemblyPath = "AssemblyName.dll",
                        Function = "Servernet.Generator.UnitTests.AFunction.Run"
                    };

                    var program = new Program(
                        assemblyLoader.Object,
                        environment.Object,
                        fileSystem.Object,
                        Mock.Of<ILogger>(),
                        options);

                    program.Run();

                    fileSystem.Verify(x => x.CopyFile(
                        It.Is<string>(path => path.Equals(@"D:\fake\root\directory\dependency1.dll")),
                        It.Is<string>(path => path.Equals(@"AFunction\dependency1.dll")),
                        It.Is<bool>(overwrite => overwrite)));
                    fileSystem.Verify(x => x.CopyFile(
                        It.Is<string>(path => path.Equals(@"D:\fake\root\directory\dependency432.dll")),
                        It.Is<string>(path => path.Equals(@"AFunction\dependency432.dll")),
                        It.Is<bool>(overwrite => overwrite)));
                }
            }
        }

        [Fact]
        public void Run_AssemblyPathIncludesDirectories_DependenciesCopiedFromAssemblyDirectory()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var textWriter = new StreamWriter(memoryStream))
                {
                    var assemblyLoader = new Mock<IAssemblyLoader>();
                    assemblyLoader
                        .Setup(x => x.LoadFrom(It.IsAny<string>()))
                        .Returns(typeof(AFunction).Assembly);

                    var environment = new Mock<IEnvironment>();
                    environment
                        .Setup(x => x.CurrentDirectory)
                        .Returns(@"D:\fake\root\directory");

                    var fileSystem = new Mock<IFileSystem>();
                    fileSystem
                        .Setup(x => x.CreateFileWriter(It.IsAny<string>()))
                        .Returns(textWriter);
                    fileSystem
                        .Setup(x =>
                            x.GetFiles(It.Is<string>(path =>
                                PathExtensions.IsEqual(path, @"D:\fake\root\directory\path\to\assembly"))))
                        .Returns(new FileInfo[]
                        {
                            new FileInfo(@"D:\fake\root\directory\path\to\assembly\dependency1.dll"),
                            new FileInfo(@"D:\fake\root\directory\path\to\assembly\dependency432.dll"),
                        });

                    var options = new Options()
                    {
                        AssemblyPath = @".\path\to\assembly\AssemblyName.dll",
                        Function = "Servernet.Generator.UnitTests.AFunction.Run"
                    };

                    var program = new Program(
                        assemblyLoader.Object,
                        environment.Object,
                        fileSystem.Object,
                        Mock.Of<ILogger>(),
                        options);

                    program.Run();

                    fileSystem.Verify(x => x.CopyFile(
                        It.Is<string>(path => path.Equals(@"D:\fake\root\directory\path\to\assembly\dependency1.dll")),
                        It.Is<string>(path => path.Equals(@"AFunction\dependency1.dll")),
                        It.Is<bool>(overwrite => overwrite)));
                    fileSystem.Verify(x => x.CopyFile(
                        It.Is<string>(path => path.Equals(@"D:\fake\root\directory\path\to\assembly\dependency432.dll")),
                        It.Is<string>(path => path.Equals(@"AFunction\dependency432.dll")),
                        It.Is<bool>(overwrite => overwrite)));
                }
            }
        }

        [Fact]
        public void Run_OutputPathProvided_DependenciesCopiedUnderOutputPath()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var textWriter = new StreamWriter(memoryStream))
                {
                    var assemblyLoader = new Mock<IAssemblyLoader>();
                    assemblyLoader
                        .Setup(x => x.LoadFrom(It.IsAny<string>()))
                        .Returns(typeof(AFunction).Assembly);

                    var environment = new Mock<IEnvironment>();
                    environment
                        .Setup(x => x.CurrentDirectory)
                        .Returns(@"D:\fake\root\directory");

                    var fileSystem = new Mock<IFileSystem>();
                    fileSystem
                        .Setup(x => x.CreateFileWriter(It.IsAny<string>()))
                        .Returns(textWriter);
                    fileSystem
                        .Setup(x => x.GetFiles(It.Is<string>(path => path.Equals(@"D:\fake\root\directory\.\path\to\assembly"))))
                        .Returns(new FileInfo[]
                        {
                            new FileInfo(@"D:\fake\root\directory\path\to\assembly\dependency1.dll"),
                            new FileInfo(@"D:\fake\root\directory\path\to\assembly\dependency432.dll"),
                        });

                    var options = new Options()
                    {
                        AssemblyPath = @".\path\to\assembly\AssemblyName.dll",
                        Function = "Servernet.Generator.UnitTests.AFunction.Run",
                        OutputDirectory = @"E:\fake\output\directory",
                    };

                    var program = new Program(
                        assemblyLoader.Object,
                        environment.Object,
                        fileSystem.Object,
                        Mock.Of<ILogger>(),
                        options);

                    program.Run();

                    fileSystem.Verify(x => x.CopyFile(
                        It.Is<string>(path => path.Equals(@"D:\fake\root\directory\path\to\assembly\dependency1.dll")),
                        It.Is<string>(path => path.Equals(@"E:\fake\output\directory\AFunction\dependency1.dll")),
                        It.Is<bool>(overwrite => overwrite)));
                    fileSystem.Verify(x => x.CopyFile(
                        It.Is<string>(path => path.Equals(@"D:\fake\root\directory\path\to\assembly\dependency432.dll")),
                        It.Is<string>(path => path.Equals(@"E:\fake\output\directory\AFunction\dependency432.dll")),
                        It.Is<bool>(overwrite => overwrite)));
                }
            }
        }
    }
}
