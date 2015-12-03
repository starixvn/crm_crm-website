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
        var productColumnLength = report ? report.Products.length : 0;
        var mainCategoryLength = report ? report.ChildReports.length : 0;

        var i, j;
            for (i = 0; i < mainCategoryLength; i++) {
                var mainCategory = report.ChildReports[i];
                var subRows = self.getDataRows(mainCategory, report.Products, communicationToolLength);

                bodyData.push({
                    Columns: self.getRowValue(mainCategory, report.Products, communicationToolLength),
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
                Columns: self.getRowValue(grandTotal, report.Products, communicationToolLength)
            });
        }

        var groupHeaderRows = [{
            HeaderText: casinoDailyEnhancementResources.Complaints_Enquiries,
            Name: "ColComplaints_Enquiries",
            MergeRowNumber: 2
        }];

        var headerColumns = [];
        headerColumns.push({
            HeaderText: casinoDailyEnhancementResources.Complaints_Enquiries,
            Width: 300,
            Name: "ColComplaints_Enquiries",
            HeaderAlign: "center",
            CellAlign: "left",
            ShowCollapseExpand: true
        });
        for (i = 0; i < productColumnLength; i++) {
            var product = report.Products[i];
            groupHeaderRows.push({
                HeaderText: product.Name,
                Name: product.Id + "_ColCommunicationTool_0",
                MergeColNumber: communicationToolLength + 1
            });

            for (j = 0; j < communicationToolLength; j++) {
                var communicationTool = report.CommunicationTools[j];
                headerColumns.push({
                    HeaderText: communicationTool.Name,
                    Width: 75,
                    Name: product.Id + "_ColCommunicationTool_" + j,
                    HeaderAlign: "center",
                    CellAlign: "center"
                });
            }

            headerColumns.push({
                HeaderText: casinoDailyEnhancementResources.Grand_Total,
                Width: 75,
                Name: product.Id + "_ColCommunicationTool_GrandTotal",
                HeaderAlign: "center",
                CellAlign: "center",
            });
        }

        var viewChartIcon = '<input class="btnCharting " type="button" title="' + casinoDailyEnhancementResources.ViewChart + '" />';
        var viewDetailIcon = '<input class="btnViewDetail " type="button" title="' + casinoDailyEnhancementResources.ViewDetail + '" />';
        var gridOptions = {
            customizeHeader: true,
            groupHeaderRows: [{ Columns: groupHeaderRows}],
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
            numberOfFrozenColumn: 1,
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
    self.getDataRows = function(subReport, products, communicationToolLength){
        var rows = [];

        for(var i = 0; i < subReport.ChildReports.length; i++){
            if(subReport.ChildReports[i].ChildReports){
                self.getDataRows(subReport.ChildReports[i], products, communicationToolLength);
            }
        }

        var i;
        var rowCount = subReport.ChildReports.length;
        for (i = 0; i < rowCount; i++) {
            var category = subReport.ChildReports[i];
            rows.push({ Columns: self.getRowValue(category, products, communicationToolLength) });
        }

        return rows;
    };
    self.getRowValue = function (categoryIssuesInfo, products, communicationToolLength) {
        var row = [{ Name: "ColComplaints_Enquiries", Value: categoryIssuesInfo.Name}];
        for(var i = 0; i < products.length; i++){
            for(var j = 0; j < communicationToolLength; j++){
                row.push({
                    Name: products[i].Id + "_ColCommunicationTool_" + j,
                    Value: numeral(categoryIssuesInfo.NumberOfIssueGrandTotal).format(crmConstants.Enums.FormatConstant.IntNumber)
                });
            }
            row.push({
                Name: products[i].Id + "_ColCommunicationTool_GrandTotal",
                Value: numeral(categoryIssuesInfo.NumberOfIssueGrandTotal).format(crmConstants.Enums.FormatConstant.IntNumber)
            });
        }

        return row;
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