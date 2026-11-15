_SOFTWARE DESIGN SPECIFICATION_

~1.1. BeatFlagItem~
Describes timepoint state with a leverage of some length on both sides, placed in time scenario and containing state variable

~1.2. BeatFlagController~
Takes control of several *BeatFlagItem* objects
Allowing for state control over *BeatFlagItem* on certain timepoint values (by exact second value) [!]
Allowing for estimation of tough or timeflags

~2.7. RocketBehaviour~
Empty, as of now
Will potentially define Rocket wobble or Meteor interaction

~2.8. TapPanelBehaviour~
Provides User interaction states: pressed/non-pressed

~2.3. LevelBrowserBehaviour~
Obtains predefined button sprite from Unity editor

~2.4. MenuManagerBehaviour~
Responsible for UI switching and level selection

~2.1. EndgameMenuBehaviour~
Defines functions for return to Main Menu

~2.2. GameBehaviour~
Responsible for general game mechanics during Play Mode
Responsible for timepoint capture during Setup Mode
Responsible for game mechanic-related music toggle

~2.5. MusicBehaviour~
Controlls music in the Main Menu

~2.6. PregameMenuBehaviour~
Controlls game Game initial "get ready" pause after Start button pressed already