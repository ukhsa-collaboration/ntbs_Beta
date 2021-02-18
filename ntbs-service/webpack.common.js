const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CopyPlugin = require("copy-webpack-plugin");

module.exports = {
  entry: { 'main': './wwwroot/source/app.ts'},
  output: {
    path: path.resolve(__dirname, 'wwwroot/dist'),
    publicPath: 'dist/',
    filename: 'bundle.js'
  },
  plugins: [
    new CopyPlugin({
      patterns: [
        // Copy the GOV.UK assets (fonts and crests mainly) to public path /assets/
        // These are referenced by the GOV.UK SCSS and publishing these assets is a
        // requirement for using this style sheet.
        { from: 'node_modules/govuk-frontend/govuk/assets/', to: 'assets/' },
      ],
    }),
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
                      "@babel/preset-env",
                      {
                          "targets": {"ie": 11}
                      }
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
        use: {
            loader: 'url-loader',
            options: {
              limit: 10_000, // bytes
              publicPath: './'
            }
        }
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