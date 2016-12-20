$(document).ready(function () {
    $('.menu').jqsimplemenu();
});

//Prevent back browser button
function preventBack() { window.history.forward(); }
setTimeout("preventBack()", 0);
window.onunload = function () { null };

//Set the menu item as selected
function HighlightSelectedMenuItem(menu) {
    if (document.getElementById(menu) != null) {
        document.getElementById(menu).className = "selectedmenuitem";
    }
}

//Disable enter key for control
//Purpose : When pressing enter,this is hitting save and edit/create popup shows in new page.
//Bug 1347 : For all pop up:: EDIT pop up:: while editing record, if we hit the "Enter" key then pop up gets maximized and then it doesnt allow to perform any action:: Reported by Jeevan
function DisableEnterKeyForControl(control) {
    var code = null;
    $(control).keypress(function (e) {
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) e.preventDefault();
        //e.preventDefault();
    });
}

//Disable Backspace key for control
//Purpose: When pressing backspace pop up get closed for read only textbox fields.
function DisableBackspaceKeyForControl(control) {
    var code = null;
    $(control).keydown(function (e) {
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 8) e.preventDefault();

    });
}

//Set focus on provided control 
function SetFocus(control) {
    $(control).focus();
}

//Disable invalid keys for control (Keys like <,>,~)
function DisableInvalidKeyForControl(control) {
    var code = null;
    $(control).keypress(function (e) {
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 60 || code == 62 || code == 126) e.preventDefault();
        //e.preventDefault();
    });
}

//Disable invalid keys for control (Keys like |)
function DisablePipeKeyForControl(control) {
    var code = null;
    $(control).keypress(function (e) {
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 124) e.preventDefault();
        //e.preventDefault();
    });
}

//Function to Add item into drop down
function AddOption(text, value, control) {
    var option = document.createElement("option");
    option.text = text;
    option.value = value;
    $(control).append(option);
    option.innerText = text;
}

//Function to count characters remaining in Billing line textbox(es) (For MaintenanceBillingLines and MileStoneBilligLines)
//var characters = 50;
function CountCharacter(control) {
    $("#" + control).focus(function () {
        CountRemaining(control);
    });

    $("#" + control).keyup(function () {
        CountRemaining(control);
    });

    $("#" + control).focusout(function () {
        $("#" + counter).html("You have <strong>" + 0 + "</strong> characters remaining").css("color", "blue");
    });
}

//Function to count remaining characters in textbox for billing line text
function CountRemaining(control) {
    if ($("#" + control).val().length > characters) {
        $("#" + control).val($("#" + control).val().substr(0, characters));
    }
    var remaining = characters - $("#" + control).val().length;
    //To restrict user to enter more than 48 characters in contractmaintenance billing linetext
    if (counter == "CounterContractmaintenance") {
        if (remaining > 102) {
            $("#" + counter).html("You have <strong>" + remaining + "</strong> characters remaining").css("color", "blue");
        }
        else {
            $("#" + counter).html("You have <strong>" + remaining + "</strong> characters remaining. <br \> It is recommended, not to enter more than 48 characters").css("color", "red");
        }
    }
    else {
        $("#" + counter).html("You have <strong>" + remaining + "</strong> characters remaining").css("color", "blue");
    }
}

//Function to display error message. Also redirect to login page, If session is timeout During Save, Delete operation.
function DisplayErrorMessage(response, showErrorText) {
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

//Function to provide hyperlink to open edit pop up on each row in datatable
function OpenEditDialog(tbl, iRow, nTd, sData) {

    //var b = $('<a href=' + "Gillians Product" + '>' + "Gillians Product" + '</a>');

    var b = $('<a href=' + '' + '>' + sData + '</a>');
    // var b = $('<button style="margin: 0">Edit</button>');

    b.on('click', function () {
        b.siblings('tr').addClass('row_selected');
        tbl.fnOpenEdit(iRow);
        //tbl.$('tr.row_selected td').addClass('row_selected');

        //remove the selection of the currently selected row
        tbl.$('tr.row_selected').removeClass('row_selected');

        $(this).closest('tr').addClass('row_selected');
        tbl.fnCallPropeties();
        return false;
    });

    $(nTd).empty();
    $(nTd).prepend(b);
}

function fnValidateDateFormat(date) {

    date = date.trim();

    if (date.match(/^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/)) {
        return true;
    } else {
        return false;
    }
        
}


//To check all values in Array identical or not
//Array.prototype.AllValuesSame = function () {
//    if (this.length > 0) {
//        for (var i = 1; i < this.length; i++) {
//            if (this[i] !== this[0])
//                return false;
//        }
//    }
//    return true;
//}

//Remove Empty fields from Array
function removeEmptyFields(array) {
    var newArray = [];
    for (var i = 0; i < array.length; i++) {
        if (typeof array[i] == 'string' && array[i]) {
            newArray.push(array[i]);
        }
    }

    return newArray;
}


//Function to provide hyperlink to open Group contract page on selecting row in datatable
function OpenGroup(tbl, iRow, nTd, sData, oData, selectedcontractId) {

    var b = $('<a href=' + '' + '>' + sData + '</a>');
    var contractId = selectedcontractId;
    var contractMaintenanceId = tbl.fnGetData(iRow, 1);
    var chargeFrequency = tbl.fnGetData(iRow, 5);
    var periodFrequencyId = tbl.fnGetData(iRow, 22);
    var firstPeriodStartDate = tbl.fnGetData(iRow, 7);
    var firstRenewalDate = tbl.fnGetData(iRow, 8);
    //Khushboo
    var finalBillingPeriodStartDate = tbl.fnGetData(iRow, 9);
    var finalBillingPeriodEndDate = tbl.fnGetData(iRow, 10);
    var documentTypeId = oData[23];
    var invoiceAdvancedArrears = oData[24];
    var invoiceInAdvance = oData[25];
        
    
    //firstPeriodStartDate = new Date(firstPeriodStartDate).toString("dd'/'MM'/'yyyy");
    //finalBillingPeriodEndDate = new Date(finalBillingPeriodEndDate).toString("dd'/'MM'/'yyyy");

    //After clicking on group        
    b.on('click', function () {        
        b.siblings('tr').addClass('row_selected');        
        debugger;
        if (sData == 'Group') {            
            var postData = { contractId: contractId, periodFrequencyId: periodFrequencyId, 
                             firstPeriodStartDate: firstPeriodStartDate, firstRenewalDate: firstRenewalDate, finalBillingPeriodStartDate: finalBillingPeriodStartDate,
                             finalBillingPeriodEndDate: finalBillingPeriodEndDate, documentTypeId: documentTypeId,
                             invoiceAdvancedArrears: invoiceAdvancedArrears, invoiceInAdvance: invoiceInAdvance
                           };
            //To check mininum two records of same criteria are available
            //$.getJSON("/Contract/GetRecordCount", postData, function (response) {
            //}).done(function (response) {

            //    //To check mininum two records of same criteria are available
            //    if (response < 2) {
            //        alert("You need at least two records with the same first period start date, final billing period end date and charge frequency to create a group.");
            //        return false;
            //    }
            //    else {
                    //Returns that is selected record parameters has any existing group or not
            $.getJSON("/Contract/IsContractMaintenanceGrouped", postData, function (response) {
                    }).done(function (response) {
                        if (response == true) {
                            //Open dialog having all ungrouped records fall in given crieria - For new group
                            _fnInitializeDialogForContractGroup(selectedcontractId, periodFrequencyId,
                                                                firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                                finalBillingPeriodEndDate, documentTypeId,
                                                                invoiceAdvancedArrears, invoiceInAdvance,
                                                                false, false, null);
                            //var target = $(this);
                            $('#contractMaintenanceGroupDialog').dialog('open');
                        }
                        else {
                            //Gets the record count of Ungrouped elements
                            $.getJSON("/Contract/GetRecordCountOfUngroupedElements", postData, function (response) {
                            }).done(function (response) {
                                if (response == 1) {
                                    //Gets the record count of grouped elements
                                    $.getJSON("/Contract/GetRecordCountOfGroupedElements", postData, function (response) {
                                    }).done(function (response) {
                                        if (response == 1) {
                                            _fnInitializeDialogForContractGroup(selectedcontractId, periodFrequencyId,
                                                                                firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                                                finalBillingPeriodEndDate, documentTypeId,
                                                                                invoiceAdvancedArrears, invoiceInAdvance,
                                                                                false, false, null);
                                            //var target = $(this);
                                            $('#contractMaintenanceGroupDialog').dialog('open');
                                        }
                                        else {
                                            //Open Existing group screen
                                            _fnInitializeDialogForContractGroup(selectedcontractId, periodFrequencyId,
                                                                                firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                                                finalBillingPeriodEndDate, documentTypeId, 
                                                                                invoiceAdvancedArrears, invoiceInAdvance,
                                                                                false, true, null);
                                            //var target = $(this);
                                            $('#contractMaintenanceGroupDialog').dialog('open');
                                        }
                                    }).fail(function (response) {
                                        DisplayErrorMessage(response, false);
                                    });
                                }
                                else {
                                    fnOpenContractMaintenanceForGroup(selectedcontractId, periodFrequencyId,
                                                                      firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                                      finalBillingPeriodEndDate, documentTypeId,
                                                                      invoiceAdvancedArrears, invoiceInAdvance);
                                }

                            }).fail(function (response) {
                                DisplayErrorMessage(response, false);
                            });
                        }

                    }).fail(function (response) {
                        DisplayErrorMessage(response, false);
                    });
                //}

            //}).fail(function (response) {
            //    DisplayErrorMessage(response, false);
            //});

            //fnOpenContractMaintenanceForGroup(selectedcontractId, periodFrequencyId);
        }
        else if (sData == "Ungroup") {
            var postData = { contractMaintenanceId: contractMaintenanceId };
            $.getJSON("/Contract/GetContractMaintenanceById", postData, function (response) {
            }).done(function (response) {
                var groupId = response.GroupId;
                var groupName = response.GroupName;
                //Check that selected line is default or not if line is default opens the pop up having all records of selected line group
                if (response.IsDefaultLineInGroup == true) {
                    var confirmUngroup = confirm("This is a default printing line. Please select another default printing line and try again.");
                    if (confirmUngroup == true) {
                        _fnInitializeDialogForContractGroup(contractId, periodFrequencyId,
                                                            firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                            finalBillingPeriodEndDate, documentTypeId,
                                                            invoiceAdvancedArrears, invoiceInAdvance,
                                                            false, false, groupName);
                        $('#contractMaintenanceGroupDialog').dialog('open');
                    }
                }
                else {
                    postData = { contractId: contractId, periodFrequencyId: periodFrequencyId, groupId: groupId }
                    //Get record count of grouped items by group Id
                    $.getJSON("/Contract/GetRecordCountofGroupedItemsByGroupId", postData, function (response) {
                    }).done(function (response) {
                        if (response == 2) {
                            //If List contains only two records it will ask for confirmation
                            var confirmUngroup = confirm("By ungrouping this billing line the entire group will be removed and Milestone(s) with status “Ready For Calculating” will be deleted. Are you sure you want to ungroup this?");
                            if (confirmUngroup == true) {
                                var urlAction = '/Contract/UngroupContractMaintenance';
                                $.ajax({
                                    url: urlAction,
                                    data: postData,
                                    type: 'POST',
                                    traditional: true,
                                    dataType: 'text',
                                    success: function (entityObj) {
                                        contractMaintenanceTbl.fnDraw(false);
                                    },
                                    error: function (response, status, error) {
                                        ShowErrorMessage(response, false);
                                    }
                                });
                            }
                        }
                        else if (response > 2) {
                            var confirmUngroup = confirm("By ungrouping this billing line, Milestone(s) with status “Ready For Calculating” will be deleted. Are you sure you want to ungroup this?");
                            if (confirmUngroup == true) {
                                postData = { contractMaintenanceId: contractMaintenanceId }
                                $.ajax({
                                    url: '/Contract/UngroupContractMaintenanceItem',
                                    data: postData,
                                    type: 'POST',
                                    traditional: true,
                                    dataType: 'text',
                                    success: function (entityObj) {
                                        contractMaintenanceTbl.fnDraw(false);
                                    },
                                    error: function (response, status, error) {
                                        ShowErrorMessage(response, false);
                                    }
                                });
                            }
                        }

                    }).fail(function (response) {                        
                        DisplayErrorMessage(response, false);
                    });
                }

            }).fail(function (response) {                
                DisplayErrorMessage(response, false);
            });
        }

        //remove the selection of the currently selected row
        tbl.$('tr.row_selected').removeClass('row_selected');

        $(this).closest('tr').addClass('row_selected');
        tbl.fnCallPropeties();

        return false;
    });

    $(nTd).empty();
    $(nTd).prepend(b);
}

//Call on Group/UnGroup selection
function fnOpenContractMaintenanceForGroup(selectedcontractId, periodFrequencyId,
                                           firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                           finalBillingPeriodEndDate, documentTypeId,
                                           invoiceAdvancedArrears, invoiceInAdvance) {
    //var target = $(this);
    //var periodFrequencyId = periodFrequencyId;
    _fnInitializeDialogForContractGroupConfirmation(selectedcontractId, periodFrequencyId,
                                                    firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                    finalBillingPeriodEndDate, documentTypeId,
                                                    invoiceAdvancedArrears, invoiceInAdvance)
    $('#contractMaintenanceGroupConnfirmationDialog').dialog('open');
    //$('#contractMaintenanceGroupConnfirmationDialog').dialog("widget").position({
    //    my: 'left top',
    //    at: 'left bottom',
    //    of: target
    //});
}


//Intialize dialog 
function _fnInitializeDialogForContractGroupConfirmation(contractId, periodFrequencyId,
                                                         firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                         finalBillingPeriodEndDate, documentTypeId,
                                                         invoiceAdvancedArrears, invoiceInAdvance) {

    var contractMaintenanceGroupConfirmationDialog = $("#contractMaintenanceGroupConnfirmationDialog").dialog({
        modal: true,
        show: "blind",
        hide: "explode",
        autoOpen: false,
        resizable: false,
        closeOnEscape: false,
        title: "Billing Details Group",
        width: 300,
        height: 125,
        open: function (event, ui) {
            $.ajaxSetup({ cache: false })
            $('body').css('overflow', 'hidden'); $('.ui-widget-overlay').css('width', '100%');
        },
        close: function (event, ui) {
            $('body').css('overflow', 'auto');
            $(this).dialog("destroy");
        },
        buttons: {
            'New': function () {
                contractMaintenanceGroupConfirmationDialog.dialog('close');                
                _fnInitializeDialogForContractGroup(contractId, periodFrequencyId,
                                                    firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                    finalBillingPeriodEndDate, documentTypeId,
                                                    invoiceAdvancedArrears, invoiceInAdvance,
                                                    true, false, null);
                //var target = $(this);
                $('#contractMaintenanceGroupDialog').dialog('open');
                //$('#contractMaintenanceGroupDialog').dialog("widget").position({
                //    my: 'left top',
                //    at: 'left bottom',
                //    of: target
                //});
            },
            'Existing': function () {
                contractMaintenanceGroupConfirmationDialog.dialog('close');                
                _fnInitializeDialogForContractGroup(contractId, periodFrequencyId,
                                                    firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                                    finalBillingPeriodEndDate, documentTypeId,
                                                    invoiceAdvancedArrears, invoiceInAdvance,
                                                    false, true, null);
                //var target = $(this);
                $('#contractMaintenanceGroupDialog').dialog('open');
                //$('#contractMaintenanceGroupDialog').dialog("widget").position({
                //    my: 'left top',
                //    at: 'left bottom',
                //    of: target
                //});
            }
        }
    });
}

//Intialize Main dialog 
function _fnInitializeDialogForContractGroup(contractId, periodFrequencyId,
                                             firstPeriodStartDate, firstRenewalDate, finalBillingPeriodStartDate,
                                             finalBillingPeriodEndDate, documentTypeId,
                                             invoiceAdvancedArrears, invoiceInAdvance,
                                             isNewGroup, isExistingGroup, groupName) {
    var loadAction = '/Contract/ContractMaintenanceGroupIndex?contractId=' + contractId
                                                                           + '&periodFrequencyId=' + periodFrequencyId
                                                                           + '&firstPeriodStartDate=' + firstPeriodStartDate
                                                                           + '&firstRenewalDate=' + firstRenewalDate
                                                                           + '&finalBillingPeriodStartDate=' +finalBillingPeriodStartDate
                                                                           + '&finalBillingPeriodEndDate=' + finalBillingPeriodEndDate
                                                                           + '&documentTypeId=' + documentTypeId
                                                                           + '&invoiceAdvancedArrears=' + invoiceAdvancedArrears
                                                                           + '&invoiceInAdvance=' + invoiceInAdvance
                                                                           + '&isNewGroup=' + isNewGroup
                                                                           + '&isExistingGroup=' + isExistingGroup
                                                                           + '&groupName=' + escape(groupName);

    var urlSaveAction = '/Contract/ApplyGrouping';

    var contractMaintenanceGroupDialog = $("#contractMaintenanceGroupDialog").dialog({
        modal: true,
        //show: "blind",
        //hide: "explode",
        autoOpen: false,
        resizable: false,
        //closeOnEscape: false,
        title: "Billing Details Group",
        width: 900,
        height: 500,
        open: function (event, ui) {
            $.ajaxSetup({ cache: false })
            $('body').css('overflow', 'hidden'); $('.ui-widget-overlay').css('width', '100%');
        },
        close: function (event, ui) {            
            $('body').css('overflow', 'auto');                        
            $(this).dialog("destroy");
        },
        buttons: {
            'Save': function () {
                //validate the form - the form is embedded within a div,
                //use this dialog div to select the form within it.

                var $form = $('#contractMaintenanceGroupDialog' + ' form');
                if ($form.valid()) {
                    //validateContractMaintenanceGroup to check that checkboxes and radio button is selected
                    if ((validateContractMaintenanceGroupCheckBox(contractMaintenanceGroupTbl) == false) || (validateContractMaintenanceGroupRadioButton(contractMaintenanceGroupTbl) == false)) {
                        return false;
                    }
                    else {
                        var selectedIds = validateContractMaintenanceGroupCheckBox(contractMaintenanceGroupTbl);
                        var defaultInGroup = validateContractMaintenanceGroupRadioButton(contractMaintenanceGroupTbl);
                        var postData = { Ids: selectedIds, defaultInGroupId: defaultInGroup };
                        $.ajax({
                            url: urlSaveAction,
                            data: postData,
                            type: 'POST',
                            traditional: true,
                            dataType: 'text',
                            success: function (entityObj) {
                                contractMaintenanceTbl.fnDraw(false);

                                //ResetBillingDetailInfo();
                                contractMaintenanceGroupDialog.dialog('close');                                
                            },
                            error: function (response, status, error) {
                                ShowErrorMessage(response, false);
                            }
                        });
                    }
                }
            },
            'Close': function () {
                contractMaintenanceGroupDialog.dialog('close');
            }
        }
    }).load(loadAction);
}



//Validate ContractMaintenace Group Checkbox
function validateContractMaintenanceGroupCheckBox(tbl) {
    var selectedIds = new Array();

    var node = tbl.fnGetNodes();
    var inputCheckBoxes = $('input:checkbox', node);

    //iterate over the check boxes
    for (var i = 0; i < inputCheckBoxes.length; i++) {

        if ((inputCheckBoxes[i]).checked == true) {
            selectedIds[i] = $(inputCheckBoxes[i]).val();
        }
    }

    selectedIds = removeEmptyFields(selectedIds);

    //To check atleast two checkbox are selected
    if (selectedIds.length < 2) {
        alert("Please select at least 2 records to create a group.");
        return false;
    }    

    return (selectedIds);
}

//Validate ContractMaintenanceGroup RadioButton
function validateContractMaintenanceGroupRadioButton(tbl) {
    var selectedIds = new Array();
    var defaultInGroup = new Array();

    var node = tbl.fnGetNodes();
    var inputCheckBoxes = $('input:checkbox', node);
    var inputRadio = $('input:radio', node);

    //iterate over the check boxes and set the return the row id
    for (var i = 0; i < inputCheckBoxes.length; i++) {

        if ((inputCheckBoxes[i]).checked == true) {
            selectedIds[i] = $(inputCheckBoxes[i]).val();

            //Set Radio button value
            if ((inputRadio[i]).checked == true) {
                defaultInGroup[i] = $(inputRadio[i]).val();
            }
        }
    }

    //To remove empty values from array                
    defaultInGroup = removeEmptyFields(defaultInGroup);

    //To check radio button is selected or not
    if (defaultInGroup == "") {
        alert("Please select valid default printing line.")
        return false;
    }

    return (defaultInGroup[0]);
}









