using System;

namespace QRCodeManagement.Models.ViewModels
{
    public class ChartRawDataViewModel
    {
        public DateTime ScanDate { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Device { get; set; }

        public string OperationSystem { get; set; }
    }
}
