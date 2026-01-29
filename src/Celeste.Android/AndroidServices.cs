using Android.Content;
using Android.OS;
using Android.App;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using Celeste.Core;
using AndroidEnvironment = Android.OS.Environment;

namespace Celeste.Android;

/// <summary>
/// Implementação de plataforma para Android
/// </summary>
public class AndroidPlatformService : IPlatformService
{
	private readonly Context _context;
	private bool _fullscreen = true;
	private DisplayOrientation _orientation = DisplayOrientation.Portrait | DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

	public AndroidPlatformService(Context context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public string GetSavesDirectory()
	{
		string saveDir = Path.Combine(GetBaseDirectory(), "Saves");
		Directory.CreateDirectory(saveDir);
		return saveDir;
	}

	public string GetAppDataPath()
	{
		return GetBaseDirectory();
	}

	public string GetLogsDirectory()
	{
		string logsDir = Path.Combine(GetBaseDirectory(), "Logs");
		Directory.CreateDirectory(logsDir);
		return logsDir;
	}

	public void RequestFullscreen(bool fullscreen)
	{
		_fullscreen = fullscreen;
		// Android Activity já está em fullscreen por padrão
	}

	public bool IsFullscreen()
	{
		return _fullscreen;
	}

	public void SetDisplayOrientation(DisplayOrientation orientation)
	{
		_orientation = orientation;
		// MonoGame handled via DisplayOrientation
	}

	private string GetBaseDirectory()
	{
		var filesDir = _context.GetExternalFilesDir(null);
		return filesDir?.AbsolutePath ?? _context.FilesDir.AbsolutePath;
	}
}

/// <summary>
/// LogSystem para Android (persiste em app-specific storage)
/// </summary>
public class AndroidLogSystem : ILogSystem
{
	private readonly string _logsDir;
	private readonly Context _context;

	public AndroidLogSystem(Context context)
	{
		_context = context;
		_logsDir = Path.Combine(
			context.GetExternalFilesDir(null)?.AbsolutePath ?? context.FilesDir.AbsolutePath,
			"Logs");
		Directory.CreateDirectory(_logsDir);
		LogToFile($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] === CELESTE ANDROID LOG START ===");
		CaptureUncaughtExceptions();
	}

	public void Log(string message)
	{
		string formatted = $"[{DateTime.Now:HH:mm:ss}] [INFO] {message}";
		LogToFile(formatted);
	}

	public void LogError(string message, Exception ex = null)
	{
		string msg = ex != null ? $"{message}\n{ex}" : message;
		string formatted = $"[{DateTime.Now:HH:mm:ss}] [ERROR] {msg}";
		LogToFile(formatted);
	}

	public void LogWarning(string message)
	{
		string formatted = $"[{DateTime.Now:HH:mm:ss}] [WARN] {message}";
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
			File.AppendAllText(GetLogFilePath(), message + System.Environment.NewLine);
		}
		catch { }
	}

	private void CaptureUncaughtExceptions()
	{
		AppDomain.CurrentDomain.UnhandledException += (s, e) =>
		{
			Exception ex = (Exception)e.ExceptionObject;
			LogCrash("UnhandledException", ex);
		};
	}

	private void LogCrash(string type, Exception ex)
	{
		try
		{
			string crashFile = Path.Combine(_logsDir, $"crash_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");
			string crashInfo = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] CRASH ({type})\n{ex}\n";
			File.WriteAllText(crashFile, crashInfo);
		}
		catch { }
	}
}

