var allNum = [];

var allDicEng = [];
var allDicHeb = [];
var allDicCst = [];

var allTexEng = [];
var allTexHeb = [];
var allTexFrn = [];
var allTexGrm = [];
var allTexCst = [];

window.staticfir = true;
//window.hebrowload = false;
//window.englishload = false;
window.readfiles = true;

function ReadAllTranslate(lang) {
    var rawRead = new XMLHttpRequest();
    var array;
    //console.log(tofound);

    rawRead.open("GET", "/../Texts/" + lang + "/UI.txt", false);
    //console.log("succsess");
    index = 0;
    rawRead.onreadystatechange = function () {

        if (rawRead.readyState === 4) {
            array = rawRead.responseText.split('\n');
            //window.englishload = true;
        }
    }
    rawRead.send();
    return array;
}

function retstr(indexstr) {
    if (window.readfiles) {

        allTexHeb = ReadAllTranslate("HEB");
        allTexEng = ReadAllTranslate("ENG");
        allTexFrn = ReadAllTranslate("FRN");
        allTexGrm = ReadAllTranslate("GRM");
        allTexCst = ReadAllTranslate("CST");

        window.readfiles = false;
    }


    if (window.lan == "h") {
        return allTexHeb[indexstr];
    }
    else if (window.lan == "f") {
        return allTexFrn[indexstr];
    }
    else if (window.lan == "g") {
        return allTexGrm[indexstr];
    }
    else if (window.lan == "c") {
        return allTexCst[indexstr];
    }
    else {
        return allTexEng[indexstr];
    }
}

function readDic(lang) {
    var rawFile = new XMLHttpRequest();
    //console.log(tofound);
    var array;

    // for server version
    //rawFile.open("GET", "/../launch/Texts/" + lang + "/Dic.txt", false);

    //for local version
    rawFile.open("GET", "/../Texts/" + lang + "/Dic.txt", false);
    rawFile.onreadystatechange = function () {

        if (rawFile.readyState === 4) {
            //window.englishload = true;
            array = rawFile.responseText.split('\n');
            //console.log(allTexEng);

        }
    }
    rawFile.send();
    return array;
}

function translateDic() {
    allDicEng = readDic("ENG");
    allDicHeb = readDic("HEB");
    allDicCst = readDic("CST");
}

function retDesc(tofound) {
    if (window.staticfir) {
        window.staticfir = false;
        translateDic();
    }

    //console.log(allTexEng.length);
    for (var i = 0; i < allDicEng.length; i++) {

        if (allDicEng[i].trim() == tofound.trim()) {
            //console.log(allTexHeb[i]);
            if (window.lan == "h")
                return allDicHeb[i];
            else if (window.lan == "c")
                return allDicCst[i];
        }
    }

}