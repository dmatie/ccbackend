using Afdb.ClientConnection.Domain.Common;

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

    public Country? GoodOriginCountry { get; private set; }

    private DisbursementA3() { }

    public DisbursementA3(
        Guid disbursementId,
        string periodForUtilization,
        int itemNumber,
        string goodDescription,
        Guid goodOriginCountryId,
        int goodQuantity,
        decimal annualBudget,
        decimal bankShare,
        decimal advanceRequested,
        DateTime dateOfApproval)
    {
        DisbursementId = disbursementId;
        PeriodForUtilization = periodForUtilization;
        ItemNumber = itemNumber;
        GoodDescription = goodDescription;
        GoodOriginCountryId = goodOriginCountryId;
        GoodQuantity = goodQuantity;
        AnnualBudget = annualBudget;
        BankShare = bankShare;
        AdvanceRequested = advanceRequested;
        DateOfApproval = dateOfApproval;
        CreatedBy = "System";
        CreatedAt = DateTime.UtcNow;
    }

    public DisbursementA3(
        Guid id,
        Guid disbursementId,
        string periodForUtilization,
        int itemNumber,
        string goodDescription,
        Guid goodOriginCountryId,
        int goodQuantity,
        decimal annualBudget,
        decimal bankShare,
        decimal advanceRequested,
        DateTime dateOfApproval,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt = null,
        string? updatedBy = null,
        Country? goodOriginCountry = null)
    {
        Id = id;
        DisbursementId = disbursementId;
        PeriodForUtilization = periodForUtilization;
        ItemNumber = itemNumber;
        GoodDescription = goodDescription;
        GoodOriginCountryId = goodOriginCountryId;
        GoodQuantity = goodQuantity;
        AnnualBudget = annualBudget;
        BankShare = bankShare;
        AdvanceRequested = advanceRequested;
        DateOfApproval = dateOfApproval;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        GoodOriginCountry = goodOriginCountry;
    }
}
