# XMod – FMOD Audio Manager for Unity

**XMod** is a lightweight, modular Unity package that simplifies working with **FMOD** audio in your game projects. It provides a high-level API for playing, managing, and organizing audio, with built-in support for channel groups, global volume control, and GameObject-following 3D sounds.

---

## Features

- **Global Volume Control**  
  Change the volume of all active sounds with a single static property.

- **One-Shot & Loopable Sounds**  
  Choose whether sounds should only play once or be reusable.

- **3D Audio Support**  
  Automatically follow GameObjects for spatialized sound playback.

- **Channel Grouping**  
  Organize and control related sounds using named FMOD channel groups.

- **Auto Cleanup**  
  Handles FMOD instance lifecycles and removes stopped sounds from memory.

- **Editor-Friendly API**  
  Includes `SimpleSoundPlayer` for drag-and-drop editor configuration with `PlayOnStart`.

---

## Installation

1. **Install [FMOD for Unity](https://www.fmod.com/unity)** if you haven’t already.
2. Clone or download the XMod source files into your Unity project’s `Assets` folder:
3. Use the `Sound` class programmatically or use `SimpleSoundPlayer` for quick setup in the Unity Inspector.

---

## Usage

### Playing a Sound via Script

```csharp
[SerializeField] private Sound explosionSound;

void TriggerExplosion()
{
    explosionSound.Play("explosions");
}
```
- **Using SimpleSoundPlayer**

  - **Play On Start** – plays the sound automatically when the scene starts.
  - **Channel Group Name** - assign soudns to a shared group for easy control.
  - **Sound** - reference to the 'Sound' object.

---

## API Overview

**Global Volume**
Change the volume for all active sounds:
```csharp
Sound.GlobalVolumeMultiplier = 0.75f;
```

**Playing a Sound**
```csharp
sound.Play("sound_group");
```

**Stopping a Sound**
```csharp
sound.Stop();
```

**Following GameObjects**
For 3D sounds that follow an object
```csharp
sound.FollowGameObject = someGameObject;
sound.Play();
```

**Accessing Active Sounds**
```csharp
foreach (var sound in Sound.ActiveSounds)
{
    Debug.Log(sound.Volume);
}
```
