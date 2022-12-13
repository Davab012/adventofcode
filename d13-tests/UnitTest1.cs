using d13_packet_pairs;

namespace d13_tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow("[1,2,3]", "[1,2,3]", 0)]
        [DataRow("[1,2,3]", "[1,2,3,4]", -1)]
        [DataRow("[1,2,3]", "[1,2]", 1)]
        [DataRow("1", "[1,2]", -1)]
        [DataRow("[1,2]", "1", 1)]
        [DataRow("[1,2]", "2", -1)]
        [DataRow("[5,2]", "1", 1)]
        public void TestMethod1(string a, string b, int expected)
        {
            var actual = Util.Compare(a, b);
            Assert.AreEqual(expected, actual);
        }
    }
}