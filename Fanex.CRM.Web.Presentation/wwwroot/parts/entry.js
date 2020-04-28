const jsSourcePath = './sources/js/pages';
const jsDistPath = 'js';
const styleSourcePath = './sources/styles';
const styleDistPath = 'styles';

module.exports = {
    client: {
        [`${styleDistPath}/main`]: `${styleSourcePath}/main.scss`,
        [`${styleDistPath}/inform.headline`]: `${styleSourcePath}/pages/inform-headline.scss`,
        [`${styleDistPath}/manage.feedback`]: `${styleSourcePath}/pages/manage-feedback.scss`,
        [`${styleDistPath}/commtool.setting`]: `${styleSourcePath}/pages/commtool-setting.scss`,
        [`${styleDistPath}/create.multiple.logs`]: `${styleSourcePath}/pages/create-multiple-logs.scss`,
        [`${styleDistPath}/management.setting`]: `${styleSourcePath}/pages/management-setting.scss`,

        //
        //[`${jsDistPath}/main`]: './sources/js/main.js',
        [`${jsDistPath}/inform.headline`]: `${jsSourcePath}/inform-headline/index.js`,
        [`${jsDistPath}/manage.feedback`]: `${jsSourcePath}/feedbacks/manage-feedback/index.js`,
        [`${jsDistPath}/commtool.setting`]: `${jsSourcePath}/commtool-setting/index.js`,
        [`${jsDistPath}/create.multiple.logs`]: `${jsSourcePath}/feedbacks/create-multiple-logs/index.js`,
        [`${jsDistPath}/management.setting`]: `${jsSourcePath}/AdminTool/SettingManagement/index.js`
    }
};