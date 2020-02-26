const merge = require('webpack-merge');
const webpack = require('webpack');
const common = require('./webpack.common.js');
const SentryWebpackPlugin = require('@sentry/webpack-plugin');

module.exports = merge(common, {
    mode: 'production',
    devtool: 'source-map',
    plugins: [
        new webpack.NormalModuleReplacementPlugin(/(.*)-APP_TARGET(\.*)/, function (resource) {
            resource.request = resource.request.replace(/-APP_TARGET/, `-production`);
        }),
        new SentryWebpackPlugin({
            release: process.env.RELEASE,
            include: '.',
            ignoreFile: '.sentrycliignore',
            ignore: ['node_modules', 'webpack.config.js']
        })
    ]
});