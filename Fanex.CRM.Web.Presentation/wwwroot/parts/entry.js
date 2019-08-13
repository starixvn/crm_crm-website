const jsSourcePath = './sources/js/pages';
const jsDistPath = 'js';
const styleSourcePath = './sources/styles';
const styleDistPath = 'styles';

module.exports = {
    client: {
        [`${styleDistPath}/main`]: `${styleSourcePath}/main.scss`,
        [`${styleDistPath}/inform.headline`]: `${styleSourcePath}/pages/inform-headline.scss`,
        [`${styleDistPath}/manage.feedback`]: `${styleSourcePath}/pages/manage-feedback.scss`,

        //
        //[`${jsDistPath}/main`]: './sources/js/main.js',
        [`${jsDistPath}/inform.headline`]: `${jsSourcePath}/inform-headline/index.js`,
        [`${jsDistPath}/manage.feedback`]: `${jsSourcePath}/feedbacks/manage-feedback/index.js`
    }
};