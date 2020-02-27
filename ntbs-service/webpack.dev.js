const merge = require('webpack-merge');
const webpack = require('webpack');
const common = require('./webpack.common.js');

module.exports = merge(common, {
    devtool: 'inline-source-map',
    mode: 'development',
    watchOptions: {
        aggregateTimeout: 300
    },
    plugins: [
        new webpack.NormalModuleReplacementPlugin(/(.*)-APP_TARGET(\.*)/, function (resource) {
            resource.request = resource.request.replace(/-APP_TARGET/, `-development`);
        })
    ]
});