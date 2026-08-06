using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Mapping;

public class FinanceProfile : Profile
{
    public FinanceProfile()
    {
        CreateMap<FinancaTransaction, FinancaTransactionDto>();
        CreateMap<FinancaTransactionCreateDto, FinancaTransaction>();
        CreateMap<FinancaTransactionUpdateDto, FinancaTransaction>();

        CreateMap<SavingGoal, SavingGoalDto>();
        CreateMap<SavingGoalCreateDto, SavingGoal>();
    }
}