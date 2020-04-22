using System;
using System.Collections.Generic;
using Webdiyer.WebControls.Mvc;

namespace QRCodeManagement.Models.ViewModels
{
    public class ChartViewModel
    {
        public QrCodeModel QrCodeModel { get; set; }

        public int TotalScans { get; set; }

        public int TotalScansLast48h { get; set; }

        public int TotalCities { get; set; }

        public Dictionary<DateTime, int> ScanOverTime { get; set; }

        public List<ScanByOperationSystemViewModel> ScanByOperationSystemViewModels { get; set; }

        public List<ScanByTopCitiesViewModel> ScanByTopCities { get; set; }

        public PagedList<ChartRawDataViewModel> ChartRawDataViewModel { get; set; }
    }
}
