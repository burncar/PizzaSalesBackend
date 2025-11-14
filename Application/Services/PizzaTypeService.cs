

using PizzaSalesBackend.Application.Intreface;
using PizzaSalesBackend.Model.Dto;
using PizzaSalesBackend.Model.Entities;
using PizzaSalesBackend.Model.Map;
using PizzaSalesBackend.Models.Response;

namespace PizzaTypeSalesBackend.Application.Services
{
    public class PizzaTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICsvHelper _csvHelper;
        public PizzaTypeService(IUnitOfWork unitOfWork, ICsvHelper csvHelper)
        {
            _unitOfWork = unitOfWork;
            _csvHelper = csvHelper;

        }

        public async Task<ApiResponse> GetAllPizzaTypes(string SearchString = "", int PageNumber = 1, int PageSize = 20)
        {
            var data = await _unitOfWork.PizzaTypes.GetAll(SearchString, PageNumber, PageSize);
            return data;
        }
        public async Task<ApiResponse> GetPizzaTypeById(string Id)
        {
            var data = await _unitOfWork.PizzaTypes.GetById(Id);
            if (data == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { "PizzaType not found." }
                };
            }
            return new ApiResponse
            {
                IsSuccess = true,
                Result = data,
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public async Task<ApiResponse> AddPizzaType(PizzaTypeDtoAdd pizza)
        {
            try
            {
                var dataToSend = new PizzaType
                {
                    PizzaTypeId = pizza.PizzaTypeId,
                    Name = pizza.Name,
                   Category = pizza.Category,
                   Ingredients = pizza.Ingredients,
                        
                };
                var addedPizzaType = await _unitOfWork.PizzaTypes.Add(dataToSend);
                await _unitOfWork.Complete();
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = addedPizzaType,
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
        public async Task<ApiResponse> AddMultiplePizzaType(string filePath)
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
                var pizzas = await _csvHelper.CsvImportAsync<PizzaType, PizzaTypeMap>(filePath);
                var dataToSendList = new List<PizzaType>();
                foreach (var item in pizzas)
                {
                    var dataToSend = new PizzaType
                    {
                        PizzaTypeId = item.PizzaTypeId,
                        Name = item.Name,
                        Category = item.Category,
                        Ingredients = item.Ingredients,

                    };
                    dataToSendList.Add(dataToSend);
                }

                var addedPizzaType = await _unitOfWork.PizzaTypes.AddRange(dataToSendList);
                await _unitOfWork.Complete();
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = addedPizzaType,
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
        public async Task<ApiResponse> UpdatePizzaType(string id, PizzaTypeDto dto)
        {
            try
            {
                var fromDbPizzaType = await _unitOfWork.PizzaTypes.GetById(id);
                if (fromDbPizzaType != null)
                {
                    fromDbPizzaType.Name = dto.Name;
                    fromDbPizzaType.Ingredients = dto.Ingredients;
                    fromDbPizzaType.Category = dto.Category;
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
        public async Task<ApiResponse> RemovePizzaType(string Id)
        {
            var pizza = await _unitOfWork.PizzaTypes.GetById(Id);
            if (pizza == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { "PizzaType not found." }
                };
            }
            var data = await _unitOfWork.PizzaTypes.Remove(pizza);

            var res = await _unitOfWork.Complete();
            if (!data)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "PizzaType not found." }
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
