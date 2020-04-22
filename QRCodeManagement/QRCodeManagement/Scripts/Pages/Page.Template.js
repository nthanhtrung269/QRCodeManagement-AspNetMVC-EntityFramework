var Page = Page || {};

Page.Template = (function () {
    'use strict';

    var selectAfterReload = function (isSavePerformed, id) {
        if (isSavePerformed) {//after save
            $('#templateList').find('.template-item.active').click();
        } else {//after delete
            var hiddenValue = $('#templateId').val();
            if (hiddenValue === '0' || (typeof id !== 'undefined' && parseInt(hiddenValue) === id)) {
                $('#templateList').find('[data-template-id=0]').click();
                return;
            }
            $('.template-item').each(function() {
                if ($(this).attr('data-template-id') === hiddenValue) {
                    $(this).click();
                }
            });
        }
    };

    var remove = function (id) {
        var templateList = $('#templateList');
        $.ajax({
            type: 'GET',
            url: '/qrmanagement/deletetemplate/' + id,
            timeout: 5000,
            success: function (result) {
                if (result) {
                    templateList.html(result);
                    templateList.fadeIn(200);
                    selectAfterReload(false, id);
                } else {
                    App.Modal.showErrorModal('Cannot delete the template!');
                }
            }
        }).fail(function (textStatus, error) {
            App.Modal.showErrorModal('Cannot delete the template!');
            templateList.fadeIn(200);
            selectAfterReload();
        });
    };

    var save = function () {
        var obj = {};
        obj.foreground = $("#foreground").val();
        obj.background = $("#background").val();
        obj.embedLogo = $("#embedLogo").val();

        var jsonData = JSON.stringify(obj);
        var templateList = $('#templateList');

        $.ajax({
            url: '/qrmanagement/savetemplate/',
            type: 'POST',
            data: jsonData,
            success: function (result) {
                templateList.html(result);
                templateList.fadeIn(200);
                $('#btnSaveTemplate').removeClass('hidden');
                selectAfterReload(true);
            },
            error: function (errorText) {
                App.Modal.showErrorModal('Cannot save to template!');
                templateList.fadeIn(200);
            }
        });
    };

    return {
        remove: remove,
        save: save,
        selectAfterReload: selectAfterReload
    };

})();

