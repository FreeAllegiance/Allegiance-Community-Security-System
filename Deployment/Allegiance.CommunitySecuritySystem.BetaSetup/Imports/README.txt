How to setup CSS for the first beta.

1. Extract all files to a subdirectory under C:\Program Files\Microsoft Games\Allegiance, called CSS. The complete directory will be named: C:\Program Files\Microsoft Games\Allegiance\CSS.

64 bit users will extract thier files to C:\Program Files (x86)\Microsoft Games\Allegiance\CSS



2. Update registry settings for Allegiance.

Open key: HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft Games\Allegiance\1.1

** Save or note old values first! **

Create new String Key: "Exe Path", set the Exe Path to point to your CSS directory you used in step 1. Do not put the executable name in the Exe Path. 

Example for 32 bit users: C:\Program Files\Microsoft Games\Allegiance\CSS
For 64 bit users: C:\Program Files (x86)\Microsoft Games\Allegiance\CSS
