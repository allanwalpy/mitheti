using System;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Mitheti.Core.Database
{
    public class GuidGenerator : ValueGenerator<string>
    {
        public static string DefaultGuid = System.Guid.Empty.ToString();

        private static string Generate() => Guid.NewGuid().ToString();

        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry) => Generate();
    }
}
