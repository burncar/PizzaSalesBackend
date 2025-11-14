

using PizzaSalesBackend.Application.Intreface;
using PizzaSalesBackend.Model.Dto;
using PizzaSalesBackend.Model.Entities;
using PizzaSalesBackend.Model.Map;
using PizzaSalesBackend.Models.Response;

namespace OrderSalesBackend.Application.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICsvHelper _csvHelper;
        public OrderService(IUnitOfWork unitOfWork, ICsvHelper csvHelper)
        {
            _unitOfWork = unitOfWork;
            _csvHelper = csvHelper;

        }

        public async Task<ApiResponse> GetAllOrders(
               DateTime FromDate = default,
            DateTime ToDate = default,
            int PageNumber = 1,
            int PageSize = 20)
        {
            var data = await _unitOfWork.Orders.GetAll(FromDate,ToDate, PageNumber, PageSize);
            return data;
        }
        public async Task<ApiResponse> GetOrderById(int Id)
        {
            var data = await _unitOfWork.Orders.GetById(Id);
            if (data == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { "Order not found." }
                };
            }
            return new ApiResponse
            {
                IsSuccess = true,
                Result = data,
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public async Task<ApiResponse> AddOrder(OrderDto order)
        {
            try
            {
                var dataToSend = new Order
                {
                    Date = order.Date,
                    Time = order.Time,
                };
                var addedOrder = await _unitOfWork.Orders.Add(dataToSend);
                await _unitOfWork.Complete();
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = addedOrder,
                    StatusCode = System.Net.HttpStatusCode.Created,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    ErrorMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<ApiResponse> AddMultipleOrder(string filePath)
        {
            try
            {

                if (!File.Exists(filePath))
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessages = new List<string> { "File path not exist" }
                    };
                }
                var pizzas = await _csvHelper.CsvImportAsync<Order, OrderMap>(filePath);
                var dataToSendList = new List<Order>();
                foreach (var item in pizzas)
                {
                    var dataToSend = new Order
                    {
                       //OrderId = item.OrderId,
                       Time = item.Time,
                       Date = item.Date,

                    };
                    dataToSendList.Add(dataToSend);
                }

                var addedOrder = await _unitOfWork.Orders.AddRange(dataToSendList);
                await _unitOfWork.Complete();
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = addedOrder,
                    StatusCode = System.Net.HttpStatusCode.Created,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    ErrorMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<ApiResponse> UpdateOrder(int id, OrderDto dto)
        {
            try
            {
                var fromDbOrder = await _unitOfWork.Orders.GetById(id);
                if (fromDbOrder != null)
                {
                    fromDbOrder.Date = dto.Date;
                    fromDbOrder.Time = dto.Time;
                    await _unitOfWork.Complete();
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        StatusCode = System.Net.HttpStatusCode.OK,
                    };
                }

            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    ErrorMessages = new List<string> { ex.Message }
                };
            }
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = System.Net.HttpStatusCode.InternalServerError,

            };

        }
        public async Task<ApiResponse> RemoveOrder(int Id)
        {
            var pizza = await _unitOfWork.Orders.GetById(Id);
            if (pizza == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { "Order not found." }
                };
            }
            var data = await _unitOfWork.Orders.Remove(pizza);

            var res = await _unitOfWork.Complete();
            if (!data)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "Order not found." }
                };
            }
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = pizza
            };
        }

    }
}
