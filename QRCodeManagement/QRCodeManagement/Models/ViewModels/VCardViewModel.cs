using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QRCodeManagement.Common;

namespace QRCodeManagement.Models.ViewModels
{
    public class VCardViewModel : QrCodeModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        public string JobTitle { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string MobileNumber { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string About { get; set; }

        public string ProfileImage { get; set; }

        public string OldProfileImage { get; set; }

        public string HeaderColor { get; set; }

        public string ButtonColor { get; set; }
    }
}
