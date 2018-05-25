using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveSystem.Presentation.Models.ManageLeaveViewModels
{
    public class CreateLeaveViewModel: IValidatableObject
    {
        [Required]
        [Display(Name ="Reason")]
        [MaxLength(50)]
        public string Reason { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ToDate < FromDate)
            {
                yield return
                  new ValidationResult(errorMessage: "ToDate must be greater than FromDate",
                                       memberNames: new[] { "ToDate" });
            }
        }
    }
}
