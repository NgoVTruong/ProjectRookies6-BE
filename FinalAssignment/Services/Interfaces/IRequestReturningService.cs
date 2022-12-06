using FinalAssignment.DTOs.Request;

namespace FinalAssignment.Services.Interfaces
{
    public interface IRequestReturningService
    {
        Task<CreateRequestReturningResponse> CreateRequestForReturning(CreateRequestReturningRequest requestReturning);
    }
}