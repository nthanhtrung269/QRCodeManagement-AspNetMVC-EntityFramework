var App = App || {};

App.Modal = (function () {
    'use strict';

    var showConfirmModal = function (msg, yesActionCallback) {
        BootstrapDialog.confirm({
            title: 'Confirm',
            message: msg,
            type: BootstrapDialog.TYPE_SUCCESS, // <-- Default value is BootstrapDialog.TYPE_PRIMARY
            closable: true, // <-- Default value is false
            draggable: true, // <-- Default value is false
            btnCancelLabel: 'No', // <-- Default value is 'No',
            btnOKLabel: 'Yes', // <-- Default value is 'OK',
            btnOKClass: 'btn-success', // <-- If you didn't specify it, dialog type will be used,
            callback: function (result) {
                // result will be true if button was click, while it will be false if users close the dialog directly.
                if (result) {
                    yesActionCallback();
                }
            }
        });
    };

    var showErrorModal = function (msg) {
        BootstrapDialog.alert({
            title: 'Error',
            message: msg,
            type: BootstrapDialog.TYPE_DANGER, 
            closable: true, // <-- Default value is false
            draggable: true, // <-- Default value is false
            btnOKClass: 'btn-error' // <-- If you didn't specify it, dialog type will be used,
        });
    };

    var showFormModal = function(title, page) {
        BootstrapDialog.show({
            title: title,
            type: BootstrapDialog.TYPE_SUCCESS,
            message: function (dialog) {
                var $message = $('<div></div>');
                var pageToLoad = dialog.getData('pageToLoad');
                $message.load(pageToLoad);

                return $message;
            },
            data: {
                'pageToLoad': page
            }
        });
    }

    return {
        showConfirmModal: showConfirmModal,
        showErrorModal: showErrorModal,
        showFormModal: showFormModal
    };

})();
