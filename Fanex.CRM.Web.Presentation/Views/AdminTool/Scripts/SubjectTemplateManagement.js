"use strict";
/*global topwnd, showAlertMessage, head, jlinq */
/* exported tmplSetting, ShowPopupAddSubjectTemplate, 
            ShowPopupEditFeedbackTemplate, DeleteFeedbackTemplateById */

//Contain the template
var tmplSetting = null;
var listFeedbacksLasted = [];
var numberOfFeedbackItem = 0;
var isUseCache = false;
var fbSubjectTemplate;

function appendListFeedbacksLasted(range) {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetFeedbackSubjectTemplateItemsRange",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{ "range": "' + range.toString() + '" }',
        success: function (rs) {
            if (rs != null && rs.length > 0) {
                listFeedbacksLasted = listFeedbacksLasted.concat(rs);
            }
        },
        error: function () {
        },
        complete: function () {
        }
    });
}

//mark because it may conflict with other function 
function getNumOfFeedbackItems() {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetNumOfFeedbackSubjectTemplateItem",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {
            numberOfFeedbackItem = rs;
            listFeedbacksLasted = [];
            var numOfRange = Math.round((numberOfFeedbackItem / 100000) + 0.5);
            for (var i = 1; i <= numOfRange; i++) {
                appendListFeedbacksLasted(i);
            }
        },
        error: function () {
        },
        complete: function () {
        }
    });
}

function ShowPopupAddSubjectTemplate(type) {
    /*jshint unused: vars */
    var title = 'Insert New Feedback Subject Template';
    var url = '../AdminTool/SubjectTemplateIns';
    var width = 760;
    var height = 150;
    parent.OpenPopup(url, title, width, height, true, false, window, 2000, function () { fbSubjectTemplate.loadSubjectTemplate(); });
}

function ShowPopupEditFeedbackTemplate(Id) {
    var title = 'Edit Feedback Subject Template';
    var url = '../AdminTool/SubjectTemplateEdit?Id=' + Id;
    var width = 680;
    var height = 190;
    parent.OpenPopup(url, title, width, height, true, false, window, 2000, function () { fbSubjectTemplate.loadSubjectTemplate(); });
}

function DeleteFeedbackTemplateById(Id) {

    $("#divPopup").html('Are you sure you want to delete Feedback Subject Template ' + Id + '?');
    $("#divPopup").dialog({ width: 380, height: 120, zIndex: 500, resizable: false, modal: true,
        position: [200, 200], //
        close: function (event, ui) {
            /*jshint unused: vars */
        },
        buttons: [
                    {
                        text: "Ok",
                        click: function () {
                            try { parent.BlockUI(); } catch (e) { }
                            $.ajax({
                                type: "POST",
                                url: window.rootUrl + "AdminTool/DeleteSubjectFeedbackTemplate",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: '{"templateId":"' + Id + '"}',
                                success: function (rs) {
                                    if (parseInt(rs.ErrCode) > 0) {
                                        showAlertMessage("errMsg", rs.ErrMsg, 1);
                                    }
                                    else {
                                        setTimeout(function () {
                                            fbSubjectTemplate.loadSubjectTemplate();
                                        }, 1000);
                                    }
                                },
                                error: function () { },
                                complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
                            });
                            $(this).dialog("close");
                        }
                    },
                    {
                        text: "Cancel",
                        click: function () {
                            $(this).dialog("close");
                        }
                    }
                ]
    });
}

//Render Table Feedback Subject Template Management
var pagerOpts = {
    callback: null,
    startPerPage: 50,
    stepPerPage: 50,
    maxItemsPerPage: 500,
    itemsPerPage: 50,
    currentPage: 1
};

fbSubjectTemplate = {
    totalRows: 0,
    pageIndex: 1,
    pageSize: 50,
    gridHeight: 60,
    resetPager: function () {
        this.totalRows = 0;
        this.pageIndex = 1;
        this.pageSize = 50;
    },
    initPager: function (totalItems, itemsPerPage, currentPage) {
        pagerOpts.callback = fbSubjectTemplate.pageSelectCallback;
        pagerOpts.itemsPerPage = itemsPerPage;
        pagerOpts.currentPage = currentPage;
        /*jshint camelcase: false */
        $("#pagination").pagination(totalItems, {
            callback: pagerOpts.callback,
            start_per_page: pagerOpts.startPerPage,
            step_per_page: pagerOpts.stepPerPage,
            max_items_per_page: pagerOpts.maxItemsPerPage,
            items_per_page: pagerOpts.itemsPerPage,
            current_page: pagerOpts.currentPage
        });
    },
    pageSelectCallback: function (pageIndex, itemsPerPage, jq) {
        /*jshint unused: vars */
        var maxPage = Math.ceil(fbSubjectTemplate.totalRows / itemsPerPage);
        pageIndex = pageIndex < maxPage ? pageIndex : maxPage - 1;
        fbSubjectTemplate.pageIndex = pageIndex + 1;
        fbSubjectTemplate.pageSize = itemsPerPage;
        fbSubjectTemplate.loadSubjectTemplate();
        return false;
    },
    renderSubjectTemplateManagementRow: function (row, css) {
        var html = '<tr class="' + css + '">\
                        <td><div class="c" style="width:80px;">' + row.TemplateId.toString() + '</div></td>\
                        <td><div class="l" style="width:470px;" id="templateHiddenId' + row.TemplateId.toString() + '">' + row.Subject + '</div></td>\
                        <td>\
                            <div class="c" style="width:60px;">\
                                <a class="c hand editSetting" href="JavaScript:void(0);" onclick="ShowPopupEditFeedbackTemplate(' + 
                                    row.TemplateId.toString() + ');">\
                                </a>\
                            </div>\
                        </td>\
                        <td>\
                            <div class="c" style="width:60px;">\
                                <a class="c hand deleteSetting" href="JavaScript:void(0);" onclick="DeleteFeedbackTemplateById(' + 
                                    row.TemplateId.toString() + ');">\
                                </a>\
                            </div>\
                        </td>\
                    </tr>';

        return html;
    },
    renderSubjectTemplateManagementLeftRow: function (row, css) {
        var html = '<tr class="' + css + '">\
                        <td><div class="c" style="width:80px;">' + row.TemplateId.toString() + '</div></td>\
                    </tr>';

        return html;
    },
    searchSubjectTemplate: function () {
        this.resetPager();
        this.loadSubjectTemplate();
    },
    loadSubjectTemplate: function () {
        var data =
        {
            'searchString': $('#txtTemplateSubject').val(),
            'pageIndex': fbSubjectTemplate.pageIndex,
            'pageSize': fbSubjectTemplate.pageSize
        };

        try { topwnd.BlockUI(); } catch (e) { }
        $('#errmsg').html('');
        var mainreport = $("#feedbackTemplateManagementgrid");

        $.ajax({
            type: "POST",
            url: window.rootUrl + "AdminTool/SelFeedbackTemplatePageWithSearch",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null && rs.rows != null && rs.rows.length > 0) {
                    var arr = [];
                    var arrLeft = [];
                    fbSubjectTemplate.totalRows = parseInt(rs.total);
                    var css = 'bgodd';
                    for (var i = 0; i < rs.rows.length; i++) {
                        css = (i % 2) > 0 ? "bgodd" : "bgeven";
                        arr.push(fbSubjectTemplate.renderSubjectTemplateManagementRow(rs.rows[i], css));
                        arrLeft.push(fbSubjectTemplate.renderSubjectTemplateManagementLeftRow(rs.rows[i], css));
                    }

                    $("#feedbackTemplateManagementbody tbody").empty().append(arr.join(''));
                    $("#feedbackTemplateManagementbg tbody").empty().append(arrLeft.join(''));

                    var gridOpts = {
                        tblContentId: 'feedbackTemplateManagementbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: fbSubjectTemplate.gridHeight,
                        freezeWidth: 80,
                        numberFreezeCol: 1
                    };

                    mainreport.removeClass("hidden").gridScroller(gridOpts);

                    fbSubjectTemplate.initPager(fbSubjectTemplate.totalRows, fbSubjectTemplate.pageSize, fbSubjectTemplate.pageIndex - 1);
                    fbSubjectTemplate.reRenderGrid();
                    mainreport.find(".y-scrollbar").scrollTop(0);
                }
                else {
                    $('#errmsg').html('<div class="alertboxerr"><div>There is no information.</div></div>');
                    mainreport.addClass('hidden');
                }
            },
            error: function () {
                $('#errmsg').html('<div class="alertboxerr"><div>There is no information.</div></div>');
                mainreport.addClass('hidden');
            },
            complete: function () {
                try { parent.UnBlockUI(); } catch (e) { }
            }
        });
    },
    reRenderGrid: function () {

        var trFrozen = $('#feedbackTemplateManagementbg tbody tr');
        var totalRows = trFrozen.length;

        for (var i = 0; i < totalRows; i++) {
            var frozenHeight = $('#feedbackTemplateManagementbg tbody tr:eq(' + i + ') td:eq(0)').height();
            var scrollHeight = $('#feedbackTemplateManagementbody tbody tr:eq(' + i + ') td:eq(0)').height();

            var maxHeight = 18;
            if (frozenHeight > scrollHeight) {
                maxHeight = frozenHeight + 1;
            }
            else if (scrollHeight > frozenHeight) {
                maxHeight = scrollHeight + 1;
            }

            $('#feedbackTemplateManagementbody tbody tr:eq(' + i + ') td:eq(0) div').attr('style', 'width:80px;height:' + maxHeight + 'px;');
            $('#feedbackTemplateManagementbg tbody tr:eq(' + i + ') td:eq(0) div').attr('style',
                ' display:table-cell; vertical-align: middle; width:80px; height:' + maxHeight + 'px');
        }

        var mainreport = $("#feedbackTemplateManagementgrid");
        var gridOpts = {
            tblContentId: 'feedbackTemplateManagementbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: fbSubjectTemplate.gridHeight,
            freezeWidth: 80,
            numberFreezeCol: 1
        };

        mainreport.gridScroller(gridOpts);
    }
};

head.ready(
    function () {
        var gridOpts = {
            tblContentId: 'feedbackTemplateManagementbody',
            isShowPager: true
        };
        $("#feedbackTemplateManagementgrid").initGrid(gridOpts);

        //tmplSetting = $("#tplSetting").template();
        fbSubjectTemplate.loadSubjectTemplate();
        //get lasted feedbacks data
        getNumOfFeedbackItems();
        setInterval(function () {
            getNumOfFeedbackItems();
        }, 100000);
        //add input listen for auto complete Subject name input text box
        $("#txtTemplateSubject").keyup(function (e) {
            if (e.keyCode === 37 ||
                e.keyCode === 38 ||
                e.keyCode === 39 ||
                e.keyCode === 40) {
                return true;
            }
            else if (e.keyCode === 13) {
                fbSubjectTemplate.searchSubjectTemplate(isUseCache);
            }
            else {
                var rows = [];
                if ($(this).val() !== "") {
                    rows = jlinq.from(listFeedbacksLasted).starts("Subject", $(this).val()).sort("Subject").select();
                    rows = jlinq.from(rows).skip(0).take(15);
                    if (rows.length > 0) {
                        $(this).autoComplete(rows, "Subject");
                    }
                    else {
                        $(this).removeAutoComplete();
                    }
                }
                else {
                    $(this).removeAutoComplete();
                }
            }
        });
    }
);

