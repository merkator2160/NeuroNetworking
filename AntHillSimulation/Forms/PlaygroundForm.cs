using System.Drawing;
using System.Windows.Forms;
using AntHillSimulation.Core;
using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger;
using AntHillSimulation.Core.Messenger.Enums;
using Assets.Icons;

namespace AntHillSimulation.Forms
{
    internal partial class PlaygroundForm : Form
    {
        private readonly ApplicationConfig _config;
        private readonly ICommunicationBus _communicationbus;

        
        public PlaygroundForm(ApplicationConfig config, ICommunicationBus communicationbus)
        {
            InitializeComponent();

            _config = config;
            _communicationbus = communicationbus;
            
            Renderer = new ImageRenderer(Display);

            Text = $"{config.Name} - Playground";
            Icon = Icons.ResourceManager.GetObject(config.IconName) as Icon;
        }


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        public ImageRenderer Renderer { get; set; }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        private void Playground_KeyDown(object sender, KeyEventArgs e)
        {
            _communicationbus.Send(Buses.PlaygroundFormKeyDown.ToString(), e);
        }
    }
}