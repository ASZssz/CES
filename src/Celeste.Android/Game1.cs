using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Celeste.Core;
using Celeste.Core.Input;
using Celeste.Core.Data;
using MonoGameDisplayOrientation = Microsoft.Xna.Framework.DisplayOrientation;

namespace Celeste.Android;

public class Game1 : Game
{
	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;
	private InputManager _inputManager;
	private SaveDataManager _saveDataManager;

	public Game1()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = false;

		// Fullscreen nativo Android
		_graphics.IsFullScreen = true;
		_graphics.SupportedOrientations = MonoGameDisplayOrientation.Portrait | MonoGameDisplayOrientation.LandscapeLeft | MonoGameDisplayOrientation.LandscapeRight;
	}

	protected override void Initialize()
	{
		base.Initialize();
		ServiceLocator.LogSystem?.Log("Game1.Initialize() called (Android)");

		// Inicializar InputManager para Android (com touch)
		_inputManager = new InputManager(ServiceLocator.LogSystem, isAndroid: true);
		ServiceLocator.LogSystem?.Log("InputManager inicializado com Touch");

		// Inicializar SaveDataManager
		_saveDataManager = new SaveDataManager(ServiceLocator.PlatformService, ServiceLocator.LogSystem);
		ServiceLocator.LogSystem?.Log("SaveDataManager inicializado");
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		ServiceLocator.LogSystem?.Log("Game1.LoadContent() called (Android)");
	}

	protected override void Update(GameTime gameTime)
	{
		// Voltar ao pressionar botão Back do dispositivo ou ESC
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		// Atualizar entrada
		_inputManager.Update();

		// Log de entrada para debug
		if (_inputManager.IsJumpPressed)
		{
			ServiceLocator.LogSystem?.Log("[Input] Jump pressionado");
		}

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();

		// Renderizar botões virtuais em debug
		var touchInput = _inputManager.GetTouchInput();
		if (touchInput != null)
		{
			foreach (var button in touchInput.GetButtons())
			{
				_spriteBatch.Draw(
					texture: new Texture2D(GraphicsDevice, 1, 1),
					destinationRectangle: button.Bounds,
					color: Color.LimeGreen * 0.3f
				);
			}
		}

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
