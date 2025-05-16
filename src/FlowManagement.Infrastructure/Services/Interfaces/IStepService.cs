using FlowManagement.Core.Entities;
using FlowManagement.Core.Models;

namespace FlowManagement.Infrastructure.Services.Interfaces
{
  public interface IStepService
  {
    Task<List<Step>> GetStepsAsync();
    Task<Step> CreateStepAsync(CreateStepDto step);
  }
}