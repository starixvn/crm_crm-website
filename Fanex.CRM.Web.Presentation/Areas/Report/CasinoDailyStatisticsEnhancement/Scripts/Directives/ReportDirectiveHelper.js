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

        var mainCategories = report.MainCategories;
        var products = report.Products;

        var productColumns = report.ProductColumns;
        var communicationColumns = report.CommunicationColumns;

        var mainCategoryLength = mainCategories.length;

        var i, j;
            for (i = 0; i < mainCategoryLength; i++) {
                var mainCategory = mainCategories[i];

                var subCategories = mainCategory.ChildReports;            
                var subCategoryRows = [];
                for(var j = 0; j < subCategories.length; j++){
                    var productsGroup = self.getProductsBySubCategory(subCategories[j].Id, products);
                    var subCategoryDataRows = self.getSubCategoryData(subCategories[j], productsGroup);

                    subCategoryRows.push({ Columns: subCategoryDataRows });
                }

                bodyData.push({
                    Columns: self.getMainCategoryDataRows(mainCategory, productColumns),
                    SubRows: subCategoryRows
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
        for (i = 0; i < productColumns.length; i++) {
            groupHeaderRows.push({
                HeaderText: productColumns[i].Name,
                Name: productColumns[i].Id + "_ColCommunicationTool_0",
                MergeColNumber: communicationColumns.length + 1
            });

            for (j = 0; j < communicationColumns.length; j++) {
                headerColumns.push({
                    HeaderText: communicationColumns[j].Name,
                    Width: 75,
                    Name: productColumns[i].Id + "_ColCommunicationTool_" + j,
                    HeaderAlign: "center",
                    CellAlign: "center"
                });
            }

            headerColumns.push({
                HeaderText: casinoDailyEnhancementResources.Grand_Total,
                Width: 75,
                Name: productColumns[i].Id + "_ColCommunicationTool_GrandTotal",
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
    self.getMainCategoryDataRows = function(mainCategory, products, productColumns, communicationColumns) {
        var mainCategoryRows = [{ Name: "ColComplaints_Enquiries", Value: mainCategory.Name}];
        var listOfCommunicationCount = mainCategory.NumberOfCommunication;

        for(var i = 0; i < productColumns.length; i++){
            var product = products[i];

            for(var j = 0; j < communicationColumns; j++){
                var communicationCountDataRows = self.getCommunicationCountDataRows(communicationColumns, product);
                mainCategoryRows = mainCategoryRows.concat(communicationCountDataRows);
            }

            mainCategoryRows.push({
                Name: product.Id + "_ColCommunicationTool_GrandTotal",
                Value: numeral(mainCategory.NumberOfIssueGrandTotal).format(crmConstants.Enums.FormatConstant.IntNumber)
            });
        }

        return mainCategoryRows;
    };
    self.getSubCategoryData = function(subCategory, products) {
        var subCategoryRows = [{ Name: "ColComplaints_Enquiries", Value: subCategory.Name}];

        var dataDetail = self.getRowValue(products);

        subCategoryRows = subCategoryRows.concat(dataDetail);
        return subCategoryRows;
    };
    self.getProductsBySubCategory = function(subCategoryId, products) {
        var productsBySubcategory = [];
        for(var i = 0; i < products.length; i++){
            if(products[i].SubCategoryId === subCategoryId){
                productsBySubcategory.push(products[i]);
            }
        }

        return productsBySubcategory;
    };

    self.getRowValue = function (products) {
        var dataDetail = [];
        for(var i = 0; i < products.length; i++){
            var product = products[i];
            var listOfCommunicationCount = product.NumberOfCommunication;
            var communicationCountDataRows = self.getCommunicationCountDataRows(listOfCommunicationCount, product);
            dataDetail = dataDetail.concat(communicationCountDataRows);

            dataDetail.push({
                Name: product.Id + "_ColCommunicationTool_GrandTotal",
                Value: numeral(product.NumberOfIssueGrandTotal).format(crmConstants.Enums.FormatConstant.IntNumber)
            });
        }

        return dataDetail;
    };
    self.getCommunicationCountDataRows = function (listOfCommunicationCount, product){
        var communicationCountDataRows = [];
        for(var i = 0; i < listOfCommunicationCount.length; i++){
            communicationCountDataRows.push({
                Name: product.Id + "_ColCommunicationTool_" + i,
                Value: numeral(listOfCommunicationCount[i]).format(crmConstants.Enums.FormatConstant.IntNumber)
            });
        }

        return communicationCountDataRows;
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