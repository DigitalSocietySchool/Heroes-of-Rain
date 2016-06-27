<?php

// question pattern: q;question_id;question;answer_a@answer_b@answer_c;answer_id
// answer pattern: a;correct_boolean

// return values:
// -1 - failure, something went horribly wrong, contact almar plx
// pattern - success, requested pattern
// 1 - failure, the action key is not set
// 2 - failure, action value is not recognized
// 3 - failure, question id is not set
// 4 - failure, answer id is not set
// 5 - failure, question id is not correct
// 6 - failure, answer id is not correct

$sActionName = "get";
$sActionGetQuestionValue = "question";
$sActionGetResultValue = "answer";
$sQuestionIDName = "question_id";
$sAnswerIDName = "answer_id";

if (!isset($_GET[$sActionName]))
{
	echo 1;
	exit;
}

$sAction = $_GET[$sActionName];
if ($sAction != $sActionGetQuestionValue && $sAction != $sActionGetResultValue)
{
	echo 2;
	exit;
}

$asQuestions = array();
addQuestion($asQuestions, "Which of these prevention measures will help the most during extreme rainfalls (>50 mm)?", array("Grass bricks", "Rain barrels", "Threshold"), 0);
addQuestion($asQuestions, "Which prevention measure might cause problems for people who have difficulties walking?", array("Rain barrels", "Threshold", "Disconnected drainpipe"), 1);
addQuestion($asQuestions, "What is the most efficient surface to rainproof with plants?", array("A car roof", "A roof", "A window"), 1);
addQuestion($asQuestions, "What could be a temporary measure to avoid the water behind the door?", array("Sand bags", "Grass bricks", "Rain barrel"), 0);
addQuestion($asQuestions, "With what can you replace the tiles in our garden?", array("Not recommended to do", "Stone bricks", "Plants"), 2);
addQuestion($asQuestions, "Who took the initiative to start group called Amsterdam Rainproof?", array("The dutch national government", "Citizens of Amsterdam", "Waternet"), 2);
addQuestion($asQuestions, "What is the main goal of Amsterdam Rainproof?", array("Raise awareness and create engagement among citizens", "Taking care about drinking water supply", "Being an independent company"), 0);
addQuestion($asQuestions, "What does the Municipality of Amsterdam wants to achieve in 2040?", array("Make Amsterdam Rainproof bigger than Waternet", "Work together with the waterboard", "Make Amsterdam’s sewage being able to handle 50 mm/h"), 2);
addQuestion($asQuestions, "How many mm/h is Amsterdam’s sewage able to handle at the moment?", array("50 mm/h", "20 mm/h", "10 mm/h"), 1);
addQuestion($asQuestions, "How did the roof of Amsterdam’s Central station become rainproof?", array("The roof became a green roof", "The drain pipe has been disconnected", "The materials were changed into waterproof materials."), 1);
addQuestion($asQuestions, "Over the past 100 years, how much has the temperature increased in The Netherlands?", array("It is still the same", "0.7 degrees celcius", "1.7 degrees celcius"), 2);
addQuestion($asQuestions, "How many degrees will temperature increase if the countries will not cut down on carbon dioxide in the next 50 years?", array("Between 0.8 and 1", "Between 1 and 2.3", "It will not increase"), 1);
addQuestion($asQuestions, "How much has the total amount of precipitation increased during the last century?", array("21%", "3%", "31%"), 0);
addQuestion($asQuestions, "Who has developed different climates scenarios for The Netherlands?", array("KNMI", "Municipalities", "Private companies"), 0);
addQuestion($asQuestions, "In which place will be the highest increase in heavy rainfall event?", array("Groningen", "Coastal Provinces (Noord Holland & Zuid Holland)", "East of the Netherlands (Achterhoek)"), 1);

//var_dump($asQuestions);

switch ($sAction)
{
	case $sActionGetQuestionValue:
	$iRandomIndex = rand(0, count($asQuestions) - 1);
	$asRandomQuestion = $asQuestions[$iRandomIndex];
	$sAnswers = $asRandomQuestion[1][0] . "@" . $asRandomQuestion[1][1] . "@" . $asRandomQuestion[1][2];
	echo "q;" . $iRandomIndex . ";" . $asRandomQuestion[0] . ";" . $sAnswers . ";" . $asRandomQuestion[2];
	exit;
	
	case $sActionGetResultValue:
	if (!isset($_POST[$sQuestionIDName]))
	{
		echo 3;
		exit;
	}
	
	if (!isset($_POST[$sAnswerIDName]))
	{
		echo 4;
		exit;
	}
	
	$iQuestionID = $_POST[$sQuestionIDName];
	if ($iQuestionID < 0 || $iQuestionID >= count($asQuestions))
	{
		echo 5;
		exit;
	}
	
	$iAnswerID = $_POST[$sAnswerIDName];
	if ($iAnswerID < 0 || $iAnswerID >= 3)
	{
		echo 6;
		exit;
	}
	
	//var_dump($iAnswerID);
	//var_dump($asQuestions[$iQuestionID][2]);
	$intAnswerID = (int)$iAnswerID;

	if ($intAnswerID == $asQuestions[$iQuestionID][2])
	{
		echo "<script>
alert('You got it!');
window.location.href='reward.php';
</script>";
		exit;
	}
	else
	{
		echo "<script>
alert('That was the wrong answer. Try this new question for a reward');
window.location.href='quest.php';
</script>";
		exit;
	}

	
	echo -1;
	exit;
}

function addQuestion(&$asArray, $sQuestion, $asAnswers, $iAnswerIndex)
{
	array_push($asArray, array($sQuestion, $asAnswers, $iAnswerIndex));
}

?>