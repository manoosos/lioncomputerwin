using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LionWin.Properties;

namespace LionWin
{
    public partial class frmMemoryAddressInput : Form
    {
        private Settings settings = Settings.Default;
        public bool cancel = true;

        public frmMemoryAddressInput()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveAndExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMemoryAddressInput_Load(object sender, EventArgs e)
        {
            cmbInput.Items.Clear();
            cmbInput.Text = settings.MemoryLoadAddress;

            if (settings.MemoryAddressComboCollection != null)
                cmbInput.Items.AddRange(settings.MemoryAddressComboCollection.Split(new char[] { ',' }));
            cmbInput.Items.Remove("");
            cancel = true;
        }

        private void cmbInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SaveAndExit();
            }
        }

        private void SaveAndExit()
        {
            if (!string.IsNullOrEmpty(cmbInput.Text.Trim()))
            {
                cmbInput.Text = cmbInput.Text.Trim();
                if (!cmbInput.Items.Contains(cmbInput.Text))
                    cmbInput.Items.Add(cmbInput.Text);
                settings.MemoryLoadAddress = cmbInput.Text;
                ArrayList arraylist = new ArrayList(cmbInput.Items);
                arraylist.Sort();
                settings.MemoryAddressComboCollection = string.Join(",", arraylist.ToArray());
                settings.Save();
                cancel = false;
            }
            this.Close();
        }
    }
}
