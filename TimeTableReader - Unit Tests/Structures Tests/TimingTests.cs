using Microsoft.VisualStudio.TestTools.UnitTesting;
using Time_Table_Generator;
using Newtonsoft.Json;

namespace TimeTableReader___Unit_Tests
{
    [TestClass]
    public class TimingTests
    {
        [TestMethod]
        public void TimingClassJSON_NonEmpty()
        {
            Timing t1 = new Timing("M W F", "1 2 6");
            string s = JsonConvert.SerializeObject(t1);
            Timing t2 = JsonConvert.DeserializeObject<Timing>(s);

            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void TimingClassJSON_EmptyHour()
        {
            Timing t1 = new Timing("", "1 2 6");
            string s = JsonConvert.SerializeObject(t1);
            Timing t2 = JsonConvert.DeserializeObject<Timing>(s);

            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void TimingClassJSON_EmptyDays()
        {
            Timing t1 = new Timing("M W F", "");
            string s = JsonConvert.SerializeObject(t1);
            Timing t2 = JsonConvert.DeserializeObject<Timing>(s);

            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void TimingClassJSON_AllEmptyEntries()
        {
            Timing t1 = new Timing("", "");
            string s = JsonConvert.SerializeObject(t1);
            Timing t2 = JsonConvert.DeserializeObject<Timing>(s);

            Assert.IsTrue(t1.Equals(t2));
        }
    }
}
