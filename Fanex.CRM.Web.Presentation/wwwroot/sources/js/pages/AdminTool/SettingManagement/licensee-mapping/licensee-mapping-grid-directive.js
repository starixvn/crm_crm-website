(function (angular) {
    'use strict';

    angular.module('addmintool-settingmanagement-application').directive('licenseeMappingGrid', licenseeMappingGrid);
    licenseeMappingGrid.$inject = ['$window'];

    function licenseeMappingGrid($window) {
        let $gridwrapper;

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
                            + '<button type="button" class="icon-broom js-clear" data-setting-id="' + settingId + '"></button>'
                    });
                }

                rows.push({ Columns: columns });
            }

            return rows;
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

        function registerEventCancel(scope, element, $tr, sourceMapping, licenseeManager) {
            $('.js-cancel:not(.disabled)', $(element)).off('click').on('click', function () {
                $tr.find('.cell-source-mapping').html(sourceMapping);
                $tr.find('.cell-licensee-manager').html(licenseeManager);

                $(this).removeClass('fa fa-times js-cancel').addClass('icon-broom js-clear');
                $tr.find('.js-save').removeClass('fa-floppy-o js-save').addClass('fa-pencil js-edit');
                $(element).find('.js-edit').prop('disabled', false);
                $(element).find('.js-clear').prop('disabled', false);

                registerEventEdit(scope, element);
                registerEventClear(scope, element);
            });
        }

        function registerEventEdit(scope, element) {
            $('.js-edit:not(disabled)', $(element)).off('click').on('click', function () {
                const $tr = $(this).closest('tr'),
                    sourceMapping = $tr.find('.cell-source-mapping')[0].innerHTML,
                    licenseeManager = $tr.find('.cell-licensee-manager')[0].innerHTML;

                $(this).removeClass('fa-pencil js-edit').addClass('fa-floppy-o js-save');
                $tr.addClass('selected-row');
                $tr.find('.js-clear').removeClass('icon-broom js-clear').addClass('fa fa-times js-cancel');

                $(element).find('.js-edit').prop('disabled', true);
                $(element).find('.js-clear').prop('disabled', true);

                registerEventSave(scope, element);
                registerEventCancel(scope, element, $tr, sourceMapping, licenseeManager);

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

                let $select2 = $tr.find('.js-select2').select2({
                    allowClear: true,
                    placeholder: '',
                    dropdownParent: $gridwrapper,
                    data: sourceMappings
                });
                $select2.data('select2').$container.addClass('js-select2-source-mapping');
                $select2.data('select2').$dropdown.addClass('js-select2-source-mapping');

                $select2 = $tr.find('.js-select2-ajax').select2({
                    allowClear: true,
                    placeholder: '',
                    minimumInputLength: 2,
                    dropdownParent: $gridwrapper,
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
                $select2.data('select2').$container.addClass('js-select2-licensee-manager');
                $select2.data('select2').$dropdown.addClass('js-select2-licensee-manager');
            });
        }

        function registerEventClear(scope, element) {
            $('.js-clear:not(.disabled)', $(element)).off('click').on('click', function () {
                const settingId = parseInt($(this).data('setting-id'), 10);
                scope.onClear(settingId);
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
                autoResizeColumn: true,
                gridExpandHeight: 300
            };

            $(element).removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

            registerEventEdit(scope, element);
            registerEventClear(scope, element);

            $($window.parent.document).delegate('html', 'click', function (e) {
                let $select2 = $gridwrapper.find('.select2-container');
                if (!$select2.is(e.target) && $select2.has(e.target).length === 0 && $select2.hasClass('select2-container--open')) {
                    $select2.siblings('select').select2('close');
                }

                $select2 = $gridwrapper.find('.js-select2-source-mapping');
                if (($select2.is(e.target) || $select2.has(e.target).length > 0)
                    && $gridwrapper.find('.js-select2-licensee-manager').hasClass('select2-container--open')) {
                    $gridwrapper.find('.js-select2-licensee-manager').siblings('select').select2('close');
                }

                $select2 = $gridwrapper.find('.js-select2-licensee-manager');
                if (($select2.is(e.target) || $select2.has(e.target).length > 0)
                    && $gridwrapper.find('.js-select2-source-mapping').hasClass('select2-container--open')) {
                    $gridwrapper.find('.js-select2-source-mapping').siblings('select').select2('close');
                }
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
                    $gridwrapper = $($(element).closest('.licensee-mapping-grid-wrapper'));
                    renderGrid(scope, element);
                });
            }
        };
    }
})(angular);