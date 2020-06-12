import { movableOuterIframeModalDirective } from '../../shared/movable-outer-iframe-modal-directive';

(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function picRegisterPicker() {
        var picRegister = new RegisterTemplate(angular.module('pic-management-application'));
        picRegister.registerRolePicker();
        picRegister.registerDownloadFile();
    }

    picRegisterPicker();
    movableOuterIframeModalDirective(angular.module('pic-management-application'));
})();