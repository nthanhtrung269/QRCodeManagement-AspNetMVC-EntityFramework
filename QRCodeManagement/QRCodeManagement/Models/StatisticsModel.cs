using System;

namespace QRCodeManagement.Models
{
    public class StatisticsModel
    {
        public int Id { get; set; }
        public string QrId { get; set; }
        public string Device { get; set; }
        public string Os { get; set; }
        public string LongTitude { get; set; }
        public string LatTitude { get; set; }
        public DateTime? ScanDate => DateTime.Now;
        public string Country { get; set; }
        public string City { get; set; }

    }
}