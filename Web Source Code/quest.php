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
		<title>Heroes Of Rain | Question</title>
	</head>
	<body>
		<a class="back" href="choice.php"> I want to do something else</a>
		<img id="logo" src="assets/images/hor.png" alt="Heroes of Rain Logo">
		<h1>Answer this question correctly to choose a reward</h1>

		<form method="POST" action="Questions.php?get=answer">
			<label for="Question"></label>
			<input type="hidden" name="question_id">
			<input type="radio" name="answer_id" value="0">
			<input type="radio" name="answer_id" value="1">
			<input type="radio" name="answer_id" value="2">
			<input type="submit">
		</form>
		<script type='text/javascript'>

		var call = $.ajax({
			url: "Questions.php?get=question"

		}).done(function() {
		  $( this ).addClass( "done" );
		  console.log(call);
		  var str = call.responseText;
		  var res = str.split(";");

		  var answers = res[3].split("@")

		  $("form").html("<label for=\"question\">" + res[2] + "</label> <input type=\"hidden\" name=\"question_id\" id=\"QeustId\" value=" + res[1] + "><label for=\"answer1\"><input type=\"radio\" name=\"answer_id\" id=\"answer1\" value=\"0\">" + answers[0] + "</label><label for=\"answer2\"><input type=\"radio\" name=\"answer_id\" id=\"answer2\" value=\"1\">" + answers[1] + "</label><label for=\"answer3\"><input type=\"radio\" name=\"answer_id\" id=\"answer3\"value=\"2\">" + answers[2] + "<input class=\"button\" type=\"submit\">")
		});
		</script>

	</body>
</html>