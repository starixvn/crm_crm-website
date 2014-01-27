customerDetailApplication.directive('dropdownSite', function () {
    return {
        restrict: 'A',
        template: '<option value="' + crmConstants.Enums.Site.NotApply + '"' +
                     ' data-ng-click="updateSelectedSite();"' +
                     ' ng-selected="customerDetail.SiteSetting.Id == crmConstants.Enums.Site.NotApply">' + viewResources.Select + '</option>\
                   <optgroup data-ng-repeat="siteGroup in siteList track by $index" label="{{ siteGroup.Name }}" value="{{ siteGroup.Id }}">\
                        <option data-ng-repeat="site in siteGroup.ChildSites track by $index"' +
                            ' data-ng-click="updateSelectedSite(site);"' +
                            ' value="{{ site.Id }}"' +
                            ' ng-selected="customerDetail.SiteSetting.Id == site.Id">{{ site.Name }}</option>\
                   </optgroup>',
        replace: false,
        priority: 0,
        transclude: false,
        scope: true,
        terminal: false,
        require: false,
        compile: function compile(tElement, tAttrs, transclude) {
        },
        link: function (scope, elem, attrs) {
        }
    }
}).directive('dropdownCountry', function () {
    return {
        restrict: 'A',
        template: '<option value="' + crmConstants.Enums.Country.NotApply +
                        ' data-ng-click="updateSelectedCountry(country);"' +
                        ' ng-selected="customerDetail.Country.Id == crmConstants.Enums.Country.NotApply">' +
                        viewResources.Symbol_NotApply +
                  '</option>\
                   <option data-ng-repeat="country in countryList track by $index"' +
                        ' data-ng-click="updateSelectedCountry(country);"' +
                        ' value="{{ country.Id }}"' +
                        ' ng-selected="customerDetail.Country.Id == country.Id"' +
                        '>{{ country.Name }}</option>',
        replace: false,
        priority: 0,
        transclude: false,
        scope: true,
        terminal: false,
        require: false,
        compile: function compile(tElement, tAttrs, transclude) {
        },
        link: function (scope, elem, attrs) {
        }
    }
}).directive('dropdownCountryBank', function () {
    return {
        restrict: 'A',
        template: '<option value="' + crmConstants.Enums.Country.NotApply + '"' +
                        ' ng-selected="customerDetail.BankInfo.AddressInfo.Country.Id == crmConstants.Enums.Country.NotApply">' +
                            viewResources.Symbol_NotApply +
                  '</option>\
                   <option data-ng-repeat="country in countryList track by $index"' +
                        ' data-ng-click="updateSelectedCountryBank(country);"' +
                        ' value="{{ country.Id }}"' +
                        ' ng-selected="customerDetail.BankInfo.AddressInfo.Country.Id == crmConstants.Enums.Country.NotApply">{{ country.Name }}</option>',
        replace: false,
        priority: 0,
        transclude: false,
        scope: true,
        terminal: false,
        require: false,
        compile: function compile(tElement, tAttrs, transclude) {
        },
        link: function (scope, elem, attrs) {
        }
    }
});

//.directive('dropdownLanguage', function () {
//    return {
//        restrict: 'A',
//        template: '',
//        scope: true,
//        link: function (scope, element, attrs) {
//            var abc = 'abc';
//            console.log('link');
//            //            self.DOMElements.dropdownLanguage.empty();
//            //            var dropdownLanguage = [];
//            //            var prefixId = 'customerinfo-languageoptions-';
//            //            var isCheckAll = true;

//            //            if (result && result.length > 0) {
//            //                var optionLength = result.length;
//            //                for (var i = 0; i < optionLength; i++) {
//            //                    var info = result[i];
//            //                    var isChecked = self.IsCustomerKnowLanguage(info.Id);
//            //                    if (!isChecked) {
//            //                        isCheckAll = false;
//            //                    }

//            //                    dropdownLanguage.push({
//            //                        Id: prefixId + info.Id,
//            //                        Value: info.Id,
//            //                        Text: info.Value,
//            //                        Checked: isChecked,
//            //                        Enable: true,
//            //                        CssClass: "",
//            //                        Items: []
//            //                    });
//            //                }
//            //            }

//            //            self.DOMElements.dropdownLanguage.multiSelect(
//            //        {
//            //            minWidth: 177,
//            //            maxHeight: 450,
//            //            shadow: true,
//            //            showHeader: true,
//            //            isCheckAll: isCheckAll,
//            //            showFooter: false,
//            //            showTooltip: false,
//            //            checkAllText: self.CacheData.ViewResources.All_Languages,
//            //            isUsedCustomCheckAllText: true,
//            //            customCheckAllText: self.CacheData.ViewResources.All_Languages,
//            //            noItemText: self.CacheData.ViewResources.No_languages_found,
//            //            noneSelectedText: self.CacheData.ViewResources.Symbol_NotApply,
//            //            selectedList: 3,
//            //            listData: dropdownLanguage
//            //        });

//            //            self.DOMElements.dropdownLanguage = $("#customerinfo-languageoptions");
//            //            self.CacheData.isLoadedLanguage = true;
//        }
//    }
//});