"use strict";
/* global redemptionViewLogApplication,
viewResources,
crmConstants,
sharedScriptManager */
/* jshint camelcase: false */
redemptionViewLogApplication.directive('tableRedemptionLogsContainer', function () {
    var cachedLogs = [];
    function compareValues(sortInfo, valueA, valueB) {
        var sortDataTypeEnums = crmConstants.Enums.SortDataType;
        var isGreaterThan;
        switch (sortInfo.sortDataType) {
            case sortDataTypeEnums.Number:
                isGreaterThan = (sortInfo.sortFactor === 0 || valueB === valueA) ?
                                0 :
                                (valueA - valueB) * sortInfo.sortFactor > 0 ?
                                1 :
                                -1;
                break;
            case sortDataTypeEnums.String:
                isGreaterThan = (sortInfo.sortFactor === 0 || valueB === valueA) ?
                                0 :
                                (valueB < valueA && sortInfo.sortFactor === 1) ||
                                (valueA < valueB && sortInfo.sortFactor === -1) ?
                                1 :
                                -1;
                break;
            case sortDataTypeEnums.Date:
                var dateA = new Date(valueA);
                var dateB = new Date(valueB);
                isGreaterThan = (sortInfo.sortFactor === 0 || sharedScriptManager.datesEqual(dateB, dateA)) ?
                                0 :
                                (dateB < dateA && sortInfo.sortFactor === 1) ||
                                (dateA < dateB && sortInfo.sortFactor === -1) ?
                                1 :
                                -1;
                break;
        }

        return isGreaterThan;
    }

    function getGridOptions_MappingSortInfo(columnName,
        currentDirection) {
        var sortDirectionEnums = crmConstants.Enums.SortDirection;
        var sortDataTypeEnums = crmConstants.Enums.SortDataType;
        var sortInfo = {
            sortFactor: 0,
            sortField: '',
            sortDataType: sortDataTypeEnums.String
        };

        currentDirection = currentDirection.toUpperCase();
        switch (currentDirection) {
            case sortDirectionEnums.Asc:
                sortInfo.sortFactor = 1;
                break;
            case sortDirectionEnums.Desc:
                sortInfo.sortFactor = -1;
                break;
            default:
                sortInfo.sortFactor = 0;
                break;
        }

        switch (columnName) {
            case "ColSubject":
                sortInfo.sortField = "Subject";
                sortInfo.sortDataType = sortDataTypeEnums.String;
                break;
            case "ColClaimedTime":
                sortInfo.sortField = "ClaimedTime";
                sortInfo.sortDataType = sortDataTypeEnums.Date;
                break;
            case "ColStatus":
                sortInfo.sortField = "StatusName";
                sortInfo.sortDataType = sortDataTypeEnums.String;
                break;
        }

        return sortInfo;
    }

    function sortReport(sortInfo, cachedLogs) {
        switch (sortInfo.sortField) {
            case "Subject":
                return cachedLogs.sort(function (elementA, elementB) {
                    var valueA = elementA[sortInfo.sortField].toLowerCase();
                    var valueB = elementB[sortInfo.sortField].toLowerCase();
                    var isGreaterThan = compareValues(sortInfo, valueA, valueB);
                    return isGreaterThan;
                });
            case "ClaimedTime":
                return cachedLogs.sort(function (elementA, elementB) {
                    var valueA = elementA[sortInfo.sortField];
                    var valueB = elementB[sortInfo.sortField];
                    var isGreaterThan = compareValues(sortInfo, valueA, valueB);
                    return isGreaterThan;
                });
            case "StatusName":
                return cachedLogs.sort(function (elementA, elementB) {
                    var valueA = elementA.Status.Value.toLowerCase();
                    var valueB = elementB.Status.Value.toLowerCase();
                    var isGreaterThan = compareValues(sortInfo, valueA, valueB);
                    return isGreaterThan;
                });
        }

        return cachedLogs;
    }

    /* jshint camelcase: false */
    function getGridOptions_SortedAndPagingData(sortInfo) {
        var logList = sortReport(sortInfo, cachedLogs);
        var bodyData = [];
        if (logList && logList.length > 0) {
            var logLength = logList.length;
            for (var i = 0; i < logLength; i++) {
                var info = logList[i];
                bodyData.push({
                    Columns: [
                        {
                            Name: "ColSubject",
                            Value: '<span class="link editlog-link" data-logid="' + info.Id + '">' + info.Subject + '</span>',
                            TdClass: 'editlog-link-td'
                        },
                        { Name: "ColClaimedTime", Value: info.ClaimedTime },
                        { Name: "ColStatus", Value: info.Status.Value}],
                    CssClass: 'editlog-link-row'
                });
            }
        }

        return bodyData;
    }

    function renderRedemptionLogs(scope, element, attrs) {
        if (!scope.isSearched) {
            return;
        }

        /* jshint unused: vars */
        var tableRedemptionLogs = element.find("#table-redemption-logs");
        var logList = scope.logs;
        cachedLogs = logList;
        var bodyData = [];
        var footerData = [];
        tableRedemptionLogs.empty();
        if (logList && logList.length > 0) {
            var logLength = logList.length;
            for (var i = 0; i < logLength; i++) {
                var info = logList[i];
                bodyData.push({
                    Columns: [
                        {
                            Name: "ColSubject",
                            Value: '<span class="link editlog-link" data-logid="' + info.Id + '">' + info.Subject + '</span>',
                            TdClass: 'editlog-link-td'
                        },
                        { Name: "ColClaimedTime", Value: info.ClaimedTime },
                        { Name: "ColStatus", Value: info.Status.Value}],
                    CssClass: 'editlog-link-row'
                });
            }
        }

        /* jshint camelcase: false */
        var gridOptions = {
            columns: [{ HeaderText: viewResources.Subject, Width: 675, Name: "ColSubject", HeaderAlign: "center", CellAlign: "left", Sort: true },
                { HeaderText: viewResources.Claimed_Time, Width: 120, Name: "ColClaimedTime", HeaderAlign: "center", CellAlign: "center", Sort: true },
                { HeaderText: viewResources.Status, Width: 140, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center", Sort: true}],
            showTitle: true,
            titleInfo: {
                text: viewResources.Customer_Redemption_Logs
            },
            bodyRows: bodyData,
            footerRows: footerData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 150,
            showDistinctRow: true,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 29,
            sortOption:
            {
                customGridSort: function (currentBodyRows, columnName, currentDirection) {
                    var sortInfo = getGridOptions_MappingSortInfo(columnName,
                        currentDirection);
                    var bodyRows = getGridOptions_SortedAndPagingData(sortInfo);
                    return bodyRows;
                },
                afterGridSorted: function (currentBodyRows, columnName, currentDirection) {
                    var editLogLinks = tableRedemptionLogs.find(".editlog-link-td .editlog-link");
                    editLogLinks.unbind("click");
                    editLogLinks.click(function () {
                        scope.editLog($(this).attr("data-logid"));
                    });

                    var editLogRows = tableRedemptionLogs.find(".editlog-link-row");
                    editLogRows.dblclick(function () {
                        scope.editLog($(this).find('.editlog-link').attr("data-logid"));
                    });

                    return currentBodyRows;
                }
            }
        };

        tableRedemptionLogs.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        var editLogLinks = tableRedemptionLogs.find(".editlog-link-td .editlog-link");
        editLogLinks.unbind("click");
        editLogLinks.click(function () {
            scope.editLog($(this).attr("data-logid"));
        });

        var editLogRows = tableRedemptionLogs.find(".editlog-link-row");
        editLogRows.dblclick(function () {
            scope.editLog($(this).find('.editlog-link').attr("data-logid"));
        });
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (oldval, newval) {
                /* jshint unused: vars */
                renderRedemptionLogs(scope, element, attrs);
            });
        }
    };
});