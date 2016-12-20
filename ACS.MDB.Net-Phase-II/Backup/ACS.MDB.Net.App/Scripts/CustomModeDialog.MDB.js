
var spinnerVisible = false;
var emailId = '';

//script for change password modal dialog
function OpenChangePassword() {
    var changePasswordDlg = $('#changePasswordDialog').dialog({
        autoOpen: false,
        resizable: false,
        title: "Change Password",
        width: 420,
        height: 320,
        modal: true,
        open: function (event, ui) {
            $.ajaxSetup({ cache: false });
            //this is to avoid having scroll bars when the dialog opens
            //Note : Comment it to avoid scroll bars.
            $(this).load("/ChangePassword/ChangePasswordIndex");
            $('body').css('overflow', 'hidden'); $('.ui-widget-overlay').css('width', '100%');
        },
        close: function (event, ui) {
            //reset the style to original after dialog closes
            //Note : Comment it to avoid scroll bars.
            $('body').css('overflow', 'auto');

            //Close the dialog - this is also done to avoid caching
            $(this).dialog("close");
        },
        buttons: {
            'Ok': function () {
                //validate the form - the form is embedded within a div,
                //use this dialog div to select the form within it.

                var password = $("#txtPassword").val();
                var newPassword = $("#txtNewPassword").val();
                var confirmPassword = $("#txtConfirmPassword").val();

                var $form = $('#changePasswordDialog form');
                if ($form.valid()) {
                    $.ajax({
                        url: '/ChangePassword/ChangePasswordAfterLogin?oldPassword=' + password + '&newPassword=' + newPassword + '&confirmPassword=' + confirmPassword + '',
                        data: $form.serialize(),
                        type: 'POST',
                        dataType: 'text',
                        success: function () {
                            alert("Password has been changed successfully");
                            changePasswordDlg.dialog('close');
                        },
                        error: function (response, status, error) {
                            //alert(response.statusText);
                            ShowErrorMessage(response, true);
                        }
                    });
                }
            },

            'Close': function () {
                changePasswordDlg.dialog('close');
            }
        }
    });

    $('#changePasswordDialog').dialog('open');
};



//script for About Us modal dialog
function OpenAboutUs() {
    $.ajax({
        url: '/Home/About',
        //data: $form.serialize(),
        type: 'POST',
        dataType: 'text',
        success: function () {
            var aboutUsDlg = $('#aboutUsDialog').dialog({
                autoOpen: false,
                resizable: false,
                title: "About Us",
                width: 425,
                height: 420,
                modal: true,
                open: function (event, ui) {
                    $.ajaxSetup({ cache: false });
                    //this is to avoid having scroll bars when the dialog opens
                    //Note : Comment it to avoid scroll bars.
                    $(this).load("/Home/About");
                    $('body').css('overflow', 'hidden'); $('.ui-widget-overlay').css('width', '100%');
                },
                buttons: {
                    'Ok': function () {
                        aboutUsDlg.dialog('close');
                    },
                }
            });
            $('#aboutUsDialog').dialog('open');
        },
        error: function (response, status, error) {
            alert(response.statusText);
        }
    });
}


    //script for Contact Us modal dialog
    function OpenContactUs() {
        var contactUsDlg = $('#contactUsDialog').dialog({
            autoOpen: false,
            resizable: false,
            title: "Contact Us",
            width: 450,
            height: 425,
            modal: true,
            open: function (event, ui) {
                $.ajaxSetup({ cache: false });
                //this is to avoid having scroll bars when the dialog opens
                //Note : Comment it to avoid scroll bars.
                $(this).load("/Home/Contact");
                $('body').css('overflow', 'hidden'); $('.ui-widget-overlay').css('width', '100%');
            },
            buttons: {
                'Ok': function () {
                    contactUsDlg.dialog('close');
                },
            }
        });
        $('#contactUsDialog').dialog('open');
    };

    //script for About Us modal dialog
    function OpenForgotPassword() {
        var emailId = $("#txtEmailId").val();
        var detailsDlg = $('#forgotPassword').dialog({
            autoOpen: false,
            resizable: false,
            title: "Forgot Password",
            width: 450,
            height: 190,
            modal: true,
            open: function (event, ui) {
                $.ajaxSetup({ cache: false });
                $(".ui-dialog-titlebar-close").hide();
                //this is to avoid having scroll bars when the dialog opens
                //Note : Comment it to avoid scroll bars.
                $(this).load("/ForgotPassword/ForgotPasswordIndex?email=" + emailId);
                $('body').css('overflow', 'hidden'); $('.ui-widget-overlay').css('width', '100%');
            },
            close: function (event, ui) {
                //reset the style to original after dialog closes
                //Note : Comment it to avoid scroll bars.
                $('body').css('overflow', 'auto');

                //Close the dialog - this is also done to avoid caching
                $(this).dialog("close");
            },
            buttons: {
                'Ok': function () {
                    //validate the form - the form is embedded within a div,
                    //use this dialog div to select the form within it.

                    var $form = $('#forgotPassword form');

                    if ($form.valid()) {
                        //ShowProgressAnimation();
                        ShowProgress("spinner");
                        var email = $("#txtEmailIdForForgotPassword").val();

                        $.ajax({
                            url: '/ForgotPassword/ResetPassword?email=' + email,
                            data: $form.serialize(),
                            type: 'POST',
                            dataType: 'text',
                            success: function () {
                                HideProgress("spinner");
                                alert("Your temporary password has been sent to your registered Email id.");
                                detailsDlg.dialog('close');
                            },
                            error: function (response, status, error) {
                                HideProgress("spinner");
                                alert(response.statusText);
                            }
                        });
                    }
                },

                'Close': function () {
                    detailsDlg.dialog('close');
                }
            }
        });
        $('#forgotPassword').dialog('open');
    };

    function OpenChangePasswordFromLoginScreen() {
        var changePasswordDlg = $('#changePasswordDialog').dialog({
            autoOpen: false,
            resizable: false,
            title: "Change Password",
            width: 420,
            height: 320,
            modal: true,
            open: function (event, ui) {
                $.ajaxSetup({ cache: false });
                //this is to avoid having scroll bars when the dialog opens
                //Note : Comment it to avoid scroll bars.
                $(this).load("/ChangePassword/ChangePasswordIndex");
                $('body').css('overflow', 'hidden'); $('.ui-widget-overlay').css('width', '100%');
            },
            close: function (event, ui) {
                //reset the style to original after dialog closes
                //Note : Comment it to avoid scroll bars.
                $('body').css('overflow', 'auto');

                //Close the dialog - this is also done to avoid caching
                $(this).dialog("close");
            },
            buttons: {
                'Ok': function () {
                    //validate the form - the form is embedded within a div,
                    //use this dialog div to select the form within it.

                    var password = $("#txtPassword").val();
                    var newPassword = $("#txtNewPassword").val();
                    var confirmPassword = $("#txtConfirmPassword").val();

                    var $form = $('#changePasswordDialog form');
                    if ($form.valid()) {
                        $.ajax({
                            url: '/ChangePassword/FirstLogin?email=' + emailId + '&oldPassword=' + password + '&newPassword=' + newPassword + '&confirmPassword=' + confirmPassword + '',
                            data: $form.serialize(),
                            type: 'POST',
                            dataType: 'text',
                            success: function () {
                                alert("Password has been changed successfully");
                                window.location.href = "/Home/Index";
                                changePasswordDlg.dialog('close');
                            },
                            error: function (response, status, error) {
                                alert(response.statusText);
                            }
                        });
                    }
                },

                'Close': function () {
                    changePasswordDlg.dialog('close');
                }
            }
        });
        $('#changePasswordDialog').dialog('open');
    };

    //Show progress image
    function ShowProgress(control) {
        if (!spinnerVisible) {
            $("div#" + control).fadeIn("fast");
            //$("div#spinner").fadeIn("fast");
            spinnerVisible = true;
        }
    };

    //Hide progress image
    function HideProgress(control) {
        if (spinnerVisible) {
            //var spinner = $("div#spinner");
            var spinner = $("div#" + control);
            spinner.stop();
            spinner.fadeOut("fast");
            spinnerVisible = false;
        }
    };

    //Function show error messages. Also to redirect to login page. If session is timeout During Save, Delete operation.
    function ShowErrorMessage(response, showErrorText) {
        //Redirect to Login page if page is forbidden. (e.g when session timeout)
        if (response.status == 403) {
            window.location.href = "/Login/Logout";
        }
        else {
            if (showErrorText)
            { alert('Error: ' + response.statusText); }
            else { alert(response.statusText); }
        }
    }

    ///Function to open customized model dialog
    function OpenDialog(title, width, height, RedirectOnClose) {
        $("#" + title).dialog({
            modal: true,
            resizable: false,
            closeOnEscape: false,
            width: width,
            height: height,
            open: function (event, ui) {
                $('body').css('overflow', 'hidden'); $('.ui-widget-overlay').css('width', '100%');
                //$('.ui-dialog-titlebar-close').css('display', 'none');
            },
            close: function (event, ui) {
                $('body').css('overflow', 'auto');
            },
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                    RedirectOnClose;
                    //RedirectToContractDetails(entityObj);
                }
            },
        });
    }
