var Page = Page || {};

Page.ManageQR = (function () {
    'use strict';

    var archive = function (id) {
        $.ajax({
            type: 'GET',
            url: '/qrmanagement/archive/' + id,
            timeout: 10000,
            success: function (result) {
                if (result) {
                    $('#searchForm').submit();
                } else {
                    App.Modal.showErrorModal('Cannot archive the QR Code!');
                    $('#searchForm').submit();
                }
            }
        }).fail(function (textStatus, error) {
            App.Modal.showErrorModal('Cannot archive the QR Code!');
            $('#searchForm').submit();
        });
    };

    var restore = function (id) {
        $.ajax({
            type: 'GET',
            url: '/qrmanagement/restore/' + id,
            timeout: 10000,
            success: function (result) {
                if (result) {
                    $('#searchForm').submit();
                }
            }
        }).fail(function (textStatus, error) {
            App.Modal.showErrorModal('Cannot restore the QR Code!');
            $('#searchForm').submit();
        });
    };

    var remove = function (id) {
        $.ajax({
            type: 'GET',
            url: '/qrmanagement/delete/' + id,
            timeout: 10000,
            success: function (result) {
                if (result) {
                    $('#searchForm').submit();
                }
            }
        }).fail(function (textStatus, error) {
            App.Modal.showErrorModal('Cannot delete the QR Code!');
            $('#searchForm').submit();
        });
    };

    var init = function () {
        $(document).on('change', '#userId', function () {
            $('#searchForm').submit();
        });

        $(document).on('click', '.js-btn-archive', function () {
            var qrId = $(this).data('qr-id');
            confirm('Do you want to archive this QR code ?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    Page.ManageQR.archive(qrId);
                }
            });
        });

        $(document).on('click', '.js-btn-restore', function () {
            var qrId = $(this).data('qr-id');
            confirm('Do you want to restore this QR code ?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    Page.ManageQR.restore(qrId);
                }
            });
        });

        $(document).on('click', '.js-btn-delete', function () {
            var qrId = $(this).data('qr-id');
            confirm('Do you want to delete this QR code ?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    Page.ManageQR.remove(qrId);
                }
            });
        });
    }
    return {
        init: init,
        archive: archive,
        restore: restore,
        remove: remove
    };

})();

$(document).ready(function() {
    Page.ManageQR.init();
});
