using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vroom.Extentions
{
    public class YearRange:RangeAttribute
    {
        public YearRange(int startYear):base(startYear,DateTime.Today.Year)
        {

        }
    }
}
