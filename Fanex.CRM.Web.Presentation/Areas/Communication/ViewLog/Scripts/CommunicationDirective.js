(function () {
    "use strict";
    /* global moment,
    crmConstants,
    sharedScriptManager,
    viewResources */
    /* jshint camelcase: false */
    angular.module('communication-application').directive('tableCommunicationLogsContainer', function () {
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
                case "ColRunningNumber":
                    sortInfo.sortField = "RunningNumber";
                    sortInfo.sortDataType = sortDataTypeEnums.Number;
                    break;
                case "ColFeedbackDate":
                    sortInfo.sortField = "FeedbackDate";
                    sortInfo.sortDataType = sortDataTypeEnums.Date;
                    break;
                case "ColCategory":
                    sortInfo.sortField = "Category";
                    sortInfo.sortDataType = sortDataTypeEnums.String;
                    break;
                case "ColSubject":
                    sortInfo.sortField = "Subject";
                    sortInfo.sortDataType = sortDataTypeEnums.String;
                    break;
                case "ColStatus":
                    sortInfo.sortField = "StatusName";
                    sortInfo.sortDataType = sortDataTypeEnums.String;
                    break;
                case "ColDuration":
                    sortInfo.sortField = "Duration";
                    sortInfo.sortDataType = sortDataTypeEnums.Date;
                    break;
                case "ColHandler":
                    sortInfo.sortField = "Handler";
                    sortInfo.sortDataType = sortDataTypeEnums.String;
                    break;
                case "ColReviewer":
                    sortInfo.sortField = "Reviewer";
                    sortInfo.sortDataType = sortDataTypeEnums.String;
                    break;
            }

            return sortInfo;
        }

        function sortReport(sortInfo, cachedLogs) {
            if (sortInfo.sortField === "Subject" ||
            sortInfo.sortField === "StatusName" ||
            sortInfo.sortField === "Handler" ||
            sortInfo.sortField === "Reviewer") {
                return cachedLogs.sort(function (elementA, elementB) {
                    var valueA = elementA[sortInfo.sortField].toLowerCase();
                    var valueB = elementB[sortInfo.sortField].toLowerCase();
                    var isGreaterThan = compareValues(sortInfo, valueA, valueB);
                    return isGreaterThan;
                });
            }

            if (sortInfo.sortField === "RunningNumber" ||
            sortInfo.sortField === "FeedbackDate") {
                return cachedLogs.sort(function (elementA, elementB) {
                    var valueA = elementA[sortInfo.sortField];
                    var valueB = elementB[sortInfo.sortField];
                    var isGreaterThan = compareValues(sortInfo, valueA, valueB);
                    return isGreaterThan;
                });
            }

            if (sortInfo.sortField === "Category") {
                return cachedLogs.sort(function (elementA, elementB) {
                    var valueA = (elementA.MainCategory.Name + "<br/>" + elementA.SubCategory.Name);
                    var valueB = (elementB.MainCategory.Name + "<br/>" + elementB.SubCategory.Name);
                    var isGreaterThan = compareValues(sortInfo, valueA, valueB);
                    return isGreaterThan;
                });
            }

            if (sortInfo.sortField === "Duration") {
                return cachedLogs.sort(function (elementA, elementB) {
                    var valueA = elementA[sortInfo.sortField];
                    var valueB = elementB[sortInfo.sortField];

                    var dateA = new Date(1, 1, 1, valueA.Hours, valueA.Minutes, valueA.Seconds);
                    var dateB = new Date(1, 1, 1, valueB.Hours, valueB.Minutes, valueB.Seconds);
                    var isGreaterThan =
                                (sortInfo.sortFactor === 0 || sharedScriptManager.datesEqual(dateB, dateA)) ?
                                0 :
                                (dateB < dateA && sortInfo.sortFactor === 1) ||
                                (dateA < dateB && sortInfo.sortFactor === -1) ?
                                1 :
                                -1;
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
                            Name: "ColRunningNumber",
                            Value: '<span class="link editlog-link" data-logindex="' + i + '">' + info.RunningNumber + '</span>',
                            TdClass: 'editlog-link-td'
                        },
                        {
                            Name: "ColUpdate",
                            Value: '<span class="updateIcon"  data-id="' + info.RunningNumber + '"></span>'
                        },
                        {
                            Name: "ColFeedbackDate",
                            Value: moment(new Date(info.FeedbackDate)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_2Line_String_Moment)
                        },
                        {
                            Name: "ColCategory",
                            Value: ('<div class="maincategory">' + info.MainCategory.Name + '</div>') +
                                (info.SubCategory.Name !== "" ? '<div class="subcategory">' + info.SubCategory.Name + '</div>' : "")
                        },
                        { Name: "ColSubject", Value: sharedScriptManager.encodeHtml(info.Subject) },
                        { Name: "ColStatus", Value: info.StatusName },
                        {
                            Name: "ColDuration",
                            Value: sharedScriptManager.stringFormat("{0}:{1}:{2}",
                                [info.Duration.Hours, info.Duration.Minutes, info.Duration.Seconds])
                        },
                        { Name: "ColHandler", Value: info.Handler },
                        { Name: "ColReviewer", Value: info.Reviewer}],
                        CssClass: 'editlog-link-row row-' + info.StatusName.toLowerCase()
                    });
                }
            }

            return bodyData;
        }

        function renderCommunicationLogs(scope, element) {
            if (scope.logs === null || scope.mainCategory == null) {
                return;
            }

            var tableCommunicationLogs = element.find("#table-communication-logs");
            var logList = scope.logs;
            cachedLogs = logList;
            var bodyData = [];
            var footerData = [];

            tableCommunicationLogs.empty();
            if (logList && logList.length > 0) {
                var logLength = logList.length;
                for (var i = 0; i < logLength; i++) {
                    var info = logList[i];
                    bodyData.push({
                        Columns: [
                        {
                            Name: "ColRunningNumber",
                            Value: '<span class="link editlog-link" data-logindex="' + i + '">' + info.RunningNumber + '</span>',
                            TdClass: 'editlog-link-td'
                        },
                        {
                            Name: "ColUpdate",
                            Value: '<span class="updateIcon"  data-id="' + info.RunningNumber + '"></span>'
                        },
                        {
                            Name: "ColFeedbackDate",
                            Value: moment(new Date(info.FeedbackDate)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_2Line_String_Moment)
                        },
                        {
                            Name: "ColCategory",
                            Value: nameMapping.renderCategoryColumn(info, scope.mainCategory, true)
                        },
                        { Name: "ColSubject", Value: sharedScriptManager.encodeHtml(info.Subject) },
                        { Name: "ColStatus", Value: info.StatusName },
                        {
                            Name: "ColDuration",
                            Value: sharedScriptManager.stringFormat("{0}:{1}:{2}",
                                [info.Duration.Hours, info.Duration.Minutes, info.Duration.Seconds])
                        },
                        { Name: "ColHandler", Value: info.Handler },
                        { Name: "ColReviewer", Value: info.Reviewer}],
                        CssClass: 'editlog-link-row row-' + info.StatusName.toLowerCase()
                    });
                }
            }

            var gridOptions = {
                columns: [{ HeaderText: viewResources.Running_No, Width: 100, Name: "ColRunningNumber", HeaderAlign: "center", CellAlign: "left", Sort: true },
                { HeaderText: viewResources.Update, Width: 50, Name: 'ColUpdate', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: viewResources.Feedback_Date, Width: 110, Name: "ColFeedbackDate", HeaderAlign: "center", CellAlign: "center", Sort: true },
                {
                    HeaderText: sharedScriptManager.stringFormat("{0}/{1}", [viewResources.Main_Category, viewResources.Sub_Category]),
                    Width: 220,
                    Name: "ColCategory",
                    HeaderAlign: "center",
                    CellAlign: "left",
                    Sort: true
                },
                { HeaderText: viewResources.Subject, Width: 130, Name: "ColSubject", HeaderAlign: "center", CellAlign: "left", Sort: true },
                { HeaderText: viewResources.Status, Width: 70, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center", Sort: true },
                { HeaderText: viewResources.Duration, Width: 80, Name: "ColDuration", HeaderAlign: "center", CellAlign: "center", Sort: true },
                { HeaderText: viewResources.Handler, Width: 100, Name: "ColHandler", HeaderAlign: "center", CellAlign: "left", Sort: true },
                { HeaderText: viewResources.Reviewer, Width: 100, Name: "ColReviewer", HeaderAlign: "center", CellAlign: "left", Sort: true}],
                showTitle: true,
                titleInfo: {
                    text: viewResources.Customer_Communication_Logs
                },
                noDataText: viewResources.There_is_no_data,
                bodyRows: bodyData,
                footerRows: footerData,
                numberOfFrozenColumn: 0,
                fullWidth: true,
                fullHeight: true,
                gridHeight: 150,
                showDistinctRow: false,
                showHighlightRow: true,
                headerColumnHeight: 29,
                bodyRowHeight: 51,
                showPager: true,
                pagerOption: {
                    itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage,
                    currentPage: 0,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                    totalItem: bodyData.length
                },
                sortOption:
            {
                customGridSort: function (currentBodyRows, columnName, currentDirection) {
                    /* jshint unused: false */
                    var sortInfo = getGridOptions_MappingSortInfo(columnName,
                        currentDirection);
                    var bodyRows = getGridOptions_SortedAndPagingData(sortInfo);
                    return bodyRows;
                },
                afterGridSorted: function (currentBodyRows, columnName, currentDirection) {
                    /* jshint unused: false */
                    var editLogLinks = tableCommunicationLogs.find(".editlog-link-td .editlog-link");
                    editLogLinks.unbind("click");
                    editLogLinks.click(function () {
                        scope.editCommunicationLog($(this).attr("data-logindex"));
                    });

                    var editLogRows = tableCommunicationLogs.find(".editlog-link-row");
                    editLogRows.dblclick(function () {
                        scope.editCommunicationLog($(this).find('.editlog-link').attr("data-logindex"));
                    });
                    return currentBodyRows;
                }
            }
            };

            tableCommunicationLogs.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
            var editLogLinks = tableCommunicationLogs.find(".editlog-link-td .editlog-link");
            editLogLinks.unbind("click");
            editLogLinks.click(function () {
                scope.editCommunicationLog($(this).attr("data-logindex"));
            });

            tableCommunicationLogs.find('.updateIcon')
                .unbind('click')
                .bind('click', function () {
                    scope.openQuickUpdateFeedbackPopup($(this).data('id'));
                });

            var editLogRows = tableCommunicationLogs.find(".editlog-link-row");
            editLogRows.dblclick(function () {
                scope.editCommunicationLog($(this).find('.editlog-link').attr("data-logindex"));
            });
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    /* jshint unused: false */
                    renderCommunicationLogs(scope, element);
                });
            }
        };
    }).directive('defaultFocus', function () {
        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    if (newval && !oldval) {
                        element.find("#handler").focus();
                        scope.completeSetFocus();
                    }
                });
            }
        };
    });
})();