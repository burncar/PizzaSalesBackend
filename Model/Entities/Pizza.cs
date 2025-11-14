using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaSalesBackend.Model.Entities
{
    public class Pizza
    {
        [Key]
        public string PizzaId { get; set; }
        public string PizzaTypeId{ get; set; }
        [ForeignKey(nameof(PizzaTypeId))]
        public PizzaType? PizzaType { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
    }
}
