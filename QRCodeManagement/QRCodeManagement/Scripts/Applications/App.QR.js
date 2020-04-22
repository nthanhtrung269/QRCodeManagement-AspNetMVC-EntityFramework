var App = App || {};
App.QR = App.QR || {};

App.QR = (function () {
    'use strict';

    var render = function () {
        var bgValue = $('#background').val(),
            fgValue = $('#foreground').val();

        if (bgValue) {
            bgValue = bgValue.replace('#', '%23');
        }

        if (fgValue) {
            fgValue = fgValue.replace('#', '%23');
        }

        var handlerUrl = '/webhandler/getcode.ashx'
            + '?c=' + $('#statisticsUrl').val()
            + '&b=' + bgValue
            + '&f=' + fgValue
            + '&l=' + $('#embedLogo').val();

            $('#imgPreview').attr('src', handlerUrl);
            $('#imgPreview').css('display', 'block');
            $('#imgNoAvailable').css('display', 'none');
    }

    return {
        render: render
    };

})();
