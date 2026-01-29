#pragma warning disable
// FMOD Stub Types and Audio Stub for MVP Build
// Full FMOD integration will be completed in ETAPA 6

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Celeste.Core;

namespace FMOD.Studio
{
	public class EventInstance 
	{ 
		public void start() { }
		public void stop(STOP_MODE mode) { }
		public void release() { }
		public void setParameterValue(string name, float value) { }
		public RESULT getParameter(string name, out float value) { value = 0f; return RESULT.OK; }
		public RESULT get3DAttributes(out _3D_ATTRIBUTES attributes) { attributes = default; return RESULT.OK; }
		public RESULT set3DAttributes(_3D_ATTRIBUTES attributes) { return RESULT.OK; }
	}
	
	public class EventDescription { }
	
	public class Bank { }
	
	public class System { }
	
	public class VCA { }
	
	public enum LOAD_BANK_FLAGS { NORMAL = 0 }
	
	public struct _3D_ATTRIBUTES { 
		public Vector3 position;
		public Vector3 forward;
		public Vector3 up;
	}
	
	public enum RESULT { OK = 0 }
	
	public enum STOP_MODE { IMMEDIATE = 0, ALLOWED_TO_CONTINUE = 1 }
}

namespace FMOD
{
	public class Channel { }
	public class Reverb3D { }
	public class Geometry { }
	public struct DSP { }
	public struct DSPConnection { }
	public struct Vector { }
	public struct ATTRIBUTES_3D { }
	public enum RESULT { OK = 0 }
	public enum INITFLAGS { NORMAL = 0 }
}

namespace Celeste
{
	/// <summary>
	/// Stub Audio class for MVP - full FMOD integration in ETAPA 6
	/// </summary>
	public static class Audio
	{
		public static class Banks
		{
			public static object Master;
			public static object Music;
			public static object Sfxs;
			public static object UI;
			public static object DlcMusic;
			public static object DlcSfxs;
		}

		public static string CurrentMusic { get; set; } = "";
		public static object CurrentMusicEventInstance { get; set; }
		public static object CurrentAmbienceEventInstance { get; set; }
		public static object EndSnapshot { get; set; }
		public static object MusicUnderwater { get; set; }
		public static bool BusPaused { get; set; }
		public static bool Ready { get; private set; } = false;

		public static void Initialize() { }
		public static void Shutdown() { }

		public static void PlaySfx(string sfx, Vector3 position, float volume = 1f) { }
		
		// Overloads for different parameter types
		public static void Play(string sfx, string position = null, float volume = 1f) 
		{ 
			ServiceLocator.LogSystem?.Log($"[Audio] PlaySfx: {sfx} at {position} vol={volume}");
		}
		public static void Play(string sfx, Vector2 position, float volume = 1f) 
		{ 
			PlaySfx(sfx, new Vector3(position, 0), volume);
		}
		public static void Play(string sfx, string position, float volume, float pitch) 
		{ 
			ServiceLocator.LogSystem?.Log($"[Audio] PlaySfx: {sfx} at {position} vol={volume} pitch={pitch}");
		}
		public static void Play(string sfx, Vector2 position, float volume, float pitch) 
		{ 
			PlaySfx(sfx, new Vector3(position, 0), volume);
		}
		public static void Play(string sfx, Vector3 position, float volume, float pitch) 
		{ 
			PlaySfx(sfx, position, volume);
		}
		public static void Play(string sfx, string position, float volume, float pitch, float pan) 
		{ 
			ServiceLocator.LogSystem?.Log($"[Audio] PlaySfx: {sfx} at {position} vol={volume} pitch={pitch} pan={pan}");
		}
		
		public static void PlayMusic(string music, bool fadeOut = false, bool fadeIn = false) { CurrentMusic = music; }
		public static void StopMusic(bool fadeOut = false) { }
		public static bool SetMusic(string music, bool fadeOut = false, bool fadeIn = false, bool startPlaying = true) { CurrentMusic = music; return true; }
		public static void SetAltMusic(string music) { }
		public static void SetAltMusic(string music, bool startPlaying = true) { }
		public static bool SetAmbience(string ambience, bool startPlaying = true) { return true; }
		public static void PauseMusic() { }
		public static void ResumeMusic() { }
		
		public static void SetMusicVolume(float volume) { }
		public static float GetMusicVolume() => 1f;
		
		public static void SetSfxVolume(float volume) { }
		public static float GetSfxVolume() => 1f;
		
		public static void SetMusicParam(string param, float value) { }
		public static void SetParameter(string param, float value) { }
		public static void SetParameter(object instance, string param, float value) { }
		
		public static object GetEventInstance(string path) => new FMOD.Studio.EventInstance();
		public static object CreateInstance(string path) => new FMOD.Studio.EventInstance();
		public static object CreateSnapshot(string path) => new FMOD.Studio.EventInstance();
		public static object ReleaseSnapshot(object snapshot) => null;
		public static object Load(string eventName) => null;
		
		public static void PlayEventInstance(object instance) { }
		public static void StopEventInstance(object instance) { }
		public static void StopEventInstance(object instance, string mode) { }
		public static void StopEventInstance(object instance, FMOD.Studio.STOP_MODE mode) { }
		public static void ReleasEventInstance(object instance) { }
		public static void PauseMusic(object snapshot) { }
		public static void ResumeSnapshot(object snapshot) { }
		
		public static void Update() { }
		public static void ListenFor(string eventName) { }
		public static void Stop() { }
		public static void Stop(string snapshot) { }
		public static bool PauseGameplaySfx { get; set; }
		public static void BusStop(string busName, bool immediate = true) { }
		public static void BusStopAll() { }
		public static void Unload() { }
	}
}
#pragma warning restore
