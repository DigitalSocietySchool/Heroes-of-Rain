Hi and welcome to the Heroes of Rain project! This manual will consist of 3 major parts I will explain:

1. Setting it up
2. The game source code
3. The website source code

For both of them I will explain what the general logic is and how it works.

# ====== Setting It Up

#### Website
In order for the website to run, it makes use of the following:

- HTML & CSS
- JavaScript
- PHP

To install the website part, you can just put all the files in a folder inside a web server that supports the previous list. This can be done using any FTP client that has access to the internet. It is important to note that the files should to able to have writing permissions.

#### Game

The game is build to be run on a single desktop connected to a big screen. The desktop (or laptop) can be connected to a screen that supports inputs like HDMI, VGA, DVI, etc. Basically any cable that supports graphical transmission. It is very important to note, that every game instance is currently requesting from the same web server. So multiple game instances will request each other's queues. In order to fix this, every instance needs its own URL to request from. Meaning that for every game instance, there should be another copy of all the website files in a seperate folder.

# ====== The Game

The game is build in Unity C#.

#### Loop
I will explain the general loop in which the game functions and will mark important pieces in bold which I will explain after.

1. The game starts up
2. The splash screen is shown with all the relevant logos
3. **The introduction screen is shown**
4. It will wait for the first user input
5. The gameplay starts
6. Things happen **based on user input** and **certain conditions**
7. **At 100% time** the user input is stopped and the final cloud is spawned
8. The game will wait for the cloud to **reach it's end**
9. A highscore screen is shown
10. After 30 seconds, goes back to step 2


#### Important Notes
**The introduction screen is shown:**
_This is not really a unity scene on itself. The introduction screen is just the gameplay scene, but with a UI image overlay, The gameplay is paused while this image is shown._

**Based on user input:**
_This is a pretty important part. The players can send characters into the game via our website. This user data is saved on the web server in seperate files. Unity will send a POST request to the website, requesting all the user data. This happens in an customizable interval to relieve web bandwith usage. This data is retreived as a single long string containing all queued characters and rewards. This retreived string is processed and characters will spawn._

**Certain conditions:**
_The user input is not the only thing that can happen in the game. The game itself will also make stuff happen. First of all, rain clouds will spawn over time based on the level progression; where 0 means start of the round and 1 means the end. Secondly, these rain clouds will cause floods on certain points in the level if it rains to heavy; these points are indicated by a SewageDrain gameobject. Thridly, every so often a the game will send a character into the world to clean a part of the active floods if needed._

**At 100% time:**
_The timing of the game is based on our a calendar. The game will run X years in Y minutes and depending on these settings will run slowly or really fast. The code will calculate how many seconds 1 day is, and will count the days, weeks, months and years passed. Every milestone has it's own event which can be bound to (OnDayPassed, OnWeekPassed, etc). The Y minutes does currently not 100% correctly match the X years due a math error, so a 0% to 100% timer is shown in the bottom left corner of the screen._

**Reach it's end:**
_When the timer reaches 100%, all the active rain clouds will despawn and a super heavy rain cloud will spawn. Every rain cloud will spawn outside the screen and will move towards another point outside the screen. This position is based on a gameobject somewhere that has a collider. Spawned rain clouds will move towards a random position within this collider and thus giving them a random moving direction. Upon reaching this collider, they destroy themselfs._

On a last note, there is a gameobject called Controllers in the scene. It has several childs with attached scripts that control most of the game logic. Most settings can also be tweaked here, but some are specific and can be found in the prefab folder on their respective object.

# ====== The Website

The website is build in HTML, CSS, JavaScript and PHP.

#### Flow
I will explain the general flow of the website and mark important parts in bold.

1. The index page is loaded.
2. The users can fill in their name.
3. Users can now choose to either build a rainproof measure or take the quiz for a reward


1. Upon choosing to build a rainproofing measure
2. A screen is shown that lists the rainproofing measures
3. Upon selecting which measure to build, the code **saves this user request (A)**


1. Upon choosing to take the quiz for a reward
2. **A question is shown**
3. Upon selecting the answer, **if it's correct,** the a page with rewards is shown
4. A screen is shown that lists the rewards
5. Upon selecting which reward to use, the code **saves this user request (B)**


#### Important Notes

Every PHP file will echo a result based on a the request. This can be the requested data, or a number indicating an error. These results can be find in the top of the PHP file itself.

**Saves this user request (A):**
_The page will submit a form request into a PHP file, which will check what type of rainproofing measure has been selected. If no error occured, the code will save the player name and the selected measure into a file. This file ([random_number].character) is placed at /characters/ and contains a string with the data. The way this data is saved, can be found inside the PHP file._

**A question is shown:**
_There is a PHP file that has all the questions in it (hardcoded). Whenever this file receives a request asking for a question, if there are no errors, it will echo a string consisting of a random question including with the answer. The way this question is echo'd, can be found inside the PHP file._

**If it's correct:**
_The same PHP file that will echo a random question, is also responsible for check if a question is correct. All the questions are hardcoded into this file. The submitted question and answer is compared against the question data, and a result will be echo'd if there are no errors. The way this data should be submitted, can be seen in the PHP file._

**Saves this user request (B):**
_The page will submit a form request into a PHP file, which will check what type of reward has been selected. If no error occured, the code will save the player name and the selected reward into a file. This file ([random_number].reward) is placed at /rewards/ and contains a string with the data. The way this data is saved, can be found inside the PHP file._