using AutoMapper;
using CashFlowApp.Application.DTOs;
using CashFlowApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashFlowApp.Infrastructure.Mappings
{
    public class TransactionMapping : Profile
    {
        public TransactionMapping()
        {
            CreateMap<Transaction, TransactionDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap();

            CreateMap<DailyBalance, DailyBalanceDTO>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.TotalCredits, opt => opt.MapFrom(src => src.TotalCredit))
                .ForMember(dest => dest.TotalDebits, opt => opt.MapFrom(src => src.TotalDebit))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance.Value));
        }
    }
}
