<?php

// save pattern: name,rainproof_measure

// return values of this script:
// 0 - success, character is queued
// 1 - failure, player name is empty
// 2 - failure, rainproof measure is empty

$sPlayerNameName = "player_name";
$sRainproofMeasureName = "rainproof_measure";
$sSavePath = "characters/";

if (!isset($_POST[$sPlayerNameName]))
{
	echo 1;
	exit;
}

if (!isset($_POST[$sRainproofMeasureName]))
{
	echo 2;
	exit;
}

if (!is_dir($sSavePath))
{
	mkdir($sSavePath);
}

$sSaveData = $_POST[$sPlayerNameName] . "," . $_POST[$sRainproofMeasureName];
$sRandomID = rand(0, PHP_INT_MAX);

file_put_contents($sSavePath . "/" . $sRandomID . ".character", $sSaveData);

echo "<script>
alert('Your character called \"" . $_POST[$sPlayerNameName] . "\" will appear in the world and install a " . $_POST[$sRainproofMeasureName] . " in a few seconds');

window.location.href='measure.php#new-measure';

</script>";

//echo 0;

?>