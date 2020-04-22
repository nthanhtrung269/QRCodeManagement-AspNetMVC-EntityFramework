var Page = Page || {};

Page.UserManager = (function () {
    'use strict';

    var blockUnblockAccount = function(userId, isBlock) {
        $.ajax({
            url: '/manageuser/blockunblockaccount/' + userId + '?isBlock=' + isBlock,
            type: 'POST',
            success: function (result) {
                if (result) {
                    $('#searchUserForm').submit();
                }
            },
            error: function (errorText) {
                App.Modal.showErrorModal('Has problem, please try again!');
            }
        });
    }

    var replaceBlockIcon = function (isBlock) {
        $('#block_icon').removeAttr('class');
        $('#block_icon').addClass(isBlock ? 'fa fa-unlock icon-block' : 'fa fa-lock icon-block');
        $('#changeBlock').attr('data-blocked', !isBlock);
    }

    var blockUnblockAccountOnEditPage = function (userId, isBlock) {
        $.ajax({
            url: '/manageuser/blockunblockaccount/' + userId + '?isBlock=' + isBlock,
            type: 'POST',
            success: function (result) {
                if (result) {
                    replaceBlockIcon(!isBlock);
                }
            },
            error: function (errorText) {
                App.Modal.showErrorModal('Has problem, please try again!');
            }
        });
    }

    var resendConfirmEmail = function (id) {
        $.ajax({
            url: '/manageuser/ResendComfirmationEmail/' + id,
            type: 'POST',
            success: function (result) {
                if (result) {
                    $('#editResult').html('An email was sent successful');
                }
            },
            error: function (errorText) {
                App.Modal.showErrorModal('Has problem, please try again!');
            }
        });
    }

    var resetPassword = function (id) {
        $.ajax({
            url: '/manageuser/resetpassword/' + id,
            type: 'POST',
            success: function (result) {
                if (result) {
                    $('#editResult').html('New password is : "<b>' + result + '</b>". An email contains new password was sent to user');
                }
            },
            error: function (errorText) {
                App.Modal.showErrorModal('Has problem, please try again!');
            }
        });
    }

    var init = function() {
        $(document).on('change', '#isBlock, #isConfirm', function () {
            $('#searchUserForm').submit();
        });

        $(document).on('click', '.js-unblock-icon', function () {
            var userId = $(this).attr('data-user-id');
            confirm('Are you sure you want to block this account?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    blockUnblockAccount(userId, true);
                }
            });
        });

        $(document).on('click', '.js-blocked-icon', function () {
            var userId = $(this).attr('data-user-id');
            confirm('Are you sure you want to unblock this account?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    blockUnblockAccount(userId, false);
                }
            });
        });

        $(document).on('click', '#changeBlock', function () {
            $('#editResult').html('');
            var userId = $(this).attr('data-user-id');
            var isBlock = $(this).attr('data-blocked').toLowerCase() === 'true';
            confirm('Are you sure you want to ' + (isBlock ? 'unblock' : 'block') + ' this account?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    blockUnblockAccountOnEditPage(userId, !isBlock);
                }
            });
        });

        $(document).on('click', '#resendConfirm', function () {
            $('#editResult').html('');
            var userId = $(this).attr('data-user-id');
            confirm('Are you sure you want to resend confirmation email for this account?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    resendConfirmEmail(userId);
                }
            });
        });

        $(document).on('click', '#resetPassword', function () {
            $('#editResult').html('');
            var userId = $(this).attr('data-user-id');
            confirm('Are you sure you want to reset password of this account?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    resetPassword(userId);
                }
            });
        });

        $(document).on('click', '#deleteAccount', function () {
            confirm('Are you sure you want to delete this account?', 'Confirm', function (result) {
                // cancel: result=false, ok: result=true
                if (result) {
                    $('#editAccount').submit();
                }
            });
        });
    };


    return {
        init: init
    };

})();
$(document).ready(function () {
    Page.UserManager.init();
});
