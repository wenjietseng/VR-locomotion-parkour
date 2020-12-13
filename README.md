# VR-locomotion-parkour
## Update
1. `LocomotionTechinque.cs` Line 41:
```
startPos = (OVRInput.GetLocalControllerPosition(leftController) + OVRInput.GetLocalControllerPosition(rightController)) / 2;
```

## Demo Video
<a href="http://www.youtube.com/watch?feature=player_embedded&v=5s-vTwTFc7U
" target="_blank"><img src="http://img.youtube.com/vi/5s-vTwTFc7U/0.jpg" 
alt="IMAGE ALT TEXT HERE" width="240" height="180" border="10" /></a>

## How to Start
```
git clone https://github.com/wenjietseng/VR-locomotion-parkour.git
```
- download the GitHub repo and open __VRParkour__ folder as a Unity project
- implement your locomotion technique in `LocomotionTechnique.cs`
- play and see how fast and how many coins you can get!

## Used Assets and Packages
### core
- Unity 2019.4
- Oculus Integration
- Oculus Desktop (for Oculus Link)
- TMPro
### cybersickness reduction
- [Ginger VR](https://github.com/angsamuel/GingerVR)
### scene
- [Low Poly Ultimate Pack](https://assetstore.unity.com/packages/3d/props/low-poly-ultimate-pack-54733)
- [Forest - Low Poly Toon Battle Arena / Tower Defense Pack](https://assetstore.unity.com/packages/3d/environments/forest-low-poly-toon-battle-arena-tower-defense-pack-100080)
### misc
- Winner Winner Funky Chicken Dinner (YouTube Audio Library)
- [tone beep](https://freesound.org/people/pan14/sounds/263133/)
- [crowd yay](https://freesound.org/people/mlteenie/sounds/169233/)
