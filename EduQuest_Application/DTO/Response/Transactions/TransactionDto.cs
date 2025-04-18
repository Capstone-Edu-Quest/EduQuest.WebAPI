using AutoMapper;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;

namespace EduQuest_Application.DTO.Response.Transactions;

public class TransactionDto : IMapFrom<Transaction>, IMapTo<Transaction>
{
    public string TransactionId { get; set; }
    public string UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal? NetAmount { get; set; }
    public decimal? StripeFee { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerName { get; set; }
    public string? Url { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<TransactionDetailDto> Details { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id)) 
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.TransactionDetails));
    }
}

public class TransactionDetailDto : IMapFrom<TransactionDetail>, IMapTo<TransactionDetail>
{
    public string Id { get; set; }                
    public string TransactionId { get; set; }
    public string? InstructorId { get; set; }
    public string ItemType { get; set; }
    public string ItemId { get; set; }
    public decimal Amount { get; set; }
    public decimal? StripeFee { get; set; }
    public decimal? NetAmount { get; set; }
    public decimal? SystemShare { get; set; }
    public decimal? InstructorShare { get; set; }
    public string? TransferGoup { get; set; }

    public DateTime CreatedAt { get; set; }        
    public DateTime? UpdatedAt { get; set; }
}

