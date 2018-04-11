using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class ScratchTesting
    {
        [Fact]
        public void TakeTesting()
        {
            var ints = new List<int> {1, 2, 3, 4, 5};

            IEnumerable<int> enumerable = ints.Take(10);

            Assert.Equal(5, enumerable.Count());
        }
    }
}
