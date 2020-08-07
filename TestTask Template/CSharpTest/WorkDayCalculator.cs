using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest
{

    public class WorkDayCalculator : IWorkDayCalculator
    {
        private TimeSpan Oneday = new TimeSpan(1, 0, 0, 0);

        // if there are any interseptions of weekends
        private List<WeekEnd> WeekendsMegre(WeekEnd[] weekEnds)
        {
            if (weekEnds == null) { return new List<WeekEnd>(); }
            List<WeekEnd> list = new List<WeekEnd>();
            WeekEnd w=null;
            foreach( WeekEnd weekEnd in weekEnds)
            {
                if (w is null) { w = weekEnd; continue; }
                if (weekEnd.StartDate - this.Oneday > w.EndDate)  { list.Add(w); w = weekEnd; }
                else { w.EndDate = w.EndDate > weekEnd.EndDate ? w.EndDate : weekEnd.EndDate; }
            }
            list.Add(w);
            return list;
        }

        public DateTime Calculate(DateTime startDate, int dayCount, WeekEnd[] weekEnds)
        {
            // sort weekEnds by StartDate
            Array.Sort(weekEnds, delegate (WeekEnd w1, WeekEnd w2)
            {
                return w1.StartDate.CompareTo(w2.StartDate);
            });

            List<WeekEnd> WeekEndList = WeekendsMegre(weekEnds);

            DateTime endDate = startDate.AddDays(dayCount-1);

            //check weekends
            DateTime min, max;


            foreach (WeekEnd weekEnd in WeekEndList)
            {
                if (weekEnd.StartDate > endDate) { return endDate; }

                
                if (weekEnd.StartDate <= endDate)
                {
                    //min = weekEnd.EndDate > endDate ? endDate : weekEnd.EndDate;
                    max = weekEnd.StartDate < startDate ? startDate : weekEnd.StartDate;
                    endDate += (weekEnd.EndDate - max).Add(this.Oneday); }
            }

            return endDate;
        }
    }
}
