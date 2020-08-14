namespace TimeTableReader
{
    internal class TimingMap
    {
        readonly int[] Record;

        public TimingMap()
        {
            Record = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
        }

        public TimingMap(Timing t) : this()
        {
            foreach (var x in t.Days) 
                Record[(int)x] = t.hours;
        }

        public static TimingMap Union(TimingMap t1, TimingMap t2)
        {
            var map = new TimingMap();
            for (int i = 0; i < 7; ++i)
                map.Record[i] = t1.Record[i] | t2.Record[i];

            return map;
        }

        public static bool Clash(TimingMap t1, TimingMap t2)
        {
            for (int i = 0; i < 7; ++i)
                if ((t1.Record[i] & t2.Record[i]) != 0)
                    return true;

            return false;
        }
    }
}