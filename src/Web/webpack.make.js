var path = require('path');
var webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = function make(env) {
    return {
        devtool: isRelease(env) ? 'cheap-module-source-map' : 'source-map',
        entry: getEntry(env),
        output: {
            path: path.join(__dirname, 'dist'),
            filename: isRelease(env) ? 'js/[name].[hash].js' : 'js/[name].js',
            sourceMapFilename: '[file].map'
        },
        resolve: {
            extensions: ['', '.ts', '.js', '.scss', '.css', '.html']
        },
        module: {
            loaders: [
                { test: /\.ts$/, loader: 'ts' },
                { test: /\.(scss|css)$/, loader: 'style!css!sass' },
                { test: /\.html$/, loader: 'html' },
                { test: /\.(woff2?|svg)$/, loader: 'url?limit=10000&name=assets/[name].[ext]' },
                { test: /\.(ttf|eot)$/, loader: 'file?name=assets/[name].[ext]' },
                { test: /bootstrap-sass\/assets\/javascripts\//, loader: 'imports?jQuery=jquery' }
            ]
        },
        plugins: getPlugins(env)
    };
}

function getEntry(env) {
    if (isTest(env))
        return undefined;
    
    return {
        app: './app/main.ts',
        vendor: './app/vendor.ts'
    }
}

function getPlugins(env) {
    var plugins = [
        new HtmlWebpackPlugin({
            template: './index.html',
            filename: 'index.html',
            inject: 'body'
        }),
        new webpack.ProvidePlugin({
            'jQuery': 'jquery'
        }),
        new CopyWebpackPlugin([
            { 
                from: isRelease(env) ? 'config.release.json' : 'config.local.json', 
                to: 'config.json' 
            }
        ])
    ];

    if (isTest(env))
        return plugins;

    plugins.push(new webpack.optimize.CommonsChunkPlugin({
            name: 'vendor',
            filename: 'js/vendor.[hash].js',
            minChunks: Infinity
        })
    );

    if (isRelease(env)) {
        plugins.push(new webpack.optimize.DedupePlugin());
        plugins.push(new webpack.optimize.OccurenceOrderPlugin());
        plugins.push(new webpack.optimize.UglifyJsPlugin());
    }
    return plugins;
}

function isTest(env) {
    return env === 'test';
}

function isRelease(env) {
    return env === 'release';
}