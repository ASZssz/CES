using Microsoft.Xna.Framework;
using System;
using System.IO;
using Celeste.Core;

namespace Celeste.Desktop;

/// <summary>
/// Implementação de plataforma para Desktop
/// </summary>
public class DesktopPlatformService : IPlatformService
{
	private bool _fullscreen = false;
	private DisplayOrientation _orientation = DisplayOrientation.Portrait | DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

	public string GetSavesDirectory()
	{
		string saveDir = Path.Combine(AppContext.BaseDirectory, "Saves");
		Directory.CreateDirectory(saveDir);
		return saveDir;
	}

	public string GetAppDataPath()
	{
		return AppContext.BaseDirectory;
	}

	public string GetLogsDirectory()
	{
		string logsDir = Path.Combine(AppContext.BaseDirectory, "Logs");
		Directory.CreateDirectory(logsDir);
		return logsDir;
	}

	public void RequestFullscreen(bool fullscreen)
	{
		_fullscreen = fullscreen;
		// MonoGame fullscreen control would go here
	}

	public bool IsFullscreen()
	{
		return _fullscreen;
	}

	public void SetDisplayOrientation(DisplayOrientation orientation)
	{
		_orientation = orientation;
	}
}

/// <summary>
/// LogSystem para Desktop
/// </summary>
public class DesktopLogSystem : ILogSystem
{
	private readonly string _logsDir;

	public DesktopLogSystem()
	{
		_logsDir = Path.Combine(AppContext.BaseDirectory, "Logs");
		Directory.CreateDirectory(_logsDir);
		LogToFile($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] === CELESTE DESKTOP LOG START ===");
	}

	public void Log(string message)
	{
		string formatted = $"[{DateTime.Now:HH:mm:ss}] [INFO] {message}";
		Console.WriteLine(formatted);
		LogToFile(formatted);
	}

	public void LogError(string message, Exception ex = null)
	{
		string msg = ex != null ? $"{message}\n{ex}" : message;
		string formatted = $"[{DateTime.Now:HH:mm:ss}] [ERROR] {msg}";
		Console.WriteLine(formatted);
		LogToFile(formatted);
	}

	public void LogWarning(string message)
	{
		string formatted = $"[{DateTime.Now:HH:mm:ss}] [WARN] {message}";
		Console.WriteLine(formatted);
		LogToFile(formatted);
	}

	public string GetLogFilePath()
	{
		return Path.Combine(_logsDir, $"session_{DateTime.Now:yyyy-MM-dd}.log");
	}

	private void LogToFile(string message)
	{
		try
		{
			File.AppendAllText(GetLogFilePath(), message + Environment.NewLine);
		}
		catch { }
	}
}
