/*global angular */
(function () {
    "use strict";
    angular.module('selectwithsearch', [])
        .filter("itemFormatFilter", function () {
            return function (value, item, level, formatCallback) {
                return formatCallback ? formatCallback(value, item.model, level) : value;
            };
        })
        .provider('selectwithsearchConfig', function () {
            var config = {
                collapsedAll: false, // temp setting, not 2 way binding
                hasFilter: true,
                hasToolbar: true,
                idMember: "id",
                nameMember: "name",
                selectedMember: null,
                dataMember: null,
                childrenMember: "children",
                itemFormatter: null,
                templateUrl: window.rootUrl + "Scripts/Directives/selectwithsearch/selectwithsearch.template.html",
                sourceData: null,
                checkedFilter: null, // function to checked item
                width: 250,
                height: null, // auto
                maxTextLength: 100,
                selectedTextLength: 30
            };
            return {
                set: function (options) {
                    angular.extend(config, options);
                },
                $get: function () {
                    return angular.extend({}, config);
                }
            };
        })
        .directive('selectwithsearchtree', ['$timeout', '$parse', '$compile', '$http', '$filter', 'selectwithsearchConfig',
            function ($timeout, $parse, $compile, $http, $filter, dropdowntreeConfig) {
                function containsText(item, searchText) {
                    var found = item.label.toLowerCase().indexOf(searchText.toLowerCase()) > -1;

                    if (!found && item.children) {
                        for (var i = 0; i < item.children.length; i++) {
                            found = containsText(item.children[i], searchText);
                            if (found) {
                                break;
                            }
                        }
                    }

                    return found;
                }               


                function createDropDownModelItem(settings, modelItem) {
                    var itemLabel = modelItem[settings.nameMember];
                    var item = {
                        id: settings.idMember ? modelItem[settings.idMember] : undefined,
                        label: itemLabel ? itemLabel.substr(0, settings.maxTextLength) : '',
                        model: modelItem,
                        checked: modelItem.checked === true ||
                            (settings.checkedFilter && settings.checkedFilter(modelItem)),
                        disabled: modelItem.disabled === true,
                        cssClass: modelItem.cssClass,
                        showChildren: true
                    };

                    if (modelItem[settings.childrenMember]) {
                        item.children = [];
                        var children = modelItem[settings.childrenMember];
                        var length = children.length;
                        for (var i = 0; i < length; i++) {
                            var child = createDropDownModelItem(settings, children[i]);
                            child.parent = item;
                            item.children.push(child);
                        }
                    }

                    return item;
                }

                // RETURN
                return {
                    restrict: 'E',
                    require: 'ngModel',
                    scope: {
                        selectedItems: "=ngModel",
                        settings: "=?",
                        textSettings: "=?",
                        ngOnOpen: "&",
                        ngOnClose: "&",
                        ngChange: "&",
                        disabled: '=ngDisabled'
                    },
                    link: function (scope, element, attrs, modelCtrl) {
                        var changeHandler = scope.ngChange || angular.noop; 
                        
                        scope._settings = scope._settings = angular.extend({}, dropdowntreeConfig);
                        scope._textSettings = {};

                        scope.searchText = {
                            value: ""
                        };
                        scope.collapsedAll = false;
                        scope.items = []; // dropdown model items
                        scope.header = "";
                        var _isWatchSelectedItems = true;

                        function parseSourceData(sourceData) {
                            var items = [];
                            if (!sourceData) {
                                return;
                            }
                            var settings = scope._settings;
                            var length = sourceData.length;
                            for (var i = 0; i < length; i++) {
                                var item = createDropDownModelItem(settings, sourceData[i]);
                                items.push(item);
                            }

                            return items;
                        }

                        function getHeaderText(selectedItems) {
                            if (angular.isArray(scope._settings.sourceData) === false || scope._settings.sourceData.length === 0) {
                                return scope._textSettings.NoData;
                            }

                            if (!selectedItems || selectedItems.length === 0) {
                                return scope._textSettings.NoItemChecked;
                            }

                            return selectedItems.label || selectedItems[scope._settings.nameMember];
                        }

                        function refreshViewValue(isRefreshSelectedItems) {
                            modelCtrl.$setViewValue(scope.selectedItems);                            
                        }

                        function parseData() {
                            if (scope._settings.dataMember) {
                                scope._settings.sourceData = scope._settings.sourceData[scope._settings.dataMember];
                            }

                            scope.items = parseSourceData(scope._settings.sourceData);                          
                        }

                        
                        scope.$watch("settings", function (newValue) {
                            if (!newValue) {
                                return;
                            }

                            scope._settings = angular.extend({}, dropdowntreeConfig, scope.settings);
                            if (typeof scope._settings.itemFormatter === "string") {
                                var filterInstance = $filter(scope._settings.itemFormatter);
                                scope._settings.itemFormatter = function (value, item, level) {
                                    return filterInstance(value, item, level);
                                };
                            }

                            var sourceDataType = typeof scope._settings.sourceData;

                            function startParseData() {
                                $timeout(function () {
                                    parseData();

                                    scope.toggleCollapseExpanse(scope._settings.collapsedAll);
                                }, 100);
                            }

                            if (sourceDataType === "string") // url
                            {
                                $http.get(scope._settings.sourceData)
                                    .success(function (response) {
                                        scope._settings.sourceData = response;

                                        startParseData();
                                    });
                            } else if (sourceDataType === "function") // function
                            {
                                scope._settings.sourceData = scope._settings.sourceData();
                                startParseData();
                            } else {
                                startParseData();
                            }
                        });

                        scope.$watch("textSettings", function () {
                            var defaultTextSettings = {
                                Filter: "Filter",
                                CheckAll: "Check all",
                                CollapseExpanse: "Collapse/Expanse",
                                CollapseBranch: "Collapse/Expanse this branch",
                                NoItemChecked: "Select options",
                                ItemsSelected: "# selected",
                                NoData: "N/A"
                            };

                            scope._textSettings = angular.extend(defaultTextSettings, scope.textSettings);
                            scope.header = getHeaderText(scope.selectedItems);
                        }, true);

                        //watch model change
                        scope.$watch("selectedItems", function (newVal) {
                            if (_isWatchSelectedItems === true && scope._settings && angular.isDefined(newVal)) {
                                _isWatchSelectedItems = false;

                                changeHandler();

                                scope.header = getHeaderText(scope.selectedItems);

                                _isWatchSelectedItems = true;
                            }
                        });

                        var popup = angular.element('<selectwithsearch-popup></selectwithsearch-popup>');
                        element.append($compile(popup)(scope));

                        scope.checkItem = function (item) {
                            scope.selectedItems = item;
                            refreshViewValue(true);
                            scope.toggleSelect();
                        };

                        scope.getCollapseExpandClass = function (item) {
                            if (!item.children || item.children.length === 0) {
                                return 'glyphicon-none';
                            }
                            return item.showChildren ? 'glyphicon-triangle-bottom' : 'glyphicon-triangle-right';
                        };

                        scope.getItemClass = function (item) {
                            return item.disabled ? 'disabled' : item.cssClass;
                        };

                        scope.toggleCollapseExpanse = function (value) {
                            if (typeof value === "boolean") {
                                scope.collapsedAll = value;
                            } else {
                                scope.collapsedAll = !scope.collapsedAll;
                            }

                            angular.forEach(scope.items, function (item) {
                                item.showChildren = !scope.collapsedAll;
                            });

                            refreshViewValue(true);
                        };

                        scope.filterSearchText = function (item) {
                            return scope.searchText.value ? containsText(item, scope.searchText.value) : true;
                        };

                        scope.clearSearchText = function () {
                            scope.searchText.value = "";
                            scope.focusSearchBox();
                        };

                        scope.touched = function () {
                            modelCtrl.$setTouched();
                        };
                    }
                };
            }
        ])

    .directive('selectwithsearchPopup', ['$document',
        function ($document) {
            return {
                restrict: 'E',
                replace: true,
                scope: false,
                template: '<div><div ng-include="_settings.templateUrl"></div><div>',
                link: function (scope, element) {
                    var onOpenHandler = scope.ngOnOpen || angular.noop;
                    var onCloseHandler = scope.ngOnClose || angular.noop;
                    var dropdownElement;

                    function isElementInArray(element, elementArray) {
                        for (var i = 0; i < elementArray.length; i++) {
                            if (element === elementArray[i]) {
                                return true;
                            }
                        }
                        return false;
                    }

                    function assignDropdownElement() {
                        if (!dropdownElement) {
                            dropdownElement = angular.element(element[0].querySelector('.dropdown'));
                        }
                    }

                    scope.toggleSelect = function () {
                        assignDropdownElement();
                        if (dropdownElement.hasClass('open')) {
                            dropdownElement.removeClass('open');
                            $document.unbind('click', clickHandler);
                            onCloseHandler();
                        } else {
                            dropdownElement.addClass('open');
                            $document.bind('click', clickHandler);
                            scope.focusSearchBox();
                            onOpenHandler();
                        }
                    };

                    function clickHandler(event) {
                        if (isElementInArray(event.target, element.find(event.target.tagName))) {
                            return;
                        }
                        dropdownElement.removeClass('open');
                        $document.unbind('click', clickHandler);
                        onCloseHandler();
                        scope.$apply();
                    }

                    scope.focusSearchBox = function focusSearchBox() {
                        var searchBox = dropdownElement.find('input')[0];
                        if (searchBox) {
                            searchBox.focus();
                        }
                    };
                }
            };
        }
    ]);
}());