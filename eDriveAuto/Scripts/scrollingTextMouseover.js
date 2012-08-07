/* This script and many more are available free online at
The JavaScript Source!! http://javascript.internet.com
Created by: voidvector | http://www.webdeveloper.com/forum/showthread.php?t=201460 */

var UpdateInterval = 25;
var PixelPerInterval = 3;
var scorllerInterval;
var scorllerInterval1;

function start_scroll_right() {
    scorllerInterval = setInterval(scroll_left, UpdateInterval);
}

function scroll_right() {
	document.getElementById('scroller').scrollLeft -= PixelPerInterval;
}

function start_scroll_left() {
    scorllerInterval = setInterval(scroll_right, UpdateInterval);
}

function scroll_left() {
	document.getElementById('scroller').scrollLeft += PixelPerInterval;
}

function start_scroll_bottom() {
	scorllerInterval = setInterval(scroll_top, UpdateInterval);
}

function scroll_bottom() {
	document.getElementById('scroller').scrollTop -= PixelPerInterval;
}

function start_scroll_top() {
	scorllerInterval = setInterval(scroll_bottom, UpdateInterval);
}

function scroll_top() {
	document.getElementById('scroller').scrollTop += PixelPerInterval;
}

function stop_scrolling() {
	clearInterval(scorllerInterval);
}

/**** Updated by Henisha on 6th may, 2011 ****/

function start_scroll_right1() {
    scorllerInterval1 = setInterval(scroll_left1, UpdateInterval);
}

function scroll_right1() {
	document.getElementById('scroller1').scrollLeft -= PixelPerInterval;
}

function start_scroll_left1() {
    scorllerInterval1 = setInterval(scroll_right1, UpdateInterval);
}

function scroll_left1() {
	document.getElementById('scroller1').scrollLeft += PixelPerInterval;
}

function start_scroll_bottom1() {
	scorllerInterval1 = setInterval(scroll_top1, UpdateInterval);
}

function scroll_bottom1() {
	document.getElementById('scroller1').scrollTop -= PixelPerInterval;
}

function start_scroll_top1() {
	scorllerInterval1 = setInterval(scroll_bottom1, UpdateInterval);
}

function scroll_top1() {
	document.getElementById('scroller1').scrollTop += PixelPerInterval;
}

function stop_scrolling1() {
	clearInterval(scorllerInterval1);
}





function start_scroll_right3() {
    scorllerInterval1 = setInterval(scroll_left3, UpdateInterval);
}

function scroll_right3() {
	document.getElementById('scroller3').scrollLeft -= PixelPerInterval;
}

function start_scroll_left3() {
    scorllerInterval1 = setInterval(scroll_right3, UpdateInterval);
}

function scroll_left3() {
	document.getElementById('scroller3').scrollLeft += PixelPerInterval;
}

function start_scroll_bottom3() {
	scorllerInterval1 = setInterval(scroll_top3, UpdateInterval);
}

function scroll_bottom3() {
	document.getElementById('scroller3').scrollTop -= PixelPerInterval;
}

function start_scroll_top3() {
	scorllerInterval1 = setInterval(scroll_bottom3, UpdateInterval);
}

function scroll_top3() {
	document.getElementById('scroller3').scrollTop += PixelPerInterval;
}

function stop_scrolling1() {
	clearInterval(scorllerInterval1);
}









//Added by jinal
function start_scroll_right2() {
    scorllerInterval = setInterval(scroll_right2, UpdateInterval);
}

function scroll_right2() {
    document.getElementById('scroller2').scrollLeft += PixelPerInterval;
}

function start_scroll_up2() {
    scorllerInterval = setInterval(scroll_up2, UpdateInterval);
}

function scroll_up2() {
    document.getElementById('scroller2').scrollTop -= PixelPerInterval;
}

function start_scroll_left2() {
	scorllerInterval = setInterval(scroll_left2, UpdateInterval);
}

function scroll_left2() {
    var test=document.getElementById('scroller2');
    test.scrollLeft -= PixelPerInterval;	
}

function start_scroll_down2() { 
     scorllerInterval = setInterval(scroll_down2, UpdateInterval);
}

function scroll_down2() {
    document.getElementById('scroller2').scrollTop += PixelPerInterval;
}

function stop_scrolling() {
	clearInterval(scorllerInterval);
}


//Added by henisha



function start_scroll_right4() {
    scorllerInterval = setInterval(scroll_right4, UpdateInterval);
}

function scroll_right4() {
    document.getElementById('scroller4').scrollLeft += PixelPerInterval;
}

function start_scroll_up4() {
    scorllerInterval = setInterval(scroll_up4, UpdateInterval);
}

function scroll_up4() {
    document.getElementById('scroller4').scrollTop -= PixelPerInterval;
}

function start_scroll_left4() {
    scorllerInterval = setInterval(scroll_left4, UpdateInterval);
}

function scroll_left4() {
    var test = document.getElementById('scroller4');
    test.scrollLeft -= PixelPerInterval;
}

function start_scroll_down4() {
    scorllerInterval = setInterval(scroll_down4, UpdateInterval);
}

function scroll_down4() {
    document.getElementById('scroller4').scrollTop += PixelPerInterval;
}

function stop_scrolling() {
    clearInterval(scorllerInterval);
}



//Added by henisha



function start_scroll_right5() {
    scorllerInterval = setInterval(scroll_right5, UpdateInterval);
}

function scroll_right5() {
    document.getElementById('scroller5').scrollLeft += PixelPerInterval;
}

function start_scroll_up5() {
    scorllerInterval = setInterval(scroll_up5, UpdateInterval);
}

function scroll_up5() {
    document.getElementById('scroller5').scrollTop -= PixelPerInterval;
}

function start_scroll_left5() {
    scorllerInterval = setInterval(scroll_left5, UpdateInterval);
}

function scroll_left5() {
    var test = document.getElementById('scroller5');
    test.scrollLeft -= PixelPerInterval;
}

function start_scroll_down5() {
    scorllerInterval = setInterval(scroll_down5, UpdateInterval);
}

function scroll_down5() {
    document.getElementById('scroller5').scrollTop += PixelPerInterval;
}

function stop_scrolling5() {
    clearInterval(scorllerInterval);
}