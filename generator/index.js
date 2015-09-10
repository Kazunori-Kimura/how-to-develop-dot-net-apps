// index.js
"use strict";
var fs = require("fs");
var path = require("path");
var marked = require("marked");
var ECT = require("ect");
var extend = require("extend");
var Q = require("q");
var colors = require("colors");
var FileUtil = require("./lib/FileUtil");

function initialize(){
  // markedの設定
  marked.setOptions({
    highlight: function(code){
      return require("highlight.js").highlightAuto(code).value;
    }
  });

  // define color theme
  colors.setTheme({
      debug: "grey",
      info: "green",
      warn: "yellow",
      error: "red"
  });

  logger(">>> end initialize <<<");
}

function loadConfig(){
  logger(">>> start loadConfig <<<");

  var defaultConfig = {
    template: path.join(__dirname, "template"),
    src: path.join(__dirname, "markdown"),
    dst: path.join(__dirname, "html"),
    title: "hoge",
    copyright: "2015 Kazunori Kimura",
    project: "my project"
  };

  var config = {};

  if(fs.existsSync(path.resolve(__dirname, "config.json"))){
    config = require("./config.json");

    // path変換
    ["template", "src", "dst"].forEach(function(item){
      if (config[item]) {
        config[item] = path.resolve(__dirname, config[item]);
      }
    });

    logger(JSON.stringify(config));
  }

  return extend(defaultConfig, config);
}

function getRenderer(config){
  logger(">>> start getRenderer <<<");
  var rendererOptions = {
    root: config.template,
    ext: ".ect"
  };

  return ECT(rendererOptions);
}

/**
 * markdown -> html変換
 * @param {string} - markdown file path
 * @return {string} - parsed html
 */
function parse(srcPath){
  var data = fs.readFileSync(srcPath, {encoding: "utf8"});
  return marked(data);
}

/**
 * templateにhtmlを埋め込む
 * @param {string} - content html
 * @return {string} - html
 */
function render(html, config, renderer){
  var data = {
    content: html,
    base_url: config.base_url,
    title: config.title,
    project: config.project,
    copyright: config.copyright
  };

  return renderer.render("template.ect", data);
}

/**
 * 変換処理本体
 * @param {object} - configuration object
 * @param {object} - ECT renderer object
 */
function convert(config, renderer){
  logger(">>> start convert <<<");
  logger("    source: " + config.src);

  FileUtil.readAllFiles(config.src, function(err, files){
    if(err){
      console.log(err.toString().error);
      return;
    }

    files.forEach(function(file){
      // コピー先
      var dest = FileUtil.getDestinationPath(
          config.src,
          file,
          config.dst
        );

      if(/\.md$/i.test(file)){
        // markdownをhtmlに変換
        var content = parse(file);
        var html = render(content, config, renderer);

        // ファイルパス
        dest = dest.replace(/\.md$/i, ".html");

        // フォルダ作成
        FileUtil.mkdirSync(dest);

        // ファイル作成
        fs.writeFileSync(dest, html, {encoding: "utf8"});

        console.log("(create)> %s".info, dest);
      }else{
        // コピー先ファイルが存在しないか、
        // 元ファイルと異なる場合はコピーする
        if(!fs.existsSync(dest) ||
          FileUtil.isModified(file, dest)){
          // ファイルをコピー
          FileUtil.copyFileSync(file, dest);
          console.log("(copy  )> %s".info, dest);
        }else{
          console.log("(skip  )> %s".debug, dest);
        }
      }
    });
  });
}

function main(){
  console.log(">>> start main <<<");

  // 初期化処理
  initialize();

  // configファイルの読み込み
  var config = loadConfig();

  // ectの設定
  var renderer = getRenderer(config);

  // 変換処理
  convert(config, renderer);
}

function logger(msg){
  console.log(msg.debug);
}

main();
