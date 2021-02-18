const merge = require('webpack-merge').merge;
const webpack = require('webpack');
const common = require('./webpack.common.js');
const SentryWebpackPlugin = require('@sentry/webpack-plugin');

const plugins = [
    new webpack.NormalModuleReplacementPlugin(/(.*)-APP_TARGET(\.*)/, function (resource) {
        resource.request = resource.request.replace(/-APP_TARGET/, `-production`);
    })
];

if (process.env.RELEASE && process.env.SENTRY_AUTH_TOKEN) {
    plugins.push(new SentryWebpackPlugin({
        release: process.env.RELEASE,
        include: '.',
        ignoreFile: '.sentrycliignore',
        ignore: ['node_modules', 'webpack.config.js']
    }));
} else {
    console.warn(  "Cannot publish sentry release, make sure RELEASE and SENTRY_AUTH_TOKEN env variables are set")
}

module.exports = merge(common, {
    mode: 'production',
    devtool: 'source-map',
    plugins: plugins
});