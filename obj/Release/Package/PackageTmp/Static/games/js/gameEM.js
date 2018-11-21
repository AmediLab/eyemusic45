
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
        crossDomain: true,
        data: JSON.stringify({ pdfString: afterimg }),
        contentType: 'application/json',
        success: function (data) {

            tempwav = "~/../../../" + data.voice;
            console.log(tempwav);
            console.log("1");
        }
    });
}

var play = false;
var tempwav = "";

myAudio = document.getElementById("thePlayaudio");
//myBeep = document.getElementById("myBeep");

handlerBeep = function () {

    try {
        this.currentTime = 0;
        this.pause();
        $("#thePlayaudio").attr("src", tempwav);
        //document.getElementById("thePlayaudio").playbackRate = window.speedjs;
    }
    catch (err) {
        console.log(err);
    }

    this.play();
}


/*handler = function () {
    try {
        this.currentTime = 0;
        this.pause();
        myAudio.play();
    }
    catch (err) {
        console.log(err);
    }

}
*/

startEM = function () {
    myAudio.addEventListener('ended', handlerBeep, false);
    //myBeep.addEventListener('ended', handler, false);
    myAudio.play();
    play = true;
}

myAudio.addEventListener('ended', handlerBeep, false);
//myBeep.addEventListener('ended', handler, false);
window.startGame.addEventListener('ended', startEM, false);

myAudio.play();

function init1() {
    myAudio = document.getElementById("thePlayaudio");

    //myBeep = document.getElementById("myBeep");
    //document.getElementById("thePlayaudio").playbackRate = window.speedjs;
    //init1beep = document.getElementById("myBeep");
    //init1beep.volume = window.voilumejs;
    document.getElementById("thePlayaudio").pause();

    window.startGame.play();
    window.gameStart = true;
}

function init2() {
    //document.getElementById("thePlayaudio").playbackRate = window.speedjs;
    //init2beep = document.getElementById("myBeep");
    //init2beep.volume = window.voilumejs;

    myAudio.addEventListener('ended', handlerBeep, false);
    //myBeep.addEventListener('ended', handler, false);
    play = true;
}

window.onload = init1;

function UIplayButton_click() {

    if (play == false) {
        myAudio.addEventListener('ended', handlerBeep, false);
        //myBeep.addEventListener('ended', handler, false);
        myAudio.play();
        play = true;
    }
    else {
        myAudio.removeEventListener('ended', handlerBeep, false);
        //myBeep.removeEventListener('ended', handler, false);
        play = false;
        myAudio.pause();
        myAudio.currentTime = 0;
    }

}