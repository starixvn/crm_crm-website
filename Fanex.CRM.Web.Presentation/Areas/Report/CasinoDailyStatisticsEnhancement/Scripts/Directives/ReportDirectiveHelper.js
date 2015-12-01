"use strict";
/* global
casinoDailyEnhancementResources,
crmConstants,
numeral */
/* jshint camelcase: false,
unused: vars */
/* exported reportDirectiveHelper */

var ReportDirectiveHelper = function () {
    var self = this;
    self.getGridOptions = function (report) {
        var bodyData = [];
        var footerData = [];
        var communicationToolLength = report ? report.CommunicationTools.length : 0;
        var mainCategoryLength = report ? report.ChildReports.length : 0;

        var i, j;
        for (i = 0; i < mainCategoryLength; i++) {
            var mainCategory = report.ChildReports[i];
            var subCategoryLength = mainCategory.ChildReports.length;
            var subRows = [];
            for (j = 0; j < subCategoryLength; j++) {
                var subCategory = mainCategory.ChildReports[j];
                subRows.push({ Columns: self.getRowColumns(subCategory, communicationToolLength) });
            }

            bodyData.push(
            {
                Columns: self.getRowColumns(mainCategory, communicationToolLength),
                SubRows: subRows
            });
        }

        if (mainCategoryLength) {
            var grandTotal =
                {
                    Name: casinoDailyEnhancementResources.Grand_Total,
                    NumberOfIssueList: report.NumberOfIssueList,
                    NumberOfIssueGrandTotal: report.NumberOfIssueGrandTotal
                };
            footerData.push(
            {
                Columns: self.getRowColumns(grandTotal, communicationToolLength)
            });
        }

        var headerColumns = [];
        headerColumns.push({
            HeaderText: casinoDailyEnhancementResources.Complaints_Enquiries,
            Width: 300,
            Name: "ColComplaints_Enquiries",
            HeaderAlign: "center",
            CellAlign: "left",
            ShowCollapseExpand: true
        });
        for (i = 0; i < communicationToolLength; i++) {
            var communicationTool = report.CommunicationTools[i];
            headerColumns.push({
                HeaderText: communicationTool.Name,
                Width: 75,
                Name: "ColCommunicationTool_" + i,
                HeaderAlign: "center",
                CellAlign: "center"
            });
        }

        headerColumns.push({
            HeaderText: casinoDailyEnhancementResources.Grand_Total,
            Width: 75,
            Name: "ColCommunicationTool_GrandTotal",
            HeaderAlign: "center",
            CellAlign: "center"
        });
        var viewChartIcon = '<input class="btnCharting " type="button" title="' + casinoDailyEnhancementResources.ViewChart + '" />';
        var viewDetailIcon = '<input class="btnViewDetail " type="button" title="' + casinoDailyEnhancementResources.ViewDetail + '" />';
        var gridOptions = {
            columns: headerColumns,
            showTitle: true,
            titleInfo: {
                text: report ? report.Name : "",
                showExcelButton: true,
                exportExcelFunction: null,
                showCollapseExpandButton: true,
                customButtons: viewChartIcon + viewDetailIcon
            },
            bodyRows: bodyData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: false,
            gridExpandHeight: 0,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 29,
            showTree: true,
            treeParentPaddingLeft: 5,

            showFooter: true,
            footerRows: footerData
        };

        return gridOptions;
    };
    self.getRowColumns = function (categoryIssuesInfo, communicationToolLength) {
        var rowColumns = [{ Name: "ColComplaints_Enquiries", Value: categoryIssuesInfo.Name}];
        for (var i = 0; i < communicationToolLength; i++) {
            rowColumns.push({
                Name: "ColCommunicationTool_" + i,
                Value: numeral(categoryIssuesInfo.NumberOfIssueList[i]).format(crmConstants.Enums.FormatConstant.IntNumber)
            });
        }

        rowColumns.push({
            Name: "ColCommunicationTool_GrandTotal",
            Value: numeral(categoryIssuesInfo.NumberOfIssueGrandTotal).format(crmConstants.Enums.FormatConstant.IntNumber)
        });

        return rowColumns;
    };
    self.viewChart = function (scope, report) {
        var crmChart = parent.crmChart;
        crmChart.title = report ? report.Name : "";
        var chartInfo = report;
        var chartData = {
            labels: self.getChartLabels(chartInfo, crmChart),
            datasets: self.getChartDatasets(chartInfo, crmChart)
        };

        var chartOption = angular.copy(crmChart.newopts);
        chartOption = $.extend(chartOption, {
            chartType: crmChart.chartType.bar,
            xAxisLabel: "",
            yAxisLabel: "",
            yAxisUnit: casinoDailyEnhancementResources.DailyStatisticsReport_ChartSuffix
        });

        crmChart.viewChart(chartData, chartOption);
        parent.$(".ui-dialog-titlebar-close").click(function () {
            scope.setFocus();
        });
    };

    self.getChartLabels = function (chartInfo) {
        var xAxisCategories = [];
        var communicationToolLength = chartInfo ? chartInfo.CommunicationTools.length : 0;
        for (var i = 0; i < communicationToolLength; i++) {
            var communicationTool = chartInfo.CommunicationTools[i];
            xAxisCategories.push(communicationTool.Name);
        }

        return xAxisCategories;
    };
    self.getChartDatasets = function (chartInfo, crmChart) {
        var chartSeries = [];
        var chartSerie = null;
        var childReportLength = chartInfo ? chartInfo.ChildReports.length : 0;
        for (var xIndex = 0; xIndex < childReportLength; xIndex++) {
            var mainCategoryReport = chartInfo.ChildReports[xIndex];
            var backgroundColor = crmChart.getBackGroundColor(xIndex);
            chartSerie = {
                fillColor: backgroundColor,
                strokeColor: backgroundColor,
                data: mainCategoryReport.NumberOfIssueList,
                title: mainCategoryReport.Name
            };
            chartSeries.push(chartSerie);
        }

        return chartSeries;
    };
};

var reportDirectiveHelper = new ReportDirectiveHelper();