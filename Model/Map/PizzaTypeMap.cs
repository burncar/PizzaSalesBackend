using CsvHelper.Configuration;
using PizzaSalesBackend.Model.Entities;

namespace PizzaSalesBackend.Model.Map
{
  
        public sealed class PizzaTypeMap : ClassMap<PizzaType>
        {
            public PizzaTypeMap()
            {
                Map(m => m.PizzaTypeId).Name("pizza_type_id");
                Map(m => m.Name).Name("name");
                Map(m => m.Category).Name("category");
                Map(m => m.Ingredients).Name("ingredients");
            }
        }
  
}
