using System.Collections.Generic;
using System.Collections.Immutable;

namespace Servernet.Generator
{
    public class Package
    {
        public string PackageId { get; }

        public ImmutableHashSet<string> Assemblies { get; }

        public ImmutableHashSet<Package> Dependencies { get; }

        public Package(string packageId, HashSet<string> assemblies)
            : this(packageId, assemblies, dependencies: new HashSet<Package>())
        { }

        public Package(string packageId, HashSet<string> assemblies, HashSet<Package> dependencies)
        {
            PackageId = packageId;
            Assemblies = assemblies.ToImmutableHashSet();
            Dependencies = dependencies.ToImmutableHashSet();
        }
    }
}