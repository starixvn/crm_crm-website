var pathConfig = require('../config/path');
var pluginGenerator = require('../config/plugin');
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");
var OptimizeCssAssetsPlugin = require('optimize-css-assets-webpack-plugin');

function generateConfig(environment) {
    var jsPath = `${pathConfig.scriptPaths.build}/${environment}`;
    var cssPath = `${pathConfig.cssPaths.build}/${environment}`;
    var commonPath = `${pathConfig.commonPath}`;
    var commonConfigs = {
        resolve: {
            extensions: ['.js', '.jsx']
        },
        entry: {
            [`${jsPath}/main`]: pathConfig.scriptPaths.main,
            //[`${jsPath}/pages/home/home`]: pathConfig.scriptPaths.home,

            [`${cssPath}/main`]: pathConfig.cssPaths.main,
            //[`${cssPath}/pages/home`]: pathConfig.cssPaths.home,
            //[`${cssPath}/pages/error`]: pathConfig.cssPaths.error
        },
        output: {
            path: pathConfig.projectRoot,
            filename: '[name].chunk.js',
            chunkFilename: commonPath + '/[name].chunk.js',
        },
        module: {
            rules: [
                {
                    test: /\.(js|jsx)$/,
                    loader: ["babel-loader"],
                    exclude: /node_modules/
                },
                {
                    test: /\.(png|jpg|jpeg|gif|svg)$/,
                    use: [{
                        loader: 'url-loader',
                        options: {
                            limit: 10, // Convert images < 8kb to base64 strings
                            name: '[hash]-[name].[ext]',
                            outputPath: '/wwwroot/images/',
                            publicPath: '/images/'
                        }
                    }]
                },
                {
                    test: /\.(woff|woff2|eot|ttf)$/,
                    use: [{
                        loader: 'url-loader',
                        options: {
                            limit: 10, // Convert images < 8kb to base64 strings
                            name: '[hash]-[name].[ext]',
                            outputPath: '/wwwroot/fonts/',
                            publicPath: '/fonts/'
                        }
                    }]
                }
            ]
        },
        plugins: pluginGenerator(environment),
        stats: {
            assets: false,
            colors: true,
            version: false,
            hash: false,
            timings: true,
            chunks: false,
            chunkModules: false,
            children: true
        },
        optimization: {
            minimizer: [
                new UglifyJsPlugin({
                    sourceMap: true,
                    uglifyOptions: {
                        output: {
                            comments: false
                        }
                    }
                }),
                new OptimizeCssAssetsPlugin()
            ],
            splitChunks: {
                chunks: 'all',
                minSize: 30000,
                maxSize: 0,
                minChunks: 2,
                maxAsyncRequests: 5,
                maxInitialRequests: 3,
                name: false,
                cacheGroups: {
                    // disable default cache group
                    default: false,
                    vendors: false,
                    reactVendor: {
                        name: 'react-vendor',
                        test: /[\\/]node_modules[\\/](react|react-dom)[\\/]/,
                        minChunks: 2,
                        priority: -10
                    }
                }
            }
        }
    };

    return commonConfigs;
}

module.exports = generateConfig;