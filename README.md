# NRGScoutingApp2019DeepSpace
NRG Scouting app for 2019 Deep Space FRC Game
## How to Install?

### iOS
#### App Store Link Coming Soon...
Check here by week 2, it should be live
#### Install builds w/o App Store 
Head to the [https://github.com/NRG948/NRGScoutingApp2019DeepSpace/releases](releases) page and download the ".ipa" file for the latest version of the application. Then, go to the [https://docs.google.com/document/d/1pQ8tAQsyVpHBQo1SHUSvb6X0ylbjBsiniisp-dAJFd8/edit#bookmark=id.g6e35tc9zquw](install instructions) and follow the steps to install the app without the App Store

### Android
#### Play Store Link Coming Soon...
Should be live by week 1
#### Install builds w/o Play Store
Head to the [https://github.com/NRG948/NRGScoutingApp2019DeepSpace/releases](releases) page and download the ".apk" file containing "Download.this.one" in the file name (onto your android phone/tablet). Then, click the file and you should be prompted with a dialog asking you enable unknown sources. Do so and install the app.

## Features (images coming soon)

### Exporting and Importing Data
#### Exporting Data
This feature allows the user to share the Match and Pit Scouting Data with others. They can simply copy the text (or share easily via the sharesheet using AirDrop, Bluetooth, Email, etc.). For those who are curious, the data is stored in the app (and exported) as JSON, making it easily readable by any future implementations of Javascript servers or similar.
As for Match Scouting, the app can also export to Excel (in iOS), which is useful to see all the matches, their teams, sides, and stats for all the game elements (climb, place time for pieces and elements)
Pit Scouting data is also exported for easy viewing.
#### Importing Data
This feature allows the user to upload exported data from other's devices to their own. The app pulls in all the matches and adds them to the list, allowing for true overall rankings for teams. This also looks for matches with the same number and side and prompts the user to "Ignore" or "Overwrite." If a match has the same team number, it is simply overwritten.
As for Pit Scouting, if two scouts have gathered data on the same team, both of their notes are combined into one and separated with dashes to denote this.

### Match Scouting
After clicking the "+" button at the top right of your screen, you can choose which team you will be scouting for the match.
As soon as the mathc starts, you can start the timer and record the events completed by the team, recording all the data.
When the match ends, go to the parameters tab and select all the correct options (this will help with rankings and sortings of matches)
#### Rankings
This ties in directly with the match scouting. If a team has a lower time, they will be ranked higher. You can choose which type of ranking you want by selecting between "Overall", "Climb", "Low", "Medium", "High", "Cargo", and "Hatch." 
If a team has received a Red Card, their name will be highlighted in red. A Yellow card with highlight in yellow and two Yellow Cards will hihglight in Red (2 Yellow Cards == Red Card).
To dive deeper into a team, you can click on them and see all the average times and score for them as well as all the matches they have participated in. Clicking on a match will open it to be editied/viewed.
#### Detail View Page
If a match is clicked on from the matches main page, its details will be displayed (JSON Stored file) along with the self explanatory options for Open, Delete, or Cancel (go back).

### Pit Scouting
This works quite similarly to Match Scouting, where you can choose the team you are pit scouting and fill in the questions in the form (that you think may be useful to help your team decide in alliance selections)
