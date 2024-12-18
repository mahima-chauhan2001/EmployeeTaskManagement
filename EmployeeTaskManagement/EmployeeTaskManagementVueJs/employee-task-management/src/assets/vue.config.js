module.exports = {
    configureWebpack: {
      devtool: 'source-map',  // This enables source maps for better debugging
    },
    devServer: {
        hot: true,  // Enables Hot Module Replacement (HMR)
      },
  }