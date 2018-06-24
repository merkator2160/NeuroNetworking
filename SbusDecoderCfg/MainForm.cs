using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace SbusDecoderCfg
{
	public partial class MainForm : Form
	{
		private readonly Dictionary<Int32, ComboBox> cbBankA = new Dictionary<Int32,ComboBox>();
	    private readonly Dictionary<Int32, ComboBox> cbBankB = new Dictionary<Int32, ComboBox>();

		private readonly SerialPort _comPort;


		public MainForm()
        {
            InitializeComponent();

	        _comPort = new SerialPort();

	        CbBanksInit();
	        cbPorts.Items.Clear();
	        cbPorts.Items.AddRange(SerialPort.GetPortNames());

	        if(cbPorts.Items.Count > 0)
	        {
		        cbPorts.SelectedIndex = 0;
	        }
		}


		// HANDLERS ///////////////////////////////////////////////////////////////////////////////
        private void ChkPPMB_CheckedChanged(Object sender, EventArgs e)
        {
            if (!chkPPMB.Checked)
            {
                // Frame = 20ms
                cbFrameB.SelectedIndex = 1;
                // Channels <8
                Update_cbChannelsPWM(cbChannelsB, cbFrameB.SelectedIndex, 8);
            }
            else
            {
                // Frame = 24ms
                cbFrameB.SelectedIndex = 2;
                // Channels up to 16
                Update_cbChannelsPPM(cbChannelsB, cbFrameB.SelectedIndex, 16);
            }
            UpdateBankEnabled(cbBankB, cbChannelsB.Items.Count);
        }
        private void ChkPPMA_CheckedChanged(Object sender, EventArgs e)
        {
            if (!chkPPMA.Checked)
            {
                // Frame = 20ms
                cbFrameA.SelectedIndex = 1;
                // Channels <8
                Update_cbChannelsPWM(cbChannelsA, cbFrameA.SelectedIndex, 8);
            }
            else
            {
                // Frame = 24ms
                cbFrameA.SelectedIndex = 2;
                // Channels up to 16
                Update_cbChannelsPPM(cbChannelsA, cbFrameA.SelectedIndex, 16);
            }
            UpdateBankEnabled(cbBankA, cbChannelsA.Items.Count);
        }
        private void BtnRead_Click(Object sender, EventArgs e)
        {
            var buff = new Byte[41];
            buff[0] = 0xaa;
            buff[1] = 0x52; // R

            for (var i = 2; i < 40; i++)
                buff[i] = 0;

            buff[40] = 0x5d;

            _comPort.Open();
            _comPort.Write(buff, 0, 41);

            var cnt = 0;
            while (cnt < 40)
            {
                if (_comPort.BytesToRead > 40) 
                    cnt = _comPort.Read(buff, 0, 41);
            }
            _comPort.Close();

            chkPPMA.Checked = (buff[2] == 1);
            if (buff[3] - 4 < 7) cbFrameA.SelectedIndex = buff[3] - 4;
            if (buff[4] - 1 < 16) cbChannelsA.SelectedIndex = buff[4] - 1;

            if (buff[5] < 17) cbChA1.SelectedIndex = buff[5];
            if (buff[6] < 17) cbChA2.SelectedIndex = buff[6];
            if (buff[7] < 17) cbChA3.SelectedIndex = buff[7];
            if (buff[8] < 17) cbChA4.SelectedIndex = buff[8];
            if (buff[9] < 17) cbChA5.SelectedIndex = buff[9];
            if (buff[10] < 17) cbChA6.SelectedIndex = buff[10];
            if (buff[11] < 17) cbChA7.SelectedIndex = buff[11];
            if (buff[12] < 17) cbChA8.SelectedIndex = buff[12];
            if (buff[13] < 17) cbChA9.SelectedIndex = buff[13];
            if (buff[14] < 17) cbChA10.SelectedIndex = buff[14];
            if (buff[15] < 17) cbChA11.SelectedIndex = buff[15];
            if (buff[16] < 17) cbChA12.SelectedIndex = buff[16];
            if (buff[17] < 17) cbChA13.SelectedIndex = buff[17];
            if (buff[18] < 17) cbChA14.SelectedIndex = buff[18];
            if (buff[19] < 17) cbChA15.SelectedIndex = buff[19];
            if (buff[20] < 17) cbChA16.SelectedIndex = buff[20];

            chkPPMB.Checked = (buff[21] == 1);
            if (buff[22] - 4 < 7) cbFrameB.SelectedIndex = buff[22] - 4;
            if (buff[23] - 1 < 16) cbChannelsB.SelectedIndex = buff[23] - 1;

            if (buff[24] < 17) cbChB1.SelectedIndex = buff[24];
            if (buff[25] < 17) cbChB2.SelectedIndex = buff[25];
            if (buff[26] < 17) cbChB3.SelectedIndex = buff[26];
            if (buff[27] < 17) cbChB4.SelectedIndex = buff[27];
            if (buff[28] < 17) cbChB5.SelectedIndex = buff[28];
            if (buff[29] < 17) cbChB6.SelectedIndex = buff[29];
            if (buff[30] < 17) cbChB7.SelectedIndex = buff[30];
            if (buff[31] < 17) cbChB8.SelectedIndex = buff[31];
            if (buff[32] < 17) cbChB9.SelectedIndex = buff[32];
            if (buff[33] < 17) cbChB10.SelectedIndex = buff[33];
            if (buff[34] < 17) cbChB11.SelectedIndex = buff[34];
            if (buff[35] < 17) cbChB12.SelectedIndex = buff[35];
            if (buff[36] < 17) cbChB13.SelectedIndex = buff[36];
            if (buff[37] < 17) cbChB14.SelectedIndex = buff[37];
            if (buff[38] < 17) cbChB15.SelectedIndex = buff[38];
            if (buff[39] < 17) cbChB16.SelectedIndex = buff[39];
        }
        private void BtnWrite_Click(Object sender, EventArgs e)
        {
            var buff = new Byte[41];
            buff[0] = 0xaa;
            buff[1] = 0x57; // W
            buff[2] = Convert.ToByte(chkPPMA.Checked);
            buff[3] = (Byte)(cbFrameA.SelectedIndex + 4);
            buff[4] = (Byte)(cbChannelsA.SelectedIndex + 1);

            buff[5] = (Byte)(cbChA1.SelectedIndex);
            buff[6] = (Byte)(cbChA2.SelectedIndex);
            buff[7] = (Byte)(cbChA3.SelectedIndex);
            buff[8] = (Byte)(cbChA4.SelectedIndex);
            buff[9] = (Byte)(cbChA5.SelectedIndex);
            buff[10] = (Byte)(cbChA6.SelectedIndex);
            buff[11] = (Byte)(cbChA7.SelectedIndex);
            buff[12] = (Byte)(cbChA8.SelectedIndex);
            buff[13] = (Byte)(cbChA9.SelectedIndex);
            buff[14] = (Byte)(cbChA10.SelectedIndex);
            buff[15] = (Byte)(cbChA11.SelectedIndex);
            buff[16] = (Byte)(cbChA12.SelectedIndex);
            buff[17] = (Byte)(cbChA13.SelectedIndex);
            buff[18] = (Byte)(cbChA14.SelectedIndex);
            buff[19] = (Byte)(cbChA15.SelectedIndex);
            buff[20] = (Byte)(cbChA16.SelectedIndex);

            buff[21] = Convert.ToByte(chkPPMB.Checked);
            buff[22] = (Byte)(cbFrameB.SelectedIndex + 4);
            buff[23] = (Byte)(cbChannelsB.SelectedIndex + 1);

            buff[24] = (Byte)(cbChB1.SelectedIndex);
            buff[25] = (Byte)(cbChB2.SelectedIndex);
            buff[26] = (Byte)(cbChB3.SelectedIndex);
            buff[27] = (Byte)(cbChB4.SelectedIndex);
            buff[28] = (Byte)(cbChB5.SelectedIndex);
            buff[29] = (Byte)(cbChB6.SelectedIndex);
            buff[30] = (Byte)(cbChB7.SelectedIndex);
            buff[31] = (Byte)(cbChB8.SelectedIndex);
            buff[32] = (Byte)(cbChB9.SelectedIndex);
            buff[33] = (Byte)(cbChB10.SelectedIndex);
            buff[34] = (Byte)(cbChB11.SelectedIndex);
            buff[35] = (Byte)(cbChB12.SelectedIndex);
            buff[36] = (Byte)(cbChB13.SelectedIndex);
            buff[37] = (Byte)(cbChB14.SelectedIndex);
            buff[38] = (Byte)(cbChB15.SelectedIndex);
            buff[39] = (Byte)(cbChB16.SelectedIndex);

            buff[40] = 0x5d;

            _comPort.Open();
            _comPort.Write(buff, 0, 41);
            _comPort.Close();
        }
        private void CbFrameA_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!chkPPMA.Checked)
            {
                // Channels <8
                Update_cbChannelsPWM(cbChannelsA, cbFrameA.SelectedIndex, 8);
            }
            else
            {
                // Channels up to 16
                Update_cbChannelsPPM(cbChannelsA, cbFrameA.SelectedIndex, 16);
            }
            UpdateBankEnabled(cbBankA, cbChannelsA.Items.Count);
        }
        private void CbFrameB_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!chkPPMB.Checked)
            {
                // Channels <8
                Update_cbChannelsPWM(cbChannelsB, cbFrameB.SelectedIndex, 8);
            }
            else
            {
                // Channels up to 16
                Update_cbChannelsPPM(cbChannelsB, cbFrameB.SelectedIndex, 16);
            }
            UpdateBankEnabled(cbBankB, cbChannelsB.Items.Count);
        }
        private void CbPorts_SelectedIndexChanged(Object sender, EventArgs e)
        {
            _comPort.PortName = cbPorts.SelectedItem.ToString();
        }


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		private void CbBanksInit()
		{
			cbBankA.Clear();
			cbBankA.Add(1, cbChA1);
			cbBankA.Add(2, cbChA2);
			cbBankA.Add(3, cbChA3);
			cbBankA.Add(4, cbChA4);
			cbBankA.Add(5, cbChA5);
			cbBankA.Add(6, cbChA6);
			cbBankA.Add(7, cbChA7);
			cbBankA.Add(8, cbChA8);
			cbBankA.Add(9, cbChA9);
			cbBankA.Add(10, cbChA10);
			cbBankA.Add(11, cbChA11);
			cbBankA.Add(12, cbChA12);
			cbBankA.Add(13, cbChA13);
			cbBankA.Add(14, cbChA14);
			cbBankA.Add(15, cbChA15);
			cbBankA.Add(16, cbChA16);

			cbBankB.Clear();
			cbBankB.Add(1, cbChB1);
			cbBankB.Add(2, cbChB2);
			cbBankB.Add(3, cbChB3);
			cbBankB.Add(4, cbChB4);
			cbBankB.Add(5, cbChB5);
			cbBankB.Add(6, cbChB6);
			cbBankB.Add(7, cbChB7);
			cbBankB.Add(8, cbChB8);
			cbBankB.Add(9, cbChB9);
			cbBankB.Add(10, cbChB10);
			cbBankB.Add(11, cbChB11);
			cbBankB.Add(12, cbChB12);
			cbBankB.Add(13, cbChB13);
			cbBankB.Add(14, cbChB14);
			cbBankB.Add(15, cbChB15);
			cbBankB.Add(16, cbChB16);

			SetBankDefaultCh(cbBankA);
			SetBankDefaultCh(cbBankB);
		}
		private Int32 GetChannelsCount(Int32 FrameIndex)
		{
			var ms = (FrameIndex * 4) + 16;
			Double us = ms * 1000;
			// minus sync
			us -= 2700;
			// 2200us per channel
			var chc = Convert.ToInt32(Math.Floor(us / 2200));
			return chc;
		}
		private void Update_cbChannelsPPM(ComboBox cb, Int32 FrameIndex, Int32 maxCh)
		{
			var maxChannelsCount = GetChannelsCount(FrameIndex);
			if(maxChannelsCount > maxCh)
			{
				maxChannelsCount = maxCh;
			}

			cb.Items.Clear();
			for(var i = 1; i <= maxChannelsCount; i++)
			{
				cb.Items.Add(i);
			}

			// Default = maxChannelsCount
			cb.SelectedIndex = maxChannelsCount - 1;
		}
		private void Update_cbChannelsPWM(ComboBox cb, Int32 FrameIndex, Int32 maxCh)
		{
			var maxChannelsCount = GetChannelsCount(FrameIndex) + 1;
			if(maxChannelsCount > maxCh)
			{
				maxChannelsCount = maxCh;
			}

			cb.Items.Clear();

			for(var i = 1; i <= maxChannelsCount; i++)
			{
				cb.Items.Add(i);
			}

			// Default = maxChannelsCount
			cb.SelectedIndex = maxChannelsCount - 1;
		}
		private void UpdateBankEnabled(Dictionary<Int32, ComboBox> Bank, Int32 chCount)
		{
			foreach(var itm in Bank)
			{
				itm.Value.Enabled = itm.Key <= chCount;
			}
		}
		private void SetBankDefaultCh(Dictionary<Int32, ComboBox> Bank)
		{
			foreach(var itm in Bank)
			{
				itm.Value.SelectedIndex = itm.Key - 1;
			}
		}
	}
}