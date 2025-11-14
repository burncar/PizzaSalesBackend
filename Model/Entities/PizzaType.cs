using System.ComponentModel.DataAnnotations;

namespace PizzaSalesBackend.Model.Entities
{
    public class PizzaType
    {
        [Key]
        public string PizzaTypeId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Ingredients { get; set; }
    }
}
