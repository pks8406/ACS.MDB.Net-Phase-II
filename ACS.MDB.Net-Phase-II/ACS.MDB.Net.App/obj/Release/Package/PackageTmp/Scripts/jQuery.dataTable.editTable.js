(function ($) {

    $.fn.editTable = function (options) {
        var defaultColumnDefs = [{ bSortable: false, aTargets: [0] }, { bVisible: false, aTargets: [1] }];
        //var confirmDeleteMessage = "Are you sure you want to delete the selected rows?";

        // function to set the new load dialog url		
        this.fnSetNewDialogURL = function (url) {
            properties.sNewDialogLoadURL = url;
        }

        // Function called to set the edit dialog url
        this.fnSetEditDialogURL = function (url) {
            properties.sEditDialogLoadURL = url;
        }

        // Function called to open the edit dialog
        this.fnOpenEdit = function (ID) {
            if (ID != -1) {
                //De select all checkbox
                fnDeselectCheckBoxExceptSelectedRecord(ID);
                _fnInitializeDialog(ID);
                $('#' + properties.sDialogDivId).dialog('open');
            }
        }

        // Function called to open the edit dialog
        this.fnCallPropeties = function () {
            if (properties.fnRowSelected != null) {
                properties.fnRowSelected();
            }
        }

        // Function to show or hide toolbar
        this.fnShowToolBar = function (show) {
            if (show) {
                //$(properties.sToolbarSelector).show();                
                $("#" + properties.sAddButtonId).prop('disabled', false);
                $("#" + properties.sEditButtonId).prop('disabled', false);
                $("#" + properties.sDeleteButtonId).prop('disabled', false);
            }
            else {
                //$(properties.sToolbarSelector).hide();
                $("#" + properties.sAddButtonId).prop('disabled', true);
                $("#" + properties.sEditButtonId).prop('disabled', true);
                $("#" + properties.sDeleteButtonId).prop('disabled', true);
            }
        }

        //Function to set checkbox column header selector to false
        //Call after we do sorting,paging,searching, save (create/edit) of record
        this.fnSetCheckBoxItemSelector = function () {
            $(properties.sCheckBoxItemSelector).prop('checked', false);
        }

        ///Function to change the name of pop up title and set color for it.
        this.fnChangeEntityName = function (value) {
            var entityName = properties.sEntityName.split(":")[0];
            properties.sEntityName = (entityName + " : '" + (value).fontcolor("#004bff") + "'");
        }

        var spinnerVisible = false;

        var defaults = {
            sListURL: '',                           //URL for the getting the list
            fnServerData: '',                       //call back function for server data
            sUpdateURL: '',                         //URL used to update the selected record
            sAddURL: '',                            //URL used to add a new record
            sDeleteURL: '',                         //URL used to delete the selected records
            sNewDialogLoadURL: 'LoadNewDialog',     //URL that returns partial view that will be loaded in dialog to create new record
            sEditDialogLoadURL: 'LoadEditDialog',   //URL that returns partial view that will be loaded in dialog to edit a record
            sToolbarSelector: '#div_toolbar',       //The selector for the toolbar div
            sDialogDivId: 'popup_dialog',           //The selector for the dialog bar
            sAddButtonId: 'btn_new',                //The add button id
            sEditButtonId: 'btn_edit',              //The edit button id
            sDeleteButtonId: 'btn_delete',          //The delete button id
            iDialogWidth: 400,                      //Dialog width
            iDialogHeight: 400,                     //Dialog height
            iDialogOffset: 400,                     //Dialog position
            oColumnDefs: defaultColumnDefs,         //default column definitions
            fnRowSelected: null,                    //Function that should called when row gets selected
            fnAfterDelete: null,                    //Fuction that should called when record gets deleted from first grid
            sCheckBoxItemSelector: '#selectall',    //The selector for the select all checkbox click
            sEntityName: 'Record',                  //The name of the entity being managed
            sDeleteMessage: "Are you sure you want to delete the selected rows?", //The delete message to show when click on delete button
            sSpinnerForDeleteDivId: '',             //Id to show processing image after Deleting record(s)
            sSpinnerForSaveDivId: '',               //Id to show processing image after Saving a record
            iDisplayLength: 25,                     // Number of rows to be display on data grid
            sScrollY: "",
            bPaginate: true,
            bScrollCollapse: false,
            sRowGroupingApplied: false

        };

        var properties = $.extend(defaults, options);
        var divTable = this;                        //the table <div> use to construct the datatable

        // Setup an event handler for select all check box click for the specified selector.
        var first = properties.sCheckBoxItemSelector + ":first";
        $(this).find(first).click(function () {
            var selectStatus = this.checked;

            divTable.find('td :checkbox').each(function () {
                this.checked = selectStatus;
            });
        });

        $(this).find('td :checkbox').live('click', function () {
            var checkAll = true;
            if (this.checked === false) {
                checkAll = false;
            }
            else {
                divTable.find('td :checkbox').each(function () {
                    if (this.checked === false) {
                        checkAll = false;
                    }
                });
            }

            divTable.find(properties.sCheckBoxItemSelector).each(function () {
                this.checked = checkAll;
            });
        });


        //intialise the local reference of the data table
        var oTable = _fnSetupMyDatatable();
        _fnSetupMyRowClicks();
        return oTable;

        // Function called to setup a data table for the
        // specified selector. The  selector has to be
        // an id for the <table></table> element
        function _fnSetupMyDatatable() {
            var myDataTable;
            if (properties.sListURL != '') {
                if (properties.fnServerData != '') {
                    if (properties.sRowGroupingApplied == true) {
                        myDataTable = divTable.dataTable({
                            bServerSide: true,
                            sAjaxSource: properties.sListURL,
                            fnServerData: properties.fnServerData,
                            aoColumnDefs: properties.oColumnDefs,
                            bProcessing: true,
                            bJQueryUI: true,
                            bStateSave: false,
                            sPaginationType: 'full_numbers',
                            iDisplayLength: properties.iDisplayLength,
                            sScrollY: properties.sScrollY,
                            bScrollCollapse: properties.bScrollCollapse,
                            bPaginate: properties.bPaginate,
                            bSort: true,
                            //Apply row grouping on contractMaintenanceTable 
                            "fnDrawCallback": function (oSettings) {
                                if (oSettings.aiDisplay.length == 0) {
                                    return;
                                }
                                var nTrs = $('#contractMaintenance_tbl tbody tr');
                                var iColspan = nTrs[0].getElementsByTagName('td').length;
                                var sLastGroup = "";
                                for (var i = 0 ; i < nTrs.length ; i++) {
                                    var iDisplayIndex = oSettings._iDisplayStart + i;
                                    var sGroup = oSettings.aoData[oSettings.aiDisplay[iDisplayIndex]]._aData[19];

                                    if (sGroup != sLastGroup) {
                                        var nGroup = document.createElement('tr');
                                        var nCell = document.createElement('td');
                                        nCell.colSpan = iColspan;
                                        nCell.className = "group";
                                        nCell.innerHTML = sGroup;
                                        nGroup.appendChild(nCell);
                                        nTrs[i].parentNode.insertBefore(nGroup, nTrs[i]);
                                        sLastGroup = sGroup;
                                    }
                                }
                            }
                        });
                    }
                    else {
                        myDataTable = divTable.dataTable({
                            bServerSide: true,
                            sAjaxSource: properties.sListURL,
                            fnServerData: properties.fnServerData,
                            aoColumnDefs: properties.oColumnDefs,
                            bProcessing: true,
                            bJQueryUI: true,
                            bStateSave: false,
                            sPaginationType: 'full_numbers',
                            iDisplayLength: properties.iDisplayLength,
                            sScrollY: properties.sScrollY,
                            bScrollCollapse: properties.bScrollCollapse,
                            bPaginate: properties.bPaginate
                        });
                    }
                }
                else {
                    myDataTable = divTable.dataTable({
                        bServerSide: true,
                        sAjaxSource: properties.sListURL,
                        aoColumnDefs: properties.oColumnDefs,
                        bProcessing: true,
                        bJQueryUI: true,
                        bStateSave: false,
                        sPaginationType: 'full_numbers',
                        iDisplayLength: properties.iDisplayLength,
                        sScrollY: properties.sScrollY,
                        bScrollCollapse: properties.bScrollCollapse,
                        bPaginate: properties.bPaginate
                    });
                }
            }
            else {
                myDataTable = divTable.dataTable({
                    bServerSide: false,
                    aoColumnDefs: [{ bSortable: false, aTargets: [0] }],
                    bProcessing: false,
                    bJQueryUI: true,
                    bStateSave: false,
                    sPaginationType: 'full_numbers',
                    iDisplayLength: properties.iDisplayLength,
                    sScrollY: properties.sScrollY,
                    bScrollCollapse: properties.bScrollCollapse,
                    bPaginate: properties.bPaginate
                });
            }

            _fnAddToolBarButtons();
            return myDataTable;
        }

        function _fnAddToolBarButtons() {
            var oToolBar = $(properties.sToolbarSelector);
            if (oToolBar != null) {
                //add the New button and setup its properties
                if (properties.sAddURL != '') {
                    oToolBar.append('<button id="' + properties.sAddButtonId + '" value="Create">New</button>');
                    var oAddNewButton = $('#' + properties.sAddButtonId);
                    oAddNewButton.click(function () {

                        //De select all checkbox except the one is editing
                        fnDeselectCheckBoxExceptSelectedRecord(-1);
                        _fnInitializeDialog(-1);
                        $('#' + properties.sDialogDivId).dialog('open');
                    });
                }

                //add the edit button and setup its properties
                if (properties.sUpdateURL != '') {
                    oToolBar.append('<button id="' + properties.sEditButtonId + '" value="Edit">Edit</button>');
                    var oEditButton = $('#' + properties.sEditButtonId);
                    (oEditButton).hide();
                    oEditButton.click(function () {
                        var selectedRow = fnGetSelectedRowIndex();
                        if (selectedRow != -1) {

                            //De select all checkbox
                            fnDeselectCheckBoxExceptSelectedRecord(selectedRow);
                            _fnInitializeDialog(selectedRow);
                            $('#' + properties.sDialogDivId).dialog('open');
                        }
                        else {
                            alert("Please select record.");
                        }
                    });
                }

                //add the delete button and setup its properties
                if (properties.sDeleteURL != '') {
                    oToolBar.append('<button id="' + properties.sDeleteButtonId + '" value="Delete">Delete</button>');
                    var oDeleteButton = $('#' + properties.sDeleteButtonId);
                    oDeleteButton.click(function () {
                        var selectedIds = fnGetSelectedIds();
                        if (selectedIds.length > 0) {
                            _fnDeleteRecords();
                        }
                        else {
                            alert("Please select record(s).");
                        }
                    });
                }
            }
        }

        function _fnInitializeDialog(rowId) {
            var loadAction = properties.sNewDialogLoadURL;
            var dialogTitle = 'Add ' + properties.sEntityName;
            var urlSaveAction = properties.sAddURL;

            if (rowId !== null && rowId !== -1) {
                var recId = oTable.fnGetData(rowId, 1);

                //edit record
                loadAction = properties.sEditDialogLoadURL + '/' + recId;
                dialogTitle = 'Edit ' + properties.sEntityName;
                urlSaveAction = properties.sUpdateURL;
            }

            var dlg_offset_x = $('body').width() - properties.iDialogWidth - properties.iDialogOffset;
            var dlg_margin_top = $("#headertable").outerHeight(true); // includeMargins=true
            //var dlg_margin_bottom = $("#footer").outerHeight(true); // 

            var detailsDlg = $('#' + properties.sDialogDivId).dialog({
                autoOpen: false,
                resizable: false,
                title: dialogTitle,
                width: properties.iDialogWidth,
                height: properties.iDialogHeight,

                //Set Modal popup position when it opens first time
                position: [dlg_offset_x, dlg_margin_top],
                modal: true,
                open: function (event, ui) {
                    $.ajaxSetup({ cache: false });

                    // Prevent the backspace key from navigating back.
                    $(document).unbind('keydown').bind('keydown', function (event) {
                        var doPrevent = false;
                        if (event.keyCode === 8) {
                            var d = event.srcElement || event.target;
                            if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD'))
                                 || d.tagName.toUpperCase() === 'TEXTAREA') {
                                doPrevent = d.readOnly || d.disabled;
                            }
                            else {
                                doPrevent = true;
                            }
                        }

                        if (doPrevent) {
                            event.preventDefault();
                        }
                    });

                    //this is to avoid having scroll bars when the dialog opens
                    //Note : Comment it to avoid scroll bars.
                    //This is uncommented for fix of issue no(1343)
                    //$('body').css('overflow', 'hidden'); $('.ui-widget-overlay').css('width', '100%');
                },
                close: function (event, ui) {

                    //reset the style to original after dialog closes
                    //Note : Comment it to avoid scroll bars.
                    //This is uncommented for fix of issue no(1343)
                    //$('body').css('overflow', 'auto');

                    //destroy the dialog - this is also done to avoid caching
                    $(this).dialog("destroy");
                },
                buttons: {
                    'Save': function () {

                        //validate the form - the form is embedded within a div,
                        //use this dialog div to select the form within it.
                        var $form = $('#' + properties.sDialogDivId + ' form');
                        if ($form.valid()) {
                            //To check Dialog is End user or not
                            if (properties.sDialogDivId == "enduser_dialog") {
                                var data = $form.serializeArray();
                                var endUserId;
                                for (var i = 0; i < data.length; i++) {
                                    if ((data[i].name) == "EndUserId") {
                                        endUserId = data[i].value;
                                        break;
                                    }
                                }

                                //to check whether the end user popup is opened for new/update the record and
                                //to display confirmation box on end user dialog popup
                                NewEditEndUserPopUp(endUserId, $form, urlSaveAction);

                            }
                            else {
                                //if ($form.valid()) {
                                //ShowProgressonSaveandDeleteAnimation();

                                if (properties.sSpinnerForSaveDivId != '') {
                                    $("#" + properties.sSpinnerForSaveDivId).show();
                                    //Show processing image
                                    ShowProgressonSaveandDelete(properties.sSpinnerForSaveDivId);
                                }

                                $.ajax({
                                    url: urlSaveAction,
                                    data: $form.serialize(),
                                    type: 'POST',
                                    dataType: 'text',
                                    success: function (entityObj) {
                                        if (properties.sSpinnerForSaveDivId != '') {
                                            //Hide processing image
                                            HideProgressonSaveandDelete(properties.sSpinnerForSaveDivId);
                                            $("#" + properties.sSpinnerForSaveDivId).hide();
                                        }
                                        oTable.fnDraw(false);

                                        ResetBillingDetailInfo();
                                        detailsDlg.dialog('close');
                                    },
                                    error: function (response, status, error) {

                                        if (properties.sSpinnerForSaveDivId != '') {
                                            //Hide processing image
                                            HideProgressonSaveandDelete(properties.sSpinnerForSaveDivId);
                                            $("#" + properties.sSpinnerForSaveDivId).hide();
                                        }

                                        ShowErrorMessage(response, false);
                                    }
                                });
                            }
                        }
                    },
                    'Close': function () {
                        detailsDlg.dialog('close');
                    }
                }
            }).load(loadAction);

            //to check whether the end user popup is opened for new/update the record and
            //to display confirmation box on end user dialog popup
            function NewEditEndUserPopUp(endUserId, $form, urlSaveAction) {
                var postData = { endUserId: endUserId };
                $.ajax({
                    url: "/Contract/GetMaintenanceBillingLineStatus",
                    data: postData,
                    type: 'POST',
                    //dataType: 'text',
                    success: function (response) {
                        if (response == 'True') {
                            var confirmation = confirm("Contracts exists with this End User name");
                            if (confirmation == true) {
                                $.ajax({
                                    url: urlSaveAction,
                                    data: $form.serialize(),
                                    type: 'POST',
                                    dataType: 'text',
                                    success: function (entityObj) {
                                        if (properties.sSpinnerForSaveDivId != '') {
                                            //Hide processing image
                                            HideProgressonSaveandDelete(properties.sSpinnerForSaveDivId);
                                            $("#" + properties.sSpinnerForSaveDivId).hide();
                                        }
                                        oTable.fnDraw(false);

                                        ResetBillingDetailInfo();
                                        detailsDlg.dialog('close');
                                    },
                                    error: function (response, status, error) {

                                        if (properties.sSpinnerForSaveDivId != '') {
                                            //Hide processing image
                                            HideProgressonSaveandDelete(properties.sSpinnerForSaveDivId);
                                            $("#" + properties.sSpinnerForSaveDivId).hide();
                                        }

                                        ShowErrorMessage(response, false);
                                    }
                                });
                            }

                        }
                        else {
                            $.ajax({
                                url: urlSaveAction,
                                data: $form.serialize(),
                                type: 'POST',
                                dataType: 'text',
                                success: function (entityObj) {
                                    if (properties.sSpinnerForSaveDivId != '') {
                                        //Hide processing image
                                        HideProgressonSaveandDelete(properties.sSpinnerForSaveDivId);
                                        $("#" + properties.sSpinnerForSaveDivId).hide();
                                    }
                                    oTable.fnDraw(false);

                                    ResetBillingDetailInfo();
                                    detailsDlg.dialog('close');
                                },
                            });
                        }
                    }
                });

            }
            //This code is responsible to move the modal popup based on moving of parent window scroll bar. (In short make the popup as floatable)
            $(window).bind('scroll', function (evt) {
                var scrollTop = $(window).scrollTop();
                var bottom = $(document).height() - scrollTop;
                $('#' + properties.sDialogDivId).dialog("option", {
                    "position": [
                        dlg_offset_x,
                        ((dlg_margin_top - scrollTop > 0) ?
                            dlg_margin_top - scrollTop :
                                ((bottom - properties.iDialogHeight) ?
                                    0 :
                                    bottom - properties.iDialogHeight
                                )
                        )
                    ]
                });
            });
        }

        //// Function called to reload the server side side
        //function _fnReloadNonServerSideTable() {
        //}

        function _fnDeleteRecords() {
            var answer = confirm(properties.sDeleteMessage);
            if (answer) {
                var selectedIds = fnGetSelectedIds();
                if (selectedIds.length != 0) {
                    var postData = { ids: selectedIds };

                    if (properties.sSpinnerForDeleteDivId != '') {
                        $("#" + properties.sSpinnerForDeleteDivId).show();
                        //Show processing image
                        ShowProgressonSaveandDelete(properties.sSpinnerForDeleteDivId);
                    }

                    $.ajax({
                        url: properties.sDeleteURL,
                        type: 'POST',
                        data: postData,
                        traditional: true,
                        success: function (response) {
                            if (properties.sSpinnerForDeleteDivId != '') {
                                //Hide processing image
                                HideProgressonSaveandDelete(properties.sSpinnerForDeleteDivId);
                                $("#" + properties.sSpinnerForDeleteDivId).hide();
                            }

                            if (properties.fnAfterDelete != null) {
                                properties.fnAfterDelete();
                            }
                            oTable.fnDraw(false);
                        },
                        error: function (response, status, error) {
                            if (properties.sSpinnerForDeleteDivId != '') {
                                //Hide processing image
                                HideProgressonSaveandDelete(properties.sSpinnerForDeleteDivId);
                                $("#" + properties.sSpinnerForDeleteDivId).hide();
                            }

                            ShowErrorMessage(response, true);
                        }
                    });
                }
            }
        }

        // This function sets up click handler for the rows of the
        // data table.
        function _fnSetupMyRowClicks() {
            //Handle the clicks for the rows in the employee table
            //oTable.find('tbody').find('tr').live('click', function (event) {
            $(document).off('click', 'tbody > tr');

            //$(document).on('click', 'tbody > tr', function () {
            oTable.on('click', 'tbody > tr', function () {
                //remove the selection of the currently selected row

                oTable.$('tr.row_selected').removeClass('row_selected');

                //add selection to the row that was clicked
                $(this).addClass('row_selected');

                if (properties.fnRowSelected != null) {
                    properties.fnRowSelected();
                }
            });
        }

        // This function returns the index of the row selected from 
        // the data table.
        function fnGetSelectedRowIndex() {
            var index = -1;
            var aTrs = oTable.fnGetNodes();

            for (var i = 0; i < aTrs.length; i++) {
                if ($(aTrs[i]).hasClass('row_selected')) {
                    index = i;
                }
            }

            return index;
        }

        // Function that returns an array of IDs for rows whose
        // check boxes have been checked.
        function fnGetSelectedIds() {
            var selectedIds = new Array();

            //get all the nodes whose check boxes have been checked
            var inputCheckBoxes = $('input:checked', oTable.fnGetNodes());
            if (inputCheckBoxes != null) {
                //iterate over the check boxes and set the return the row id
                for (var i = 0; i < inputCheckBoxes.length; i++) {
                    selectedIds[i] = $(inputCheckBoxes[i]).val();
                }
            }

            return selectedIds;
        }

        //Function that unselected all check box
        function fnDeselectCheckBoxExceptSelectedRecord(selectedRow) {

            $(properties.sCheckBoxItemSelector).prop('checked', false);

            //$('#tblControls').hide();
            //var indexVal = 0;
            $('input:checked', oTable.fnGetNodes()).each(function () {
                //if (selectedRow != indexVal)
                //{
                this.checked = false;
                //}
                //indexVal++;
            });
        }

        //Show progress image
        function ShowProgressonSaveandDelete(control) {

            if (!spinnerVisible) {
                //$("div#spinner").fadeIn("fast");
                $("div#" + control).fadeIn("fast");
                spinnerVisible = true;
            }
        };

        //Hide progress image
        function HideProgressonSaveandDelete(control) {
            if (spinnerVisible) {
                //var spinner = $("div#spinner");
                var spinner = $("div#" + control);
                spinner.stop();
                //spinner.fadeOut("fast");
                spinner.fadeOut("fast");
                spinnerVisible = false;
            }

        };

        //Reset the billing details info (e.g. Hide coding details after saving record)
        function ResetBillingDetailInfo() {
            $('#tblCodingDetailControls').hide();
            $('#btnGenerateMilestones').prop('disabled', false);
        }


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
    };

    

})(jQuery);