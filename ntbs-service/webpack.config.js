const path = require('path');
module.exports = {
  entry: { 'main': './wwwroot/source/app.js'},
  output: {
    path: path.resolve(__dirname, 'wwwroot/dist'),
    publicPath: 'dist/',
    filename: 'bundle.js'
  },
  mode: 'development',
  module: {
    rules: [{
      test: /\.css$/,
      use: [
        {
          loader: "style-loader"
        },
        {
          loader: "css-loader"
        }
      ]
    },{
      test: /\.scss$/,
      use: [
        {
          loader: "style-loader"
        },
        {
          loader: "css-loader"
        },
        {
          loader: "sass-loader"
        }
      ]
    }]
  }
};