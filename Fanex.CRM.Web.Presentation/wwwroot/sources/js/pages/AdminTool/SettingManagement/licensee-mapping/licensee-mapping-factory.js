(function (angular) {
    'use strict';

    angular.module('addmintool-settingmanagement-application').factory('licenseeMappingFactory', licenseeMappingFactory);
    licenseeMappingFactory.$inject = ['$http'];

    function licenseeMappingFactory($http) {
        function getLicensee() {
            return $http.post(window.rootUrl + 'LicenseeMapping/GetLicensee');
        }

        function getLicenseeMapping(param) {
            return $http.post(window.rootUrl + 'LicenseeMapping/GetLicenseeMapping', param);
        }

        function updateLicenseeMapping(param) {
            return $http.post(window.rootUrl + 'LicenseeMapping/UpdateLicenseeMapping', param);
        }

        return {
            getLicensee: getLicensee,
            getLicenseeMapping: getLicenseeMapping,
            updateLicenseeMapping: updateLicenseeMapping
        };
    }
})(angular);