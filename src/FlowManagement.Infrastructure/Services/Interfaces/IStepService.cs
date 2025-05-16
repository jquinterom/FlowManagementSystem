using FlowManagement.Application.DTOs.Steps;
using FlowManagement.Core.Entities;

namespace FlowManagement.Infrastructure.Services.Interfaces
{
  public interface IStepService
  {
    Task<List<Step>> GetStepsAsync();
    Task<Step> CreateStepAsync(CreateStepDto step);
  }
}