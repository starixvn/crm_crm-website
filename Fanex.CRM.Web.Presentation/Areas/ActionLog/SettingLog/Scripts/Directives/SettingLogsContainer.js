(function () {
    "use strict";
    /* global
    settingLogResources,
    crmConstants,
    sharedScriptManager */
    angular.module('settinglog-application').directive('tableSettingLogsContainer', function () {
        /* jshint camelcase: false */
        function renderSettingLogs(scope, element) {
            var gridOptions;
            function ReRenderAfterPaged(page_index, itemsPerPage) {
                gridOptions.pagerOption.currentPage = page_index;
                gridOptions.pagerOption.itemsPerPage = itemsPerPage;
                tableSettingLogs.fanexGrid(gridOptions);

                scope.searchSettingLogs(page_index, itemsPerPage);
                return false;
            }

            if (!scope.isSearched) {
                return;
            }

            var tableSettingLogs = element.find('#table-setting-logs');
            var logList = scope.settingLogs;
            var bodyData = [];
            var footerData = [];
            tableSettingLogs.empty();
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
                    { Name: 'ColAction', Value: info.ActionName, MergeRowNumber: mergeRowNumber },
                    { Name: 'ColEntity', Value: info.Entity, MergeRowNumber: mergeRowNumber },
                    { Name: 'ColEntityId', Value: info.Id, MergeRowNumber: mergeRowNumber}];
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
                            bodyData.push({
                                Columns: row.concat([
                                { Name: 'ColFieldName', Value: fieldNames[j] },
                                { Name: 'ColOldValues',
                                    Value: (oldValues && oldValues.length) > 0 ? sharedScriptManager.replaceLineBreak(oldValues[j]) : '',
                                    CssClass: 'word-wrap'
                                },
                                { Name: 'ColNewValues', Value: sharedScriptManager.replaceLineBreak(newValues[j]), CssClass: 'word-wrap'}]),
                                CssClass: evenClass
                            });
                        }
                    }
                }
            }

            /* jshint camelcase: false */
            gridOptions = {
                columns: [{ HeaderText: settingLogResources.LogTime, Width: 80, Name: 'ColLogDate', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: settingLogResources.EditedBy, Width: 100, Name: 'ColUsername', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: settingLogResources.Action, Width: 120, Name: 'ColAction', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: settingLogResources.Entity, Width: 100, Name: 'ColEntity', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: settingLogResources.EntityId, Width: 50, Name: 'ColEntityId', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: settingLogResources.FieldName, Width: 120, Name: 'ColFieldName', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: settingLogResources.OldValue, Width: 170, Name: 'ColOldValues', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: settingLogResources.NewValue, Width: 170, Name: 'ColNewValues', HeaderAlign: 'center', CellAlign: 'left'}],
                showTitle: true,
                titleInfo: {
                    text: settingLogResources.Setting_Logs
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

            tableSettingLogs.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newValues, oldValues) {
                    /* jshint unused: false */
                    renderSettingLogs(scope, element);
                });
            }
        };
    });
})();