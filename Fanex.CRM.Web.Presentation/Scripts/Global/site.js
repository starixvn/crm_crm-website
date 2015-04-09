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