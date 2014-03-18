"use strict";
/* global showAlertMessage, lblErroTimeDate, head,
lblErrorTimeDuration,
currentDate */
/* exported arrWidth */

var arrWidth = [];
var maxHeightOfPopUp = ($(window).height() + 50);
var maxWidthOfPopUp = 1030;
var masterPage;
var crmChart;

function addDays(startDate, n) {
    var endDate = new Date();
    endDate.setTime(startDate.getTime() + n * 86400000);
    return endDate;
}

function durationTimeForceNumberic() {
    $("#txtFDDay").ForceNumericOnly();
    $("#txtFDHour").ForceNumericOnly();
    $("#txtFDMinute").ForceNumericOnly();
    $("#txtFDSecond").ForceNumericOnly();

    $("#txtTDDay").ForceNumericOnly();
    $("#txtTDHour").ForceNumericOnly();
    $("#txtTDMinute").ForceNumericOnly();
    $("#txtTDSecond").ForceNumericOnly();
}

// -- Calendar --- //
function setupCalendar(arrDateValues, arrIds) {
    var config = {
        dateFormat: 'M dd, yy',
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true
    };

    for (var i = 0; i < arrIds.length; i++) {
        if (arrDateValues[i]) {
            var datepickerElement = $('#' + arrIds[i]);
            datepickerElement.datepicker(config);
            datepickerElement.datepicker('setDate', arrDateValues[i]);
        }
    }
}

function number(i, row) {
    var n = row.Cells[i];
    var mainCatId = row.MainCategoryId;
    var subCatId = row.SubCategoryId;
    /* jshint camelcase: false */
    var formatedValue = $().number_format(n,
    {
        numberOfDecimals: 0,
        decimalSeparator: '.',
        thousandSeparator: ',',
        symbol: ''
    });
    if (row.IsToal) {
        formatedValue = '<b>' + formatedValue + '</b>';
    }
    if (n > 0) {
        if (row.MainCategoryID === 0) {
            formatedValue = "<span class='lnk b' onclick='feedbackStatistic.viewStatistics(" + mainCatId +
                "," + subCatId +
                "," + i + ")'>" + formatedValue + "</span>";
        }
        else {
            formatedValue = "<span class='lnk' onclick='feedbackStatistic.viewStatistics(" + mainCatId +
                "," + subCatId +
                "," + i + ")'>" + formatedValue + "</span>";
        }
    }

    return formatedValue;
}

var pagerOpts = {
    /* jshint camelcase: false */
    callback: null,
    start_per_page: 50,
    step_per_page: 100,
    max_items_per_page: 500,
    items_per_page: 50,
    current_page: 1
};

var feedbackStatistic = {
    fromDate: '',
    toDate: '',
    pageIndex: 1,
    pageSize: 50,
    totalRows: 0,
    arrColumns: [],
    title: '',
    type: '',
    gridHeight: 145,
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
    chartSetting: { categories: [], series: [] },
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = feedbackStatistic.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        /* jshint unused: vars */
        var max_page = Math.ceil(feedbackStatistic.totalRows / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        feedbackStatistic.pageIndex = page_index + 1;
        feedbackStatistic.pageSize = _items_per_page;
        feedbackStatistic.loadFeedback(1);
        return false;
    },
    loadFeedback: function (loadOldData) {
        $('#errmsg').html('');
        var mainreport = $("#feedbackstatisticgrid");

        feedbackStatistic.fromDate = $("#txtfromdate").val();
        feedbackStatistic.toDate = $("#txttodate").val();
        feedbackStatistic.title = 'Feedback Statistics Report from ' + feedbackStatistic.fromDate + ' to ' + feedbackStatistic.toDate;

        if (Date.parse($("#txtfromdate").val()) > Date.parse($("#txttodate").val())) {
            showAlertMessage("errmsg", lblErroTimeDate, 1);
            return false;
        }

        if (!loadOldData) {
            feedbackStatistic.pageIndex = 1;
        }

        var data = { param: feedbackStatistic.getParams(loadOldData) };

        var timeBegin = parseInt(feedbackStatistic.FromDuration.Day) * 86400 +
            parseInt(feedbackStatistic.FromDuration.Hour) * 3600 +
            parseInt(feedbackStatistic.FromDuration.Minute) * 60 +
            parseInt(feedbackStatistic.FromDuration.Second);
        var timeEnd = parseInt(feedbackStatistic.ToDuration.Day) * 86400 +
            parseInt(feedbackStatistic.ToDuration.Hour) * 3600 +
            parseInt(feedbackStatistic.ToDuration.Minute) * 60 +
            parseInt(feedbackStatistic.ToDuration.Second);

        if ((timeBegin !== 0 && timeEnd === 0) ||
            (timeBegin > timeEnd && timeBegin !== 0 && timeEnd !== 0)) {
            showAlertMessage("errmsg", lblErrorTimeDuration, 1);
            return false;
        }

        $('#feedbackstatistictitle').html(feedbackStatistic.title);

        try { masterPage.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/GetFeedbackStatistic",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null && rs.rows != null && rs.rows.length > 0) {
                    feedbackStatistic.assignChartData(rs);
                    var totalChild = rs.ColumnNames.length;
                    var divWidth = ((110 * totalChild) + totalChild * 2);

                    feedbackStatistic.arrColumns = rs.ColumnIDs;
                    feedbackStatistic.totalRows = rs.Total;

                    mainreport.removeClass("hidden");
                    //push header
                    var reportHeader = feedbackStatistic.renderFeedbackHeader(rs.ColumnNames);
                    mainreport.find(".col_header_content thead").empty().html(reportHeader);
                    //content                    
                    var arrFirstCol = [];
                    var arrScroll = [];

                    for (var i = 0; i < rs.rows.length; i++) {
                        var arrNewData = rs.rows[i];

                        if (arrNewData != null && arrNewData.FeedbackStatistics.length > 0) {
                            var mainId = arrNewData.FeedbackStatistics[0].MainCategoryId;
                            //group title
                            arrFirstCol.push(feedbackStatistic.renderFeedbackScrollGroupFirstCol(mainId, arrNewData.Title));
                            arrScroll.push(feedbackStatistic.renderFeedbackScrollGroupRow(totalChild, divWidth, mainId, arrNewData.Title));

                            for (var j = 0; j < arrNewData.FeedbackStatistics.length; j++) {
                                //content
                                var css = (j % 2 === 0 ? "bgeven" : "bgodd");
                                var totalCss = arrNewData.FeedbackStatistics[j].IsTotal === true ? 'style="font-weight: bold;"' : '';
                                arrFirstCol.push(feedbackStatistic.renderFeedbackScrollDetailFirstCol(arrNewData.FeedbackStatistics[j], mainId, css, totalCss));
                                arrScroll.push(feedbackStatistic.renderFeedbackScrollDetailRow(arrNewData.FeedbackStatistics[j], mainId, css, totalCss));
                            }
                        }
                    }
                    $("#feedbackstatisticbody tbody").empty().append(arrScroll.join(''));
                    $("#divBackground tbody").empty().append(arrFirstCol.join(''));
                    //-----grand total
                    mainreport.find(".row_footer_content thead").empty().append(
                        '<tr>\
                            <th>\
                                <div style="height:17px; width:400px;" class="table-cell l">Grand Total</div>\
                            </th>\
                         </tr>');
                    mainreport.find(".col_footer_content thead")
                              .empty()
                              .empty()
                              .append(feedbackStatistic.renderFeedbackScrollFooterRow(rs.GrandTotal, 0, ""));
                    //------------------------------

                    /*#region set height of header*/
                    var maxHeightOfGirdHeader = 30;
                    var heightOfTbBgHeader = $("#divBackground").find("thead").height() - 5;
                    var heightOfTbContentHeader = $("#feedbackstatisticbody").find("thead").height();

                    if (heightOfTbBgHeader >= 30 && heightOfTbContentHeader >= 30) {
                        if (heightOfTbBgHeader > heightOfTbContentHeader) {
                            maxHeightOfGirdHeader = heightOfTbBgHeader;
                        }
                        else {
                            maxHeightOfGirdHeader = heightOfTbContentHeader;
                        }
                    }
                    mainreport.find(".maxheight").css('height', maxHeightOfGirdHeader + 'px');
                    /*#endregion set height of header*/
                    //------------------
                    var gridOpts = {
                        tblContentId: 'feedbackstatisticbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: feedbackStatistic.gridHeight,
                        freezeWidth: 400,
                        numberFreezeCol: 1
                    };
                    mainreport.gridScroller(gridOpts);
                    feedbackStatistic.initPager(feedbackStatistic.totalRows, feedbackStatistic.pageSize, feedbackStatistic.pageIndex - 1);

                }
                else {
                    mainreport.addClass('hidden');
                    showAlertMessage("errmsg", 'There is no information.', 1);
                    $('#pagination').html('&nbsp;');
                }
            },
            error: function () {
                mainreport.addClass('hidden');
                showAlertMessage("errmsg", 'There is no information.', 1);
                $('#pagination').html('&nbsp;');
                try { masterPage.UnBlockUI(); } catch (e) { }
            },
            complete: function () {
                try { masterPage.UnBlockUI(); } catch (e) { }
            }

        });

    },
    renderFeedbackScrollGroupRow: function (totalCol, width, id, title) {
        var html = '';

        html += '<tr class="total">';
        html += '<td>\
                    <div class="l b" style="width:400px; height:17px;">\
                        <span class="tree-expanded" onclick="feedbackStatistic.toggleFeedback(this,\'' + (id) + '\')"></span>' +
                        title +
                    '</div>\
                 </td>';
        html += '<td colspan=' + totalCol + '><div style="width:' + width + 'px; height:17px;">&nbsp;</div></td></tr>';
        return html;
    },
    renderFeedbackScrollDetailRow: function (row, id, css, style) {
        var html = '';
        if (row) {
            html = '<tr class="fb_' + id + ' ' + css + '">';
            html += '<td ' + style + '><div style="width:400px; height:17px;"><span class="child">&nbsp;&nbsp;</span>' + row.SubCategory + '</div></td>';
            for (var i = 0; i < row.Cells.length; i++) {
                html += '<td ' + style + '><div style="width:110px; height:17px;" class="c ' + css + '">' + number(i, row) + '</div></td>';
            }
            html += '</tr>';
        }
        return html;
    },
    renderFeedbackScrollGroupFirstCol: function (id, title) {
        var html = '';

        html += '<tr class="total">';
        html += '<td>\
                    <div class="l b" style="width:400px; height:17px;">\
                        <span class="tree-expanded" onclick="feedbackStatistic.toggleFeedback(this,\'' + (id) + '\')"></span>' +
                        title +
                    '</div>\
                 </td>';
        return html;
    },
    renderFeedbackScrollDetailFirstCol: function (row, id, css, style) {
        var html = '';
        if (row) {
            html = '<tr class="fb_' + id + ' ' + css + '">';
            html += '<td ' + style + '><div style="width:400px; height:17px;"><span class="child">&nbsp;&nbsp;</span>' + row.SubCategory + '</div></td>';
            html += '</tr>';
        }
        return html;
    },
    renderFeedbackScrollFooterRow: function (row, id, css) {
        /* jshint unused: vars */
        var html = '';
        if (row) {
            html = '<tr>';
            html += '<th><div class="table-cell" style="width:400px; height:17px">&nbsp;</div></th>';
            for (var i = 0; i < row.Cells.length; i++) {
                html += '<th><div style="width:110px; height:17px;" class="table-cell c">' + number(i, row) + '</div></th>';
            }
            html += '</tr>';
        }
        return html;
    },
    toggleFeedback: function (obj, id) {
        var mainreport = $('#feedbackstatisticgrid');
        if ($(obj).hasClass("tree-expand")) {
            $(obj).removeClass("tree-expand");
            $("#feedbackstatisticbody tbody tr.fb_" + id).removeClass("hidden");
            $("#divBackground tbody tr.fb_" + id).removeClass("hidden");
        }
        else {
            $("#feedbackstatisticbody tbody tr.fb_" + id).addClass("hidden");
            $("#divBackground tbody tr.fb_" + id).addClass("hidden");

            $(obj).addClass("tree-expand");
        }

        var gridOpts = {
            tblContentId: 'feedbackstatisticbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: feedbackStatistic.gridHeight,
            freezeWidth: 400,
            numberFreezeCol: 1
        };

        mainreport.gridScroller(gridOpts);
    },
    renderFeedbackHeader: function (row) {
        var html = '<tr>';
        html += '<th><div class="table-cell maxheight" style="width:400px;" >Subcategory</div></th>';
        for (var i = 0; i < row.length; i++) {
            html += '<th><div class="table-cell maxheight" style="width:110px;">' + row[i] + '</div></th>';
        }
        html += '</tr>';
        return html;
    },
    viewStatistics: function (mainCatId, subCatId, index) {
        var fromDate = $('#txtfromdate').val();
        var toDate = $('#txttodate').val();

        feedbackStatistic.type = parseInt($('#ddlType').val());
        var valueid = feedbackStatistic.arrColumns[index];

        var url = "../Reports/FeedbackStatisticDetail?type=" + feedbackStatistic.type + "&valueid=" + valueid +
        "&maincatid=" + mainCatId + "&subcatid=" + subCatId + "&fromdate=" + fromDate + "&todate=" + toDate +
        '&fdd=' + feedbackStatistic.FromDuration.Day +
        '&fdh=' + feedbackStatistic.FromDuration.Hour + '&fdmi=' + feedbackStatistic.FromDuration.Minute +
        '&fds=' + feedbackStatistic.FromDuration.Second +
        '&tdd=' + feedbackStatistic.ToDuration.Day + '&tdh=' + feedbackStatistic.ToDuration.Hour +
        '&tdmi=' + feedbackStatistic.ToDuration.Minute + '&tds=' + feedbackStatistic.ToDuration.Second;

        masterPage.OpenPopup(url, "Feedback Statistic Detail Report", maxWidthOfPopUp, maxHeightOfPopUp, true, false, window, 2000, null, null);
    },
    export2excel: function () {
        var url = "FeedbackStatistics?export=true&type=" + parseInt($('#ddlType').val()) +
            "&fdate=" + feedbackStatistic.fromDate +
            "&tdate=" + feedbackStatistic.toDate +
            "&title=" + feedbackStatistic.title;
        $("#frm").attr("src", url);

    },
    getParams: function (loadOldData) {

        if ($('#txtFDDay').val().length > 0) {
            feedbackStatistic.FromDuration.Day = $('#txtFDDay').val();
        }
        else {
            feedbackStatistic.FromDuration.Day = 0;
        }

        if ($('#txtFDHour').val().length > 0) {
            feedbackStatistic.FromDuration.Hour = $('#txtFDHour').val();
        }
        else {
            feedbackStatistic.FromDuration.Hour = 0;
        }

        if ($('#txtFDMinute').val().length > 0) {
            feedbackStatistic.FromDuration.Minute = $('#txtFDMinute').val();
        }
        else {
            feedbackStatistic.FromDuration.Minute = 0;
        }

        if ($('#txtFDSecond').val().length > 0) {
            feedbackStatistic.FromDuration.Second = $('#txtFDSecond').val();
        }
        else {
            feedbackStatistic.FromDuration.Second = 0;
        }

        if ($('#txtTDDay').val().length > 0) {
            feedbackStatistic.ToDuration.Day = $('#txtTDDay').val();
        }
        else {
            feedbackStatistic.ToDuration.Day = 0;
        }

        if ($('#txtTDHour').val().length > 0) {
            feedbackStatistic.ToDuration.Hour = $('#txtTDHour').val();
        }
        else {
            feedbackStatistic.ToDuration.Hour = 0;
        }

        if ($('#txtTDMinute').val().length > 0) {
            feedbackStatistic.ToDuration.Minute = $('#txtTDMinute').val();
        }
        else {
            feedbackStatistic.ToDuration.Minute = 0;
        }

        if ($('#txtTDSecond').val().length > 0) {
            feedbackStatistic.ToDuration.Second = $('#txtTDSecond').val();
        }
        else {
            feedbackStatistic.ToDuration.Second = 0;
        }

        var params = {
            'Type': parseInt($('#ddlType').val()),
            'FromDate': feedbackStatistic.fromDate,
            'ToDate': feedbackStatistic.toDate,
            'FromCache': loadOldData,
            'ItemPerPage': feedbackStatistic.pageSize,
            'CurrentPage': feedbackStatistic.pageIndex,
            'FromDuration': feedbackStatistic.FromDuration,
            'ToDuration': feedbackStatistic.ToDuration
        };

        return params;
    },
    assignChartData: function (rs) {
        var categories = [];
        var series = [];

        if (rs && rs.rows) {
            var totalMaincategories = rs.rows.length;
            var totalColumnsName = rs.ColumnNames.length;

            for (var i = 0; i < totalColumnsName; i++) {
                series.push({ name: rs.ColumnNames[i], data: [] });
            }

            for (var k = 0; k < totalMaincategories; k++) {
                var category = rs.rows[k];
                if (category && category.FeedbackStatistics) {
                    var totalSubCategoris = category.FeedbackStatistics.length - 1;
                    categories.push(category.FeedbackStatistics[0].MainCategory);

                    for (var j = 0; j < totalColumnsName; j++) {
                        series[j].data.push(category.FeedbackStatistics[totalSubCategoris].Cells[j]);
                    }
                }
            }

            this.chartSetting.categories = categories;
            this.chartSetting.series = series;
        }
    },
    viewAllStatisticsDetail: function () {
        var fromDate = $('#txtfromdate').val();
        var toDate = $('#txttodate').val();
        feedbackStatistic.type = parseInt($('#ddlType').val());

        var inputParams =
                    "type=" + feedbackStatistic.type +
                    "&fromDate=" + fromDate +
                    "&toDate=" + toDate +
                    "&fDay=" + feedbackStatistic.FromDuration.Day +
                    "&fHour=" + feedbackStatistic.FromDuration.Hour +
                    "&fMinute=" + feedbackStatistic.FromDuration.Minute +
                    "&fSecond=" + feedbackStatistic.FromDuration.Second +
                    "&tDay=" + feedbackStatistic.ToDuration.Day +
                    "&tHour=" + feedbackStatistic.ToDuration.Hour +
                    "&tMinute=" + feedbackStatistic.ToDuration.Minute +
                    "&tSecond=" + feedbackStatistic.ToDuration.Second;

        var url = "../Reports/FeedbackStatisticsDetailView?" + inputParams;
        masterPage.OpenPopup(url, "Feedback Statistic Detail Report", (maxWidthOfPopUp + 15), maxHeightOfPopUp, true, false, window, 2000, null, null);
    },
    viewChart: function () {
        crmChart.title = $('#feedbackstatistictitle').html();
        var chartOptions = {
            xAxis: {
                categories: this.chartSetting.categories
            },
            series: this.chartSetting.series,
            tooltip: {
                valueSuffix: ' Feedback(s)'
            }
        };

        crmChart.viewChart(chartOptions);
    }
};

head.ready(function () {
    var tdate = new Date(currentDate);
    var fdate = addDays(tdate, -7);

    var arrDateValues = [fdate, tdate];
    var arrIds = ['txtfromdate', 'txttodate'];
    setupCalendar(arrDateValues, arrIds);
    masterPage = parent;

    durationTimeForceNumberic();

    try { masterPage.UnBlockUI(); } catch (e) { }

    var gridOpts = {
        tblContentId: 'feedbackstatisticbody',
        isShowPager: true
    };
    $("#feedbackstatisticgrid").initGrid(gridOpts);

    crmChart = parent.crmChart;
});