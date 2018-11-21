/* 
* The code that runs behind the experiment
*/
var toggleClickUserMenu = false;
var u;
var brokenPlugin = -1;
var missingPlugin = 0;
var workingPlugin = 1;
var all;
//Updates the page according to user status
function updateLogged(plugin) {
    if (logged == 0) {
        
        $("#levelsFrame").hide();
        $("#logginForm").show();
        $("#missing").hide();
        $("#broken").hide();
    }
    else{
        $("#levelsFrame").show();
        $("#logginForm").hide();
        $("#missing").hide();
        $("#broken").hide();
    }
}

function updatePluginStatus(plugin) {
    if (plugin == workingPlugin) {
        $("#unityPlayer").fancybox("allowfullscreen   : 'false',closeBtn:true'").click();
        $("#levelsFrame").show();
        $("#logginForm").hide();
        $("#missing").hide();
        $("#broken").hide();
    }
    else if (plugin == missingPlugin) { //missing
        $("#levelsFrame").hide();
        $("#logginForm").hide();
        $("#missing").show();
        $("#broken").hide();
    }
    else { //broken
        $("#levelsFrame").hide();
        $("#logginForm").hide();
        $("#missing").hide();
        $("#broken").show();
    }
}

$(document).ready(function () {

    all = window.speedjs + "&" + window.voilumejs + "&" + window.beep_noise;
    initPlayer();

    $("#level1Btn").click(startLevel1);
    $("#level2Btn").click(startLevel2);
    $("#level3Btn").click(startLevel3);
    $("#level4Btn").click(startLevel4);
    $("#level5Btn").click(startLevel5);
    $("#level6Btn").click(startLevel6);

    $("#level1BtnHide").click(startLevel1Hide);
    $("#level2BtnHide").click(startLevel2Hide);
    $("#level3BtnHide").click(startLevel3Hide);
    $("#level4BtnHide").click(startLevel4Hide);
    $("#level5BtnHide").click(startLevel5Hide);
    $("#level6BtnHide").click(startLevel6Hide);

    $("#camera").click(camera1);
    $("#shadow").click(shadow);
    $("#realroom").click(realroom);
	$("#loginBtn").click(startLoggin);
    $("#errorMsg1").hide();
    $("#errorMsg2").hide();
    updateLogged();
});

function initPlayer() {
    var config = {
        width: 1000,
        height: 600,
        params: { enableDebugging: "0" }

    };
    u = new UnityObject2(config);

    jQuery(function () {

        var $missingScreen = jQuery("#unityPlayer").find(".missing");
        var $brokenScreen = jQuery("#unityPlayer").find(".broken");
        u.observeProgress(function (progress) {
            switch (progress.pluginStatus) {
                case "broken": //Case broken plugin
                    $brokenScreen.find("a").click(function (e) {
                        e.stopPropagation();
                        e.preventDefault();
                        u.installPlugin();
                        return false;
                    });
                    updatePluginStatus(brokenPlugin);
                    break;
                case "missing": //Case missing plugin
                    $missingScreen.find("a").click(function (e) {
                        e.stopPropagation();
                        e.preventDefault();
                        u.installPlugin();
                        return false;
                    });
                    updatePluginStatus(missingPlugin);
                    break;
                case "installed":  //Case everything ok
                    updatePluginStatus(workingPlugin);
                    break;
                case "first":
                    break;
            }
        });
    });
   
}

function endLevel() {
    $.fancybox(' תודה רבה, הניסוי נקלט במערכת');
}
function showError(error) {
    $.fancybox.close();
    alert(error);
}

/*
Start the chosen level
*/
function startLevel1() {
    document.getElementById("unityPlayer").style.height = "auto";
    document.getElementById("unityPlayer").style.width = "auto";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel1.unity3d?" + all);
}

function startLevel2() {
    document.getElementById("unityPlayer").style.height = "auto";
    document.getElementById("unityPlayer").style.width = "auto";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel2.unity3d?" + all);
}
function startLevel3() {
    document.getElementById("unityPlayer").style.height = "auto";
    document.getElementById("unityPlayer").style.width = "auto";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel3.unity3d?" + all);
}
function startLevel4() {
    document.getElementById("unityPlayer").style.height = "auto";
    document.getElementById("unityPlayer").style.width = "auto";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel4.unity3d?" + all);
}
function startLevel5() {
    document.getElementById("unityPlayer").style.height = "auto";
    document.getElementById("unityPlayer").style.width = "auto";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel5.unity3d?" + all);
}
function startLevel6() {
    document.getElementById("unityPlayer").style.height = "auto";
    document.getElementById("unityPlayer").style.width = "auto";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel6.unity3d?" + all);
}

function startLevel1Hide() {
    document.getElementById("unityPlayer").style.height = "100px";
    document.getElementById("unityPlayer").style.width = "100px";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel1.unity3d?" + all);
}

function startLevel2Hide() {
    document.getElementById("unityPlayer").style.height = "10px";
    document.getElementById("unityPlayer").style.width = "10px";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel2.unity3d?" + all);
}
function startLevel3Hide() {
    document.getElementById("unityPlayer").style.height = "10px";
    document.getElementById("unityPlayer").style.width = "10px";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel3.unity3d?" + all);
}
function startLevel4Hide() {
    document.getElementById("unityPlayer").style.height = "10px";
    document.getElementById("unityPlayer").style.width = "10px";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel4.unity3d?" + all);
}
function startLevel5Hide() {
    document.getElementById("unityPlayer").style.height = "10px";
    document.getElementById("unityPlayer").style.width = "10px";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel5.unity3d?" + all);
}
function startLevel6Hide() {
    document.getElementById("unityPlayer").style.height = "10px";
    document.getElementById("unityPlayer").style.width = "10px";
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildLevel6.unity3d?" + all);
}

function camera1() {
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/trycameraWeb.unity3d?" + all);
}

function callbackunity(arg) {
    u.SendMessage("Rezzer", "inboundfunction", "item1~item2~item3");
}

function shadow()
{
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/buildShadow.unity3d?" + all);
}

function realroom() {
    u.initPlugin(jQuery("#unityPlayer")[0], "~/../../unity/realroom.unity3d?" + all);
}

// If user is not logged this manages the loggin
function startLoggin() {

    nickname = $("#nameInput").val();
    var agree = $("#agree").is(':checked');
    $("#first").val($("#firstBox").is(':checked'));
    if (nickname == "") {
        $("#errorMsg2").hide();
        $("#errorMsg1").show();
    }
    else if (agree == false) {
        $("#errorMsg1").hide();
        $("#errorMsg2").show();
    }
    else {
        $("#logginForm").submit();
    }
}


