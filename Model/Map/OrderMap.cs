using CsvHelper.Configuration;
using PizzaSalesBackend.Helper;
using PizzaSalesBackend.Model.Entities;

namespace PizzaSalesBackend.Model.Map
{
    public sealed class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Map(m => m.OrderId).Name("order_id");
            Map(m => m.Date).Name("date").TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.Time).Name("time").TypeConverter<FlexibleTimeSpanConverter>();

        }
    }
}
