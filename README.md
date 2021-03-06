# Readarr

Readarr is a PVR for Usenet and BitTorrent users. It can monitor multiple RSS feeds for new ebooks and will grab, sort and rename them.

## Major Features Include:

* Support for major platforms: Windows, Linux, macOS, Raspberry Pi, etc.
* Automatically detects new ebooks
* Automatic failed download handling will try another release if one fails
* Manual search so you can pick any release or to see why a release was not downloaded automatically
* Fully configurable episode renaming
* Full integration with SABnzbd and NZBGet
* Full integration with your favourite ebook management software (tbd)
* And a beautiful UI

## Configuring Development Environment:

### Requirements

* [Visual Studio 2017/2019](https://www.visualstudio.com/vs/)
* [Git](https://git-scm.com/downloads)
* [NodeJS](https://nodejs.org/en/download/)
* [Yarn](https://yarnpkg.com/)

### Setup

* Make sure all the required software mentioned above are installed
* Clone the repository into your development machine. [*info*](https://help.github.com/en/articles/working-with-forks)
* Grab the submodules `git submodule init && git submodule update`
* Install the required Node Packages `yarn -g`

### Backend Development

* Run `yarn build` to build the UI
* Open `Sonarr.sln` in Visual Studio
* Make sure `NzbDrone.Console` is set as the startup project
* Build `NzbDrone.Windows` and `NzbDrone.Mono` projects
* Build Solution

### UI Development

* Run `yarn watch` to build UI and rebuild automatically when changes are detected
* Run Sonarr.Console.exe (or debug in Visual Studio)

### License


* [GNU GPL v3](http://www.gnu.org/licenses/gpl.html)
* Copyright 2010-2019
