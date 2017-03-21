using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servernet;

namespace SampleFunction.Model
{
    [Serializer(typeof(JsonSerializer))]
    public class Purchase
    {
        public double Amount { get; set; }

        public string CustomerName { get; set; }
    }
}
