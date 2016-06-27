$(document).ready( function(){
	pageLoad();
});


//On click of submit in player name form. Name is stored in the local storage.
function store(){
	var inputPlayerName = document.getElementById('player_name');
	sessionStorage.setItem("name", inputPlayerName.value);
}

function showLog(){
	console.log(sessionStorage.name);
}

function attachName(){
	document.getElementById('nameWMeasure').value = sessionStorage.name;
}

function pageLoad() {
    if (window.location.hash === "#new-measure") {
        $('h1').html('Want to do more?');
    } 

    if (window.location.hash === "#new-quest") {
    	$('h1').html('You can go for another reward if you want.');
    }
}

