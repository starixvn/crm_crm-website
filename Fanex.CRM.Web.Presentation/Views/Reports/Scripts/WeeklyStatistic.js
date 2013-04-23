var topwnd;
var arrWidth = new Array();
var maxHeightOfSummaryReport = ($(window).height() - 50) / 2;
var maxHeightOfPopUp = ($(window).height() + 50)

//add n number of days
Date.prototype.goto = function (n) {
    this.setTime(this.getTime() + n * 86400000);
    return this;
};

// -- Calendar --- //
function SetupCalendar(arrDateValues, arrIds) {

    for (var i = 0; i < arrIds.length; i++) {

        $('#' + arrIds[i]).datepicker();
        if (arrDateValues[i] != null && arrDateValues[i] != "") {
            $('#' + arrIds[i]).val(arrDateValues[i]).datepicker('setDate', (new Date(arrDateValues[i])));
        }
    }
};

var dailyReport = {
    fromDate: '',
    toDate: '',
    defaultTotalCols: 6,
    columnWidthDefault: 100,
    maxHeigh: 260,
    getReportData: function () {
        dailyReport.fromDate = $('#txtfromdate').val();
        dailyReport.toDate = $('#txttodate').val();

        ibcDailySummaryReport.getDailyReport();
        licenseeDailySummaryReport.getDailyReport();
    }
};

var ibcDailySummaryReport = {
    title: '',
    loadCompleted: false,
    colWidth:70,
    getDailyReport: function () {
        var mainreport = $("#dailysummarygrid");
        mainreport.addClass('hidden');
        $('#dailydetailgrid').addClass('hidden');
        $('#err').html('');

        ibcDailySummaryReport.loadCompleted = false;

        try { topwnd.BlockUI(); } catch (e) { }
        var data = { 'fromDate': dailyReport.fromDate, 'toDate': dailyReport.toDate };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/GetIBCDailySummaryReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null && rs.DailySummaries && rs.DailySummaries.length > 0) {
                    ibcDailySummaryReport.title = 'IBC Daily Report From ' + dailyReport.fromDate + ' To ' + dailyReport.toDate;
                    $('#gridtitle').html(ibcDailySummaryReport.title);
                    ibcDailySummaryReport.assignChartData(rs);

                    //push header
                    var reportHeader = ibcDailySummaryReport.renderHeaderReport(rs.GrandTotal.Cells);
                    mainreport.find(".col_header_content thead").empty().html(reportHeader);
                    //---------------------------
                    var arrFrozen = [];
                    var arrScroll = [];
                    for (var i = 0; i < rs.DailySummaries.length; i++) {
                        var arrNewData = rs.DailySummaries[i];

                        if (arrNewData != null && arrNewData.DailySummaries.length > 0) {
                            var mainId = arrNewData.Total.MainCategoryId;
                            //group title
                            arrFrozen.push(ibcDailySummaryReport.renderFrozenGroup(mainId, arrNewData.Total.MainCategory));
                            arrScroll.push(ibcDailySummaryReport.renderScrollGroup(arrNewData.Total, mainId, '', 'style="font-weight: bold;"', arrNewData.Total.MainCategory));

                            for (var j = 0; j < arrNewData.DailySummaries.length; j++) {
                                //content
                                var css = (j % 2 == 0 ? "bgeven" : "bgodd");
                                var totalCss = arrNewData.DailySummaries[j].IsTotal == true ? 'style="font-weight: bold;"' : '';
                                arrFrozen.push(ibcDailySummaryReport.renderFrozenReport(arrNewData.DailySummaries[j], mainId, css, totalCss));
                                arrScroll.push(ibcDailySummaryReport.renderScollReport(arrNewData.DailySummaries[j], mainId, css, totalCss));
                            }
                        }
                    }
                    $("#dailysummarybody tbody").empty().append(arrScroll.join(''));
                    $("#dailysummarybg tbody").empty().append(arrFrozen.join(''));
                    //-----grand total
                    mainreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:17px; width:300px;" class="table-cell l">Grand Total</div></th></tr>');
                    mainreport.find(".col_footer_content thead").empty().empty().html(ibcDailySummaryReport.renderScollFooterReport(rs.GrandTotal, 0, "", ''));
                    //------------------------------                   
                    var gridOpts = {
                        tblContentId: 'dailysummarybody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: false,
                        gridHeight: maxHeightOfSummaryReport,
                        freezeWidth: 300,
                        numberFreezeCol: 1
                    };
                    mainreport.removeClass('hidden').gridScroller(gridOpts);
                }
                else {
                    $('<div id="errmsg" class="errmsg"><div class="alertboxerr"><div>There is no information of Credit site.</div></div></div>').appendTo($("#err"));
                }
            },
            error: function () { try { topwnd.UnBlockUI(); } catch (e) { } },
            complete: function () {
                ibcDailySummaryReport.loadCompleted = true;
                if (ibcDailySummaryReport.loadCompleted && licenseeDailySummaryReport.loadCompleted)
                    try { topwnd.UnBlockUI(); } catch (e) { }
            }
        });
    },
    renderHeaderReport: function (row) {
        var html = '<tr>';
        html += '<th><div class="table-cell" style="width:300px; height:30px;">Complaints/Enquiries</div></th>';
        for (var i = 0; i < row.length; i++) {
            html += '<th><div class="table-cell" class="c" style="width: ' + ibcDailySummaryReport.colWidth + 'px; height:30px;">' + row[i].Title + '</div></th>';
        }
        html += '<th><div class="table-cell" class="c" style="width: ' + ibcDailySummaryReport.colWidth + 'px; height:30px;">Grand Total</div></th>';
        html += '</tr>';

        return html;
    },
    renderFrozenGroup: function (id, title) {
        var html = '<tr class="total">\
                        <td><div class="l b" style="width: 300px"><span class="tree-expanded" onclick="ibcDailySummaryReport.toggleFeedback(this,\'' + (id) + '\')"></span>' + title + '</div></td>\
                    </tr>';
        return html;
    },
    renderFrozenReport: function (row, id, css, style) {
        var html = '';
        if (row != null) {
            html += '<tr class="fb_' + id + ' ' + css + '">\
                    <td ' + style + '><div style="width:300px;"><span class="child">&nbsp;&nbsp;</span>' + row.MainCategory + '</div></td>\
                    </tr>';
        }
        return html;
    },
    renderScrollGroup: function (row, id, css, style, title) {
        var html = '<tr class="total">';
        html += '<td><div class="l b" style="width: 300px"><span class="tree-expanded" onclick="ibcDailySummaryReport.toggleFeedback(this,\'' + (id) + '\')"></span>' + title + '</div></td>';

        for (var i = 0; i < row.Cells.length; i++) {
            html += '<td ' + style + '><div style="width:' + ibcDailySummaryReport.colWidth + 'px" class="c ' + css + '">' + row.Cells[i].Value + '</div></td>';
        }
        html += '<td ' + style + '><div style="width:' + ibcDailySummaryReport.colWidth + 'px" class="c ' + css + '">' + row.Total + '</div></td>';
        html += '</tr>';

        return html;
    },
    renderScollReport: function (row, id, css, style) {
        var html = '<tr class="fb_' + id + ' ' + css + '">';
        html += '<td ' + style + '><div style="width:300px;"><span class="child">&nbsp;&nbsp;</span>' + row.MainCategory + '</div></td>';

        for (var i = 0; i < row.Cells.length; i++) {
            html += '<td ' + style + '><div style="width:' + ibcDailySummaryReport.colWidth + 'px" class="c ' + css + '">' + row.Cells[i].Value + '</div></td>';
        }
        html += '<td ' + style + '><div style="width:' + ibcDailySummaryReport.colWidth + 'px" class="c ' + css + '">' + row.Total + '</div></td>';
        html += '</tr>';

        return html;
    },
    renderScollFooterReport: function (row, id, css, style) {
        var html = '<tr class="' + css + '">';
        html += '<th><div class="table-cell" style="width:300px; height:17px;"><span class="child">&nbsp;&nbsp;</span>' + row.MainCategory + '</div></th>';

        for (var i = 0; i < row.Cells.length; i++) {
            html += '<th><div style="width:' + ibcDailySummaryReport.colWidth + 'px;" class="table-cell c ' + css + '">' + row.Cells[i].Value + '</div></th>';
        }
        html += '<th><div style="width:' + ibcDailySummaryReport.colWidth + 'px" class="table-cell c ' + css + '">' + row.Total + '</div></th>';
        html += '</tr>';

        return html;
    },
    toggleFeedback: function (obj, id) {
        var mainreport = $('#dailysummarygrid');
        if ($(obj).hasClass("tree-expand")) {
            $(obj).removeClass("tree-expand");
            $("#dailysummarybody tbody tr.fb_" + id).removeClass("hidden");
            $("#dailysummarybg tbody tr.fb_" + id).removeClass("hidden");
        }
        else {
            $("#dailysummarybody tbody tr.fb_" + id).addClass("hidden");
            $("#dailysummarybg tbody tr.fb_" + id).addClass("hidden");

            $(obj).addClass("tree-expand");
        }

        var gridOpts = {
            tblContentId: 'dailysummarybody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: false,
            gridHeight: 200,
            freezeWidth: 300,
            numberFreezeCol: 1
        };

        mainreport.gridScroller(gridOpts);
    },
    viewDetail: function () {
        ibcDailyDetailReport.getDailyDetailReport(false);
    },
    exportExcel: function () {
        var url = "DailyStatistic?export=true&type=1&fdate=" + dailyReport.fromDate + "&tdate=" + dailyReport.toDate + "&title=" + ibcDailySummaryReport.title;
        $("#frm").attr("src", url);
    },
    assignChartData: function (rs) {
        var items = [];
        for (var i = 0; i < rs.DailySummaries.length; i++) {
            var item = rs.DailySummaries[i];
            var chartInfo = {};
            eval("chartInfo.Name='" + item.Total.MainCategory + "'");
            for (var j = 0; j < item.Total.Cells.length; j++) {
                var cell = item.Total.Cells[j];
                eval("chartInfo." + cell.Title + "=" + cell.Value);
            }
            items.push(chartInfo);
        }
        ibcDailySummaryReport.chart.setData(items);
    }
};

var licenseeDailySummaryReport = {
    totalWidth: 0,
    title: '',
    colWidth:70,
    loadCompleted: false,
    getDailyReport: function () {
        var mainreport = $("#licenseedailysummarygrid");
        mainreport.addClass('hidden');
        $('#licenseedetailgrid').addClass('hidden');
        $('#err').html('');
        licenseeDailySummaryReport.loadCompleted = false;

        try { topwnd.BlockUI(); } catch (e) { }
        var data = { 'fromDate': dailyReport.fromDate, 'toDate': dailyReport.toDate };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/GetLicenseeDailySummaryReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null && rs.DailySummaries && rs.DailySummaries.length > 0) {
                    licenseeDailySummaryReport.title = 'Licensee Daily Summary Report From ' + dailyReport.fromDate + ' To ' + dailyReport.toDate;
                    $('#licenseeSummaryTitle').html(licenseeDailySummaryReport.title);
                    licenseeDailySummaryReport.assignChartData(rs);
                    //push header
                    var reportHeader = licenseeDailySummaryReport.renderHeaderReport(rs.GrandTotal.Cells);
                    mainreport.find(".col_header_content thead").empty().html(reportHeader);
                    //-----------------------

                    var arrFrozen = [];
                    var arrScroll = [];
                    for (var i = 0; i < rs.DailySummaries.length; i++) {
                        var arrNewData = rs.DailySummaries[i];

                        if (arrNewData != null && arrNewData.DailySummaries.length > 0) {
                            var mainId = arrNewData.Total.MainCategoryId;
                            //group title
                            arrFrozen.push(licenseeDailySummaryReport.renderFrozenGroup(mainId, arrNewData.Total.MainCategory));
                            arrScroll.push(licenseeDailySummaryReport.renderScrollGroup(arrNewData.Total, mainId, '', 'style="font-weight: bold;"', arrNewData.Total.MainCategory));

                            for (var j = 0; j < arrNewData.DailySummaries.length; j++) {
                                //content
                                var css = (j % 2 == 0 ? "bgeven" : "bgodd");
                                var totalCss = arrNewData.DailySummaries[j].IsTotal == true ? 'style="font-weight: bold;"' : '';
                                arrFrozen.push(licenseeDailySummaryReport.renderFrozenReport(arrNewData.DailySummaries[j], mainId, css, totalCss));
                                arrScroll.push(licenseeDailySummaryReport.renderScollReport(arrNewData.DailySummaries[j], mainId, css, totalCss));
                            }
                        }
                    }

                    $("#licenseedailysummarybody tbody").empty().append(arrScroll.join(''));
                    $("#licenseedailysummarybg tbody").empty().append(arrFrozen.join(''));
                    //-----grand total
                    mainreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:17px; width:300px;" class="table-cell l">Grand Total</div></th></tr>');
                    mainreport.find(".col_footer_content thead").empty().empty().html(ibcDailySummaryReport.renderScollFooterReport(rs.GrandTotal, 0, "", ''));
                    //------------------------------                   
                    var gridOpts = {
                        tblContentId: 'licenseedailysummarybody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: false,
                        gridHeight: maxHeightOfSummaryReport,
                        freezeWidth: 300,
                        numberFreezeCol: 1
                    };
                    mainreport.removeClass('hidden').gridScroller(gridOpts);

                }
                else {
                    $('<div id="errmsg" class="errmsg"><div class="alertboxerr"><div>There is no information of Licensee.</div></div></div>').appendTo($("#err"));
                }
            },
            error: function () { try { topwnd.UnBlockUI(); } catch (e) { } },
            complete: function () {
                licenseeDailySummaryReport.loadCompleted = true;
                if (licenseeDailySummaryReport.loadCompleted && ibcDailySummaryReport.loadCompleted)
                    try { topwnd.UnBlockUI(); } catch (e) { }
            }
        });
    },
    renderHeaderReport: function (row) {
        var html = '<tr>';
        html += '<th><div class="table-cell" style="width:300px; height:30px;">Complaints/Enquiries</div></th>';
        for (var i = 0; i < row.length; i++) {
            html += '<th><div class="table-cell" class="c" style="width: ' + licenseeDailySummaryReport.colWidth + 'px; height:30px;">' + row[i].Title + '</div></th>';
        }
        html += '<th><div class="table-cell" class="c" style="width: ' + licenseeDailySummaryReport.colWidth + 'px; height:30px;">Grand Total</div></th>';
        html += '</tr>';

        return html;
    },
    renderFrozenGroup: function (id, title) {
        var html = '<tr class="total">\
                        <td><div class="l b" style="width: 300px"><span class="tree-expanded" onclick="licenseeDailySummaryReport.toggleFeedback(this,\'' + (id) + '\')"></span>' + title + '</div></td>\
                    </tr>';
        return html;
    },
    renderFrozenReport: function (row, id, css, style) {
        var html = '';
        if (row != null) {
            html += '<tr class="ls_' + id + ' ' + css + '">\
                    <td ' + style + '><div style="width:300px;"><span class="child">&nbsp;&nbsp;</span>' + row.MainCategory + '</div></td>\
                    </tr>';
        }
        return html;
    },
    renderScrollGroup: function (row, id, css, style, title) {
        var html = '<tr class="total">';
        html += '<td><div class="l b" style="width: 300px"><span class="tree-expanded" onclick="licenseeDailySummaryReport.toggleFeedback(this,\'' + (id) + '\')"></span>' + title + '</div></td>';

        for (var i = 0; i < row.Cells.length; i++) {
            html += '<td ' + style + '><div style="width:' + licenseeDailySummaryReport.colWidth + 'px" class="c ' + css + '">' + row.Cells[i].Value + '</div></td>';
        }
        html += '<td ' + style + '><div style="width:' + licenseeDailySummaryReport.colWidth + 'px" class="c ' + css + '">' + row.Total + '</div></td>';
        html += '</tr>';

        return html;
    },
    renderScollReport: function (row, id, css, style) {
        var html = '<tr class="ls_' + id + ' ' + css + '">';
        html += '<td ' + style + '><div style="width:300px;"><span class="child">&nbsp;&nbsp;</span>' + row.MainCategory + '</div></td>';

        for (var i = 0; i < row.Cells.length; i++) {
            html += '<td ' + style + '><div style="width:' + licenseeDailySummaryReport.colWidth + 'px" class="c ' + css + '">' + row.Cells[i].Value + '</div></td>';
        }
        html += '<td ' + style + '><div style="width:' + licenseeDailySummaryReport.colWidth + 'px" class="c ' + css + '">' + row.Total + '</div></td>';
        html += '</tr>';

        return html;
    },
    renderScollFooterReport: function (row, id, css, style) {
        var html = '<tr class="' + css + '">';
        html += '<th><div class="table-cell" style="width:300px; height:17px;"><span class="child">&nbsp;&nbsp;</span>' + row.MainCategory + '</div></th>';

        for (var i = 0; i < row.Cells.length; i++) {
            html += '<th><div style="width:' + licenseeDailySummaryReport.colWidth + 'px;" class="table-cell c ' + css + '">' + row.Cells[i].Value + '</div></th>';
        }
        html += '<th><div style="width:' + licenseeDailySummaryReport.colWidth + 'px" class="table-cell c ' + css + '">' + row.Total + '</div></th>';
        html += '</tr>';

        return html;
    },
    toggleFeedback: function (obj, id) {
        var mainreport = $('#licenseedailysummarygrid');
        if ($(obj).hasClass("tree-expand")) {
            $(obj).removeClass("tree-expand");
            $("#licenseedailysummarybody tbody tr.ls_" + id).removeClass("hidden");
            $("#licenseedailysummarybg tbody tr.ls_" + id).removeClass("hidden");
        }
        else {
            $("#licenseedailysummarybody tbody tr.ls_" + id).addClass("hidden");
            $("#licenseedailysummarybg tbody tr.ls_" + id).addClass("hidden");

            $(obj).addClass("tree-expand");
        }

        var gridOpts = {
            tblContentId: 'licenseedailysummarybody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: false,
            gridHeight: 200,
            freezeWidth: 300,
            numberFreezeCol: 1
        };

        mainreport.gridScroller(gridOpts);
    },
    viewDetail: function () {
        licenseeDailyDetailReport.getDailyDetailReport(false);
    },
    exportExcel: function () {
        var url = "DailyStatistic?export=true&type=3&fdate=" + dailyReport.fromDate + "&tdate=" + dailyReport.toDate + "&title=" + licenseeDailySummaryReport.title;
        $("#frm").attr("src", url);
    },
    assignChartData: function (rs) {
        var items = [];
        for (var i = 0; i < rs.DailySummaries.length; i++) {
            var item = rs.DailySummaries[i];
            var chartInfo = {};
            eval("chartInfo.Name='" + item.Total.MainCategory + "'");
            for (var j = 0; j < item.Total.Cells.length; j++) {
                var cell = item.Total.Cells[j];
                eval("chartInfo." + cell.Title + "=" + cell.Value);
            }
            items.push(chartInfo);
        }
        licenseeDailySummaryReport.chart.setData(items);
    }
};

var pagerOpts = {
    callback: null,
    start_per_page: 50,
    step_per_page: 50,
    max_items_per_page: 200,
    items_per_page: 50,
    current_page: 1
};

var ibcDailyDetailReport = {
    title: '',
    totalRows: 0,
    pageIndex: 1,
    pageSize: 50,
    fromDate: '',
    toDate: '',
    gridHeight: 80,
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = ibcDailyDetailReport.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#dailydetailgrid #pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(ibcDailyDetailReport.totalRows / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        ibcDailyDetailReport.pageIndex = page_index + 1;
        ibcDailyDetailReport.pageSize = _items_per_page;
        ibcDailyDetailReport.getDailyDetailReport(true);

        return false;
    },
    getDailyDetailReport: function (loadOldData) {
        $('#dailysummarygrid').addClass('hidden');
        $('#licenseedailysummarygrid').addClass('hidden');
        $('#licenseedetailgrid').addClass('hidden');
        $('#err').html('');

        try { topwnd.BlockUI(); } catch (e) { }
        var mainreport = $('#dailydetailgrid');

        if (!loadOldData) {
            mainreport.addClass('hidden');
            ibcDailyDetailReport.pageIndex = 1;
        }
        ibcDailyDetailReport.fromDate = dailyReport.fromDate;
        ibcDailyDetailReport.toDate = dailyReport.toDate;

        var data = { 'fromDate': ibcDailyDetailReport.fromDate,
            'toDate': ibcDailyDetailReport.toDate,
            'fromCache': loadOldData,
            'itemPerPage': ibcDailyDetailReport.pageSize,
            'currentPage': ibcDailyDetailReport.pageIndex
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/GetIBCDailyDetailReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                ibcDailyDetailReport.title = 'IBC Daily Customer Complaints/Enquiries From ' + ibcDailyDetailReport.fromDate + ' To ' + ibcDailyDetailReport.toDate;
                $('#detailTitle').html(ibcDailyDetailReport.title);

                if (rs && rs.DailyComplaints && rs.DailyComplaints.length > 0) {
                    var arrScroll = [];
                    var arrFrozen = [];
                    for (var i = 0; i < rs.DailyComplaints.length; i++) {
                        ibcDailyDetailReport.totalRows = parseInt(rs.total);
                        var row = rs.DailyComplaints[i];
                        var css = (i % 2 == 0 ? "bgeven" : "bgodd"); ;

                        arrScroll.push(ibcDailyDetailReport.renderScrollRow(row, css));
                        arrFrozen.push(ibcDailyDetailReport.renderFrozenRow(row, css));
                    }

                    $('#btnExportDetail').removeClass('hidden');

                    $("#dailydetailgridbody tbody").empty().append(arrScroll.join(''));
                    $("#dailydetailfirstcol tbody").empty().append(arrFrozen.join(''));

                    var gridOpts = {
                        tblContentId: 'dailydetailgridbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: ibcDailyDetailReport.gridHeight,
                        freezeWidth: 270,
                        numberFreezeCol: 3
                    };
                    mainreport.removeClass("hidden").gridScroller(gridOpts);

                    ibcDailyDetailReport.initPager(ibcDailyDetailReport.totalRows, ibcDailyDetailReport.pageSize, ibcDailyDetailReport.pageIndex - 1);
                    ibcDailyDetailReport.reRenderGrid();

                    mainreport.find(".y-scrollbar").scrollTop(0);
                }
                else {
                    ibcDailyDetailReport.renderNoDataRow();
                }
            },
            error: function () {
                ibcDailyDetailReport.renderNoDataRow();
            },
            complete: function () { try { topwnd.UnBlockUI(); } catch (e) { } }

        });

    },
    renderScrollRow: function (row, css) {
        var compaints = replaceSpecialHtmlTag(row.Complaints);
        var solutions = replaceSpecialHtmlTag(row.Solution);
        var html = '<tr class="' + css + '">\
            <td><div class="c" style="width:60px;">&nbsp;' + row.FeedbackDate + '</div></td>\
            <td><div class="c" style="width:60px;">&nbsp;' + row.FeedbackTime + '</div></td>\
            <td><div class="c" style="width:140px;">&nbsp;' + row.CustomerName + '</div></td>\
            <td><div class="c" style="width:100px;">&nbsp;' + row.Licensee + '</div></td>\
            <td><div class="c" style="width:100px;">' + row.CommunicationTool.ContactBy + '</div></td>\
            <td><div class="c" style="width:60px;">&nbsp;' + row.ExtNo + '</div></td>\
            <td><div class="l" style="width:220px;">&nbsp;' + row.CommunicationTool.Address + '</div></td>\
            <td><div class="c" style="width:100px;">&nbsp;' + (row.ProductName) + '</div></td>\
            <td><div class="c" style="width:100px;">&nbsp;' + (row.Country ? row.Country : '&nbsp;') + '</div></td>\
            <td><div class="c" style="width:140px;">&nbsp;' + row.Category + '</div></td>\
            <td><div class="l" style="width:480px;">&nbsp;' + compaints + '</div></td>\
            <td><div class="l" style="width:480px;">&nbsp;' + solutions + '</div></td>\
            <td><div class="c" style="width:60px;">&nbsp;' + row.Status + '</div></td>\
            <td><div class="c" style="width:60px;">&nbsp;' + row.Response + '</div></td>\
            <td><div class="c" style="width:100px;">&nbsp;' + row.HandlyBy + '</div></td>\
            <td><div class="c" style="width:80px;">&nbsp;' + row.CreationTime + '</div></td>\
        </tr>';
        return replaceSpaceForIE9(html);
    },
    renderFrozenRow: function (row, css) {
        var html = '<tr class="' + css + '">\
            <td><div class="c" style="width:60px;">' + row.FeedbackDate + '</div></td>\
            <td><div class="c" style="width:60px;">' + row.FeedbackTime + '</div></td>\
            <td><div class="c" style="width:140px;">' + row.CustomerName + '</div></td>\
        </tr>';

        return html;
    },
    goBack: function () {
        $('#dailysummarygrid').removeClass('hidden');
        $("#licenseedailysummarygrid").removeClass('hidden');
        $('#dailydetailgrid').addClass('hidden');
        $('#licenseedetailgrid').addClass('hidden');
    },
    openConfirmPopup: function () {
        $('#divEmailError').addClass('hidden').html('');
        $("#divPopup").dialog({ width: 350, height: 200, zIndex: 500, resizable: false,
            close: function (event, ui) { },
            buttons: [
                    {
                        text: "Ok",
                        click: function () {
                            var emailAddress = $('#txtEmail').val().trim();
                            var validateCode = emailListValidate(emailAddress);

                            if (validateCode == 1) {
                                $('#divEmailError').removeClass('hidden').html('<div>Email cannot empty.</div>');
                                return false;
                            }
                            else if (validateCode == 2) {
                                $('#divEmailError').removeClass('hidden').html('<div>Email is invalid format.</div>');
                                return false;
                            }

                            var p = {
                                'fromDate': ibcDailyDetailReport.fromDate,
                                'toDate': ibcDailyDetailReport.toDate,
                                'reportTitle': ibcDailyDetailReport.title,
                                'isSendMail': true,
                                'email': emailAddress
                            };

                            $.ajax({
                                type: "POST",
                                url: window.rootUrl + "Reports/ExportIBCDetailReport",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify(p),
                                success: function (rs) {
                                },
                                error: function () {
                                },
                                complete: function () {
                                }
                            });
                            $(this).dialog("close");
                        }
                    }
                ]
        });
    },
    exportExcel: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/AllowExportIbcStatisticDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                if (rs) {
                    if (rs.ErrCode == 0) {
                        var url = "DailyStatistic?export=true&type=2&fdate=" + ibcDailyDetailReport.fromDate + "&tdate=" + ibcDailyDetailReport.toDate + "&title=" + ibcDailyDetailReport.title;
                        $("#frm").attr("src", url);
                    }
                    else
                        ibcDailyDetailReport.openConfirmPopup();
                }
                else
                    ibcDailyDetailReport.openConfirmPopup();
            },
            error: function () {

            },
            complete: function () {

            }
        });
    },
    reRenderGrid: function () {

        var trFrozen = $('#dailydetailfirstcol tbody tr');
        var totalRows = trFrozen.length;

        for (var i = 0; i < totalRows; i++) {
            var frozenHeight = $('#dailydetailfirstcol tbody tr:eq(' + i + ') td:eq(0)').height();
            var scrollHeight = $('#dailydetailgridbody tbody tr:eq(' + i + ') td:eq(0)').height();

            var maxHeight = 18;
            if (frozenHeight > scrollHeight)
                maxHeight = frozenHeight + 1;
            else if (scrollHeight > frozenHeight)
                maxHeight = scrollHeight + 1;

            $('#dailydetailgridbody tbody tr:eq(' + i + ') td:eq(0) div').css('height', maxHeight).append(' ');
            $('#dailydetailfirstcol tbody tr:eq(' + i + ') td:eq(0) div').attr('style', ' display:table-cell; vertical-align: middle; height:' + maxHeight + 'px');
        }

        var mainreport = $('#dailydetailgrid');
        mainreport.removeClass("hidden");
        var gridOpts = {
            tblContentId: 'dailydetailgridbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: ibcDailyDetailReport.gridHeight,
            freezeWidth: 270,
            numberFreezeCol: 3
        };
        mainreport.gridScroller(gridOpts);
    },
    getFeedbackDay: function (fullday) {
        var d = new Date(fullday);
        var month = d.getMonth() > 9 ? d.getMonth() : ('0' + d.getMonth());
        var day = d.getDate() > 9 ? d.getDate() : ('0' + d.getDate());
        var year = d.getFullYear();

        return month + '/' + day + '/' + year;
    },
    getFeedbackTime: function (fullday) {
        var d = new Date(fullday);
        return d.getHours() + ':' + d.getMinutes();
    },
    renderNoDataRow: function () {
        $('#dailydetailgrid #pagination').html('&nbsp;');
        $('#btnExportDetail').addClass('hidden');

        var html = '<tr>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:140px; height:20px;">&nbsp;</div></td>\
            <td colspan="13"><div class="c" style="width:2080px; height:20px;"><div class="alertboxerr"><div>There is no information.</div></div></td>\
        </tr>';

        var mainreport = $('#dailydetailgrid');

        $("#dailydetailfirstcol tbody").empty().html('<tr>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:140px; height:20px;">&nbsp;</div></td></tr>'
            );
        $("#dailydetailgridbody tbody").empty().html(html);
        var gridOpts = {
            tblContentId: 'dailydetailgridbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: ibcDailyDetailReport.gridHeight,
            freezeWidth: 270,
            numberFreezeCol: 3
        };
        mainreport.removeClass("hidden").gridScroller(gridOpts);
    }

};

var licenseeDailyDetailReport = {
    title: '',
    totalRows: 0,
    pageIndex: 1,
    pageSize: 50,
    fromDate: '',
    toDate: '',
    gridHeight: 80,
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = licenseeDailyDetailReport.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#licenseedetailgrid #pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(licenseeDailyDetailReport.totalRows / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        licenseeDailyDetailReport.pageIndex = page_index + 1;
        licenseeDailyDetailReport.pageSize = _items_per_page;
        licenseeDailyDetailReport.getDailyDetailReport(true);

        return false;
    },
    getDailyDetailReport: function (loadOldData) {
        $('#dailysummarygrid').addClass('hidden');
        $('#licenseedailysummarygrid').addClass('hidden');
        $('#dailydetailgrid').addClass('hidden');
        $('#err').html('');

        try { topwnd.BlockUI(); } catch (e) { }
        var mainreport = $('#licenseedetailgrid');
        if (!loadOldData) {
            mainreport.addClass('hidden');
            licenseeDailyDetailReport.pageIndex = 1;
        }
        licenseeDailyDetailReport.fromDate = dailyReport.fromDate;
        licenseeDailyDetailReport.toDate = dailyReport.toDate;

        var data = { 'fromDate': licenseeDailyDetailReport.fromDate,
            'toDate': licenseeDailyDetailReport.toDate,
            'fromCache': loadOldData,
            'itemPerPage': licenseeDailyDetailReport.pageSize,
            'currentPage': licenseeDailyDetailReport.pageIndex
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/GetLicenseeDailyDetailReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                licenseeDailyDetailReport.title = 'Licensee Daily Customer Complaints/Enquiries From ' + licenseeDailyDetailReport.fromDate + ' To ' + licenseeDailyDetailReport.toDate;
                $('#licenseeDetailTitle').html(licenseeDailyDetailReport.title);

                if (rs && rs.DailyComplaints && rs.DailyComplaints.length > 0) {
                    var arrScroll = [];
                    var arrFrozen = [];
                    for (var i = 0; i < rs.DailyComplaints.length; i++) {
                        licenseeDailyDetailReport.totalRows = parseInt(rs.total);
                        var row = rs.DailyComplaints[i];
                        var css = (i % 2 == 0 ? "bgeven" : "bgodd"); ;
                        arrScroll.push(licenseeDailyDetailReport.renderScrollRow(row, css));
                        arrFrozen.push(licenseeDailyDetailReport.renderFrozenRow(row, css));
                    }

                    $('#btnLicenseeExportDetail').removeClass('hidden');

                    $("#licenseedetailgridbody tbody").empty().append(arrScroll.join(''));
                    $("#licenseedetailfirstcol tbody").empty().append(arrFrozen.join(''));

                    var gridOpts = {
                        tblContentId: 'licenseedetailgridbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: licenseeDailyDetailReport.gridHeight,
                        freezeWidth: 270,
                        numberFreezeCol: 3
                    };
                    mainreport.removeClass("hidden").gridScroller(gridOpts);
                    licenseeDailyDetailReport.initPager(licenseeDailyDetailReport.totalRows, licenseeDailyDetailReport.pageSize, licenseeDailyDetailReport.pageIndex - 1);
                    licenseeDailyDetailReport.reRenderGrid();

                    mainreport.find(".y-scrollbar").scrollTop(0);
                }
                else {
                    licenseeDailyDetailReport.renderNoDataRow();
                }
            },
            error: function () {
                licenseeDailyDetailReport.renderNoDataRow();
            },
            complete: function () { try { topwnd.UnBlockUI(); } catch (e) { } }

        });

    },
    renderScrollRow: function (row, css) {
        var compaints = replaceSpecialHtmlTag(row.Complaints);
        var solutions = replaceSpecialHtmlTag(row.Solution);
        var html = '<tr class="' + css + '">\
            <td><div class="c" style="width:60px;">&nbsp;' + row.FeedbackDate + '</div></td>\
            <td><div class="c" style="width:60px;">&nbsp;' + row.FeedbackTime + '</div></td>\
            <td><div class="c" style="width:140px;">&nbsp;' + row.CustomerName + '</div></td>\
            <td><div class="c" style="width:100px;">&nbsp;' + row.Licensee + '</div></td>\
            <td><div class="c" style="width:100px;">' + row.CommunicationTool.ContactBy + '</div></td>\
            <td><div class="c" style="width:60px;">&nbsp;' + row.ExtNo + '</div></td>\
            <td><div class="l" style="width:220px;">&nbsp;' + row.CommunicationTool.Address + '</div></td>\
            <td><div class="c" style="width:100px;">&nbsp;' + (row.ProductName) + '</div></td>\
            <td><div class="c" style="width:100px;">&nbsp;' + (row.Country ? row.Country : '&nbsp;') + '</div></td>\
            <td><div class="c" style="width:140px;">&nbsp;' + row.Category + '</div></td>\
            <td><div class="l" style="width:480px;">&nbsp;' + compaints + '</div></td>\
            <td><div class="l" style="width:480px;">&nbsp;' + solutions + '</div></td>\
            <td><div class="c" style="width:60px;">&nbsp;' + row.Status + '</div></td>\
            <td><div class="c" style="width:60px;">&nbsp;' + row.Response + '</div></td>\
            <td><div class="c" style="width:100px;">&nbsp;' + row.HandlyBy + '</div></td>\
            <td><div class="c" style="width:80px;">&nbsp;' + row.CreationTime + '</div></td>\
        </tr>';
        return replaceSpaceForIE9(html);
    },
    renderFrozenRow: function (row, css) {
        var html = '<tr class="' + css + '">\
            <td><div class="c" style="width:60px;">' + row.FeedbackDate + '</div></td>\
            <td><div class="c" style="width:60px;">' + row.FeedbackTime + '</div></td>\
            <td><div class="c" style="width:140px;">' + row.CustomerName + '</div></td>\
        </tr>';

        return html;
    },
    goBack: function () {
        $('#dailysummarygrid').removeClass('hidden');
        $("#licenseedailysummarygrid").removeClass('hidden');
        $('#dailydetailgrid').addClass('hidden');
        $('#licenseedetailgrid').addClass('hidden');
    },
    openConfirmPopup: function () {
        $('#divEmailError').addClass('hidden').html('');
        $("#divPopup").dialog({ width: 350, height: 200, zIndex: 500, resizable: false,
            close: function (event, ui) { },
            buttons: [
                    {
                        text: "Ok",
                        click: function () {
                            var emailAddress = $('#txtEmail').val().trim();
                            var validateCode = emailListValidate(emailAddress);

                            if (validateCode == 1) {
                                $('#divEmailError').removeClass('hidden').html('<div>Email cannot empty.</div>');
                                return false;
                            }
                            else if (validateCode == 2) {
                                $('#divEmailError').removeClass('hidden').html('<div>Email is invalid format.</div>');
                                return false;
                            }

                            var p = {
                                'fromDate': licenseeDailyDetailReport.fromDate,
                                'toDate': licenseeDailyDetailReport.toDate,
                                'reportTitle': licenseeDailyDetailReport.title,
                                'isSendMail': true,
                                'email': emailAddress
                            };

                            $.ajax({
                                type: "POST",
                                url: window.rootUrl + "Reports/ExportLicenseeDetailReport",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify(p),
                                success: function (rs) {
                                },
                                error: function () {
                                },
                                complete: function () {
                                }
                            });
                            $(this).dialog("close");
                        }
                    }
                ]
        });
    },
    exportExcel: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/AllowExportLicenseeStatisticDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                if (rs) {
                    if (rs.ErrCode == 0) {
                        var url = "DailyStatistic?export=true&type=4&fdate=" + licenseeDailyDetailReport.fromDate + "&tdate=" + licenseeDailyDetailReport.toDate + "&title=" + licenseeDailyDetailReport.title;
                        $("#frm").attr("src", url);
                    }
                    else
                        licenseeDailyDetailReport.openConfirmPopup();
                }
                else
                    licenseeDailyDetailReport.openConfirmPopup();
            },
            error: function () {

            },
            complete: function () {

            }
        });
    },
    reRenderGrid: function () {
        var trFrozen = $('#licenseedetailfirstcol tbody tr');
        var totalRows = trFrozen.length;

        for (var i = 0; i < totalRows; i++) {
            var frozenHeight = $('#licenseedetailfirstcol tbody tr:eq(' + i + ') td:eq(0)').height();
            var scrollHeight = $('#licenseedetailgridbody tbody tr:eq(' + i + ') td:eq(0)').height();

            var maxHeight = 18;
            if (frozenHeight > scrollHeight)
                maxHeight = frozenHeight + 1;
            else if (scrollHeight > frozenHeight)
                maxHeight = scrollHeight + 1;

            $('#licenseedetailgridbody tbody tr:eq(' + i + ') td:eq(0) div').css('height', maxHeight).append(' ');
            $('#licenseedetailfirstcol tbody tr:eq(' + i + ') td:eq(0) div').attr('style', ' display:table-cell; vertical-align: middle; height:' + maxHeight + 'px');
        }

        var mainreport = $('#licenseedetailgrid');
        mainreport.removeClass("hidden");
        var gridOpts = {
            tblContentId: 'licenseedetailgridbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: licenseeDailyDetailReport.gridHeight,
            freezeWidth: 270,
            numberFreezeCol: 3
        };
        mainreport.gridScroller(gridOpts);
    },
    renderNoDataRow: function () {
        $('#girdlicensepaging').html('&nbsp;');
        $('#btnLicenseeExportDetail').addClass('hidden');

        var html = '<tr>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:140px; height:20px;">&nbsp;</div></td>\
            <td colspan="13"><div class="c" style="width:2080px; height:20px;"><div class="alertboxerr"><div>There is no information.</div></div></td>\
        </tr>';

        var mainreport = $('#licenseedetailgrid');

        $("#licenseedetailfirstcol tbody").empty().html('<tr>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:140px; height:20px;">&nbsp;</div></td></tr>'
            );
        $("#licenseedetailgridbody tbody").empty().html(html);
        var gridOpts = {
            tblContentId: 'licenseedetailgridbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: licenseeDailyDetailReport.gridHeight,
            freezeWidth: 270,
            numberFreezeCol: 3
        };
        mainreport.removeClass("hidden").gridScroller(gridOpts);
    }
};


head.ready(
        function () {
            $('#txtfromdate').val((new Date(fromDate)).format("m/d/Y"));
            $('#txttodate').val((new Date(toDate)).format("m/d/Y"));
            try { topwnd = findMainWindow(window); topwnd.UnBlockUI(); } catch (e) { }

            var gridOpts = {
                tblContentId: 'dailysummarybody',
                isShowPager: false
            };
            $("#dailysummarygrid").initGrid(gridOpts);

            var gridOpts = {
                tblContentId: 'licenseedailysummarybody',
                isShowPager: false
            };
            $("#licenseedailysummarygrid").initGrid(gridOpts);

            var gridOpts = {
                tblContentId: 'dailydetailgridbody',
                isShowPager: true
            };
            $("#dailydetailgrid").initGrid(gridOpts);

            var gridOpts = {
                tblContentId: 'licenseedetailgridbody',
                isShowPager: true
            };
            $("#licenseedetailgrid").initGrid(gridOpts);
        }
    );

$.extend(ibcDailySummaryReport, { chart: {
    data: null,
    categories: [],
    setData: function (_data) {
        this.data = _data;
        if (_data) {
            var val = _data[0];
            var objIndex = 0;
            var newCategories = [];
            jQuery.each(val, function (name, value) {
                if (objIndex != 0) {
                    var item = { field: name, label: name, enable: true };
                    newCategories.push(item);
                }
                objIndex++;
            });
            this.categories = newCategories;
        }
    },
    viewByTotal: function () {
        if (this.data != null) {
            var data_items = this.data;
            var serial_data = []; var categories = [];
            for (var i = 0, len = data_items.length; i < len; i++) {
                var item = data_items[i];
                var toData = [];
                $.each(this.categories, function (ind, cate) {
                    if (cate.enable) {
                        var val = eval("item." + cate.field + "");
                        toData.push(ibcDailySummaryReport.chart.toNumber(val));
                        if (i == 0) { categories.push(cate.label); }
                    }
                });
                serial_data.push({
                    name: item.Name,
                    data: toData,
                    cursor: 'pointer'
                });
            }
            // init chart total
            var opts = {
                container: 'chart_content',
                type: 'spline',
                title: $('#gridtitle').html(),
                width: 1000,
                height: (maxHeightOfPopUp-100),
                categories: categories,
                series: serial_data,
                legendY: (maxHeightOfPopUp - 210)
            };
            if ($('#chart_content').length == 0) {
                this.initViewChart(opts);
            }
            accChart.renderSVG(opts);
        }
    },
    initViewChart: function (opts) {
        $('<div />').attr('id', 'chart_shadow').addClass('chart_shadow').appendTo('body').show();
        $('<div />').attr('id', 'chart_popup').addClass('chart_popup')
            .css({ width: 1000 })
            .appendTo('body');
        $('<div />').attr('id', 'chart_setting').addClass('chart_setting').appendTo('#chart_popup');
        $('<div />').attr('id', 'chart_title')
             .addClass('chart_title')
             .append('<div class="setting" title="Setting">&nbsp;</div><div class="title">' + opts.title + '</div><div class="close" title="Close">&nbsp;</div>')
             .appendTo('#chart_popup');
        $('<div />').attr('id', 'chart_content').addClass('chart_content')
             .css({ width: opts.width, height: opts.height })
             .appendTo('#chart_popup');
        $('#chart_popup').css("left", 10);
        $('#chart_popup').css("top", 10);
        $('#chart_popup').show();
        $('#chart_popup').draggable({
            handle: '#chart_title'
        });
        $('div.setting').bind('click', function (e) {
            if ($('#chart_setting').is(':hidden')) {
                $('#chart_setting').slideDown();
                $(this).attr('title', 'Hide setting');
            } else {
                $('#chart_setting').slideUp();
                $(this).attr('title', 'Display setting');
            }
        });
        $('div.close').bind('click', function (e) {
            ibcDailySummaryReport.chart.close();
        });
        this.renderSetting();
    },
    renderSetting: function () {
        $('#chart_setting').html('<ul id="ul_setting"></ul>');
        var tpl_cate = '{{each categories}}<li><input class="setcheck" type="checkbox" {{if $value.enable}}checked="${$value.enable}"{{/if}} value="${$index}"/>${$value.enable}&nbsp;${$value.label}</li>{{/each}}';
        //$('<div />').append(tpl_cate).tmpl(this).appendTo("#ul_setting");
        $.each(this.categories, function (ind, cate) {
            var is_checked = (cate.enable == true) ? 'checked' : '';
            var _itemTpl = '<li><input class="setcheck" ' + is_checked + ' type="checkbox" value="' + ind + '"/>&nbsp;' + cate.label + '</li>';
            $("#ul_setting").append(_itemTpl);
        });
        $('#chart_setting').css({ width: 185, 'margin-top': 27 });
        if ($.browser.msie) {
            if ($.browser.version.indexOf('7') != -1) {
                $('#chart_setting').css({ 'margin-left': -1000 });
                $('#ul_setting').css({ 'margin-left': 10 });
            }
        }
        window.setTimeout(function () {
            $('input.setcheck').bind('click', function (e) {
                var checked = $(this).attr("checked");
                var index = $(this).val();
                ibcDailySummaryReport.chart.categories[index].enable = checked;
                ibcDailySummaryReport.chart.viewByTotal();
            });
        });
    },
    close: function () {
        $('#chart_shadow').fadeOut();
        $('#chart_popup').fadeOut();
        window.setTimeout(function () {
            $('#chart_shadow').remove();
            $('#chart_popup').remove();
        }, 1000);
    },
    toNumber: function (numText) {
        numText = "".concat(numText);
        return parseFloat(numText.replace(/,/gi, ''));
    }
}
});

$.extend(licenseeDailySummaryReport, { chart: {
    data: null,
    categories: [],
    setData: function (_data) {
        this.data = _data;
        if (_data) {
            var val = _data[0];
            var objIndex = 0;
            var newCategories = [];
            jQuery.each(val, function (name, value) {
                if (objIndex != 0) {
                    var item = { field: name, label: name, enable: true };
                    newCategories.push(item);
                }
                objIndex++;
            });
            this.categories = newCategories;
        }
    },
    viewByTotal: function () {
        if (this.data != null) {
            var data_items = this.data;
            var serial_data = []; var categories = [];
            for (var i = 0, len = data_items.length; i < len; i++) {
                var item = data_items[i];
                var toData = [];
                $.each(this.categories, function (ind, cate) {
                    if (cate.enable) {
                        var val = eval("item." + cate.field + "");
                        toData.push(licenseeDailySummaryReport.chart.toNumber(val));
                        if (i == 0) { categories.push(cate.label); }
                    }
                });
                serial_data.push({
                    name: item.Name,
                    data: toData,
                    cursor: 'pointer'
                });
            }
            // init chart total
            var opts = {
                container: 'chart_content',
                type: 'spline',
                title: $('#licenseeSummaryTitle').html(),
                width: 1000,
                height: (maxHeightOfPopUp-100),
                categories: categories,
                series: serial_data,
                legendY: (maxHeightOfPopUp - 210)
            };
            if ($('#chart_content').length == 0) {
                this.initViewChart(opts);
            }
            accChart.renderSVG(opts);
        }
    },
    initViewChart: function (opts) {
        $('<div />').attr('id', 'chart_shadow').addClass('chart_shadow').appendTo('body').show();
        $('<div />').attr('id', 'chart_popup').addClass('chart_popup')
            .css({ width: 1000 })
            .appendTo('body');
        $('<div />').attr('id', 'chart_setting').addClass('chart_setting').appendTo('#chart_popup');
        $('<div />').attr('id', 'chart_title')
             .addClass('chart_title')
             .append('<div class="setting" title="Setting">&nbsp;</div><div class="title">' + opts.title + '</div><div class="close" title="Close">&nbsp;</div>')
             .appendTo('#chart_popup');
        $('<div />').attr('id', 'chart_content').addClass('chart_content')
             .css({ width: opts.width, height: opts.height })
             .appendTo('#chart_popup');
        $('#chart_popup').css("left", 10);
        $('#chart_popup').css("top", 10);
        $('#chart_popup').show();
        $('#chart_popup').draggable({
            handle: '#chart_title'
        });
        $('div.setting').bind('click', function (e) {
            if ($('#chart_setting').is(':hidden')) {
                $('#chart_setting').slideDown();
                $(this).attr('title', 'Hide setting');
            } else {
                $('#chart_setting').slideUp();
                $(this).attr('title', 'Display setting');
            }
        });
        $('div.close').bind('click', function (e) {
            licenseeDailySummaryReport.chart.close();
        });
        this.renderSetting();
    },
    renderSetting: function () {
        $('#chart_setting').html('<ul id="ul_setting"></ul>');
        var tpl_cate = '{{each categories}}<li><input class="setcheck" type="checkbox" {{if $value.enable}}checked="${$value.enable}"{{/if}} value="${$index}"/>${$value.enable}&nbsp;${$value.label}</li>{{/each}}';
        //$('<div />').append(tpl_cate).tmpl(this).appendTo("#ul_setting");
        $.each(this.categories, function (ind, cate) {
            var is_checked = (cate.enable == true) ? 'checked' : '';
            var _itemTpl = '<li><input class="setcheck" ' + is_checked + ' type="checkbox" value="' + ind + '"/>&nbsp;' + cate.label + '</li>';
            $("#ul_setting").append(_itemTpl);
        });
        $('#chart_setting').css({ width: 185, 'margin-top': 27 });
        if ($.browser.msie) {
            if ($.browser.version.indexOf('7') != -1) {
                $('#chart_setting').css({ 'margin-left': -1000 });
                $('#ul_setting').css({ 'margin-left': 10 });
            }
        }
        window.setTimeout(function () {
            $('input.setcheck').bind('click', function (e) {
                var checked = $(this).attr("checked");
                var index = $(this).val();
                licenseeDailySummaryReport.chart.categories[index].enable = checked;
                licenseeDailySummaryReport.chart.viewByTotal();
            });
        });
    },
    close: function () {
        $('#chart_shadow').fadeOut();
        $('#chart_popup').fadeOut();
        window.setTimeout(function () {
            $('#chart_shadow').remove();
            $('#chart_popup').remove();
        }, 1000);
    },
    toNumber: function (numText) {
        numText = "".concat(numText);
        return parseFloat(numText.replace(/,/gi, ''));
    }
}
});
