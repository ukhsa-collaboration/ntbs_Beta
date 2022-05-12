const merge = require('webpack-merge').merge;
const webpack = require('webpack');
const common = require('./webpack.common.js');

const plugins = [
    new webpack.NormalModuleReplacementPlugin(/(.*)-APP_TARGET(\.*)/, function (resource) {
        resource.request = resource.request.replace(/-APP_TARGET/, `-production`);
    })
];

module.exports = merge(common, {
    mode: 'production',
    devtool: 'source-map',
    plugins: plugins
});