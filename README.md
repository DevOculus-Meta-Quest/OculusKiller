# Oculus Killer 🚀

Revamped and supercharged to enhance your VR experience by terminating unnecessary Oculus processes and seamlessly launching SteamVR. This refactored version is modular, smoother, faster, and equipped with improved error handling, crash monitors, and automatic restart capabilities in case of a crash.

**Original Author:** [@kaitlyndotmoe](https://github.com/kaitlyndotmoe)  
**Contributors:** @UnusualNorm, @HyrumGG  
**Original Repository:** [OculusKiller](https://github.com/kaitlyndotmoe/OculusKiller)

This tool complements the [Oculus VR Dash Manager](https://github.com/DevOculus-Meta-Quest/Oculus-VR-Dash-Manager), ensuring that you have comprehensive control and optimization of your VR experience.

<img src="assets/images/OculusKiller_Logo.png" alt="OculusKiller" width="400" height="400"/>

## 🚩 Index
- [Features](#-features)
- [Download](#-download-)
- [Installation](#-installation)
- [Common Fixes](#-common-fixes)
- [Support](#support-oculus-killer-)
- [Changelog](Changelog.md)

## 🌟 Features
- Modular Design
- Enhanced Performance
- Improved Error Handling
- Crash Monitoring
- Automatic Restart on Crash
- Graceful Exit when Leaving SteamVR

## ⬇️ Download ⬇️

Grab the latest release [here](https://github.com/DevOculus-Meta-Quest/OculusKiller/releases).

<p align="center">
  <img src="https://hits.dwyl.com/DevOculus-Meta-Quest/OculusKiller.svg" alt="HitCount">
  <img src="https://img.shields.io/github/actions/workflow/status/DevOculus-Meta-Quest/OculusKiller/Build_and_Release.yml" alt="GitHub Workflow Status">
  <img src="https://img.shields.io/github/downloads-pre/DevOculus-Meta-Quest/OculusKiller/latest/total?style=plastic" alt="GitHub release (latest by SemVer including pre-releases)">
  <img src="https://img.shields.io/github/downloads/DevOculus-Meta-Quest/OculusKiller/total?style=plastic" alt="GitHub all releases">
  <img src="https://img.shields.io/github/release-date/DevOculus-Meta-Quest/OculusKiller?style=plastic" alt="GitHub Downloads">
  <a href="https://www.paypal.com/donate/?business=X76ZW4RHA6T9C&no_recurring=0&item_name=Support+the+evolution+of+Oculus+VR+Dash+Manager%21+Your+donation+fuels+innovation+and+enhanced+virtual+experiences.+%F0%9F%9A%80%F0%9F%8C%90&currency_code=USD"><img src="https://img.shields.io/badge/Donate-PayPal-green.svg" alt="PayPal"></a>
</p>

# Support Oculus Killer 🚀

We're on a mission to redefine your virtual reality experience. Oculus Killer is not just a tool; it's a movement towards a seamless, powerful, and user-centric VR journey. With the collective genius of [@kaitlyndotmoe](https://github.com/kaitlyndotmoe), @UnusualNorm, and @HyrumGG, we've reimagined the way you interact with your Oculus device.

Our latest iteration is a testament to the dedication and feedback from our vibrant community. We've listened, iterated, and delivered a modular, robust, and intuitive solution that prioritizes performance and stability. The Oculus Killer is your gateway to a VR experience that's unencumbered by unnecessary processes, giving you the freedom to enjoy SteamVR with unprecedented ease.

This endeavor is a sibling to the [Oculus VR Dash Manager](https://github.com/DevOculus-Meta-Quest/Oculus-VR-Dash-Manager), and together, they form a formidable duo that puts control back into your hands. Your support and contributions have been the cornerstone of our development process, and we're immensely grateful for the passion you bring to our projects.

🌐 [Explore Oculus Killer](https://github.com/DevOculus-Meta-Quest/OculusKiller/releases)

As we continue to push the boundaries of what's possible within VR, we invite you to join us on this journey. Your insights, feedback, and support fuel our innovation and drive us to create enhanced virtual experiences that resonate with your needs.

If you believe in what we're building and wish to contribute to the evolution of Oculus Killer, consider making a donation. Your generosity not only accelerates development but also empowers us to dream bigger and deliver better.

🚀 [Support Us with a Donation](https://www.paypal.com/donate/?business=X76ZW4RHA6T9C&no_recurring=0&item_name=Support+the+evolution+of+Oculus+VR+Dash+Manager%21+Your+donation+fuels+innovation+and+enhanced+virtual+experiences.+%F0%9F%9A%80%F0%9F%8C%90&currency_code=USD)

Every contribution, no matter the size, makes a monumental difference. Thank you for believing in Oculus Killer and for being an integral part of our community. Together, we're not just playing games; we're setting new standards for virtual reality.

With gratitude,

The Oculus Killer Team 🌟

## 🛠 Installation
1. Open Task Manager, go to Services and look for OVRService, right click on it and stop it. (If you have the Oculus app or any VR games open, they WILL close when stopping OVRService.)
2. Go to `C:\Program Files\Oculus\Support\oculus-dash\dash\bin` in Explorer.
3. Rename the original `OculusDash.exe` to `OculusDash.exe.bak` and move my replacement `OculusDash.exe` into the folder you just opened in Explorer.
4. Go back to Task Manager, look for OVRService again, right click on it and start it.

## 🛠 Common Fixes
### Headset Infinitely Loads (SteamVR doesn't launch)
- Open "File Explorer"
- Click the "View" tab (at the top)
- Enable "File name extensions"
- Follow the installation instructions

![Black Screen Fix](assets/images/BlackScreenFix.png)

### OpenXR Games launch, but cannot be seen
- Open SteamVR settings (with headset connected)
- Press "Show" under "Advanced Settings"
- Open the "Developer" tab
- Click "Set SteamVR as OpenXR runtime"

![OpenXR Fix](assets/images/OpenXR_Fix.png)

### Non-OpenXR Games launch, but cannot be seen
- Install [OVR Advanced Settings](https://store.steampowered.com/app/1009850/OVR_Advanced_Settings/) and launch it.
- Open the new overlay (found next to the desktop button)
- Open the overlay settings (bottom left)
- Turn on "Autostart"
- Turn on "Force Use SteamVR (Disable Oculus API [experimental])"

![OVR Settings Fix](assets/images/OVRSettingsFix.png)

**Logs:** All log files are located at `C:\Users\<USERNAME>\AppData\Local\OculusKiller\OculusKiller.log`
