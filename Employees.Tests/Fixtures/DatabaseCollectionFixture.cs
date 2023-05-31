using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Tests.Fixtures
{
   [CollectionDefinition("DatabaseCollection", DisableParallelization = true)]
    public class DatabaseCollectionFixture : ICollectionFixture<DataBaseFixture>
    {
    }
}
