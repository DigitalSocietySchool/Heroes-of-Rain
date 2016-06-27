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
		<title>Heroes Of Rain</title>
	</head>
	<body>
	<img id="logo" src="assets/images/hor.png" alt="Heroes of Rain Logo">
		<section>
			<article>
				<p>It is raining more and more intense in Amsterdam, this can cause problems. Floods, due to these heavy rainfall events, bring damage to public and private space. You are one of the Heroes of Rain, a homeowner in this neighbourhood in Amsterdam. You have heard about the news, that this summer, a cloudburst as heavy as the cloudburst in Copenhagen in 2011 might happen. In order to have the least amount of damage, you want to make your house prettier, greener, healthier, and more rainproof. </p>

				<p>So you start. There are several actions you could do, first of all, you have your normal salary You start with a 150 euros that you want to spend on measures.</p>
			</article>
		</section>

		<section>
			<form action="choice.php" id="nameForm">
				<label for="player_name">How should we call you?</label>
				<br />
				<input type="text" name="player_name" required="" placeholder="Your name" id="player_name">
				<input class="button" onclick="store()" type="submit">
			</form>
		</section>
	</body>
</html>