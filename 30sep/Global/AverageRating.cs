using _30sep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _30sep.Global
{
    public static class AverageRating
    {
        public static int averageRating (Book book)
        {
            var sum = 0;
            var divider = 0;
            foreach (var item in book.Review)
            {
                sum += (item.Rating.HasValue ? item.Rating.Value : 0);

                if (item.Rating != 0)
                {
                    divider++;
                }
            }
            if (divider > 0)
            {
                return sum / divider;
            }
            return 10;
        }
    }
}