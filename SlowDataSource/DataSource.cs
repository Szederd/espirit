using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlowDataSource
{
    /// <summary>
    /// Lassú háttérrendszer emulátor
    /// </summary>
    public class DataSource
    {
        /// <summary>
        /// Elem generátor, rögzített random
        /// </summary>
        /// <returns></returns>
        private List<Domain.SampleData> All()
        {
            string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec vel aliquet urna, ut interdum nibh. Nam venenatis magna lorem, vitae vestibulum lectus lobortis id. Integer tempor, ligula quis vulputate hendrerit, mi eros volutpat massa, a tincidunt justo arcu et justo. Vestibulum semper odio volutpat dolor semper aliquam. Sed sit amet condimentum quam, a vehicula ex. Maecenas lacinia porta ex. In at porta purus, eget vestibulum mi. Morbi lorem nibh, convallis id nisi ac, interdum convallis enim. Vivamus nulla quam, congue in diam quis, facilisis tincidunt ligula.";
            List<Domain.SampleData> retVal = new List<Domain.SampleData>();
            var rnd = new Random(12345678);
            for (int i = 1; i < 102; i++)
            {
                var start = rnd.Next(lorem.Length - 1);
                var end = lorem.Length - start;
                retVal.Add(new Domain.SampleData()
                {
                    Id = i,
                    NumericData = rnd.Next(10000),
                    TextData = lorem.Substring(start, end),
                    MissingGroupingData = rnd.Next(3)
                });
            }
            return retVal;
        }


        /// <summary>
        /// 10 másodpercet vár majd visszatér az összes elemmel
        /// </summary>
        /// <returns>Minden elem</returns>
        public IEnumerable<Domain.SampleData> FetchAll ()
        {
            System.Threading.Thread.Sleep(10000);
            var retVal = All();
            return retVal;
        }
    }
}
