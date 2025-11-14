using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaSalesBackend.Model.Entities
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailsId { get; set; }
        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        [ForeignKey("pizza_id")]
        public string PizzaId { get; set; }
        public Order? Order { get; set; }
        public Pizza Pizza { get; set; }
        public int Quantity { get; set; }
    }
}
