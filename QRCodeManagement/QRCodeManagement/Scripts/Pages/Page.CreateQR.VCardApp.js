var Page = Page || {};
Page.CreateQR = Page.CreateQR || {};
Page.CreateQR.VCardApp = angular.module('Page.CreateQR.VCardApp', []);

var previewVCardController = Page.CreateQR.VCardApp.controller('Page.CreateQR.VCardApp.PreviewVCardController', ['$scope', '$http', function previewVCardController($scope, $http) {
    var isEdit = typeof(vCardModel) !== 'undefined';

    $scope.vCardModel = {
        firstName: isEdit ? vCardModel.firstName : '',
        lastName: isEdit ? vCardModel.lastName : '',
        company: isEdit ? vCardModel.company : '',
        jobTitle: isEdit ? vCardModel.jobTitle : '',
        street: isEdit ? vCardModel.street : '',
        city: isEdit ? vCardModel.city : '',
        zipCode: isEdit ? vCardModel.zipCode : '',
        country: isEdit ? vCardModel.country : '',
        state: isEdit ? vCardModel.state : '',
        mobileNumber: isEdit ? vCardModel.mobileNumber : '',
        phone: isEdit ? vCardModel.phone : '',
        fax: isEdit ? vCardModel.fax : '',
        email: isEdit ? vCardModel.email : '',
        website: isEdit ? vCardModel.website : '',
        about: isEdit ? vCardModel.about : '',
        profileImage: isEdit ? vCardModel.profileImage : '/images/logo.png',
        color1: isEdit ? vCardModel.color1 : '#455A64',
        color2: isEdit ? vCardModel.color2 : '#E91E63',
        themes: [
            {
                color1: '#455A64',
                color2: '#E91E63'
            },
            {
                color1: '#0288D1',
                color2: '#64B5F6'
            },
            {
                color1: '#D32F2F',
                color2: '#EF9A9A'
            },
            {
                color1: '#4CAF50',
                color2: '#81C784'
            },
            {
                color1: '#795548',
                color2: '#FF8A65'
            },
            {
                color1: '#42A48C',
                color2: '#E9B764'
            },
            {
                color1: '#F564AC',
                color2: '#36C17D'
            },
            {
                color1: '#FF8A65',
                color2: '#4B5D71'
            },
            {
                color1: '#7A1EA1',
                color2: '#1DE9B6'
            },
            {
                color1: '#3F51B5',
                color2: '#FF4081'
            }
        ],
        changeThemeColor: function () {
            var color = $('#headerColor').val() + $('#buttonColor').val();

            for (var i = 0; i < $scope.vCardModel.themes.length; i++) {
                $scope.vCardModel.themes[i].active = false;

                if ($scope.vCardModel.themes[i].color1 + $scope.vCardModel.themes[i].color2 === color) {
                    $scope.vCardModel.themes[i].active = true;
                }
            }
        },
        changeTheme: function (theme) {
            for (var i = 0; i < $scope.vCardModel.themes.length; i++) {
                $scope.vCardModel.themes[i].active = false;
            }

            theme.active = true;

            $('#headerColor').minicolors('value', theme.color1);
            $('#buttonColor').minicolors('value', theme.color2);
        },
        downloadLink: typeof (vCardModel) !== 'undefined' ? '/webhandler/downloadvcard.ashx?qrid=' + vCardModel.qrId : 'javascript:void(0);'
    };
}]);

Page.CreateQR.VCardImageUpload = Page.CreateQR.VCardImageUpload || {};
Page.CreateQR.VCardImageUpload = (function () {
    'use strict';

    var init = function () {
        var progressBar = $('#progressBar');

        $('#vCardImageUpload').fileupload({
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
            url: '/qrmanagement/uploadvcardimage',
            autoUpload: true,
            done: function (e, data) {
                if (data) {
                    setTimeout(function () {
                        if (data.result.success) {
                            // update angular model
                            var appElement = document.getElementById('vCardAppContainer');
                            var $scope = angular.element(appElement).scope();
                            $scope.$apply(function () {
                                $scope.vCardModel.profileImage = $('#vCardImageBaseLink').val() + data.result.value;
                                $('#profileImageContainer').removeClass('hidden');
                                $('#profileImagePreview').attr('src', $('#vCardImageBaseLink').val() + data.result.value);
                                $('#profileImage').val(data.result.value);
                            });
                        }

                        progressBar.addClass('hidden');
                    }, 500);
                }
            }
        }).on('fileuploadsubmit', function (e, data) {
            $('.progress .progress-bar').css('width', '0%');
            progressBar.removeClass('hidden');
        }).on('fileuploadprogressall', function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('.progress .progress-bar').css('width', progress + '%');
        });

        $('#profileImageRemove').click(function () {
            // update angular model
            var appElement = document.getElementById('vCardAppContainer');
            var $scope = angular.element(appElement).scope();

            if (typeof $scope !== 'undefined' && $scope !== null) {
                $scope.$apply(function() {
                    $scope.vCardModel.profileImage = '';
                    $('#profileImageContainer').addClass('hidden');
                    $('#profileImagePreview').attr('src', '');
                    $('#profileImage').val('');
                });
            }
        });
    };
    return {
        init: init
    };
})();

Page.CreateQR.Utilities = Page.CreateQR.Utilities || {};
Page.CreateQR.Utilities = (function () {
    'use strict';

    var checkColorsSelected = function () {
        var color = $('#headerColor').val() + $('#buttonColor').val();

        // update angular model
        var appElement = document.getElementById('vCardAppContainer');
        var $scope = angular.element(appElement).scope();
        if (typeof $scope !== 'undefined' && $scope !== null) {
            $scope.$apply(function() {
                for (var i = 0; i < $scope.vCardModel.themes.length; i++) {
                    if ($scope.vCardModel.themes[i].color1 + $scope.vCardModel.themes[i].color2 === color) {
                        $scope.vCardModel.themes[i].active = true;
                    }
                }
            });
        }
    };

    var showInformation = function() {
        if (document.getElementById('viewVcardTemplate')) {
            $('#viewVcardTemplate #fullname').removeClass('hidden');
            $('#viewVcardTemplate #jobTitle').removeClass('hidden');
            $('#viewVcardTemplate #about').removeClass('hidden');
            $('#viewVcardTemplate #mobileNumber').removeClass('hidden');
            $('#viewVcardTemplate #phone').removeClass('hidden');
            $('#viewVcardTemplate #fax').removeClass('hidden');
            $('#viewVcardTemplate #email').removeClass('hidden');
            $('#viewVcardTemplate #company').removeClass('hidden');
            $('#viewVcardTemplate #jobTitleSmall').removeClass('hidden');
            $('#viewVcardTemplate #street').removeClass('hidden');
            $('#viewVcardTemplate #country').removeClass('hidden');
            $('#viewVcardTemplate #website').removeClass('hidden');
        }

    }
    return {
        checkColorsSelected: checkColorsSelected,
        showInformation: showInformation
    };
})();

$(document).ready(function () {
    Page.CreateQR.VCardImageUpload.init();
    Page.CreateQR.Utilities.checkColorsSelected();
    Page.CreateQR.Utilities.showInformation();
});