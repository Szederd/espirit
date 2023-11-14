using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlowDataSource.Domain
{
    public class SampleData
    {
        public int Id { get; internal set; }
        public string TextData { get; internal set; }
        public int NumericData { get; internal set; }
        public int MissingGroupingData { get; set; }
    }
}
