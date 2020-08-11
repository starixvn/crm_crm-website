const alias = require('../config/alias');
const pathConfig = require('../config/path');
const pluginGenerator = require('../config/plugin');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const OptimizeCssAssetsPlugin = require('optimize-css-assets-webpack-plugin');

function generateConfig(environment) {
	const jsPath = `${pathConfig.scriptPaths.build}/${environment}`;
	const cssPath = `${pathConfig.cssPaths.build}/${environment}`;
	const commonConfigs = {
		resolve: {
			alias: alias,
			extensions: ['.js', '.jsx']
		},
		entry: {
			/*js*/
			[`${jsPath}/lib/lang-en-US`]: pathConfig.scriptPaths.language,
			[`${jsPath}/main`]: pathConfig.scriptPaths.main,
			[`${jsPath}/pages/notificationlist/index`]: pathConfig.scriptPaths.notification_list,
			[`${jsPath}/pages/notificationtemplate/index`]: pathConfig.scriptPaths.notification_template,
			[`${jsPath}/pages/notificationtype/index`]: pathConfig.scriptPaths.notification_type,
			[`${jsPath}/pages/chattypemapping/index`]: pathConfig.scriptPaths.chattype_mapping,

			/*css*/
			[`${cssPath}/main`]: pathConfig.cssPaths.main,
			[`${cssPath}/error`]: pathConfig.cssPaths.error,
			[`${cssPath}/pages/notificationlist/index`]: pathConfig.cssPaths.notification_list,
			[`${cssPath}/pages/notificationtemplate/index`]: pathConfig.cssPaths.notification_template,
			[`${cssPath}/pages/notificationtype/index`]: pathConfig.cssPaths.notification_type,
			[`${cssPath}/pages/chattypemapping/index`]: pathConfig.cssPaths.chattype_mapping
		},
		output: {
			path: pathConfig.projectRoot,
			filename: "[name].chunk.js",
			chunkFilename: `${jsPath}/lib/[name].chunk.js`,
		},
		module: {
			rules: [
				{
					test: /\.(js|jsx)$/,
					loader: ["babel-loader"],
					exclude: /node_modules/
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