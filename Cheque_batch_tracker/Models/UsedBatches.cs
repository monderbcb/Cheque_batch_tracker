using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Cheque_batch_tracker.Models
{
    public class UsedBatches
    {
        public long ID { get; set; }
        [DisplayName("The Batch ID")]
        public int BatchID { get; set; }
        [DisplayName("The Permision ID - outside linked")]
        public long PermisionID { get; set; }
        [DisplayName("Cheque Number UQ")]
        [StringLength(14 , ErrorMessage = "Cheque Number Number must be 14 Number")]
        [RegularExpression(@"^[0-9]*$",
         ErrorMessage = "Only Numbers are allowed.")]
        public string? ChequeNumber { get; set; }
        [DisplayName("Amount - money decimal")]
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
        [DisplayName("Extra notes")]
        public string? Notes { get; set; }
        [DisplayName("Status 1-new 2-modfied 9-deleted")]
        public short Status { get; set; }
        public string? CreatedBy { get; set; } 
        public string? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public long CheckSum { get; set; }
        public virtual Batch? Batch { get; set; }
    }
}
