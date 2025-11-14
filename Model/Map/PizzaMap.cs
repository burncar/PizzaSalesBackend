using CsvHelper.Configuration;
using PizzaSalesBackend.Model.Entities;

namespace PizzaSalesBackend.Model.Map
{

    public sealed class PizzaMap : ClassMap<Pizza>
    {
        public PizzaMap()
        {
            Map(m => m.PizzaId).Name("pizza_id");
            Map(m => m.PizzaTypeId).Name("pizza_type_id");
            Map(m => m.Size).Name("size");
            Map(m => m.Price).Name("price");
        }
    }
}
