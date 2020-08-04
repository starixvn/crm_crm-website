const path = require('path');

const projectRoot = path.resolve(__dirname, '../../../');

const cssPaths = {
    build: 'wwwroot/css',

    main: ['./www/sources/css/main.scss'],
    error: ['./www/sources/css/pages/error.scss'],
    notification_list: ['./www/sources/css/pages/notification-list/index.scss'],
    notification_template: ['./www/sources/css/pages/notification-template/index.scss']
};

const scriptPaths = {
    build: 'wwwroot/js',

    language: ['./www/sources/js/common/languages/ViewResources.js'],
    main: ['./www/sources/js/main.js'],
    notification_list: ['./www/sources/js/pages/notification-list/index.js'],
    notification_template: ['./www/sources/js/pages/notification-template/index.js']
};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};