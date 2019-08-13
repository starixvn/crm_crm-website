function decodeHtml(text) {
    return angular.element('<div/>').html(text).text();
}

function encodeHtml(value) {
    return String(value)
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

export {
    decodeHtml,
    encodeHtml
};