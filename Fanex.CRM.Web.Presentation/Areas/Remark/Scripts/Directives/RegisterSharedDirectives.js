"use strict";
/* global remarkApplication,
RegisterTemplate */
function remarkRegisterPicker() {
    var remarkRegister = new RegisterTemplate(remarkApplication);
    remarkRegister.registerRawHtmlDirective();
}

remarkRegisterPicker();