using AntHillSimulation.Core;
using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger.Messages;
using Assets.Icons;
using GalaSoft.MvvmLight.Messaging;
using System.Drawing;
using System.Windows.Forms;

namespace AntHillSimulation.Forms
{
    internal partial class PlaygroundForm : Form
    {
        private readonly ApplicationConfig _config;
        private readonly IMessenger _messanger;


        public PlaygroundForm(ApplicationConfig config, IMessenger communicationBus)
        {
            InitializeComponent();

            _config = config;
            _messanger = communicationBus;

            Renderer = new ImageRenderer(Display);

            Text = $"{_config.AppName} - Playground";
            Icon = Icons.ResourceManager.GetObject(config.IconName) as Icon;
        }


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        public ImageRenderer Renderer { get; set; }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        private void Playground_KeyDown(object sender, KeyEventArgs e)
        {
            _messanger.Send(new PlaygroundKeyDownMessage
            {
                Args = e
            });
        }
    }
}