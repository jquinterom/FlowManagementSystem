using FlowManagement.Core.Entities;
using FlowManagement.Core.Interfaces;
using FlowManagement.Application.DTOs.Steps;
using FlowManagement.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowManagement.Infrastructure.Services
{
  public class StepService(IStepRepository stepRepository, ILogger<StepService> logger, IFieldRepository fieldRepository) : IStepService
  {
    private readonly IStepRepository _stepRepository = stepRepository;
    private readonly IFieldRepository _fieldRepository = fieldRepository;
    private readonly ILogger<StepService> _logger = logger;

    public async Task<Step> CreateStepAsync(CreateStepDto dto)
    {
      await ValidateFieldsExistAsync(dto.Inputs.Select(i => i.FieldCode));
      await ValidateFieldsExistAsync(dto.Outputs.Select(o => o.FieldCode));

      if (string.IsNullOrEmpty(dto.Code))
        throw new ArgumentException("The code is required");


      if (await _stepRepository.AnyAsync(s => s.Code == dto.Code))
        throw new InvalidOperationException("The code already exists");

      if (dto.Inputs.Count == 0 || dto.Outputs.Count == 0)
        throw new ArgumentException("Inputs/Outputs are required");

      var step = new Step
      {
        Id = Guid.NewGuid(),
        Code = dto.Code,
        Name = dto.Name,
        Description = dto.Description ?? string.Empty,
        Order = dto.Order,
        IsParallel = dto.IsParallel,
        ActionType = dto.ActionType,
        Inputs = [.. dto.Inputs.Select(i => new StepInput
        {
          FieldCode = i.FieldCode,
          IsRequired = i.IsRequired
        })],

        Outputs = [.. dto.Outputs.Select(o => new StepOutput
        {
          FieldCode = o.FieldCode
        })]
      };

      await _stepRepository.AddAsync(step);
      _logger.LogInformation($"Step created with id {step.Id}");

      return step;
    }

    public async Task<List<Step>> GetStepsAsync()
    {
      var steps = await _stepRepository.GetAllAsync();
      return [.. steps];
    }

    private async Task ValidateFieldsExistAsync(IEnumerable<string> fieldCodes)
    {
      foreach (var fieldCode in fieldCodes)
      {
        if (!await _fieldRepository.AnyAsync(f => f.Code == fieldCode))
          throw new KeyNotFoundException($"Field {fieldCode} does not exist");
      }
    }
  }
}