const { defaults } = require('jest-config');
module.exports = {
    setupFilesAfterEnv: ['<rootDir>/www/sources/js/setupTests/setupEnzyme.js'],
    moduleFileExtensions: [...defaults.moduleFileExtensions, 'js', 'jsx'],
    moduleNameMapper: {
        '^@jsroot(.*)$': '<rootDir>/www/sources/js$1'
    },
    collectCoverageFrom: [
        '**/www/sources/js/**/*.{js,jsx}',
        '!**/node_modules/**'
    ]
};