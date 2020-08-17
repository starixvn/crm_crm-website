const path = require('path');

const projectRoot = path.resolve(__dirname, '../../../');

const cssPaths = {
    build: 'wwwroot/css',

    main: ['./www/sources/css/main.scss'],
    error: ['./www/sources/css/pages/error.scss'],
    notification_list: ['./www/sources/css/pages/notification-list/index.scss'],
    notification_template: ['./www/sources/css/pages/notification-template/index.scss'],
    notification_type: ['./www/sources/css/pages/notification-type/index.scss'],
    chattype_mapping: ['./www/sources/css/pages/chat-type-mapping/index.scss']
};

const scriptPaths = {
    build: 'wwwroot/js',

    language: ['./www/sources/js/common/languages/ViewResources.js'],
    main: ['./www/sources/js/main.js'],
    notification_list: ['./www/sources/js/pages/notification-list/index.js'],
    notification_template: ['./www/sources/js/pages/notification-template/index.js'],
    notification_type: ['./www/sources/js/pages/notification-type/index.js'],
    chattype_mapping: ['./www/sources/js/pages/chat-type-mapping/index.js']
};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};