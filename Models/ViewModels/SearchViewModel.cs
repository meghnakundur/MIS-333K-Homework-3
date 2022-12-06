using System;
using System.ComponentModel.DataAnnotations;

namespace Kundur_Meghna_HW3.Models
{
    public enum StarOrder { greaterThan, lessThan }

    public class SearchViewModel
    {
       [Display(Name = "Search by Title:")]
       public String RepositoryName { get; set; }

       [Display(Name = "Search by Description:")]
       public String Description { get; set; }

       [Display(Name = "Search by Category:")]
       public Category? Category { get; set; }

       [Display(Name = "Search by Language")]
       public Int32 Language { get; set; }

       [Display(Name = "Search by Star Count")]
       public Decimal? StarCount { get; set; }

       public StarOrder? StarOrder { get; set; }

       [Display(Name = "Search by Last Updated Date:")]
       [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy}")]
       public DateTime? UpdatedAfter { get; set; }
    }
}

