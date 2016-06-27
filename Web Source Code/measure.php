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
		<script src="assets/scripts/script.js"></script>
		<meta name="viewport" content="width=device-width, initial-scale=1">
		<meta charset="UTF-8">
		<title>Heroes Of Rain | Install measure</title>
	</head>
	<body>
		<a class="back" href="choice.php"> I want to do something else</a>
		<img id="logo" src="assets/images/hor.png" alt="Heroes of Rain Logo">
		<h1></h1>
		<section id="sendMeasure">
			<form method="POST" action="QueueCharacter.php">
				<label>What do you want to install?</label>
				<label for="GreenGarden">
					<input type="radio" name="rainproof_measure" value="GreenGarden" id="GreenGarden">
					Green Garden <em><br/>Costs: 75 euros <br /> Benefits: 180 euros </em>
				</label>
				<label for="RainBarrel">
					<input type="radio" name="rainproof_measure" value="RainBarrel" id="RainBarrel">
					Rain Barrel <em><br/>Costs: 60 euros <br /> Benefits: 144 euros <br /> Bonus: catches rainwater</em>
				</label>
				<label for="GreenRoof">
					<input type="radio" name="rainproof_measure" value="GreenRoof" id="GreenRoof">
					Green Roof <em><br/>Costs: 320 euros <br /> Benefits: 768 euros</em>
				</label>
				<label for="DrainPipe">
					<input type="radio" name="rainproof_measure" value="DrainPipe" id="DrainPipe">
					Disconnect Drain Pipe <em><br/>Costs: 100 euros <br /> Benefits: 240 euros</em>
				</label>
				<label for="Threshold">
					<input type="radio" name="rainproof_measure" value="Threshold" id="Threshold">
					Higher Threshold <em><br/>Costs: 15 euros <br /> Benefits: 36 euros</em>
				</label>
				<label for="VerticalGarden">
					<input type="radio" name="rainproof_measure" value="VerticalGarden" id="VerticalGarden">
					Vertical Garden <em><br/>Costs: 50 euros <br /> Benefits: 120 euros</em>
				</label>
				<label for="TemporalDams">
					<input type="radio" name="rainproof_measure" value="TemporalDams" id="TemporalDams">
					Temporal Dam <em><br/>Costs: 10 euros <br /> Benefits: 24 euros</em>
				</label>
				<input type="hidden" name="player_name" id="nameWMeasure">
				<input class="button" onclick="attachName()" type="submit">
			</form>
		</section>
	</body>
</html>