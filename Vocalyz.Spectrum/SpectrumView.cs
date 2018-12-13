using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vocalyz.Midi;
using Vocalyz.Music;
using Vocalyz.Spectrum.Graphics;
using Vocalyz.Spectrum.Inputs;

namespace Vocalyz.Spectrum
{
    public class SpectrumView : Game
    {
        public const string FILE = "song.wav";

        public const int WIDOW_WIDTH = 1300;
        public const int WINDOW_HEIGHT = 800;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        DrawableSpectrum spectrum;
        bool m_paused;

        public SpectrumView()
        {

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = WIDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Content.RootDirectory = "Content";
            m_paused = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
         
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            NotesManager.Initialize();
            NotesCorrelationManager.Initialize();
            Debug.Initialize(spriteBatch, Content);

            KeyboardManager.OnKeyPressed += OnKeyPressed;

            spectrum = new DrawableSpectrum(FILE);
            spectrum.Start();
        }

        private void OnKeyPressed(Keys obj)
        {
            if (obj == Keys.Space)
            {
                if (!m_paused)
                {
                    
                    spectrum.Pause();
                    m_paused = true;

                }
                else
                {
                    spectrum.Start();
                    m_paused = false;
                }
            }
            if (obj == Keys.Back)
            {
                spectrum.Dispose();
                spectrum = new DrawableSpectrum(FILE);
                spectrum.Start();
            }
        }

        protected override void UnloadContent()
        {
            spectrum.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Debug.SpriteBatch.Begin();
            spectrum?.Draw();
            Debug.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
