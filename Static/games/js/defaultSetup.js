
/* general */

var SOUND_FORMAT = "mp3";
var voiceOver = true;
var joystick_control = true;
var x;
var y;
var screen_compatibility = 0;
var mobilecheck = false;
var appversion = 0;

var audioTime = 2000;
var dont_check_duration = false;

var use_EM = true;

var gameStart = false;

var startMessages = ['click anywhere to start the game', 'click anywhere to start the tutorial'];
var startMessage = startMessages[0];

/* SCREEN SIZE */

var windowWidth;
var windowHeight;
updateScreenSize();


//var play = false;
var tempwav = "~/../../../Sounds/Out.mp3";
var audioForEM;
var realystart = true;
var currentTime = 0;

function updateScreenSize() {

    // android
    if (navigator.appVersion.indexOf("Linux") != -1) {
        windowWidth = $(window).width();
        windowHeight = $(window).height();
    } else {
        // other
        windowWidth = window.innerWidth;
        windowHeight = window.innerHeight;
    }

}


function resize_canvas(canvas) {

    updateScreenSize();
    tableCellSize(horCells, verCells);

    canvas.width = windowWidth;
    canvas.height = windowHeight;

    if (joystick_control) {
        newJoystick();
    }

    if (screen_compatibility > 0) {
        screen_compatibility_check();
    }


}


/* canvas settings */

var canvas = document.getElementById("game");
canvas.style.background = 'black';
canvas.width = windowWidth;
canvas.height = windowHeight;
var context = canvas.getContext('2d');

//window.addEventListener('resize', resize , false);
//window.addEventListener('orientationchange', resize, false);				

function clear() {
    context.beginPath();
    context.fillStyle = 'black';
    context.fillRect(-windowWidth * 5, -windowHeight * 5, 10 * windowWidth, 10 * windowHeight);
}

function changeTextColor(c) {
    var stylesheet = document.styleSheets[0],
        selector = ".clicked", rule = "{color: " + c + "}";

    if (stylesheet.insertRule) {
        stylesheet.insertRule(selector + rule, stylesheet.cssRules.length);
    } else if (stylesheet.addRule) {
        stylesheet.addRule(selector, rule, -1);
    }
}


function GET() {
    var parts = window.location.search.substr(1).split("&");
    var $_GET = {};
    for (var i = 0; i < parts.length; i++) {
        var temp = parts[i].split("=");
        $_GET[decodeURIComponent(temp[0])] = decodeURIComponent(temp[1]);
    }

    return $_GET;
}


function check_get_param(str) {

    var gett = GET();

    if (str in gett) {
        return parseInt(gett[str]);
    }
    else {
        return 0;
    }
}


function play_audio(fileName) {
    var audio = new Audio(fileName);
    audio.play();
}

function play_audio_vol(fileName, volume) {
    var audio = new Audio(fileName);
    audio.volume = volume;
    audio.play();
}

function load_audio(fileName) {
    var audio = new Audio(fileName);
    audio.load();
}

function stopEm_url(url) {
    stopEM();
    setTimeout(function () { goTo_url_appVersion(url); }, 200);
}

function goTo_url_appVersion(url) {
    if (url.indexOf('?') === -1) {
        url = url + "?appVersion=" + appversion;
    }
    else {
        url = url + "&appVersion=" + appversion;
    }

    window.location = url;
}

function stopEM() {
    //console.log("ppp");

    if (mobilecheck && appversion) {
        //window.location = "EyeMusic://imagePlaying?action=stop";
    }
    else {
        document.title = "EM OFF";
    }
}

var audioCtx = new (window.AudioContext || 
  window.webkitAudioContext || 
  window.mozAudioContext || 
  window.oAudioContext || 
  window.msAudioContext)();

var soundSource, concertHallBuffer;

function start_EM() {
    //console.log("sss");
    //if (mobilecheck && appversion) {
    if (realystart) {
        //console.log("start_em");
        sendImage();
        //audioForEM = new Audio(tempwav);
        sendimage = setInterval(sendImage, 2500);
        //UIplayButton_click();
        realystart = false;
    }
    //audio = new Audio('audio_file.mp3');
    //audio.play();
    //window.location = "EyeMusic://imagePlaying?action=play";
    /*}
    else
    {
        document.title = "EM ON";
    }*/
}

function startEM() {
    start_EM();
}

var sound_playing;
var sound_ended = false;

function play_func(fileName, func) {
    sound_ended = false;
    var audio = new Audio(fileName);
    sound_playing = audio;
    sound_playing.play();
    sound_playing.addEventListener('ended', function () { func(); sound_ended = true; });
}

function enable_move_EM() {
    startEM();
    move.value = true;
}

function put_message(text) {
    context.font = "20px arial";
    context.fillStyle = "black";
    context.rect(0, 30, 100, 25);
    context.fill();
    context.fillStyle = "white";
    context.fillText(text, 10, 50);
}


function gamesDidDisappear() {

    stopEM();
    sound_playing.pause();
}

function gamesDidAppear() {
    //	put_message("on: ");

    if (!sound_ended) {
        sound_playing.play();
    } else {
        startEM();
    }
}
/*	-	-	-	-	-	*/


function assignment_after_time(varname, val, time) {
    setTimeout(function () { varname.value = val; }, time);
}

function sound_interval_time(time) {
    setTimeout(function () { start_EM(); }, time);
}


function assignment_after_duration(varname, val, audio) {
    if (dont_check_duration) {
        assignment_after_time(varname, val, audioTime);
        return;
    }
    var aud = new Audio(audio);

    aud.play();
    aud.pause();

    setTimeout(function () {
        var time = Math.floor(aud.duration) * 1000;
        assignment_after_time(varname, val, time);
    }, 200);

}

function sound_interval_EM(audio) {
    stopEM();
    if (dont_check_duration) {
        sound_interval_time(audioTime);
        return;
    }

    var aud = new Audio(audio);

    aud.play();
    aud.pause();

    setTimeout(function () {
        var time = Math.floor(aud.duration) * 1000;
        sound_interval_time(time);
    }, 200);
}


/*$(document).bind('keydown', 'ctrl', function () {
    UIplayButton_click();
});
*/

/*$(document).bind('keydown', 'shift+f1', function () {
    if (audioForEM.playbackRate < 3.50)
        audioForEM.playbackRate += 0.1;
});
*/

/*$(document).bind('keydown', 'shift+f2', function () {
    if (audioForEM.playbackRate > 0.59)
        audioForEM.playbackRate -= 0.1;
});
*/

function sendImage() {

    var canvas = document.getElementById("game");
    var img = canvas.toDataURL("image/png");
    var afterimg = img.replace(/^data:image\/(png|jpg);base64,/, "");

    ajaxRequest = new XMLHttpRequest();
    //ajaxRequest.open('GET', 'http://localhost:55795//Sounds/secondBeat.wav', true);

    ajaxRequest.open('POST', '~/../../../Home/getGameImg', true);
    ajaxRequest.responseType = 'arraybuffer';
    //console.log(afterimg);
    ajaxRequest.onloadend = function () {
        var audioData = ajaxRequest.response;

        try
        {
            audioCtx.decodeAudioData(audioData, function (buffer) {
                //console.log(buffer);
                concertHallBuffer = buffer;
                soundSource = audioCtx.createBufferSource();
                soundSource.buffer = concertHallBuffer;
                soundSource.connect(audioCtx.destination);
                soundSource.start();
            
                /*if (play == false)
                    UIplayButton_click();
                    */
                //soundSource.loop = true;

            }, function (e) { "Error with decoding audio data" + e.err });
        }
        catch (err)
        {
            console.log(err);
        }
    }
    
    ajaxRequest.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    ajaxRequest.send("pdfString=" + afterimg);


    /* var canvas = document.getElementById("game");
    var img = canvas.toDataURL("image/png");
    var afterimg = img.replace(/^data:image\/(png|jpg);base64,/, "");
    //console.log(afterimg);
    $.ajax({
        url: "~/../../../Home/getGameImg",
        type: 'post',
        crossDomain: true,
        data: JSON.stringify({ pdfString: afterimg }),
        contentType: 'application/json',
        success: function (data) {

            if (currentTime < data.time) {
                currentTime = data.time;
                tempwav = "~/../../../" + data.voice;
            }

            //console.log(tempwav);
        }
    });*/
}



//myAudio = document.getElementById("thePlayaudio");
//myBeep = document.getElementById("myBeep");

/*
handlerBeep = function () {

    try {
        this.currentTime = 0;
        this.pause();
        soundSource.start();
        //console.log(audioForEM.src);
        //audioForEM = new Audio(tempwav);
        //audioForEM.addEventListener('ended', handlerBeep, false);

    }
    catch (err) {
        console.log(err);
    }

    this.play();
}

handler = function () {
    try {
        this.currentTime = 0;
        this.pause();
        soundSource.start();

    }
    catch (err) {
        console.log(err);
    }

}
*/
/*startEMmenahem = function () {
    audioForEM = new Audio(tempwav);
    audioForEM.addEventListener('ended', handlerBeep, false);
    //myBeep.addEventListener('ended', handler, false);
    audioForEM.play();
    play = true;
}*/

//audioForEM = new Audio(tempwav);
//audioForEM.addEventListener('ended', handlerBeep, false);
//myBeep.addEventListener('ended', handler, false);
//window.startGame.addEventListener('ended', startEMmenahem, false);

//audioForEM.play();

//function init1() {
//myAudio = document.getElementById("thePlayaudio");
//myBeep = document.getElementById("myBeep");
//document.getElementById("thePlayaudio").playbackRate = window.speedjs;
//init1beep = document.getElementById("myBeep");
//init1beep.volume = window.voilumejs;
//document.getElementById("thePlayaudio").pause();
//window.startGame.play();
//window.gameStart = true;
//}

/*function init2() {
    //document.getElementById("thePlayaudio").playbackRate = window.speedjs;
    //init2beep = document.getElementById("myBeep");
    //init2beep.volume = window.voilumejs;

    audioForEM = new Audio(tempwav);
    audioForEM.addEventListener('ended', handlerBeep, false);
    //myBeep.addEventListener('ended', handler, false);
    play = true;
}*/

//window.onload = init1;
/*
function UIplayButton_click() {

    if (play == false) {
        //audioForEM.src = tempwav;
        //audioForEM = new Audio(tempwav);
        soundSource.addEventListener('ended', handlerBeep, false);
        //myBeep.addEventListener('ended', handler, false);
        //audioForEM.play();
        play = true;
    }
    else {
        //audioForEM.src = tempwav;
        //audioForEM = new Audio(tempwav);
        //console.log("stop");
        soundSource.removeEventListener('ended', handlerBeep, false);
        //myBeep.removeEventListener('ended', handler, false);
        play = false;
        audioForEM.pause();
        audioForEM.currentTime = 0;
    }
    
}*/

