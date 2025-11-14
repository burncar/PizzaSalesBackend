using PizzaSalesBackend.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaSalesBackend.Model.Dto
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public string PizzaId { get; set; }
        public int Quantity { get; set; }
    }
}
