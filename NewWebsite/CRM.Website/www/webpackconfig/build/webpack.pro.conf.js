var merge = require('webpack-merge');
var CleanWebpackPlugin = require('clean-webpack-plugin');
var utils = require('./utils');
var pathConfig = require('../config/path');
var baseWebpackConfigGenerator = require('./webpack.base.conf');

var environment = "pro";
var baseConfigs = baseWebpackConfigGenerator(environment);
var cssRootPath = `${pathConfig.cssPaths.build}/${environment}`;
var jsRootPath = `${pathConfig.scriptPaths.build}/${environment}`;

module.exports = merge(baseConfigs,
    utils.setDefinePlugin('process.env.NODE_ENV', 'production'),
    utils.extractCSS(true),
    {
        mode: 'production',
        watch: false,
        plugins: [
            new CleanWebpackPlugin([`${jsRootPath}`], {
                root: process.cwd()
            }),

            new CleanWebpackPlugin([`${cssRootPath}/*.css`, `${cssRootPath}/*.map`, `${cssRootPath}/*.js`], {
                root: process.cwd()
            }),

            new CleanWebpackPlugin([`${cssRootPath}/pages/*/*.css`, `${cssRootPath}/pages/*/*.js`, `${cssRootPath}/pages/*/*.map`], {
                root: process.cwd()
            })
        ]
    }
);