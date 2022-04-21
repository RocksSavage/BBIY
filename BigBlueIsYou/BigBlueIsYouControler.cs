using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using CS5410.Input;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CS5410
{
    public class ECSBigBlueIsYouControler : Game
    {
        public GraphicsDeviceManager m_graphics;
        private IGameState m_currentState;
        private KeyboardInput m_inputKeyboard;
        private GameStateEnum m_nextStateEnum = GameStateEnum.MainMenu;
        private List<int> m_level = new List<int>();
        private Dictionary<GameStateEnum, IGameState> m_states;
        public List<Keys> m_controls = new List<Keys> {Keys.Up, Keys.Down, Keys.Left, Keys.Right};
        public List<Keys> controls = new List<Keys>();
        // private bool saving = false;   
        private bool loading = false;   

        public ECSBigBlueIsYouControler()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            m_inputKeyboard = new KeyboardInput();
            IsMouseVisible = true;
            m_level.Add(1);
        }

        protected override void Initialize()
        {
            // Set window size preferences
            m_graphics.IsFullScreen = false;
            m_graphics.PreferredBackBufferWidth = 1080;
            m_graphics.PreferredBackBufferHeight = 720;
            // m_graphics.PreferredBackBufferWidth = 1920;
            // m_graphics.PreferredBackBufferHeight = 1080;
            // m_graphics.PreferredBackBufferWidth = 2560;
            // m_graphics.PreferredBackBufferHeight = 1440;

            m_graphics.ApplyChanges();

            // Create all the game states here
            m_states = new Dictionary<GameStateEnum, IGameState>();
            m_states.Add(GameStateEnum.MainMenu, new MainMenuView());
            m_states.Add(GameStateEnum.GamePlay, new GamePlayView());
            m_states.Add(GameStateEnum.Controls, new ControlsView());
            m_states.Add(GameStateEnum.Credits, new CreditsView());
            m_states.Add(GameStateEnum.YouWin, new YouWinView());

            // We are starting with the main menu
            m_currentState = m_states[GameStateEnum.MainMenu];

            loadConfigurableControls();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Give all game states a chance to load their content
            // Console.WriteLine(m_controls[0] == Keys.Up);
            // Console.WriteLine("Level" + m_level);
            foreach (var item in m_states)
            {
                item.Value.initialize(this.GraphicsDevice, m_graphics, m_inputKeyboard, m_controls, m_level);
                item.Value.loadContent(this.Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            m_nextStateEnum = m_currentState.processInput(gameTime);

            // // Special case for exiting the game
            if (m_nextStateEnum == GameStateEnum.Exit)
            {
                Exit();
            }

            m_currentState.update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            m_currentState.render(gameTime);

            if (m_currentState != m_states[m_nextStateEnum])
            {
                m_currentState = m_states[m_nextStateEnum];
                m_currentState.initializeSession();
            }
            

            base.Draw(gameTime);
        }
        private void loadConfigurableControls()
        {
            lock (this)
            {
                if (!this.loading)
                {
                    this.loading = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    finalizeLoadAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }
        private async Task finalizeLoadAsync()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        if (storage.FileExists("ConfigurableControls.xml"))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile("ConfigurableControls.xml", FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(List<Keys>));
                                    // Console.WriteLine("look here" + (m_controls[0] == Keys.Up));
                                    m_controls = (List<Keys>)mySerializer.Deserialize(fs);
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                        Console.WriteLine("-! Isolated Storage Exception");
                    }
                }

                this.loading = false;
            });
        }
    }
}
