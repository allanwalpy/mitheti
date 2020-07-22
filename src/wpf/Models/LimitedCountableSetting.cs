namespace Mitheti.Wpf.Models
{
    public class LimitedCountableSetting<T>
    {
        public T Value { get; set; }
        public T Min { get; set; }
        public T Max { get; set; }
    }
}