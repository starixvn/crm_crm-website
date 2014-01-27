"use strict";
/* global showAlertMessage, lblErroTimeDate, head,
findMainWindow, lblErrorTimeDuration, accChart */
/* exported arrWidth */

var topwnd;
var arrWidth = [];
var maxHeightOfPopUp = ($(window).height() + 50);

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
    for (var i = 0; i < arrIds.length; i++) {
        $('#' + arrIds[i]).datepicker();
        if (arrDateValues[i] != null && arrDateValues[i] !== "") {
            $('#' + arrIds[i]).val(arrDateValues[i]).datepicker('setDate', (new Date(arrDateValues[i])));
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
    gridHeight: 100,
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

        try { parent.BlockUI(); } catch (e) { }
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
                              .append(feedbackStatistic.renderFeedbackScrollFooterRow(rs.GrandTotal, 0, "", ''));
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
                try { parent.UnBlockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.UnBlockUI(); } catch (e) { }
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
        html += '<td colspan=' + totalCol + '><div style="width:' + width + 'px;">&nbsp;</div></td></tr>';
        return html;
    },
    renderFeedbackScrollDetailRow: function (row, id, css, style) {
        var html = '';
        if (row) {
            html = '<tr class="fb_' + id + ' ' + css + '">';
            html += '<td ' + style + '><div style="width:400px; height:17px;"><span class="child">&nbsp;&nbsp;</span>' + row.SubCategory + '</div></td>';
            for (var i = 0; i < row.Cells.length; i++) {
                html += '<td ' + style + '><div style="width:110px;" class="c ' + css + '">' + number(i, row) + '</div></td>';
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
    renderFeedbackScrollFooterRow: function (row, id, css, style) {
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
        html += '<th><div class="table-cell maxheight" style="width:400px;" >Sub Category</div></th>';
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

        parent.OpenPopup(url, "Feedback Statistic Detail Report", 1020, maxHeightOfPopUp, true, false, window, 2000, null);
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
        var items = [];
        for (var i = 0; i < rs.rows.length; i++) {
            var item = rs.rows[i];
            for (var k = 0; k < item.FeedbackStatistics.length; k++) {
                var cItem = item.FeedbackStatistics[k];
                if (cItem.IsTotal) {
                    var chartInfo = {};
                    chartInfo.Name = "'" + cItem.MainCategory + "'";
                    for (var j = 0; j < cItem.Cells.length; j++) {
                        var cell = cItem.Cells[j];
                        var colName = rs.ColumnNames[j];
                        colName = colName.replace(/[^a-zA-Z 0-9 ]/g, "");
                        colName = colName.replace(/ /g, "");
                        chartInfo["FB_" + colName] = cell;
                    }
                    items.push(chartInfo);
                }
            }

        }
        feedbackStatistic.chart.setData(items);
    }
};

head.ready(function () {
    var fdate, tdate;
    fdate = addDays(new Date(), -7);
    tdate = new Date();

    var arrDateValues = [fdate.format("m/d/Y"), tdate.format("m/d/Y")];
    var arrIds = ['txtfromdate', 'txttodate'];
    setupCalendar(arrDateValues, arrIds);
//    try { topwnd = findMainWindow(window); } catch (e) { }
    durationTimeForceNumberic();

    try { parent.UnBlockUI(); } catch (e) { }

    var gridOpts = {
        tblContentId: 'feedbackstatisticbody',
        isShowPager: true
    };
    $("#feedbackstatisticgrid").initGrid(gridOpts);
}
    );

$.extend(feedbackStatistic, { chart: {
    data: null,
    categories: [],
    setData: function (_data) {
        this.data = _data;
        if (_data) {
            var val = _data[0];
            var objIndex = 0;
            var newCategories = [];
            jQuery.each(val, function (name, value) {
                /* jshint unused: vars */
                if (objIndex !== 0) {
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
                for (var j = 0, categoriesLength = this.categories.length; j < categoriesLength; j++) {
                    var cate = this.categories[j];
                    if (!cate.enable) {
                        continue;
                    }

                    var val = item[cate.field];
                    toData.push(feedbackStatistic.chart.toNumber(val));
                    if (i === 0) {
                        categories.push(cate.label);
                    }
                }

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
                title: $('#feedbackstatistictitle').html(),
                width: 1000,
                height: (maxHeightOfPopUp - 100),
                categories: categories,
                series: serial_data,
                legendY: (maxHeightOfPopUp - 140)
            };
            if ($('#chart_content').length === 0) {
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
             .append('<div class="setting" title="Setting">&nbsp;</div>\
                    <div class="title">' + opts.title + '</div>\
                    <div class="close" title="Close">&nbsp;</div>')
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
            /* jshint unused: vars */
            if ($('#chart_setting').is(':hidden')) {
                $('#chart_setting').slideDown();
                $(this).attr('title', 'Hide setting');
            } else {
                $('#chart_setting').slideUp();
                $(this).attr('title', 'Display setting');
            }
        });
        $('div.close').bind('click', function (e) {
            /* jshint unused: vars */
            feedbackStatistic.chart.close();
        });
        this.renderSetting();
    },
    renderSetting: function () {
        $('#chart_setting').html('<ul id="ul_setting"></ul>');
        //        var tpl_cate = '{{each categories}}<li>\
        //                            <input class="setcheck" type="checkbox" {{if $value.enable}}checked="${$value.enable}"{{/if}} value="${$index}"/>\
        //                            ${$value.enable}&nbsp;${$value.label}</li>{{/each}}';
        //$('<div />').append(tpl_cate).tmpl(this).appendTo("#ul_setting");
        $.each(this.categories, function (ind, cate) {
            var is_checked = (cate.enable === true) ? 'checked' : '';
            var _itemTpl = '<li><input class="setcheck" ' + is_checked + ' type="checkbox" value="' + ind + '"/>&nbsp;' + cate.label + '</li>';
            $("#ul_setting").append(_itemTpl);
        });
        $('#chart_setting').css({ width: 185, 'margin-top': 27 });
        if ($.browser.msie) {
            if ($.browser.version.indexOf('7') !== -1) {
                $('#chart_setting').css({ 'margin-left': -1000 });
                $('#ul_setting').css({ 'margin-left': 10 });
            }
        }
        window.setTimeout(function () {
            $('input.setcheck').bind('click', function (e) {
                /* jshint unused: vars */
                var checked = $(this).attr("checked");
                var index = $(this).val();
                feedbackStatistic.chart.categories[index].enable = checked;
                feedbackStatistic.chart.viewByTotal();
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
