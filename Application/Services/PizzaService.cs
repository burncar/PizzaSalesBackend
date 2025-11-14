using PizzaSalesBackend.Application.Intreface;
using PizzaSalesBackend.Model.Dto;
using PizzaSalesBackend.Model.Entities;
using PizzaSalesBackend.Model.Map;
using PizzaSalesBackend.Models.Response;

namespace PizzaSalesBackend.Application.Services
{
    public class PizzaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICsvHelper _csvHelper;
        public PizzaService(IUnitOfWork unitOfWork, ICsvHelper csvHelper)
        {
            _unitOfWork = unitOfWork;
            _csvHelper = csvHelper;

        }

        public async Task<ApiResponse> GetAllPizzas(string SearchString = "", int PageNumber = 1, int PageSize = 20)
        {
            var data = await _unitOfWork.Pizzas.GetAll(SearchString, PageNumber, PageSize);
            return data;
        }
        public async Task<ApiResponse> GetPizzaById(string Id)
        {
            var data = await _unitOfWork.Pizzas.GetById(Id);
            if (data == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { "Pizza not found." }
                };
            }
            return new ApiResponse
            {
                IsSuccess = true,
                Result = data,
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public  async Task<ApiResponse> AddPizza(PizzaDtoAdd pizza)
        {
            try
            {
                var dataToSend = new Pizza
                {
                    PizzaId = pizza.PizzaId,
                    PizzaTypeId = pizza.PizzaTypeId,
                    Size = pizza.Size,
                    Price = pizza.Price,
                };
                var addedPizza = await _unitOfWork.Pizzas.Add(dataToSend);
                await _unitOfWork.Complete();
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = addedPizza,
                    StatusCode = System.Net.HttpStatusCode.Created,
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    ErrorMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<ApiResponse> AddMultiplePizza(string filePath)
        {
            try
            {

                if (!File.Exists(filePath))
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessages = new List<string> {"File path not exist"}
                    };
                }
                var pizzas = await _csvHelper.CsvImportAsync<Pizza, PizzaMap>(filePath);
                var dataToSendList = new List<Pizza>();
                foreach (var item in pizzas)
                {
                    var dataToSend = new Pizza
                    {
                        PizzaTypeId = item.PizzaTypeId,
                        Size = item.Size,
                        Price = item.Price,
                        PizzaId = item.PizzaId,
                    };
                    dataToSendList.Add(dataToSend);
                }
               
                var addedPizza = await _unitOfWork.Pizzas.AddRange(dataToSendList);
                await _unitOfWork.Complete();
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = addedPizza,
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
        public async Task<ApiResponse> UpdatePizza(string id, PizzaDto dto)
        {
            try
            {
                var fromDbPizza = await _unitOfWork.Pizzas.GetById(id);
                if (fromDbPizza != null)
                {
                    fromDbPizza.PizzaTypeId = dto.PizzaTypeId;
                    fromDbPizza.Size = dto.Size;
                    fromDbPizza.Price = dto.Price;
                    await _unitOfWork.Complete();
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        StatusCode = System.Net.HttpStatusCode.OK,
                    };
                }
            }
            catch(Exception ex)
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
        public async Task<ApiResponse> RemovePizza(string Id) { 
            var pizza = await _unitOfWork.Pizzas.GetById(Id);
            if (pizza == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { "Pizza not found." }
                };
            }
            var data = await _unitOfWork.Pizzas.Remove(pizza);

            var res = await _unitOfWork.Complete();
            if (!data)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "Pizza not found." }
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
