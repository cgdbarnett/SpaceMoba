using Microsoft.Xna.Framework;

//using FairyGUI;

//using SpaceMobaClient.Content.Gui;
using SpaceMobaClient.Systems.Scenes;

namespace SpaceMobaClient.Content.Scenes
{
    /// <summary>
    /// JUST MADE THIS TO MAKE SURE TIMING WAS WORKING - TO BE IMPLEMENTED.
    /// </summary>
    public class MainMenu : IScene
    {
        private readonly int Id;

        public MainMenu(int id)
        {
            Id = id;

            //SpaceMobaClientBinder.BindAll();
        }

        public void Draw(GameTime gameTime)
        {
            GameClient.GetGameClient().GetGraphicsDevice().Clear(Color.Black);
        }

        public int GetId()
        {
            return Id;
        }

        public void Load()
        {
            //UIPackage.AddPackage("Content/SpaceMobaClient");
            //GComponent view = UIPackage.CreateObject("SpaceMobaClient", "SplashScreen").asCom;
            //GRoot.inst.AddChild(view);
        }

        public void Unload()
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
