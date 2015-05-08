using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLexample
{
    class DataToChart
    {
        private DateTime date;
        private Double value;

        public DataToChart(DateTime date, Double value)
        {
            this.date = date;
            this.value = value;

        }


        public Double Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        

    }
}
