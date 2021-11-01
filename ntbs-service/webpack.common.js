const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = {
  entry: { 'main': './wwwroot/source/app.ts'},
  output: {
    path: path.resolve(__dirname, 'wwwroot/dist'),
    publicPath: '/dist/',
    filename: 'bundle.js',
    hashFunction: "xxhash64"
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename:'[name].css',
      chunkFilename: '[id].css',
    }),
  ],
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: 'ts-loader',
        exclude: /node_modules/
      },
      {
          test: /\.js/,
          use: {
              loader: 'babel-loader',
              options: {
                  presets: [
                      [
                          "@babel/preset-env"
                      ]
                  ]
              }
          }
      },
      {
        test: /\.(sc|c)ss$/,
        use: [
          {
            loader: MiniCssExtractPlugin.loader
          },
          "css-loader",
          "sass-loader"
        ]
      },
      {
        test: /\.(png|woff|woff2|eot|ttf|svg)$/,
        type: "asset"
      }
    ]
  },
  resolve: {
    alias: {
      'vue$': 'vue/dist/vue.esm.js'
    },
    extensions: [".ts", ".js"]
  }
};
