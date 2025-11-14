using PizzaSalesBackend.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaSalesBackend.Model.Dto
{
    public class PizzaDto 
    {
        public string PizzaTypeId { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
    }
}
