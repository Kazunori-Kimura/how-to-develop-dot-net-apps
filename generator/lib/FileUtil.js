// file utility class

// FileUtil.js
var fs = require("fs");
var path = require("path");
var crypto = require("crypto");
var Q = require("q");
var mkdirp = require("mkdirp");

/**
 * 指定フォルダ配下のファイルを再帰的に列挙する
 *
 * usage:
 *  FileUtil.readAllFiles(path.resolve(__dirname, ".."),
 *    function(err, files){
 *      console.log(files.sort());
 *    });
 *
 * callback parameters:
 *    - err {Error} - error object
 *    - files {array} - absolute path
 *
 * @param {string} - directory path
 * @param {function} - callback function
 */
function readAllFiles(dir, callback){

  if(typeof callback != "function"){
    callback = function(err, files){  };
  }

  var dirpath = path.resolve(dir);

  var files = [];
  readFolder(dirpath, files)
    .then(function(items){
      callback(null, items);
    })
    .fail(function(err){
      callback(err, []);
    })
    .done();
}

function readFolder(dir, files){
  var deferred = Q.defer();

  fs.lstat(dir, function(err, stat){
    if(err){
      deferred.reject(err);
      return;
    }

    if(stat.isFile()){
      var filePath = path.resolve(dir);
      files.push(filePath);
      deferred.resolve(files);

    }else if(stat.isDirectory()){
      readDirectory(dir, files)
        .then(deferred.resolve.bind(deferred))
        .done();
    }else{
      deferred.reject(new Error());
    }
  });
  return deferred.promise;
}

function readDirectory(dir, files){
  var deferred = Q.defer();

  fs.readdir(dir, function(err, items){
    var promises = [];

    if(err){
      deferred.reject(err);
      return;
    }

    items.forEach(function(item){
      var dirPath = path.resolve(dir, item);
      promises.push(readFolder(dirPath, files));
    });

    Q.all(promises)
      .then(function(items){
        deferred.resolve(files);
      })
      .fail(deferred.reject.bind(deferred))
      .done();
  });

  return deferred.promise;
}

/**
 * srcFilePathのフォルダ構成を維持しつつ、dstPath配下にコピーするための
 * ファイルパスを組み立てる
 *
 * @param {string} - base path
 * @param {string} - target file
 * @param {string} - destination path
 * @return {string} - absolute file path
 */
function getDestinationPath(basePath, srcFilePath, dstPath){
  var relativePath = srcFilePath.replace(basePath + path.sep, "");
  var destinationPath = path.resolve(dstPath, relativePath);
  return destinationPath;
}

/**
 * フォルダを作成する
 *
 * @param {string} - target path
 */
function mkdirSync(folder){
  var targetPath = folder;
  if(!isDir(targetPath)){
    targetPath = path.dirname(folder);
  }
  if(!fs.existsSync(targetPath)){
    mkdirp.sync(targetPath);
  }
}

/**
 * ファイルをコピーする
 * (コピー先のフォルダが無ければ作成する)
 *
 * @param {string} - source file path
 * @param {string} - destination path
 */
function copyFileSync(src, dst){
  var srcBaseName = path.basename(src);
  var dstBaseName = path.basename(dst);

  var dstPath = dst;
  if(srcBaseName !== dstBaseName){
    if(isDir(dstPath)){
      dstPath = path.join(dst, srcBaseName);
    }
  }

  // フォルダ作成
  mkdirSync(dstPath);

  // ファイルコピー
  fs.linkSync(src, dstPath);
}

/**
 * 指定されたパスがフォルダかどうかを判定する
 * @param {string} - target path
 */
function isDir(dirpath){
  if(fs.existsSync(dirpath)){
    var stat = fs.lstatSync(dirpath);
    return stat.isDirectory();
  }else{
    // 末尾がpath.sepと一致すればフォルダ
    if(typeof dirpath === "string" && dirpath.length > 0){
      return dirpath.slice(-1) === path.sep;
    }
  }
  return false;
}

function sha1(filepath){
  if(!fs.existsSync(filepath)){
    return "";
  }

  var hash = crypto.createHash("sha1");
  var buf = fs.readFile(filepath);
  return hash.digest("hex");
}

function isModified(file1, file2){
  var d1 = sha1(file1);
  var d2 = sha1(file2);
  return d1 !== d2;
}

// export function
module.exports = {
  readAllFiles: readAllFiles,
  getDestinationPath: getDestinationPath,
  copyFileSync: copyFileSync,
  mkdirSync: mkdirSync,
  isModified: isModified
};
