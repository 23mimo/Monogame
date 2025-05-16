using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameSample
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Ball related variables
        private Texture2D _ballTexture;
        private Vector2 _ballPosition;
        private Vector2 _ballVelocity;
        private int _ballRadius = 20;

        // Screen bounds
        private int _screenWidth;
        private int _screenHeight;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set screen size
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _screenWidth = _graphics.PreferredBackBufferWidth;
            _screenHeight = _graphics.PreferredBackBufferHeight;

            // Initialize ball position at center
            _ballPosition = new Vector2(_screenWidth / 2, _screenHeight / 2);

            // Initial velocity
            _ballVelocity = new Vector2(150f, 120f);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create a simple circle texture for the ball at runtime
            _ballTexture = CreateCircleTexture(_ballRadius, Color.CornflowerBlue);
        }

        private Texture2D CreateCircleTexture(int radius, Color color)
        {
            int diameter = radius * 2;
            Texture2D texture = new Texture2D(GraphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            float radiusSquared = radius * radius;

            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    int index = y * diameter + x;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.LengthSquared() <= radiusSquared)
                        colorData[index] = color;
                    else
                        colorData[index] = Color.Transparent;
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move ball according to velocity
            _ballPosition += _ballVelocity * delta;

            // Bounce off left or right edges
            if (_ballPosition.X - _ballRadius < 0)
            {
                _ballPosition.X = _ballRadius;
                _ballVelocity.X *= -1;
            }
            else if (_ballPosition.X + _ballRadius > _screenWidth)
            {
                _ballPosition.X = _screenWidth - _ballRadius;
                _ballVelocity.X *= -1;
            }

            // Bounce off top or bottom edges
            if (_ballPosition.Y - _ballRadius < 0)
            {
                _ballPosition.Y = _ballRadius;
                _ballVelocity.Y *= -1;
            }
            else if (_ballPosition.Y + _ballRadius > _screenHeight)
            {
                _ballPosition.Y = _screenHeight - _ballRadius;
                _ballVelocity.Y *= -1;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            // Draw the ball texture centered on its position
            _spriteBatch.Draw(_ballTexture, _ballPosition - new Vector2(_ballRadius, _ballRadius), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

