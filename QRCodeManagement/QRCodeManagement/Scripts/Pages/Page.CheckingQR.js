var Page = Page || {};

Page.CheckingQR = (function () {
    'use strict';

    var md = new MobileDetect(window.navigator.userAgent),
        defaultUrl = 'http://www.test.com';

    var os = 'Other',
        device = 'Other',
        city = 'Unknow',
        country = 'Unknow';

    var saveScannerInfor = function (latt, logt, os, device, city, country) {
        var infor = {
            qrId: window.location.pathname.split('-')[1],
            longtitude: logt,
            lattitude: latt,
            os: os,
            device: device,
            city: city,
            country: country
        };

        $.ajax({
            url: '/checking/ProcessScannerInfor',
            type: 'POST',
            data: JSON.stringify(infor),
            cache: false,
            timeout: 10000,
            success: function (data) {
                if (data !== '') {
                    window.location = data;
                } else {
                    alert("Mã này không tồn tại trên hệ thống! (This code does not exist in system !)");
                    window.location = defaultUrl;
                }
            },
            error: function (error) {
                alert("Có lỗi trong quá trình quét mã, vui lòng thử lại! (Error occur ! Please try again)");
                window.location = defaultUrl;
            }
        });
    };

    var saveDefaultValue = function() {
        saveScannerInfor(0, 0, os, device, city, country);
    }

    var parsePlace = function (place) {
        var location = [];

        for (var i = 0; i < place.length; i++) {
            var components = place[i].address_components;

            for (var c = 0; c < components.length; c++) {
                switch (components[c].types[0]) {
                    case 'locality':
                        location['city'] = components[c].long_name.replace(' City', '');
                        break;
                    case 'country':
                        location['country'] = components[c].long_name;
                        break;
                }
                if (typeof location['city'] !== 'undefined' && typeof location['country'] !== 'undefined') {
                    return location;
                }
            }

        }
        return location;
    }

    var successGetLocationCallback = function (position) {
        debugger;
        var x = position.coords.latitude,
            y = position.coords.longitude,
            url = 'https://maps.googleapis.com/maps/api/geocode/json?latlng=' + x + ',' + y + '&sensor=true';

        $.getJSON(url).then(function (data) {
            var place = parsePlace(data.results);
            city = place['city'];
            country = place['country'];
            saveScannerInfor(x, y, os, device, city, country);
        }).fail(function (jqXHR, textStatus, error) {
            saveDefaultValue();
        });
    };

    var errorGetLoctionCallback = function (failure) {
        alert('An error occurs. ' + failure.message);
        saveScannerInfor(0, 0, os, device, 'Unknow', 'Unknow');
    };

    var detectGeoLocation = function () {
        console.log('Get from GEO location');
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(successGetLocationCallback, errorGetLoctionCallback);
        } else {
            saveDefaultValue();
        }
    }
    // end GEO
    // Get from ip
    var parseTimeZone = function (timeZone) {
        var city = 'Unknow';
        if (timeZone.indexOf('Asia/') > -1) {
            city = timeZone.replace('Asia/', '').replace(/\_/g, ' ');
        }
        return city;
    }
    var getCityNameFromIp = function () {
        console.log('Get from IP');
        var lat = '0',
           long = '0';

        var url = 'https://freegeoip.net/json/';

        $.getJSON(url).done(function (value) {
            if (value) {
                city = value.city ? value.city.replace(' City', '') : '';
                country = value.country_name;
                lat = value.latitude;
                long = value.longitude;

                if (city === '') { // Cannot check city or using 3G
                    city = parseTimeZone(value.time_zone);
                }

                if (city !== '' && country !== '') { // Only save data when city and country know
                    saveScannerInfor(lat, long, os, device, city, country);
                } else { // Call GEO location
                    detectGeoLocation();
                }
            }
        }).error(function (jqXHR, textStatus, error) {
            detectGeoLocation(); // Call GEO location
        });
    };

    var detectMobileDevice = function () {
        if (md.mobile()) {
            os = md.os();
            device = md.phone() ? md.phone() : md.tablet();
        }
    };

    var getLocationThenSaveData = function () {
        // Get Loction from ip first
        getCityNameFromIp();
    };

    var init = function () {
        detectMobileDevice();
        getLocationThenSaveData();
    };

    return {
        init: init
    };
})();

$(document).ready(function () {
    Page.CheckingQR.init();
});