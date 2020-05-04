(function (angular) {
    'use strict';

    angular.module('addmintool-settingmanagement-application').directive('licenseeMappingGrid', licenseeMappingGrid);
    licenseeMappingGrid.$inject = [];

    function licenseeMappingGrid() {
        function generateColumns() {
            const columns = [
                { HeaderText: 'Setting', Width: 180, MaxResizeWidth: 200, Name: 'Setting', HeaderAlign: 'center', CellAlign: 'left', CanResize: true },
                {
                    HeaderText: 'Source Mapping', Width: 180, MaxResizeWidth: 200, Name: 'SettingSource',
                    HeaderAlign: 'center', CellAlign: 'left', CellCssClass: "cell-source-mapping", CanResize: true
                },
                {
                    HeaderText: 'Licensee Manager', Width: 180, MaxResizeWidth: 200, Name: 'LicenseeManager',
                    HeaderAlign: 'center', CellAlign: 'left', CellCssClass: "cell-licensee-manager", CanResize: true
                },
                { HeaderText: 'Setting Id', Width: 80, Name: 'SettingId', HeaderAlign: 'center', CellAlign: 'right' }
            ];
            if (isHost) {
                columns.push({ HeaderText: 'Edit', Width: 70, Name: 'Edit', HeaderAlign: 'center', CellAlign: 'center', CellCssClass: "cell-action" });
            }

            return columns;
        }

        function generateBodyRows(scope) {
            let rows = [];
            const totalItems = scope.dataSources.length;

            for (let i = 0; i < totalItems; i++) {
                const item = scope.dataSources[i],
                    settingId = item.SettingId;
                const columns = [
                    { Name: 'Setting', Value: item.Setting },
                    { Name: 'SettingSource', Value: item.SettingSource },
                    { Name: 'LicenseeManager', Value: item.LicenseeManager },
                    { Name: 'SettingId', Value: item.SettingId }
                ];
                if (isHost) {
                    columns.push({
                        Name: 'Edit', Value: '<button type="button" class="fa fa-pencil js-edit" data-setting-id="' + settingId + '"></button>'
                            + '<button type="button" class="fa fa-eraser js-clear" data-setting-id="' + settingId + '"></button>'
                    });
                }

                rows.push({ Columns: columns });
            }

            return rows;
        }

        function registerEventSave(scope, element) {
            $('.js-save:not(.disabled)', $(element)).off('click').on('click', function () {
                const param = {
                    settingId: parseInt($(this).data('setting-id'), 10),
                    sourceMapping: $(this).closest('tr').find('.js-select2').val(),
                    licenseeManager: $(this).closest('tr').find('.js-select2-ajax').val()
                };

                scope.onSave(param);
            });
        }

        function getSourceMapping(scope, element, sourceMapping) {
            const sourceMappings = [...scope.licensees];
            $(element).find('.cell-source-mapping').each(function () {
                if ($(this).html().length > 0) {
                    const index = sourceMappings.findIndex(x => x.id === $(this).html());
                    if (index > -1) {
                        sourceMappings.splice(index, 1);
                    }
                }
            });

            if (sourceMapping !== '') {
                sourceMappings.push({ id: sourceMapping, text: sourceMapping });
            }

            return sourceMappings;
        }

        function renderGrid(scope, element) {
            if (scope.dataSources === null) {
                return;
            }

            $(element).empty();

            const gridOptions = {
                columns: generateColumns(),
                bodyRows: generateBodyRows(scope),
                autoResizeColumn: true,
                gridExpandHeight: 300
            };

            $(element).removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

            $('.js-edit:not(disabled)', $(element)).off('click').on('click', function () {
                $(this).removeClass('js-edit').removeClass('fa-pencil').addClass('fa-floppy-o').addClass('js-save');
                $(this).closest('tr').addClass('selected-row');
                registerEventSave(scope, element);

                $(element).find('.js-edit').prop('disabled', true);
                $(element).find('.js-clear').prop('disabled', true);

                const $tr = $(this).closest('tr'),
                    sourceMapping = $tr.find('.cell-source-mapping')[0].innerHTML,
                    licenseeManager = $tr.find('.cell-licensee-manager')[0].innerHTML;

                const sourceMappings = getSourceMapping(scope, element, sourceMapping);

                if (sourceMapping !== '') {
                    $tr.find('.cell-source-mapping').html('<select class="js-select2"><option value="'
                        + sourceMapping + '" selected>' + sourceMapping + '</option></select>');
                } else {
                    $tr.find('.cell-source-mapping').html('<select class="js-select2"><option></option></select>');
                }

                if (licenseeManager !== '') {
                    $tr.find('.cell-licensee-manager').html('<select class="js-select2-ajax"><option value="'
                        + licenseeManager + '" selected>' + licenseeManager + '</option></select>');
                } else {
                    $tr.find('.cell-licensee-manager').html('<select class="js-select2-ajax"></select>');
                }

                $tr.find('.js-select2').select2({
                    allowClear: true,
                    placeholder: '',
                    dropdownParent: $(element),
                    data: sourceMappings
                });

                $tr.find('.js-select2-ajax').select2({
                    allowClear: true,
                    placeholder: '',
                    minimumInputLength: 2,
                    dropdownParent: $(element),
                    ajax: {
                        url: window.rootUrl + 'LicenseeMapping/GetLicenseeManager',
                        dataType: 'json',
                        method: 'post',
                        data: function (params) {
                            const query = {
                                username: params.term
                            };

                            return query;
                        },
                        processResults: function (data) {
                            return {
                                results: data.map(item => { return { id: item.Username, text: item.Username }; })
                            };
                        }
                    }
                });
            });

            $('.js-clear:not(.disabled)', $(element)).off('click').on('click', function () {
                const settingId = parseInt($(this).data('setting-id'), 10);
                scope.onClear(settingId);
            });
        }

        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {
                dataSources: '=ngModel',
                licensees: '=licensees',
                onSave: '=onSave',
                onClear: '=onClear'
            },
            link: function (scope, element) {
                scope.$watch('dataSources', function (oldval, newval) {
                    renderGrid(scope, element);
                });
            }
        };
    }
})(angular);