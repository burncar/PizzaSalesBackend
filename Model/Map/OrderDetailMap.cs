using CsvHelper.Configuration;
using PizzaSalesBackend.Model.Entities;

namespace PizzaSalesBackend.Model.Map
{
    public sealed class OrderDetailMap : ClassMap<OrderDetail>
    {
        public OrderDetailMap()
        {
            Map(m => m.PizzaId).Name("pizza_id");
            Map(m => m.OrderDetailsId).Name("order_details_id");
            Map(m => m.OrderId).Name("order_id");
            Map(m => m.Quantity).Name("quantity");
        }
    }
}
