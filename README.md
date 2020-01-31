# Windy
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fmyount%2Fwindy.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2Fmyount%2Fwindy?ref=badge_shield)


## Please note: Windy is no longer actively maintained.

I no longer maintain Windy, for a couple of reasons:

1. ManagedWinApi, which it uses under the hood, doesn't know about Windows 10's virtual desktops, and I did not feel like groveling through the Windows API documentation to determine whether there are new APIs that do and write my own P/Invoke wrappers for them if so, because:
2. Windy was written to solve a problem I no longer have: I had a job where I had a dockable laptop that was frequently undocked in order to take it to meetings, give presentations, etc., and it was annoying to have to redistribute my windows across the two monitors I had connected to the docking station.  I have since changed jobs to one with significantly fewer meetings and presentations, and I have just one huge 4K monitor and the laptop's built-in display, so it's less inconvenient to rearrange my windows on the rare occasion that I do have to take my laptop away from my desk.

If you are someone for whom Windy would still be useful, you are welcome to take it over.

## About Windy

Has this ever happened to you?  Your laptop or tablet is docked and you have external monitors connected, because
who can live with those tiny built-in displays?  You undock your machine, and all your windows get shuffled up and
jammed into that one single tiny screen! Then when you dock it again, all those windows are still a jumbled mess!
Never deal with cluttered windows again, with Windy!

Windy is a lightweight window manager for users of dockable mobile PCs.  It saves window positions and states on
startup and on demand, and watches for desktop layout changes.  When it detects that the desktop layout has changed
back (or when you specifically request to restore it), it restores the original window layout.

## Usage

Run Windy.exe.  Ctrl+Win+S saves the current window layout (and desktop layout).
Ctrl+Win+R restores the saved window layout.  It's that simple.

For maximum utility, place a shortcut to Windy.exe in your Startup folder.

## Contributing

Fork it, make your changes, send a pull request.  Please try to observe the existing coding style, which is
mostly the default that Visual Studio imposes anyway.


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fmyount%2Fwindy.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fmyount%2Fwindy?ref=badge_large)