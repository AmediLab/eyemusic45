
$(document).bind('keydown', 'ctrl', function () {
    UIplayButton_click();
});

$(document).bind('keydown', 'shift+f1', function () {
    if (document.getElementById("thePlayaudio").playbackRate < 3.50)
        document.getElementById("thePlayaudio").playbackRate += 0.1;
});

$(document).bind('keydown', 'shift+f2', function () {
    if (document.getElementById("thePlayaudio").playbackRate > 0.59)
        document.getElementById("thePlayaudio").playbackRate -= 0.1;
});

function sendImage() {
    var canvas = document.getElementById("game");
    var img = canvas.toDataURL("image/png");
    var afterimg = img.replace(/^data:image\/(png|jpg);base64,/, "");
    //console.log(afterimg);
    $.ajax({
        url: window.urljs,
        type: 'post',
        data: JSON.stringify({ pdfString: afterimg }),
        contentType: 'application/json',
        success: function (data) {

            tempwav = "~/../../" + data.voice;
            tempImg = "~/../../" + data.img;
        }
    });
}

var play = false;
var tempwav = "";
var tempImg = "";

myAudio = document.getElementById("thePlayaudio");

handlerBeep = function () {

    try {
        this.currentTime = 0;
        this.pause();
        $("#thePlayaudio").attr("src", tempwav);
        document.getElementById("TheImage").style.width = "100%";
        document.getElementById("TheImage").style.height = "100%";
        $("#TheImage").attr("src", tempImg);
    }
    catch (err) {
        console.log(err);
    }

    this.play();
}

/*
handler = function () {
    try {
        this.currentTime = 0;
        this.pause();
        myAudio.play();
    }
    catch (err) {
        console.log(err);
    }

}*/

startEM = function () {
    myAudio.addEventListener('ended', handlerBeep, false);
    myAudio.play();
    play = true;
}

myAudio.addEventListener('ended', handlerBeep, false);
window.startGame.addEventListener('ended', startEM, false);

myAudio.play();

function init1() {
    myAudio = document.getElementById("thePlayaudio");
    document.getElementById("thePlayaudio").pause();
    window.startGame.play();
    window.gameStart = true;
}

function init2() {
    document.getElementById("thePlayaudio").playbackRate = window.speedjs;
    myAudio.addEventListener('ended', handlerBeep, false);
    play = true;
}

window.onload = init1;

function UIplayButton_click() {

    if (play == false) {
        myAudio.addEventListener('ended', handlerBeep, false);
        myAudio.play();
        play = true;
    }
    else {
        myAudio.removeEventListener('ended', handlerBeep, false);
        play = false;
        myAudio.pause();
        myAudio.currentTime = 0;
    }

}