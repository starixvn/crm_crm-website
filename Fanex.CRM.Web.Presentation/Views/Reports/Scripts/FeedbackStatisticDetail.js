"use strict";
/* global replaceSpaceForIE9, gup,
currentPage */
/* exported openEditFeedback */

var maxHeightOfPopUp = ($(window).height() + 50);
var maxWidthOfPopUp = 980;
var feedbackDetail;
var masterPage;

function viewStatistics() {
    var url = "../Reports/FeedbackStatisticDetail?type=" + feedbackDetail.type +
    "&valueid=" + feedbackDetail.valueId +
    "&maincatid=" + feedbackDetail.mainCategoryId +
    "&subcatid=" + feedbackDetail.subCategoryId +
    "&fromdate=" + feedbackDetail.fDate +
    "&todate=" + feedbackDetail.tDate +
    "&pageindex=" + feedbackDetail.pageIndex +
    "&pagesize=" + feedbackDetail.pageSize +
    '&fdd=' + feedbackDetail.FromDuration.Day +
    '&fdh=' + feedbackDetail.FromDuration.Hour +
    '&fdmi=' + feedbackDetail.FromDuration.Minute +
    '&fds=' + feedbackDetail.FromDuration.Second +
    '&tdd=' + feedbackDetail.ToDuration.Day +
    '&tdh=' + feedbackDetail.ToDuration.Hour +
    '&tdmi=' + feedbackDetail.ToDuration.Minute +
    '&tds=' + feedbackDetail.ToDuration.Second;
    
    masterPage.OpenPopup(url, "Feedback Statistic Detail Report", 1030, (maxHeightOfPopUp + 5), true, false, null, 2000, null);
}

function openEditFeedback(url) {
    masterPage.OpenPopup(url, "Update Feedback", maxWidthOfPopUp, maxHeightOfPopUp, true, false, currentPage, 2000,
        function () {
            viewStatistics();
        },
        function () {
            viewStatistics();
        }
    );
}

var pagerOpts = {
    /* jshint camelcase: false */
    callback: null,
    start_per_page: 50,
    step_per_page: 50,
    max_items_per_page: 500,
    items_per_page: 50,
    current_page: 1
};

feedbackDetail = {
    totalRows: 0,
    pageIndex: 1,
    pageSize: 100,
    fDate: "",
    tDate: "",
    type: 0,
    valueId: 0,
    mainCategoryId: 0,
    subCategoryId: 0,
    title: '',
    gridHeight: 20,
    FromDuration: {
        Day: 0,
        Hour: 0,
        Minute: 0,
        Second: 0
    },
    ToDuration: {
        Day: 0,
        Hour: 0,
        Minute: 0,
        Second: 0
    },
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = feedbackDetail.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        /* jshint unused: vars */
        var max_page = Math.ceil(feedbackDetail.totalRows / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        feedbackDetail.pageIndex = page_index + 1;
        feedbackDetail.pageSize = _items_per_page;
        feedbackDetail.loadFeedback(true);
        return false;
    },
    loadFeedback: function (loadOldData) {
        if (!loadOldData) {
            feedbackDetail.pageIndex = 1;
        }
        var data = { 'param': feedbackDetail.getParams() };

        feedbackDetail.title = 'Feedback Statistic Detail Report from ' + feedbackDetail.fDate + ' to ' + feedbackDetail.tDate;
        $("#gridtitle").html(feedbackDetail.title);

        try { masterPage.BlockUI(); } catch (e) { }
        var mainreport = $("#feedbackstatisticgrid");
        $('#errmsg').html('');

        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/GetFeedbackStatisticDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null && rs.rows != null && rs.rows.length > 0) {
                    var arr = [];
                    var arrFrozen = [];
                    feedbackDetail.totalRows = parseInt(rs.total);
                    for (var i = 0; i < rs.rows.length; i++) {
                        arr.push(feedbackDetail.renderFeedbackRow(rs.rows[i]));
                        arrFrozen.push(feedbackDetail.renderFeedbackLeftRow(rs.rows[i]));
                    }

                    $("#feedbackstatisticheader tbody").empty().append(arrFrozen.join(''));
                    $("#feedbackstatisticbody tbody").empty().append(arr.join(''));

                    var gridOpts = {
                        tblContentId: 'feedbackstatisticbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: feedbackDetail.gridHeight,
                        freezeWidth: 380,
                        numberFreezeCol: 1
                    };
                    mainreport.removeClass("hidden").gridScroller(gridOpts);
                    feedbackDetail.initPager(feedbackDetail.totalRows, feedbackDetail.pageSize, feedbackDetail.pageIndex - 1);
                }
                else {
                    mainreport.addClass('hidden');
                    $('#errmsg').html('<div class="alertboxerr"><div>There is no information.</div></div>');
                    $('#pagination').html('&nbsp;');
                }
            },
            error: function () {
                mainreport.addClass('hidden');
                $('#errmsg').html('<div class="alertboxerr"><div>There is no information.</div></div>');
                $('#pagination').html('&nbsp;');
            },
            complete: function () {
                try { masterPage.UnBlockUI(); } catch (e) { }

            }
        });
    },
    renderFeedbackRow: function (row) {
        var trStyle = '';
        if (row.Status != null) {
            trStyle = row.Status.toLowerCase() === 'pending' ?
                'pending' :
                     row.Status.toLowerCase() === 'closed' ?
                    'closed' :
                    '';
        }
        var html = '<tr class="' + trStyle + '">\
                       <td>\
                            <div style="width:380px">\
                                <div class="lnksubject">\
                                    <a class="desTooltip" href="JavaScript:void(0);">\
                                        <b>' + row.Subject + '</b>\
                                    </a>\
                                </div>\
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Feedback date <b>' + row.FeedbackDate + '</b>\
                                <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;By&nbsp;<b>' + row.Creator + '</b> on ' +
                                row.CreateDate +
                                '<br>' + feedbackDetail.durationTimeFormat(row.DurationTime) +
                                '<br>' + feedbackDetail.reviewerFormat(row.Reviewer, row.ReviewDate) +
                            '</div>\
                        </td>\
                        <td><div class="c" style="width:120px">' + row.Username + '</div></td>\
                        <td><div style="width:200px">' + row.MainCategory + '<br/>&nbsp;&nbsp;&nbsp;' + row.SubCategory + '</div></td>\
                        <td><div class="c" style="width:60px">' + row.Status + '</div></td>\
                        <td><div class="c" style="width:180px">' + row.RelatedUsername + '</div></td>\
                </tr>';
        return replaceSpaceForIE9(html);
    },
    renderFeedbackLeftRow: function (row) {
        var trStyle = '';
        if (row.Status != null) {
            trStyle = row.Status.toLowerCase() === 'pending' ? 'pending' : row.Status.toLowerCase() === 'closed' ? 'closed' : '';
        }
        var html = '<tr class="' + trStyle + '">\
                       <td>\
                            <div style="width:380px">\
                                <div class="lnksubject">\
                                    <a class="desTooltip" href="JavaScript:void(0);" onclick="openEditFeedback(\'../Feedback/EditFeedback?id=' +
                                        row.ID + '&rep=2\', 1);">\
                                        <b>' + row.Subject + '</b>\
                                    </a>\
                                </div>\
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Feedback&nbsp;date&nbsp;<b>' + row.FeedbackDate + '</b>\
                                <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;By&nbsp;<b>' + row.Creator + '</b> on ' +
                                row.CreateDate +
                                '<br>' + feedbackDetail.durationTimeFormat(row.DurationTime) +
                                '<br>' + feedbackDetail.reviewerFormat(row.Reviewer, row.ReviewDate) +
                            '</div>\
                        </td>\
                </tr>';
        return html;
    },
    reviewerFormat: function (reviewer, reviewDate) {
        var html = '';
        if (reviewer !== "") {
            html = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Reviewed&nbsp;by&nbsp;<b>' + reviewer + '</b>&nbsp;on&nbsp;' + reviewDate + '<br>';
        }
        return html;
    },
    openConfirmPopup: function () {
        $("#divPopup").dialog({ width: 250, height: 120, zIndex: 500, resizable: false,
            close: function (event, ui) {
                /* jshint unused: vars */
            },
            buttons: [{
                text: "Ok",
                click: function () {
                    $(this).dialog("close");
                }
            }]
        });
    },
    export2excel: function () {

        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/AllowExportFeedbackStatisticDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                if (rs) {
                    if (rs.ErrCode === 0) {
                        var url = 'FeedbackStatisticDetail?export=true&type=' + feedbackDetail.type +
                            '&valueId=' + feedbackDetail.valueId +
                            '&mainCateId=' + feedbackDetail.mainCategoryId +
                            '&subCateId=' + feedbackDetail.subCategoryId +
                            '&fdate=' + feedbackDetail.fDate +
                            '&tdate=' + feedbackDetail.tDate +
                            '&title=' + feedbackDetail.title;
                        $('#frm').attr('src', url);
                    }
                    else {
                        feedbackDetail.openConfirmPopup();
                    }
                }
                else {
                    feedbackDetail.openConfirmPopup();
                }
            },
            error: function () {

            },
            complete: function () {

            }
        });
    },
    getParams: function (loadOldData) {
        var params = {
            'Type': feedbackDetail.type,
            'ValueId': feedbackDetail.valueId,
            'MainId': feedbackDetail.mainCategoryId,
            'SubId': feedbackDetail.subCategoryId,
            'FromDate': feedbackDetail.fDate,
            'ToDate': feedbackDetail.tDate,
            'FromCache': loadOldData,
            'ItemPerPage': feedbackDetail.pageSize,
            'CurrentPage': feedbackDetail.pageIndex,
            'FromDuration': feedbackDetail.FromDuration,
            'ToDuration': feedbackDetail.ToDuration
        };
        return params;
    },
    durationTimeFormat: function (time) {
        var html = '';
        if (time) {
            html = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Solving Duration: " + time;
        }
        return html;
    }
};

$(document).ready(
    function () {
        masterPage = parent;

        feedbackDetail.fDate = decodeURIComponent(gup("fromdate").replace(/\+/g, " "));
        feedbackDetail.tDate = decodeURIComponent(gup("todate").replace(/\+/g, " "));
        feedbackDetail.type = parseInt(gup("type"));
        feedbackDetail.valueId = parseInt(gup("valueid"));
        feedbackDetail.mainCategoryId = parseInt(gup("maincatid"));
        feedbackDetail.subCategoryId = parseInt(gup("subcatid"));

        feedbackDetail.FromDuration.Day = parseInt(gup("fdd"));
        feedbackDetail.FromDuration.Hour = parseInt(gup("fdh"));
        feedbackDetail.FromDuration.Minute = parseInt(gup("fdmi"));
        feedbackDetail.FromDuration.Second = parseInt(gup("fds"));

        feedbackDetail.ToDuration.Day = parseInt(gup("tdd"));
        feedbackDetail.ToDuration.Hour = parseInt(gup("tdh"));
        feedbackDetail.ToDuration.Minute = parseInt(gup("tdmi"));
        feedbackDetail.ToDuration.Second = parseInt(gup("tds"));
        feedbackDetail.loadFeedback(false);

        var gridOpts = {
            tblContentId: 'feedbackstatisticbody',
            isShowPager: true
        };
        $("#feedbackstatisticgrid").initGrid(gridOpts);

        
    }
);