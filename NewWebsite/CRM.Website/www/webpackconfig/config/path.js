const path = require('path');

const projectRoot = path.resolve(__dirname, '../../../');

const cssPaths = {
    build: 'wwwroot/css',

    main: ['./www/sources/css/main.scss'],
    error: ['./www/sources/css/pages/error.scss'],
    notificationlist: ['./www/sources/css/pages/notification-list/index.scss']
};

const scriptPaths = {
    build: 'wwwroot/js',

    main: ['./www/sources/js/main.js'],
    notificationlist: ['./www/sources/js/pages/notification-list/components/index.jsx']
};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};