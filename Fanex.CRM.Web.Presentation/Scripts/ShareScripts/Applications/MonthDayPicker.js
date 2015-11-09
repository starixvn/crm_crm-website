"use strict";
/* jshint unused: false */
angular.module('monthDayPickerApplication', [])
     .service('$monthDayPickerService', ['$rootScope', function ($rootScope) {
         var pickerOptions = {
             plugInId: null,
             today: null,
             maxDate: null,
             minDate: null,
             showDateRange: true,
             fromDayId: null,
             toDayId: null,
             fromDayLabel: null,
             toDayLabel: null,
             afterChangeMonth: null,
             afterChangeDay: null
         };

         this.render = function (pickerOptions) {
             this.pickerOptions = pickerOptions;
             $rootScope.$broadcast('monthdaypicker-render');
         };
     } ])
     .directive('monthDayPickerContainer', [
        '$monthDayPickerService',
         function ($monthDayPickerService) {
             var cachedData = {
                 isChangeMonth: false
             };

             function updateFromToDate(scope, element, attrs) {
                 if (typeof ($monthDayPickerService.pickerOptions.afterChangeMonth) === "function") {
                     var picker = element.find('#monthDayPicker');
                     var fromDate = picker.monPickr("getFromDate");
                     var toDate = picker.monPickr("getToDate");
                     $monthDayPickerService.pickerOptions.afterChangeMonth(fromDate, toDate);
                 }
             }

             function afterChangeMonth(scope, element, attrs) {
                 updateFromToDate(scope, element, attrs);
             }

             function afterChangeDay(scope, element, attrs) {
                 updateFromToDate(scope, element, attrs);
             }

             function renderPicker(scope, element, attrs) {
                 var picker = element.find('#monthDayPicker');
                 var pickerOptions = angular.copy($monthDayPickerService.pickerOptions);
                 $.extend(pickerOptions, {
                     beforeChangeMonth: function () {
                         cachedData.isChangeMonth = true;
                     },
                     afterChangeMonth: function () {
                         afterChangeMonth(scope, element, attrs);
                         cachedData.isChangeMonth = false;
                     },
                     afterChangeDay: function () {
                         if (!cachedData.isChangeMonth) {
                             afterChangeDay(scope, element, attrs);
                         }
                     }
                 });

                 picker.monPickr(pickerOptions);
             }

             return {
                 restrict: "C",
                 scope: true,
                 link: function (scope, element, attrs) {
                     scope.$on('monthdaypicker-render', function () {
                         renderPicker(scope, element, attrs);
                     });
                 }
             };
         } ]);