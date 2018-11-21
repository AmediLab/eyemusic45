var allNum = [];

var allTexEng = [];
var allTexHeb = [];
var allTexFrn = [];
var allTexGrm = [];

window.staticfir = true;
window.hebrowload = false;
window.englishload = false;
window.readfiles = true;


function ReadAllTranslate(lang)
{
        var rawRead = new XMLHttpRequest();
        var array;
        //console.log(tofound);
    
        rawRead.open("GET", "/../Texts/" + lang +"/UI.txt", false);
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
    else
    {
        return allTexEng[indexstr];
    }
}


function retDesc(tofound)
{
    if (window.staticfir) {
        window.staticfir = false;
        var rawFile = new XMLHttpRequest();
        //console.log(tofound);

        rawFile.open("GET", "/../Texts/ENG/Dic.txt", false);
        //console.log("succsess");
        index = 0;
        rawFile.onreadystatechange = function () {

            if (rawFile.readyState === 4) {
                allTexEng = rawFile.responseText.split('\n');
                //console.log(allTexEng);
                window.englishload = true;                
            }
        }
        rawFile.send();
    
        var rawFile2 = new XMLHttpRequest();
        rawFile2.open("GET", "/../Texts/HEB/Dic.txt", false);
        rawFile2.onreadystatechange = function () {
            if (rawFile2.readyState === 4) {
                allTexHeb = rawFile2.responseText.split('\n');
                window.hebrowload = true;
            }
        }
        rawFile2.send();
    }

    //console.log(allTexEng.length);
    for(var i=0;i<allTexEng.length;i++)
    {

        if (allTexEng[i].trim() == tofound.trim()) {
            //console.log(allTexHeb[i]);
            return allTexHeb[i];
        }
    }
    
}
/*
var allEng = [
    "eye music application",
    "Welcome to EyeMusic (BETA version).",
    'With the EyeMusic you can see any visual image using sound, or "sonify" it. Throughout this website you can start and stop the EyeMusic\'s special sounds with the CTRL key and change the speed with the key cobinations of shift + F1 and shift + F2 buttons',
    "This movie will explain the basics of Sensory Substitution ",
    "as preperation for the training lessons.",
    "We recommend you listen to it before starting them",
    "LINKS",
    "Learn how to see with music",
    "More information about the EyeMusic including cool movies, the science behind the  EyeMusic and other assistive devices we have developed",
    "Upload your image to EyeMusic and sound it",
    "Contact us, join mailing list, and feedback",
    "App Store EyeMusic application",
    "select lesson, eye music",
    "select lesson",
    "select level, eye music",
    "select level",
    "return to home",
    'This section contains ',
    ' images, you can move between the images with tab and shift + tab buttons, press enter to "sound" the selected image.',
    " press ctrl to start and stop the audio.",
    "play and pause",
    "Remeber the three rules of the EyeMusic:",
    "1. The image is scanned from left to right, and each scan ends with  a cue, So you can tell where things are and how wide they are by when you hear them.",
    "2. The height of the object within the image is transformed into musical notes - The higher the object the higher the note it will sound.",
    "3. Different colors will be transformed into different musical instruments.",
    "start exam",
    "Your browser does not support the audio element.",
    "Select the correct description for the image that you sound. Press ctrl to start and stop the audio.",
    "The previous image is",
    "In this exam you received grade of",
    "show all grades for all levels",
    "Please tell us what you think by filling out the online form. We would appreciate it if you gave us your email so we can answer and supply additional information",
    "please fill all fields",
    "Do you have any visual impairiments?",
    "If yes, please specify:",
    "Do you want to join our mailing list?",
    "Do you have any questions?",
    "Do you have any suggestions?",
    "Free text",
    "uplaod file from your computer to eye music",
    "In this page you can upload images (png, bmp, jpeg, jpg) from your computer.",
    'Drag and drop the image or use in "Upload file" button to upload, ',
    "you can sound it with eye music and download the audio file to your computer.",
    "Press ctrl to start and stop the audio.",
    "create zip of wav files",
    "play and pause",
    "Uploaded Files:",
    "download zip",
    "yes",
    "no",
    "submit",
    "return to lesson",
    "Android EyeMusic application",
    "Games with eye music",
    "In this page you can upload svg files from your computer.",
];

var allHeb = [
    "העין המוזיקלית",
    "(ברוכים הבאים לעין המוזיקלית (גרסת ניסוי",
    "SHIFT F1 ו SHIFT F2  בעזרת העין המוזיקלית אפשר לראות תמונות בעזרת צלילים, תוכל תמיד לעצור ולהתחיל את השמעת הצלילים של העין המוזיקלית בעזרת מקש קונטרול ולשנות את המהירות בעזרת",
    "הסרטים הללו מראים את העקרונות של התמרה חושית ",
    "אנו ממליצים לראות אותם כהכנה לאימון ",
    "",
    "קישורים",
    "למד כיצד לראות בעזרת צלילים",
    "מידע נוסף על העין המוזיקלית כולל סרטים, העיקרונות המדעיים שמאחורי העין המוזיקלית ואביזרים אחרים שאנחנו מפתחים",
    "העלה תמונה לעין המוזיקלית והשמע אותה",
    "יצירת קשר, הצטרפות לרשימת דיוור והשארת משוב",
    "APP STORE קישור להורדת האפליקצייה מה ",
    "בחירת שיעור, העין המוזיקלית",
    "בחירת שיעור",
    "בחירת רמה, העין המוזיקלית",
    "בחירת רמה",
    "חזור לדף הבית",
    ' הדף הזה מכיל  ',
    '  תמונות אתה יכול לנוע בין התמונות בעזרת TAB וSHIFT TAB ולאחר מכן לחיצת ENTER ',
    " להתחלה ולהפסקה של העין המוזיקלית לחץ CTRL",
    "נגן והשתק",
    "זכור את שלושת הכללים של העין המוזיקלית:",
    "1. התמונה נסרקת משמאל לימין וכל סריקה מסתיימת עם ביפ קצר,לכן אפשר לדעת היכן נמצאים עצמים בתמונה ומה הרוחב שלהם. ",
    "2 . הגובה של אובייקט בתמונה קובע את גובה הצליל , אובייקט גבוה יושמע בצליל גבוה.  ",
    "3. צבעים שונים יהפכו לצלילים של כלי נגינה שונים.",
    "התחל מבחן",
    "הדפדפן שלך לא תומך בהשמעת אלמנט מסוג זה",
    "בחר את התיאור הנכון לצליל שאתה שומע כעת לחץ ctrl להתחלה והפסקה של ההשמעה",
    "התמונה הקודמת הייתה:",
    "במבחן הזה קיבלת ציון של:",
    "הצג את הציונים שלי עבור כל השיעורים",
    " ספר לנו מה אתה חושב  בעזרת מילוי הטופס הזה. אנחנו נשמח אם תשאיר גם את כתובת המייל שלך כדי שנוכל לענות לך ולתת מידע נוסף.",
    "מלא בבקשה את כל השדות הבאים:",
    "האם יש לך לקות ראייה כלשהיא?",
    "אם כן בבקשה תאר אותה:",
    "האם אתה מעוניין להצרך לרשימת הדיוור שלנו",
    "האם יש לך שאלה כלשהיא?",
    "האם יש לך הצעה כלשהיא?",
    "טקסט חופשי:",
    "העלה תמונה לעין המוזיקלית והשמע אותה,",
    "בדף הזה תוכל להעלות תמונות בפורמטים הבאים  (png, bmp, jpeg, jpg) מהמכשיר שבו את גולש כעת.",
    ' גרור ושחרר תמונות מהמחשב או השתמש בכפתור בשביל לבחור תמונה מהמכשיר שלך , ',
    "אתה יכול להשמיע את התמונה או להוריד קובץ זיפ המכיל את קבצי האודיו למחשב שלך,",
    " לחץ CTRL להתחלה והפסקה של העין המוזיקלית.",
    "צור קובץ זיפ עם האודיו של התמונות שהעלית",
    "נגן והשתק",
    "הקבצים שהעלית:",
    "הורדת קובץ הזיפ",
    "כן",
    "לא",
    "שלח",
    "חזור לשיעור",
    "קישור להורדת אפליקציה לאנדרואיד",
    "משחקים עם העין המוזיקלית",
    "בדף הזה תוכל להעלות קבצי SVG מהמכשיר שבו את גולש כעת.",
];
*/
