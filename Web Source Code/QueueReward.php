<?php

// save pattern: name,reward_name

// return values of this script:
// 0 - success, reward is queued
// 1 - failure, player name is empty
// 2 - failure, reward name is empty

$sPlayerNameName = "player_name";
$sRewardName = "reward_name";
$sSavePath = "rewards/";

if (!isset($_POST[$sPlayerNameName]))
{
	echo 1;
	exit;
}

if (!isset($_POST[$sRewardName]))
{
	echo 2;
	exit;
}

if (!is_dir($sSavePath))
{
	mkdir($sSavePath);
}

$sSaveData = $_POST[$sPlayerNameName] . "," . $_POST[$sRewardName];
$sRandomID = rand(0, PHP_INT_MAX);

file_put_contents($sSavePath . "/" . $sRandomID . ".reward", $sSaveData);
echo 0;

echo "<script>
alert('Your reward \"" . $_POST[$sRewardName] . "\" will appear in the world in a few seconds');

window.location.href='quest.php#new-quest';

</script>";

?>