using OrderDetailSalesBackend.Application.Repository;
using OrderSalesBackend.Application.Repository;
using PizzaSalesBackend.Application.Repository;
using PizzaTypeSalesBackend.Application.Repository;

namespace PizzaSalesBackend.Application.Intreface
{
    public interface IUnitOfWork
    {
        PizzaRepository Pizzas { get; }
        PizzaTypeRepository PizzaTypes { get; }
        OrderRepository Orders { get; }
        OrderDetailRepository OrderDetails { get; }
        ReportRepository Reports { get; }
        Task<int> Complete();
    }
}
