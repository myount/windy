# Changelog

## v1.1.0.1 - 2015-05-20
### Fixed
* Fixed a bug where Windy would crash on startup if there was no saved window state.

## v1.1.0.0 - 2015-05-19
### Added
* Added a list of currently saved window titles to the notification area icon menu.  Clicking one will cause Windy to
attempt to bring the corresponding window to the front (*not* restore its saved position).

### Fixed
* The "Windy is running" balloon tip will now only be displayed when Windy starts and when you *left*-click on the
tray icon.
* Desktop and window state information created before the current system boot is deleted when Windy starts.  (This is
a more discriminating version of the thing I thought I had done in 1.0.0.2.)

### Changed
* Deserializing window state now no longer automatically restores the window layout.  Deserialized windows are checked
for validity and only valid ones are touched when the layout is restored.  If none of the serialized windows is valid,
an error message is displayed.

## v1.0.0.2 - 2015-05-05
### Fixed
* <del>Clear saved desktop and window state information on startup, as it is unlikely to be of much use across runs.</del>
I forgot I didn't actually do this.

### Added
* Added a German translation. Improvements welcome, as it is my second language.

## v1.0.0.1 - not publicly released
### Fixed
* Minimized and maximized windows are now restored before applying size/location changes.
* Worked around a .NET Framework deficiency that caused Windy's automatic window layout restore to fire any time
the monitor layout changed. 

## v1.0.0.0 - not publicly released
* First version.
