using System.Collections.Generic;

namespace Mitheti.Wpf.Models
{
    public class NumberRangeSetting
    {
        public int Value { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        public List<string> Labels { get; set; }
    }
}