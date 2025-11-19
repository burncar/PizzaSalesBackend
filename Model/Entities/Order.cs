using System.ComponentModel.DataAnnotations;

namespace PizzaSalesBackend.Model.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        //public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
