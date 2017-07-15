using AntHillSimulation.Models;
using System.Windows.Forms;

namespace AntHillSimulation.Forms
{
    public partial class Playground : Form
    {
        private readonly PlaygroundConfig _config;


        public Playground(PlaygroundConfig config)
        {
            InitializeComponent();
            _config = config;

            Text = config.Title;
            Icon = config.Icon;
        }
    }
}