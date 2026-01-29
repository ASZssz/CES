#pragma warning disable
using System;
using Microsoft.Xna.Framework;

namespace Monocle
{
	/// <summary>
	/// Interface para log do sistema
	/// </summary>
	public interface ILogSystem
	{
		void Log(string message);
		void LogError(string message, Exception ex = null);
		void LogWarning(string message);
		string GetLogFilePath();
	}

	/// <summary>
	/// Interface para serviços de plataforma
	/// </summary>
	public interface IPlatformService
	{
		string GetSavesDirectory();
		string GetLogsDirectory();
		void RequestFullscreen(bool fullscreen);
		bool IsFullscreen();
		void SetDisplayOrientation(DisplayOrientation orientation);
	}

	/// <summary>
	/// Service locator para injeção de dependências
	/// </summary>
	public static class ServiceLocator
	{
		private static ILogSystem logSystem;
		private static IPlatformService platformService;

		public static void RegisterLogSystem(ILogSystem system)
		{
			logSystem = system ?? throw new ArgumentNullException(nameof(system));
		}

		public static void RegisterPlatformService(IPlatformService service)
		{
			platformService = service ?? throw new ArgumentNullException(nameof(service));
		}

		public static ILogSystem LogSystem => logSystem ?? throw new InvalidOperationException("LogSystem not registered");
		public static IPlatformService PlatformService => platformService ?? throw new InvalidOperationException("PlatformService not registered");
	}
}
#pragma warning restore
