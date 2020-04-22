var Page = Page || {};
Page.CreateQR = Page.CreateQR || {};

Page.CreateQR.CreateFile = (function () {
    'use strict';

    var inputFile = document.getElementById('fileUpload');

    var setFileUploadAccept = function() {
        var fileTypeOption = $("input[name='fileType']:checked"),
            fileUpload = $('#fileUpload');
        if (fileTypeOption) {
            if (fileTypeOption.val() === 'PDF') {
                fileUpload.attr('accept', 'application/pdf');
            } else {
                fileUpload.attr('accept', 'image/*');
            }
        }
        inputFile.value = '';
    };

    var verifyImage = function () {
        var acceptFileTypes = /^image\/(gif|jpe?g|png)$/i,
            file = inputFile.files[0];
        if (file['type'].length && !acceptFileTypes.test(file['type'])) {
            App.Modal.showErrorModal('Please select an image file');
            inputFile.value = '';
        }
        if (file['size'] > 5 * 1024 * 1024) { // 5MB
            App.Modal.showErrorModal('You should upload an image has file size is smaller than 5 MB');
            inputFile.value = '';
        }
    };

    var verifyPdf = function () {
        var acceptFileTypes = 'application/pdf',
            file = inputFile.files[0];
        if (file['type'].length && file['type'] !== acceptFileTypes) {
            App.Modal.showErrorModal('Please select an pdf file');
            inputFile.value = '';
        }
        if (file['size'] > 15 * 1024 * 1024) { // 15MB
            App.Modal.showErrorModal('You should upload a PDF has file size is smaller than 15 MB');
            inputFile.value = '';
        }
    };

    var init = function() {

        $("input[name='fileType']").change(function() {
            setFileUploadAccept();
        });

        $('#fileUpload').change(function () {
            var fileTypeOption = $("input[name='fileType']:checked");
            if (fileTypeOption) {
                if (fileTypeOption.val() === 'PDF') {
                    verifyPdf();
                } else {
                    verifyImage();
                }
            }
        });
};

    return {
        init: init
 };

})();
$(document).ready(function () {
    Page.CreateQR.CreateFile.init();
});

