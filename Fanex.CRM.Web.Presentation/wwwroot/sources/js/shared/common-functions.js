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

function escapeHtml(text) {
    const entityMap = {
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': '&quot;',
        "'": '&#39;',
        "/": '&#x2F;'
    };

    return text.replace(/[&<>"'\/]/g, function (s) {
        return entityMap[s];
    });
}

export {
    decodeHtml,
    encodeHtml,
    escapeHtml
};