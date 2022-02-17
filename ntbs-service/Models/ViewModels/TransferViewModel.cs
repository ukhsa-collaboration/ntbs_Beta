using System;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.ViewModels;

public class TransferViewModel
{
    [BindProperty]
    [Display(Name = "Transfer date")]
    [Required(ErrorMessage = ValidationMessages.Mandatory)]
    [ValidDateRange(ValidDates.EarliestAllowedDate)]
    [AssertThat(nameof(TransferDateAfterNotificationStart), ErrorMessage = ValidationMessages.DateShouldBeLaterThanNotificationStart)]
    [AssertThat(nameof(TransferDateAfterLatestTransfer), ErrorMessage = ValidationMessages.DateShouldBeLaterThanLatestTransfer)]
    public DateTime? TransferDate { get; set; }
        
    public DateTime? NotificationStartDate { get; set; }
    public DateTime? LatestTransferDate { get; set; }

    public bool TransferDateAfterNotificationStart => TransferDate >= NotificationStartDate;
    public bool TransferDateAfterLatestTransfer => LatestTransferDate is null || TransferDate >= LatestTransferDate;
}
