<!DOCTYPE html>
<html lang="en">
	<head>
		<link rel="apple-touch-icon" sizes="57x57" href="favicon/apple-icon-57x57.png">
		<link rel="apple-touch-icon" sizes="60x60" href="favicon/apple-icon-60x60.png">
		<link rel="apple-touch-icon" sizes="72x72" href="favicon/apple-icon-72x72.png">
		<link rel="apple-touch-icon" sizes="76x76" href="favicon/apple-icon-76x76.png">
		<link rel="apple-touch-icon" sizes="114x114" href="favicon/apple-icon-114x114.png">
		<link rel="apple-touch-icon" sizes="120x120" href="favicon/apple-icon-120x120.png">
		<link rel="apple-touch-icon" sizes="144x144" href="favicon/apple-icon-144x144.png">
		<link rel="apple-touch-icon" sizes="152x152" href="favicon/apple-icon-152x152.png">
		<link rel="apple-touch-icon" sizes="180x180" href="favicon/apple-icon-180x180.png">
		<link rel="icon" type="image/png" sizes="192x192"  href="favicon/android-icon-192x192.png">
		<link rel="icon" type="image/png" sizes="32x32" href="favicon/favicon-32x32.png">
		<link rel="icon" type="image/png" sizes="96x96" href="favicon/favicon-96x96.png">
		<link rel="icon" type="image/png" sizes="16x16" href="favicon/favicon-16x16.png">
		<link rel="manifest" href="favicon/manifest.json">
		<meta name="msapplication-TileColor" content="#ffffff">
		<meta name="msapplication-TileImage" content="favicon/ms-icon-144x144.png">
		<meta name="theme-color" content="#ffffff">
		<link rel="stylesheet" type="text/css" href="assets/style/style.css">
		<script src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.2/jquery.min.js"></script>
		<!-- <script src="assets/scripts/script.js"></script> -->
		<meta name="viewport" content="width=device-width, initial-scale=1">
		<meta charset="UTF-8">
		<title>Heroes Of Rain | Rewards</title>
	</head>
	<body>
		<img id="logo" src="assets/images/hor.png" alt="Heroes of Rain Logo">
		<h1 id="right"></h1>
		<form method="POST" action="QueueReward.php">
		</form>
	    <script type='text/javascript'>
	    	var rewards = ["SendRainwater","SellProducts","SellRainwater","CleanFloods","Discount", "StartStopInitiative"];

	    	/*
	    	function shuffle(array) {
	    		var currentIndex = array.length, temporaryValue, randomIndex;

	    		// While there remain elements to shuffle...
	    		while (0 !== currentIndex) {

	    			//Pick a remaining element...
	    			randomIndex = Math.floor(Math.random() * currentIndex);
	    			currentIndex -= 1;

	    			// And swap it with the current element.
	    			temporaryValue = array[currentIndex];
	    			array[currentIndex] = array[randomIndex];
	    			array[randomIndex] = temporaryValue;
	    		}

	    		return array;
	    	} 
	    	*/

	    	//shuffle(rewards);
	    	//console.log(rewards);

	    	$('#right').html('<h1> That was the right answer ' + sessionStorage.name + '.<br/> You can now choose a reward! </h1>');

	    	$("form").html("<label for=\"rewards\"> Choose a reward! </label> <label for=\"reward1\"><input type=\"radio\" name=\"reward_name\" id=\"reward1\" value=" + rewards[0] + ">" + rewards[0] + "<br /> <em>Pick up a percentage of the caught rainwater and bring this to the neighbourhood initiative. When the neighbourhood initiative is not active, the rainwater will be brought to the vegetable garden.</em></label><label for=\"reward2\"><input type=\"radio\" name=\"reward_name\" id=\"reward2\" value=" + rewards[1] + ">" + rewards[1] + "<br/> <em>Sell a percentage of the produced beer. If there is no beer, vegetables will be sold.</em></label><label for=\"reward3\"><input type=\"radio\" name=\"reward_name\" id=\"reward3\" value=" + rewards[2] + ">" + rewards[2] + "<br/> <em>Sell a percentage of the caught rainwater to a third party that uses it for products.</em></label><label for=\"reward4\"><input type=\"radio\" name=\"reward_name\" id=\"reward4\" value=" + rewards[3] + ">" + rewards[3] + "<br /> <em>Waternet cleans a part of the floods.</em></label><label for=\"reward5\"><input type=\"radio\" name=\"reward_name\" id=\"reward5\" value=" + rewards[4] + ">" + rewards[4] + "<br /> <em>Get 50% discount on a random measure.</em></label><label for=\"reward6\"><input type=\"radio\" name=\"reward_name\" id=\"reward6\" value=" + rewards[5] + ">" + rewards[5] + "<br /><em>Start or stop a neighbourhood initiatief. When it has stopped, beer will be brewed from the collected amount of rainwater. </em></label><input type=\"hidden\" name=\"player_name\" id=\"nameWMeasure\"> <input class=\"button\" onclick=\"attachName()\" type=\"submit\">");

	    	function attachName(){
				document.getElementById('nameWMeasure').value = sessionStorage.name;
			}
	    </script>
	</body>
</html>