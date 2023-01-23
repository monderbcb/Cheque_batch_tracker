using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace Cheque_batch_tracker.Models
{
    public class Batch
    {
        public int ID { get; set; }
        [Range(10, 2000000000000000 , ErrorMessage = "Batch start is below minmum value")]
        public long BatchStart { get; set; }
        [Range(10, 2000000000000000, ErrorMessage = "Batch end is below minmum value")]
        public long BatchEnd { get; set; }
        public string? AccountNumber { get; set; }
        public string? Notes { get; set; }
        public long? TotalUsedFunds { get; set; }
        [Range(1, 4, ErrorMessage = "Type value is not in range")]
        public short BatchType { get; set; }
        public short Status { get; set; }
        public string? CreatedBy { get; set; } 
        public string? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public long CheckSum { get; set; }
        public virtual ICollection<UsedBatches>? UsedBatches { get; set; }

    }
}
