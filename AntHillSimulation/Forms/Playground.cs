using System.Drawing;
using System.Windows.Forms;
using AntHillSimulation.Core.Config;
using Assets.Icons;

namespace AntHillSimulation.Forms
{
    internal partial class Playground : Form
    {
        private readonly ApplicationConfig _config;


        public Playground(ApplicationConfig config)
        {
            InitializeComponent();
            _config = config;

            Text = config.Name;
            Icon = Icons.ResourceManager.GetObject(config.IconName) as Icon;
        }
    }
}