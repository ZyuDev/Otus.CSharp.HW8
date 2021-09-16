using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Otus.Teaching.Concurrency.Import.Loader.Helper
{
    public class ChankHelper
    {
        public static List<IEnumerable<Customer>> SplitToChanks(IEnumerable<Customer> data, int chanksCount)
        {
            var chanks = new List<IEnumerable<Customer>>();

            if (chanksCount < 1)
            {
                chanks.Add(data);
                return chanks;
            }

            var chankSize = data.Count() / chanksCount;

            var currentChank = new Customer[chankSize];
            chanks.Add(currentChank);

            var i = 0;
            foreach (var val in data)
            {

                if (i > currentChank.Length - 1)
                {
                    chanks.Add(currentChank);
                    currentChank = new Customer[chankSize];
                    i = 0;
                }

                currentChank[i] = val;


                i++;
            }

            return chanks;
        }
    }
}
