// Karma configuration file, see link for more information
// https://karma-runner.github.io/0.13/config/configuration-file.html

module.exports = function (config) {
    config.set({
        basePath: '.',
        frameworks: ['es6-shim','jasmine'],
        files: [
            '../../wwwroot/dist/vendor.js',
            './boot-tests.ts'
        ],
        preprocessors: {
            './boot-tests.ts': ['webpack']
        },
        reporters: ['progress','teamcity'],
        port: 9876,
        colors: true,
        logLevel: config.LOG_INFO,
        //autoWatch: true,
        browsers: ['jsdom'],
        mime: { 'application/javascript': ['ts','tsx'] },
        singleRun: true,
        webpack: require('../../webpack.config.js')().filter(config => config.target !== 'node'), // Test against client bundle, because tests run in a browser
        webpackMiddleware: { stats: 'errors-only' }
    });
};
