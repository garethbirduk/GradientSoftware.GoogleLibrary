using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GoogleLibrary.Test
{
    [TestClass]
    public class TestPrintAliases
    {
        [TestMethod]
        public void Test1()
        {
            var myClass = new MyClass()
            {
                Attendees =
                [
                    new EnumAttendeeWrapper<EnumAttendeeField1>(EnumAttendeeField1.Andy),
                    new EnumAttendeeWrapper<EnumAttendeeField1>(EnumAttendeeField1.Carl),
                ]
            };
            var list = myClass.GetAttendeesAlias();
            Assert.AreEqual("A", list[0]);
            Assert.AreEqual("C", list[1]);
        }

        [TestMethod]
        public void Test2()
        {
            var myClass = new MyClass()
            {
                Attendees = new List<IAttendee>()
                {
                    new EnumAttendeeWrapper<EnumAttendeeField2>(EnumAttendeeField2.Bob),
                    new EnumAttendeeWrapper<EnumAttendeeField2>(EnumAttendeeField2.Dave),
                }
            };
            var list = myClass.GetAttendeesAlias();
            Assert.AreEqual("B", list[0]);
            Assert.AreEqual("D", list[1]);
        }
    }
}