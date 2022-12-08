using Data.Entities;
using FinalAssignment.DTOs.Request;

namespace FinalAssignment.Services.Interfaces
{
    public interface IRequestReturningService
    {
        Task<CreateRequestReturningResponse> CreateRequestForReturning(CreateRequestReturningRequest requestReturning);
        Task<IEnumerable<ReturningRequest>> GetAllReturningRequest();


    }
}