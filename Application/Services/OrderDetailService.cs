

using PizzaSalesBackend.Application.Intreface;
using PizzaSalesBackend.Model.Dto;
using PizzaSalesBackend.Model.Entities;
using PizzaSalesBackend.Model.Map;
using PizzaSalesBackend.Models.Response;

namespace OrderDetailSalesBackend.Application.Services
{
    public class OrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICsvHelper _csvHelper;
        public OrderDetailService(IUnitOfWork unitOfWork, ICsvHelper csvHelper)
        {
            _unitOfWork = unitOfWork;
            _csvHelper = csvHelper;

        }

        public async Task<ApiResponse> GetAllOrderDetails(
              string SearchString="",
            int PageNumber = 1,
            int PageSize = 20)
        {
            var data = await _unitOfWork.OrderDetails.GetAll(SearchString, PageNumber, PageSize);
            return data;
        }
        public async Task<ApiResponse> GetOrderDetailById(int Id)
        {
            var data = await _unitOfWork.OrderDetails.GetById(Id);
            if (data == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { "OrderDetail not found." }
                };
            }
            return new ApiResponse
            {
                IsSuccess = true,
                Result = data,
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public async Task<ApiResponse> AddOrderDetail(OrderDetailDto order)
        {
            try
            {
                var dataToSend = new OrderDetail
                {
                    OrderId = order.OrderId,
                    PizzaId = order.PizzaId,
                    Quantity = order.Quantity,
                };
                var addedOrderDetail = await _unitOfWork.OrderDetails.Add(dataToSend);
                await _unitOfWork.Complete();
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = addedOrderDetail,
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
        public async Task<ApiResponse> AddMultipleOrderDetail(string filePath)
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
                var pizzas = await _csvHelper.CsvImportAsync<OrderDetail, OrderDetailMap>(filePath);
                var dataToSendList = new List<OrderDetail>();
                foreach (var item in pizzas)
                {
                    var dataToSend = new OrderDetail
                    {
                    
                        OrderId = item.OrderId,
                        PizzaId = item.PizzaId,
                        Quantity = item.Quantity,

                    };
                    dataToSendList.Add(dataToSend);
                }

                var addedOrderDetail = await _unitOfWork.OrderDetails.AddRange(dataToSendList);
                await _unitOfWork.Complete();
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = addedOrderDetail,
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
        public async Task<ApiResponse> UpdateOrderDetail(int id, OrderDetailDto dto)
        {
            try
            {
                var fromDbOrderDetail = await _unitOfWork.OrderDetails.GetById(id);
                if (fromDbOrderDetail != null)
                {
                    fromDbOrderDetail.OrderId = dto.OrderId;
                    fromDbOrderDetail.PizzaId = dto.PizzaId;
                    fromDbOrderDetail.Quantity = dto.Quantity;
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
        public async Task<ApiResponse> RemoveOrderDetail(int Id)
        {
            var pizza = await _unitOfWork.OrderDetails.GetById(Id);
            if (pizza == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { "OrderDetail not found." }
                };
            }
            var data = await _unitOfWork.OrderDetails.Remove(pizza);

            var res = await _unitOfWork.Complete();
            if (!data)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "OrderDetail not found." }
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
