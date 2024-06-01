using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test
{
    [TestClass]
    public class TestIntExtensions
    {
        [DataTestMethod]
        [DataRow(0, "A")]
        [DataRow(1, "B")]
        [DataRow(26, "AA")]
        [DataRow(27, "AB")]
        [DataRow(null, "")]
        public void TestNullableToGoogleColumn(int? value, string columnName)
        {
            Assert.AreEqual(columnName, value.ToGoogleColumn());
        }

        [DataTestMethod]
        [DataRow(0, "A")]
        [DataRow(1, "B")]
        [DataRow(26, "AA")]
        [DataRow(27, "AB")]
        public void TestToGoogleColumn(int value, string columnName)
        {
            Assert.AreEqual(columnName, value.ToGoogleColumn());
        }
    }
}