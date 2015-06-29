var Site = (function () {
    return {
        resolveUrl: function (relativeUrl) {
            return window.rootUrl + relativeUrl;
        },
        showMessageBox: function (message) {
            alert(message);
            return false;
        }
    };
} ());

// FALLBACK //////////////////////////////////////
(function () {
    if (!String.prototype.trim) {
        String.prototype.trim = function() {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }
}());