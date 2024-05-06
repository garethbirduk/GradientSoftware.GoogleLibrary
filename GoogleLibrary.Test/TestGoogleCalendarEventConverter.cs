using GoogleLibrary.Custom;
using GoogleLibrary.GoogleCalendar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GoogleLibrary.Test
{
    [TestClass]
    public class TestGoogleCalendarEventConverter
    {
        //    [TestMethod]
        //    public void TestFromDescription()
        //    {
        //        var fields1 = GoogleEventBuilder.FromDescription("Who: me");
        //        Assert.AreEqual(1, fields1.Count);
        //        Assert.AreEqual("me", fields1["Who"]);

        //        var fields2 = GoogleEventBuilder.FromDescription("Who: me\r\nWhen: now");
        //        Assert.AreEqual(2, fields2.Count);
        //        Assert.AreEqual("me", fields2["Who"]);
        //        Assert.AreEqual("now", fields2["When"]);

        //        var fields3 = GoogleEventBuilder.FromDescription("Who: me\r\nWhen: now\r\n");
        //        Assert.AreEqual(2, fields3.Count);
        //        Assert.AreEqual("me", fields3["Who"]);
        //        Assert.AreEqual("now", fields3["When"]);
        //    }

        //    [TestMethod]
        //    public void TestToDescription()
        //    {
        //        Assert.AreEqual("", GoogleEventBuilder.ToDescription(new Dictionary<string, string>()));

        //        var fields1 = new Dictionary<string, string>()
        //        {
        //            { "Who", "me" }
        //        };
        //        Assert.AreEqual("Who: me", GoogleEventBuilder.ToDescription(fields1));

        //        var fields2 = new Dictionary<string, string>()
        //        {
        //            { "Who", "me" },
        //            { "When", "now" },
        //        };
        //        Assert.AreEqual("Who: me\r\nWhen: now", GoogleEventBuilder.ToDescription(fields2));
        //    }
        //}
    }
}