using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementA3 : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    public string PeriodForUtilization { get; private set; }
    public int ItemNumber { get; private set; }
    
    public string GoodDescription { get; private set; }
    public Guid GoodOriginCountryId { get; private set; }
    public int GoodQuantity { get; private set; }
    
    public decimal AnnualBudget { get; private set; }
    public decimal BankShare { get; private set; }
    public decimal AdvanceRequested { get; private set; }
    
    public DateTime DateOfApproval { get; private set; }

    public Country? GoodOrginCountry { get; private set; }

    private DisbursementA3() { }

    public DisbursementA3(DisbursementA3NewParam param)
    {
        PeriodForUtilization = param.PeriodForUtilization;
        ItemNumber = param.ItemNumber;
        GoodDescription = param.GoodDescription;
        GoodOriginCountryId = param.GoodOrginCountryId;
        GoodQuantity = param.GoodQuantity;
        AnnualBudget = param.AnnualBudget;
        BankShare = param.BankShare;
        AdvanceRequested = param.AdvanceRequested;
        DateOfApproval = param.DateOfApproval;
        CreatedBy = "System";
        CreatedAt = DateTime.UtcNow;
    }

    public DisbursementA3(DisbursementA3LoadParam param)
    {
        Id = param.Id;
        DisbursementId = param.DisbursementId;
        PeriodForUtilization = param.PeriodForUtilization;
        ItemNumber = param.ItemNumber;
        GoodDescription = param.GoodDescription;
        GoodOriginCountryId = param.GoodOrginCountryId;
        GoodQuantity = param.GoodQuantity;
        AnnualBudget = param.AnnualBudget;
        BankShare = param.BankShare;
        AdvanceRequested = param.AdvanceRequested;
        DateOfApproval = param.DateOfApproval;
        CreatedAt = param.CreatedAt;
        CreatedBy = param.CreatedBy;
        UpdatedAt = param.UpdatedAt;
        UpdatedBy = param.UpdatedBy;
        GoodOrginCountry = param.GoodOrginCountry;
    }
}
