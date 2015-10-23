(function () {
    "use strict";
    /* global activityLogResources,
    crmConstants,
    sharedScriptManager */
    /* jshint unused: vars */
    angular.module('activitylog-application').directive('tableActivityLogsContainer', function () {
        /* jshint camelcase: false */
        function renderActivityLogs(scope, element, attrs) {
            var gridOptions;
            function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
                gridOptions.pagerOption.currentPage = page_index;
                gridOptions.pagerOption.itemsPerPage = itemsPerPage;
                tableActivityLogs.fanexGrid(gridOptions);

                scope.searchActivityLogs(page_index, itemsPerPage);
                return false;
            }

            /* jshint unused: vars */
            if (!scope.isSearched) {
                return;
            }

            var tableActivityLogs = element.find('#table-activity-logs');
            var logList = scope.activityLogs;
            var bodyData = [];
            var footerData = [];
            tableActivityLogs.empty();
            if (logList && logList.length > 0) {
                var logLength = logList.length;
                var info, fieldNames, oldValues, newValues;
                var mergeRowNumber = 0;
                var evenClass = '';
                for (var i = 0; i < logLength; i++) {
                    info = logList[i];
                    fieldNames = info.FieldNames;
                    oldValues = info.OldValuesList;
                    newValues = info.NewValuesList;
                    mergeRowNumber = newValues.length;
                    evenClass = (i % 2 === 0) ? 'freegrid-even' : '';

                    var row = [
                    { Name: 'ColLogDate', Value: info.LogDateString, MergeRowNumber: mergeRowNumber },
                    { Name: 'ColUsername', Value: info.Username, MergeRowNumber: mergeRowNumber, CssClass: 'username-wordwrap' },
                    { Name: 'ColAction', Value: info.ActionName, MergeRowNumber: mergeRowNumber}];
                    if (mergeRowNumber === 0) {
                        bodyData.push({
                            Columns: row.concat([
                        { Name: 'ColFieldName', Value: '' },
                        { Name: 'ColOldValues', Value: '' },
                        { Name: 'ColNewValues', Value: ''}]),
                            CssClass: evenClass
                        });
                    }
                    else {
                        for (var j = 0; j < mergeRowNumber; j++) {
                            row = j !== 0 ? [] : row;
                            var fieldName = fieldNames[j];
                            var oldText = (oldValues && oldValues.length) > 0 ?
                            sharedScriptManager.replaceLineBreak(oldValues[j]) :
                            '';
                            var newText = sharedScriptManager.replaceLineBreak(newValues[j]);

                            var isEncoded = fieldName !== 'Remark' &&
                            fieldName !== 'Solution' &&
                            fieldName !== 'Review' &&
                            fieldName !== 'SpecialRemark';
                            bodyData.push({
                                Columns: row.concat([
                                { Name: 'ColFieldName', Value: fieldName },
                                { Name: 'ColOldValues',
                                    Value: isEncoded ? sharedScriptManager.encodeHtml(oldText) : oldText,
                                    CssClass: 'word-wrap'
                                },
                                {
                                    Name: 'ColNewValues',
                                    Value: isEncoded ? sharedScriptManager.encodeHtml(newText) : newText,
                                    CssClass: 'word-wrap'
                                }]),
                                CssClass: evenClass
                            });
                        }
                    }
                }
            }

            /* jshint camelcase: false */
            gridOptions = {
                columns: [{ HeaderText: activityLogResources.LogTime, Width: 80, Name: 'ColLogDate', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: activityLogResources.EditedBy, Width: 100, Name: 'ColUsername', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: activityLogResources.Action, Width: 120, Name: 'ColAction', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: activityLogResources.FieldName, Width: 150, Name: 'ColFieldName', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: activityLogResources.OldValue, Width: 235, Name: 'ColOldValues', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: activityLogResources.NewValue, Width: 235, Name: 'ColNewValues', HeaderAlign: 'center', CellAlign: 'left'}],
                showTitle: true,
                titleInfo: {
                    text: activityLogResources.Customer_Activity_Logs
                },
                bodyRows: bodyData,
                footerRows: footerData,
                numberOfFrozenColumn: 0,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: 100,
                showDistinctRow: false,
                showHighlightRow: false,
                headerColumnHeight: 29,
                bodyRowHeight: 29,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: scope.searchCriteria.ItemsPerPage,
                    currentPage: scope.searchCriteria.PageIndex,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                    totalItem: scope.totalItems,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            tableActivityLogs.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newValues, oldValues) {
                    /* jshint unused: vars */
                    renderActivityLogs(scope, element, attrs);
                });
            }
        };
    });
})();