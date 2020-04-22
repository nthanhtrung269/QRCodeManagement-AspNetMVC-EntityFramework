var Page = Page || {};

Page.CreateQR = (function () {
    'use strict';

    var init = function () {
        App.QR.render();
        $('#foreground, #background').change(function() {
             App.QR.render();
        });
        $('#btnSaveTemplate').click(function () {
            $(this).addClass('hidden');
            $('#templateList').fadeOut(100);
            Page.Template.save();
        });

        $('#createOrEditQrCode').click(function() {
            $('#createOrEditQrCodeForm').submit();
        });

        $(document).on('click', '.js-logo-item', function () {
            $('.js-logo-item').removeClass('active');
            $(this).addClass('active');
            $('#embedLogo').val($(this).attr('data-logo-file-name'));
            $('#embedLogoId').val($(this).attr('data-logo-id'));
             App.QR.render();
            console.log('templateId: ' + $('#embedLogoId').val());
            console.log('embedLogo: ' + $('#embedLogo').val());
            console.log($(this).attr('data-logo-id') + ' was clicked');
        });

        $(document).on('click', '.js-template-item', function () {
            $('.js-template-item').removeClass('active');
            $(this).addClass('active');
            // set color to color text field
            $('#background').minicolors('value', $(this).attr('data-bg-color'));
            $('#foreground').minicolors('value', $(this).attr('data-fg-color'));

            // set logo value to hidden field
            $('#embedLogo').val($(this).attr('data-logo-file-name'));
            $('#embedLogoId').val($(this).attr('data-logo-id'));

            // get photo has logo-file-name match tempalte logo file-name
            var logoFileNameAttr = $(this).attr('data-logo-file-name');
            var logoItems = $('#logoList').find("[data-logo-file-name='" + logoFileNameAttr +"']");
            if (logoItems.length) {
                logoItems[0].click();
            }

            // set template hidden field value
            $('#templateId').val($(this).attr('data-template-id'));
            console.log('templateId: ' + $('#templateId').val());
            console.log($(this).attr('data-template-id') + ' was clicked');

            // render QR image
             App.QR.render();
            
        });

        $(document).on('click', '.js-template-remove', function() {
            var templateId = $(this).data('template-id');
            confirm('Do you want to remove this template ?','Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    $('#templateList').fadeOut(100);
                    Page.Template.remove(templateId);
                }
            });
        });

        $(document).on('click', '.js-logo-remove', function () {
            var logoId = $(this).data('logo-id');
            confirm('Do you want to remove this logo ?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    $('#logoList').fadeOut(100);
                    Page.Logo.remove(logoId);
                }
            });
        });
    };

    return {
        init: init
 };

})();
$(document).ready(function () {
    Page.CreateQR.init();
});

