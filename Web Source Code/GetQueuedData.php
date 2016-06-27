<?php

// return pattern: c,name,rainproof_measure;r,reward_type;etc
// c stands for 'character', rewards will start with a 'r'

// return values of this script:
// return pattern - success, returns a string with all data
// 1 - failure, the secret key is not set
// 2 - failure, the secret key is wrong

$iMaximumCharacters = 10;
$iMaximumRewards = 10;

$sRequestCode = "dj_rainfalla";
$sRequestCodeName = "secret_key";
$sCharactersPath = "characters/";
$sRewardPath = "rewards/";

if (!isset($_POST[$sRequestCodeName]))
{
	echo 1;
	exit;
}

if ($_POST[$sRequestCodeName] != $sRequestCode)
{
	echo 2;
	exit;
}

if (isset($_POST["action"]))
{
    if ($_POST["action"] == "clear")
    {
        $asCharacters = scandir($sCharactersPath);
        if (count($asCharacters) > 2)
        {
            for ($i = 2; $i < count($asCharacters); $i++)
            {
				$sPath = $sCharactersPath . "/" . $asCharacters[$i];
                unlink($sPath);
            }
        }
 
        $asRewards = scandir($sRewardPath);
        if (count($asRewards) > 2)
        {
            for ($i = 2; $i < count($asRewards); $i++)
            {
				$sPath = $sRewardPath . "/" . $asRewards[$i];
                unlink($sPath);
            }
        }
    }
   
    exit;
}

$sResult = "";

$asCharacters = scandir($sCharactersPath);
if (count($asCharacters) > 2)
{
	for ($i = 2; $i < count($asCharacters); $i++)
	{
		if ($i - 2 == $iMaximumCharacters)
			break;
		
		$sPath = $sCharactersPath . "/" . $asCharacters[$i];
		$sResult .= ";c," . file_get_contents($sPath);
		unlink($sPath);
	}
}

$asRewards = scandir($sRewardPath);
if (count($asRewards) > 2)
{
	for ($i = 2; $i < count($asRewards); $i++)
	{
		if ($i - 2 == $iMaximumRewards)
			break;
		
		$sPath = $sRewardPath . "/" . $asRewards[$i];
		$sResult .= ";r," . file_get_contents($sPath);
		unlink($sPath);
	}
}

echo substr($sResult, 1);

?>
