(function () {
    "use strict";
    /* global casinoDailyResources,
    crmConstants */
    /* jshint camelcase: false */
    /* jshint unused: vars */
    angular.module('report-casinodaily-application').directive('tableDailySummaryContainer', function () {
        function getGridOptions(scope, report, bodyData, footerData) {
            var groupHeaderRows = [{ HeaderText: casinoDailyResources.Category, Name: 'ColCategory', MergeRowNumber: 2}];
            var columns = [{
                HeaderText: casinoDailyResources.Category,
                Width: 300,
                Name: 'ColCategory',
                HeaderAlign: 'center',
                CellAlign: 'left'
            }];

            var casinoGroup, casinoGroupName, communications, commLeng, commName;
            var firstCasinoGroup = report.CategoryCasinoGroups[0].CasinoGroups;
            for (var i = 0, casinoGrpLen = firstCasinoGroup.length; i < casinoGrpLen; i++) {
                casinoGroup = firstCasinoGroup[i];
                casinoGroupName = casinoGroup.Name;

                communications = casinoGroup.Communications;
                commLeng = communications.length;
                groupHeaderRows.push({ HeaderText: casinoGroup.ResourceName, Name: casinoGroupName + communications[0].Name, MergeColNumber: commLeng });

                for (var j = 0; j < commLeng; j++) {
                    commName = communications[j].Name;
                    columns.push({
                        HeaderText: commName,
                        Width: 70,
                        Name: casinoGroupName + commName,
                        HeaderAlign: 'center',
                        CellAlign: 'center',
                        FormatNumber: crmConstants.Enums.FormatConstant.IntNumber
                    });
                }
            }

            return {
                customizeHeader: true,
                groupHeaderRows: [{ Columns: groupHeaderRows}],
                columns: columns,
                showTitle: true,
                titleInfo: {
                    text: report.Title,
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        scope.exportCasinoDailySummary();
                    },
                    customButtons: '<input id="button-viewchart" class="btnCharting" type="button" title="' + casinoDailyResources.ViewChart + '"/>\
                    <input id="button-viewdetail" class="btnViewDetail" type="button" title="' + casinoDailyResources.ViewDetail + '"/>'
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
                headerColumnHeight: 20
            };
        }

        function getGridBody(categoryCasinoGroups) {
            var bodyData = [];

            var cateCasinoGroup, casinoGroups, communications;
            var casinoGrp;
            var currentColumn;

            var reportLen = categoryCasinoGroups.length;
            for (var i = 0; i < reportLen; i++) {
                cateCasinoGroup = categoryCasinoGroups[i];
                currentColumn = [{ Name: 'ColCategory', Value: cateCasinoGroup.Category}];

                casinoGroups = cateCasinoGroup.CasinoGroups;
                for (var j = 0, casinoGrpLen = casinoGroups.length; j < casinoGrpLen; j++) {
                    casinoGrp = casinoGroups[j];
                    communications = casinoGrp.Communications;
                    for (var k = 0, comLen = communications.length; k < comLen; k++) {
                        currentColumn.push({ Name: casinoGrp.Name + communications[k].Name, Value: communications[k].Value });
                    }
                }

                bodyData.push({ Columns: currentColumn });
            }

            return bodyData;
        }

        function getGridFooter(scope, total) {
            var totalRow = [{ Name: 'ColCategory', Value: total.Category, MergeRowNumber: 2}];
            var grandTotal = [];

            // chart.
            scope.dailySummaryChart = [];
            var bgColorFunc = parent.crmChart.getBackGroundColor;

            var casinoGrp, casinoGrpName, comm, commLen;
            for (var i = 0, casinoGrpLen = total.CasinoGroups.length; i < casinoGrpLen; i++) {
                casinoGrp = total.CasinoGroups[i];
                casinoGrpName = casinoGrp.Name;

                commLen = casinoGrp.Communications.length;
                for (var j = 0; j < commLen; j++) {
                    comm = casinoGrp.Communications[j];
                    totalRow.push({ Name: casinoGrpName + comm.Name, Value: comm.Value });
                }

                grandTotal.push({ Name: casinoGrpName + casinoGrp.Communications[0].Name, Value: casinoGrp.GrandTotal, MergeColNumber: commLen });
                // chart.
                if (casinoGrp.GrandTotal !== 0) {
                    scope.dailySummaryChart.push({
                        title: casinoGrp.ResourceName,
                        value: casinoGrp.GrandTotal,
                        color: bgColorFunc(i)
                    });
                }
            }

            return [{ Columns: totalRow }, { Columns: grandTotal}];
        }

        function renderCasinoDailySummary(scope, element, attrs) {
            var report = scope.dailySummary;
            var categoryCasinoGroups = report.CategoryCasinoGroups;
            var total = report.Total;

            var bodyData = categoryCasinoGroups ? getGridBody(categoryCasinoGroups) : [];
            var footerData = getGridFooter(scope, total);
            var gridOptions = getGridOptions(scope, report, bodyData, footerData);

            var grid = element.find('#table-daily-summary');
            grid.empty().removeClass('hide').fanexGrid(gridOptions);
            // view chart event.
            element.find('#button-viewchart').click(function () {
                scope.viewCasinoDailySummaryChart();
            });
            element.find('#button-viewdetail').click(function () {
                scope.getCasinoDailyDetail();
            });
        }

        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                scope.$watch('dailySummary', function (newValue, oldValue) {
                    if (!newValue) {
                        return;
                    }

                    renderCasinoDailySummary(scope, element, attrs);
                });
            }
        };
    });
})();