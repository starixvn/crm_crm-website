const jsSourcePath = './sources/js/pages';
const jsDistPath = 'js';
const styleSourcePath = './sources/styles';
const styleDistPath = 'styles';

module.exports = {
    client: {
        [`${styleDistPath}/main`]: `${styleSourcePath}/main.scss`,
        [`${styleDistPath}/inform.headline`]: `${styleSourcePath}/pages/inform-headline.scss`,

        //
        //[`${jsDistPath}/main`]: './sources/js/main.js',
        [`${jsDistPath}/inform.headline.logs`]: `${jsSourcePath}/inform-headline/logs/index.js`
    }
};