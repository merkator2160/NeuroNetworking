using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger;
using Assets.Icons;

namespace AntHillSimulation.Forms
{
    internal partial class SecondForm : Form
    {
        private readonly ApplicationConfig _config;
        private readonly ICommunicationBus _communicationBus;


        public SecondForm(ApplicationConfig config, ICommunicationBus communicationBus)
        {
            InitializeComponent();

            _config = config;
            _communicationBus = communicationBus;

            Text = $"{config.Name} - Second form";
            Icon = Icons.ResourceManager.GetObject(config.IconName) as Icon;
        }
    }
}