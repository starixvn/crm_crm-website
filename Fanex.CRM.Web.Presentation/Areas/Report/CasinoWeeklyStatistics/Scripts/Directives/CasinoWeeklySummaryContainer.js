(function () {
    "use strict";
    /* global casinoWeeklyResources,
    crmConstants */
    angular.module('report-casinoweekly-application').directive('tableWeeklySummaryContainer', function () {
        function getGameTypeColumn(headerText, name) {
            return {
                HeaderText: headerText,
                Width: 90,
                Name: name,
                HeaderAlign: 'center',
                CellAlign: 'center',
                FormatNumber: crmConstants.Enums.FormatConstant.IntNumber
            };
        }

        function getGridOptions(scope, reportTitle, bodyData, footerData, gameTypes) {
            var columns = [{
                HeaderText: casinoWeeklyResources.Category,
                Width: 300,
                Name: 'ColCategory',
                HeaderAlign: 'center',
                CellAlign: 'left'
            }];

            for (var i = 0, leng = gameTypes.length; i < leng; i++) {
                columns.push(getGameTypeColumn(gameTypes[i], 'Col' + gameTypes[i]));
            }

            // Add Grand Total.
            columns.push(getGameTypeColumn(casinoWeeklyResources.GrandTotal, 'ColGrandTotal'));

            return {
                columns: columns,
                showTitle: true,
                titleInfo: {
                    text: reportTitle,
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        scope.exportCasinoWeeklySummaryExcel();
                    },
                    customButtons: '<input id="button-viewchart" class="btnCharting" type="button" title="' + casinoWeeklyResources.ViewChart + '" />\
                    <input id="button-viewdetail" class="btnViewDetail" type="button" title="' + casinoWeeklyResources.ViewDetail + '" />'
                },
                bodyRows: bodyData,
                showFooter: true,
                footerRows: footerData,
                numberOfFrozenColumn: 1,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: 140,
                showDistinctRow: false,
                showHighlightRow: false,
                headerColumnHeight: 29
            };
        }

        function getGridBody(scope, weeklyCategories, gameTypes) {
            var bodyData = [];
            // view chart.
            var datasets = [];
            var bgColor, bgColorIndex = 0;
            var bgColorFunction = parent.crmChart.getBackGroundColor;
            var categoryCharts;

            if (weeklyCategories) {
                var category, currentRow;
                var categoryLen, gameValueLen;

                categoryLen = weeklyCategories.length;
                for (var i = 0; i < categoryLen; i++) {
                    categoryCharts = [];
                    category = weeklyCategories[i];
                    currentRow = {
                        Columns: [{ Name: 'ColCategory', Value: category.Name}]
                    };

                    gameValueLen = category.GameTypeValues.length;
                    for (var j = 0; j < gameValueLen; j++) {
                        currentRow.Columns.push({ Name: 'Col' + gameTypes[j], Value: category.GameTypeValues[j] });

                        // view chart.
                        categoryCharts.push(category.GameTypeValues[j]);
                    }

                    // Grand Total.
                    currentRow.Columns.push({ Name: 'ColGrandTotal', Value: category.GrandTotal });
                    bodyData.push(currentRow);

                    bgColor = bgColorFunction(bgColorIndex++);
                    datasets.push({ fillColor: bgColor, strokeColor: bgColor, title: category.Name, data: categoryCharts });
                }
            }

            scope.weeklySummaryChart = { labels: gameTypes, datasets: datasets };
            return bodyData;
        }

        function getGridFooter(grandTotal, gameTypes) {
            var footerData = [];
            var cell = { Columns: [{ Name: 'ColCategory', Value: grandTotal.Name}] };

            for (var i = 0, grandTotalLen = grandTotal.GameTypeValues.length; i < grandTotalLen; i++) {
                cell.Columns.push({ Name: 'Col' + gameTypes[i], Value: grandTotal.GameTypeValues[i] });
            }

            // Grand Total.
            cell.Columns.push({ Name: 'ColGrandTotal', Value: grandTotal.GrandTotal });
            footerData.push(cell);
            return footerData;
        }

        function renderCasinoWeeklySummary(scope, element) {
            var report = scope.weeklySummary;
            if (!report || !report.WeeklyCategories || report.WeeklyCategories.length === 0) {
                element.addClass('hide');
                return;
            }

            var reportTitle = report.Title;
            var weeklyCategories = report.WeeklyCategories;
            var grandTotal = report.GrandTotal;
            var gameTypes = report.GameTypes;

            var bodyData = getGridBody(scope, weeklyCategories, gameTypes);
            var footerData = getGridFooter(grandTotal, gameTypes);

            var gridOptions = getGridOptions(scope, reportTitle, bodyData, footerData, gameTypes);

            var grid = element.find('#table-weekly-summary');
            grid.empty().removeClass('hide').fanexGrid(gridOptions);
            // view chart event.
            element.find('#button-viewchart').click(function () {
                scope.viewCasinoWeeklySummaryChart();
            });
            element.find('#button-viewdetail').click(function () {
                scope.getCasinoWeeklyDetail();
            });
        }

        return {
            restrict: 'A',
            link: function (scope, element) {
                scope.$watch('weeklySummary', function (newValue, oldValue) {
                    /* jshint unused: false */
                    if (!newValue) {
                        return;
                    }

                    renderCasinoWeeklySummary(scope, element);
                });
            }
        };
    });
})();