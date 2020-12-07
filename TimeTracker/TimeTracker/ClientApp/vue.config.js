// vue.config.js
const webpack = require("webpack");
const path = require("path");

module.exports = {
  // options...
  // publicPath: process.env.NODE_ENV === "production"? "http://1.2.3.4:8080/assets" : "/",
  // publicPath: process.env.NODE_ENV === "production"? "/assets" : "/",
  publicPath:
    process.env.NODE_ENV === "production"
      ? "/"
      : "/",
  configureWebpack: {
    plugins: [
      new webpack.ProvidePlugin({
        $: "jquery",
        jQuery: "jquery",
        "windows.jQuery": "jquery"
      })
    ],
  },
  css: {
    loaderOptions: {
      sass: {
        prependData: `@import "~@/scss/global_var.scss";`
      }
    },
    sourceMap: true
  }
};
