using Celeste.Core;
using Celeste.Desktop;

// Initialize platform and logging
ServiceLocator.RegisterPlatformService(new DesktopPlatformService());
ServiceLocator.RegisterLogSystem(new DesktopLogSystem());

ServiceLocator.LogSystem.Log("=== CELESTE DESKTOP START ===");

using var game = new Game1();
game.Run();

ServiceLocator.LogSystem.Log("=== CELESTE DESKTOP END ===");
