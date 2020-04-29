(function (angular) {
    'use strict';

    angular.module('addmintool-settingmanagement-application').directive('licenseeMappingGrid', licenseeMappingGrid);
    licenseeMappingGrid.$inject = [];

    function licenseeMappingGrid() {
        function generateColumns() {
            let columns = [
                { HeaderText: 'Setting', Width: 180, Name: 'Setting', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Source Mapping', Width: 180, Name: 'SettingSource', HeaderAlign: 'center', CellAlign: 'left', CellCssClass: "cell-source-mapping" },
                { HeaderText: 'Licensee Manager', Width: 180, Name: 'LicenseeManager', HeaderAlign: 'center', CellAlign: 'left', CellCssClass: "cell-licensee-manager" },
                { HeaderText: 'Setting Id', Width: 80, Name: 'SettingId', HeaderAlign: 'center', CellAlign: 'right' },
                { HeaderText: 'Edit', Width: 70, Name: 'Edit', HeaderAlign: 'center', CellAlign: 'center' }
            ];

            return columns;
        }

        function generateBodyRows(scope) {
            let rows = [];
            const totalItems = scope.dataSources.length;

            for (let i = 0; i < totalItems; i++) {
                const item = scope.dataSources[i],
                    settingId = item.SettingId;
                let columns = [
                    { Name: 'Setting', Value: item.Setting },
                    { Name: 'SettingSource', Value: item.SettingSource },
                    { Name: 'LicenseeManager', Value: item.LicenseeManager },
                    { Name: 'SettingId', Value: item.SettingId },
                    {
                        Name: 'Edit', Value: '<i class="fa fa-pencil js-edit" data-setting-id="' + settingId + '"></i>'
                            + '<i class="fa fa-eraser js-clear" data-setting-id="' + settingId + '"></i>'
                    }
                ];

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

        function renderGrid(scope, element) {
            if (scope.dataSources === null) {
                return;
            }

            $(element).empty();

            const gridOptions = {
                columns: generateColumns(),
                bodyRows: generateBodyRows(scope),
                gridExpandHeight: 300
            };

            $(element).removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

            $('.js-edit:not(.disabled)', $(element)).off('click').on('click', function () {
                $(this).removeClass('js-edit').removeClass('fa-pencil').addClass('fa-floppy-o').addClass('js-save');
                $(this).closest('tr').addClass('selected-row');
                registerEventSave(scope, element);

                $(element).find('.js-edit').addClass('disabled');
                $(element).find('.js-clear').addClass('disabled');

                const $tr = $(this).closest('tr'),
                    sourceMapping = $tr.find('.cell-source-mapping')[0].innerHTML,
                    licenseeManager = $tr.find('.cell-licensee-manager')[0].innerHTML;

                $tr.find('.cell-source-mapping').html('<select class="js-select2"><option></option></select>');
                $tr.find('.cell-licensee-manager').html('<select class="js-select2-ajax"></select>');

                $tr.find('.js-select2').select2({
                    allowClear: true,
                    placeholder: '',
                    dropdownParent: $(element),
                    data: scope.licensees
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