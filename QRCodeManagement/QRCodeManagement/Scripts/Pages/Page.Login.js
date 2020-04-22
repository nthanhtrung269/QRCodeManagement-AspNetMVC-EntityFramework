var Page = Page || {};

Page.Login = (function () {
    'use strict';

    var init = function() {
        $(document).on('click', '#refreshCaptcha', function() {
            var image = document.getElementById('captchaImg');
            image.src = "/WebHandler/GetCaptcha.ashx?random=" + new Date().getTime();
        });
        
        $(document).on('click', '#forgotPwdLink', function() {
            $('#loginForm').addClass('hidden');
            $('#forgotPwdForm').removeClass('hidden');
        });
    };


    return {
        init: init
    };

})();
$(document).ready(function () {
    Page.Login.init();
});
