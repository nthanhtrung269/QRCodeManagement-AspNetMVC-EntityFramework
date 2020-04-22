using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRCodeManagement.Constants
{
    public enum QrType
    {
        Url = 0,
        VCard = 1,
        File = 2
    }

    public enum QrFileType
    {
        Image,
        PDF
    }
}