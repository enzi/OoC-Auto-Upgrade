Auto Upgrade mod for Orb of Creation

*Setup*

- Download BepInEx from here: https://github.com/BepInEx/BepInEx/releases
- Extract the zip to the Orb of Creation root folder so it is: `steamapps/common/Orb of Creation/BepInEx`, with a `winhttp.dll` where the `.exe` is
- Run the game once, BepInEx will create folder and config
- Close game, it should now have several sub folders `core` and `plugins`
- Put the `OoCAutoUpgrade.dll` in the `BepInEx/plugins` folder

*Linux* 

- In Steam, set the follwing launch option: `WINEDLLOVERRIDES="winhttp=n,b" %command%`

*Usage*

In-game, press `Ctrl+A` to toggle the auto upgrade
It will upgrade any attribute that has trivial costs (greyed out costs)
