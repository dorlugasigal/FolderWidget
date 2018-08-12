using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderWidget
{
    public partial class frmRunAsDifferentUser : Form
    {
        public string user;
        public string pass;
        public frmRunAsDifferentUser(string appName)
        {
            InitializeComponent();
            lblName.Text += appName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.user = txtUser.Text;
            this.pass = txtPass.Text;
            DialogResult = DialogResult.No;
        }
    }
}
