var Page = Page || {};

Page.Logo = (function () {
    'use strict';

    var selectAfterReload = function (isSavePerformed, id) {
        if (isSavePerformed) {//after save
            $('#logoList').find('.logo-item.active').click();
        } else {//after delete
            var hiddenValue = $('#embedLogoId').val();
            if (typeof id !== 'undefined' && parseInt(hiddenValue) === id) {
                $('#logoList').find('[data-logo-id=0]').click();
                return;
            }
            if (hiddenValue === '0' || hiddenValue === '-1') {
                $('#logoList').find('[data-logo-id=' + hiddenValue + ']').click();
                return;
            }

            $('.logo-item').each(function () {
                if ($(this).attr('data-logo-id') === hiddenValue) {
                    $(this).click();
                }
            });
        }
    };

    var init = function () {
        var logoList = $("#logoList"),
            progressBar = $('#progressBar');

        $('#fileLogoUpload').fileupload({
            add: function (e, data) {
                var uploadErrors = [];
                var acceptFileTypes = /^image\/(gif|jpe?g|png)$/i;
                if (data.originalFiles[0]['type'].length && !acceptFileTypes.test(data.originalFiles[0]['type'])) {
                    uploadErrors.push('Please select an image file');
                }
                if (data.originalFiles[0]['size'] > 2*1024*1024) {//Only 2Mb
                    uploadErrors.push('You should upload an image has file size is smaller than 2MB ');
                }
                if (uploadErrors.length > 0) {
                    App.Modal.showErrorModal(uploadErrors.join("\n"));
                } else {
                    data.submit();
                }
            },
            url: '/qrmanagement/uploadlogo/',
            autoUpload: true,
            done: function (e, data) {
                if (data) {
                    setTimeout(function () {
                        logoList.html(data.result);
                        progressBar.addClass('hidden');
                        logoList.fadeIn(200);
                        selectAfterReload(true);
                    }, 500);
                }
            }
        }).on('fileuploadsubmit', function (e, data) {
            $('.progress .progress-bar').css('width', '0%');
            progressBar.removeClass('hidden');
            logoList.fadeOut(20);

        }).on('fileuploadprogressall', function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('.progress .progress-bar').css('width', progress + '%');

        });
    };

    var remove = function (id) {
        var logoList = $('#logoList');
        $.ajax({
            type: 'GET',
            url: '/qrmanagement/deletelogo/' + id,
            timeout: 5000,
            success: function (result) {
                if (result) {
                    logoList.html(result);
                    logoList.fadeIn(200);
                    selectAfterReload(false, id);
                } else {
                    App.Modal.showErrorModal('Opp ! Maybe an error occur!');
                    logoList.fadeIn(200);
                }
            }
        }).fail(function (textStatus, error) {
            App.Modal.showErrorModal('Opp ! Maybe an error occur!');
            logoList.fadeIn(200);
            selectAfterReload();
        });
    };

    return {
        init: init,
        remove: remove,
        selectAfterReload: selectAfterReload
    };

})();
$(document).ready(function () {
    Page.Logo.init();
});
