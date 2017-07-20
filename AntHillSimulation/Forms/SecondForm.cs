using AntHillSimulation.Core.Config;
using Assets.Icons;
using GalaSoft.MvvmLight.Messaging;
using System.Drawing;
using System.Windows.Forms;

namespace AntHillSimulation.Forms
{
    internal partial class SecondForm : Form
    {
        private readonly ApplicationConfig _config;
        private readonly IMessenger _communicationBus;


        public SecondForm(ApplicationConfig config, IMessenger communicationBus)
        {
            InitializeComponent();

            _config = config;
            _communicationBus = communicationBus;

            Text = $"{config.Name} - Second form";
            Icon = Icons.ResourceManager.GetObject(config.IconName) as Icon;
        }
    }
}