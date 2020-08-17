const { defaults } = require('jest-config');
module.exports = {
    moduleFileExtensions: [...defaults.moduleFileExtensions, 'js', 'jsx'],
    collectCoverageFrom: [
        '**/www/sources/js/**/*.{js,jsx}',
        '!**/node_modules/**'
    ]
};