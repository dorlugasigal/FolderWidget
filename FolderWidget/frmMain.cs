using FolderWidget.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderWidget
{
    public partial class frmMain : Form
    {
        #region Properties
        #region Properties for dragging form with mouse
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        #endregion
        #region Properties for round form edges

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );
        #endregion
        private NotifyIcon m_notifyIcon;
        private MenuItem m_menuItemExit;
        private MenuItem m_menuItemRefresh;
        private MenuItem m_menuItemDarkTheme;

        Color backgroundA;
        Color backgroundB;
        Color btnHover;

        private KeyHandler ghk;

        int m_height;
        DataTable m_iconsData;
        #endregion

        public frmMain()
        {
            InitializeComponent();
            clsManageComunication.OnSendMessage += ClsManageComunication_OnSendMessage;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Right - this.Width, Screen.PrimaryScreen.Bounds.Top + 30);

            backgroundA = Color.FromArgb(227, 228, 230);
            backgroundB = Color.FromArgb(184, 222, 255);
            btnHover = Color.FromArgb(213, 238, 255);
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent
            btnDeployToProd.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.SetValue("FolderWidget", @"C:\Projects\FolderWidget\FolderWidget\bin\Debug\FolderWidget.exe");

            AddDeployToProdButton();

            InitFormFilesAndButtons();
            SetNotifyIconAndContextMenu();

            ghk = new KeyHandler(Keys.F8, this);
            ghk.Register();
        }



        #region Form Display Handling
        private void AddDeployToProdButton()
        {

            try
            {
                if (ConfigurationManager.AppSettings["DeveloperMode"] == "1")
                {
                    System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(ConfigurationManager.AppSettings["deployToProdPath"]);
                    m_height = 110;
                    btnDeployToProd.Visible = true;
                }
                else
                {
                    m_height = 80;
                    if (tblMain.RowCount == 3)
                    {
                        tblMain.RowCount = 2;
                    }
                    btnDeployToProd.Visible = false;
                }
            }
            catch (UnauthorizedAccessException)
            {
                m_height = 80;
                if (tblMain.RowCount == 3)
                {
                    tblMain.RowCount = 2;
                }
                btnDeployToProd.Visible = false;
            }
            catch (Exception ex)
            {
                clsFileManager.WriteError(ex.Message, ex.StackTrace);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {

            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, backgroundA, backgroundB, 90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void ChangeTheme()
        {

            if (m_menuItemDarkTheme.Checked)
            {
                backgroundA = Color.FromArgb(96, 96, 96);
                backgroundB = Color.FromArgb(45, 45, 45);

                topPanel.BackColor = Color.FromArgb(49, 49, 49);

                btnClose.BackColor = Color.FromArgb(49, 49, 49);

                btnDeployToProd.ForeColor = Color.Black;
                btnDeployToProd.BackColor = Color.FromArgb(255, 204, 0);
                btnHover = Color.FromArgb(112, 112, 112);


            }
            else
            {
                btnHover = Color.FromArgb(213, 238, 255);
                backgroundA = Color.FromArgb(227, 228, 230);
                backgroundB = Color.FromArgb(184, 222, 255);

                topPanel.BackColor = Color.FromArgb(130, 172, 242);

                btnClose.BackColor = Color.FromArgb(130, 172, 242);

                btnDeployToProd.BackColor = Color.FromArgb(128, 128, 255);
                btnDeployToProd.ForeColor = Color.White;
            }
        }
        private void SetNotifyIconAndContextMenu()
        {
            this.components = new Container();
            this.ContextMenu = new ContextMenu();
            this.m_menuItemExit = new MenuItem();
            this.m_menuItemRefresh = new MenuItem();
            this.m_menuItemDarkTheme = new MenuItem();

            this.ContextMenu.MenuItems.AddRange(new MenuItem[] { m_menuItemExit, m_menuItemRefresh, m_menuItemDarkTheme });

            m_menuItemRefresh.Index = 0;
            m_menuItemRefresh.Text = "&Refresh";
            m_menuItemRefresh.Click += M_menuItemRefresh_Click;

            m_menuItemDarkTheme.Index = 1;
            m_menuItemDarkTheme.Text = "&Dark";
            m_menuItemDarkTheme.Click += M_menuItemDarkTheme_Click; ;

            m_menuItemExit.Index = 2;
            m_menuItemExit.Text = "E&xit";
            m_menuItemExit.Click += m_menuItemExit_Click;


            m_notifyIcon = new NotifyIcon(this.components);
            m_notifyIcon.Icon = this.Icon;
            m_notifyIcon.ContextMenu = this.ContextMenu;
            m_notifyIcon.Text = "FolderWidget is still running";
            m_notifyIcon.Visible = true;
            m_notifyIcon.Click += M_notifyIcon_Click;
        }

        private void M_menuItemDarkTheme_Click(object sender, EventArgs e)
        {
            m_menuItemDarkTheme.Checked = m_menuItemDarkTheme.Checked == true ? false : true;
            ChangeTheme();
            InitFormFilesAndButtons();
        }

        private void ClsManageComunication_OnSendMessage(string Message)
        {
            this.Invoke((Action)(() =>
            {
                ShowAgain();
            }));

        }
        private void M_notifyIcon_Click(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Right - 300, Screen.PrimaryScreen.Bounds.Bottom -200);
            ShowAgain();
        }

        private void m_menuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void M_menuItemRefresh_Click(object sender, EventArgs e)
        {
            ShowAgain();
        }

        private void ShowAgain()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
            this.Show();
            InitFormFilesAndButtons();
        }

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.Transparent;

            if (!lblFileName.Text.Equals("Drag something"))
            {
                lblFileName.Text = string.Empty;
            }
        }
        private void Btn_MouseHover(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = btnHover;
            string paths = ((Button)sender).Tag.ToString();
            string[] parsedPaths = paths.Split(',');
            string title = string.Empty;
            if (parsedPaths.Count() > 1)
            {
                int amount = parsedPaths.Count();
                string amountStr = parsedPaths.Count().ToString();
                title = "[ " + amountStr + " Files ]";

                lblFileName.Text = title;
                return;
            }

            title = Path.GetFileNameWithoutExtension(paths);
            lblFileName.Text = title;
        }


        private void lblFileName_DoubleClick(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Right - this.Width, Screen.PrimaryScreen.Bounds.Top + 30);
        }
        #endregion



        #region Form Data Handling

        private void ClearData()
        {
            tblIcons.Controls.Clear();
            tblIcons.RowStyles.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        private void InitFormFilesAndButtons()
        {
            ClearData();
            m_iconsData = clsFileManager.GetInitData();

            this.Height = m_height + (50 * ((int)m_iconsData.Rows.Count / 3));
            this.Size = new Size(this.Width, this.Height);
            this.FormBorderStyle = FormBorderStyle.None;

            tblIcons.RowCount = (m_iconsData.Rows.Count / 3) + 1;
            tblIcons.Dock = DockStyle.Fill;
            for (int i = 0; i < tblIcons.RowCount; i++)
            {
                tblIcons.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            }
            int counter = 0;
            bool stopAndRestart = false;
            for (int row = 0; row < tblIcons.RowCount; row++)
            {
                for (int column = 0; column < tblIcons.ColumnCount; column++)
                {
                    if (m_iconsData.Rows.Count > counter)
                    {
                        string path = m_iconsData.Rows[counter]["Path"].ToString();
                        foreach (var item in path.Split(','))
                        {
                            if (!File.Exists(item) && !Directory.Exists(item))
                            {
                                stopAndRestart = true;
                            }
                        }
                        if (stopAndRestart)
                        {
                            var res = (m_iconsData.AsEnumerable().Where(dr => dr.Field<string>("Path") == path).FirstOrDefault()) as DataRow;
                            if (!string.IsNullOrEmpty(res["Icon"].ToString()))
                            {
                                clsFileManager.DeleteIcon(res["Icon"].ToString());
                            }
                            m_iconsData.Rows.Remove(res);
                            clsFileManager.ReplaceXMLFile(m_iconsData);
                            break;
                        }
                        else
                        {
                            tblIcons.Controls.Add(MakeDynamicButton(m_iconsData.Rows[counter]), column, row);
                            counter++;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
                if (stopAndRestart)
                {
                    break;
                }
            }
            if (stopAndRestart)
            {
                InitFormFilesAndButtons();
                return;
            }
            if (m_iconsData.Rows.Count == 0)
            {
                lblFileName.Text = "Drag Something";
            }
            else
            {
                lblFileName.Text = string.Empty;
            }
            //Round Form
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 10, 10));
            DoubleBuffered = true;
        }

        #endregion
        #region Make Button
        private Button MakeDynamicButton(DataRow dr)
        {
            Button btn = new Button();
            #region Button Properties
            btn.Tag = dr["Path"].ToString();
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new Size(41, 41);
            btn.Margin = new Padding(3);
            btn.TabStop = false;
            #endregion

            #region Button Events
            btn.MouseHover += Btn_MouseHover;
            btn.MouseEnter += Btn_MouseHover;
            btn.MouseLeave += Btn_MouseLeave;
            btn.Click += Btn_Click;
            #endregion

            #region Context Menu for Button
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Change Icon");
            cm.MenuItems[0].Click += ChangeIcon_Click;

            cm.MenuItems.Add("Delete");
            cm.MenuItems[1].Click += DeleteItem_Click;

            cm.MenuItems.Add("Run as Different User");
            cm.MenuItems[2].Click += StartProcessAsDifferentUser;
            btn.ContextMenu = cm;
            #endregion

            #region Button Image and ToolTip
            string strForToolTip = string.Empty;
            btn.BackgroundImage = GetIcon(dr);
            btn.BackgroundImageLayout = ImageLayout.Stretch;

            ToolTip tp = new ToolTip();
            foreach (var item in dr["Path"].ToString().Split(','))
            {
                strForToolTip = strForToolTip + "• " + item.ToString() + Environment.NewLine;
            }
            tp.SetToolTip(btn, string.IsNullOrEmpty(strForToolTip) ? "• " + dr["Path"].ToString() : strForToolTip);
            #endregion
            return btn;
        }



        private void StartProcessAsDifferentUser(object sender, EventArgs e)
        {
            string line = ((sender as MenuItem).GetContextMenu()).SourceControl.Tag.ToString();
            var rowFromTable = (m_iconsData.AsEnumerable().Where(dr => dr.Field<string>("Path") == line).FirstOrDefault()) as DataRow;
            if (string.IsNullOrEmpty(rowFromTable["User"].ToString()) || string.IsNullOrEmpty(rowFromTable["Pass"].ToString()))
            {
                frmRunAsDifferentUser frmRunAs = new frmRunAsDifferentUser(rowFromTable["Path"].ToString());
                frmRunAs.ShowDialog();
                if (frmRunAs.DialogResult == DialogResult.OK)
                {
                    rowFromTable["User"] = frmRunAs.user;
                    rowFromTable["Pass"] = frmRunAs.pass;
                }
                else
                {
                    return;
                }


                bool stopAndRefresh = false;
                string[] parsedPaths = line.Split(',');
                foreach (var path in parsedPaths)
                {
                    if (!File.Exists(path) && !Directory.Exists(path))
                    {
                        this.Visible = false;
                        MessageBox.Show("This file doesn't exists anymore");
                        this.Visible = true;

                        if (!string.IsNullOrEmpty(rowFromTable["Icon"].ToString()))
                        {
                            clsFileManager.DeleteIcon(rowFromTable["Icon"].ToString());
                        }
                        m_iconsData.Rows.Remove(rowFromTable);
                        clsFileManager.ReplaceXMLFile(m_iconsData);
                        stopAndRefresh = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(rowFromTable["User"].ToString()) && !string.IsNullOrEmpty(rowFromTable["Pass"].ToString()))
                        {
                            SecureString passSecure = new NetworkCredential("", rowFromTable["Pass"].ToString()).SecurePassword;
                            try
                            {
                                Process.Start(path, rowFromTable["User"].ToString(), passSecure, "localhost");
                            }
                            catch (Exception ex)
                            {
                                this.Visible = false;
                                MessageBox.Show("Wrong Credentials");
                                clsFileManager.WriteError(ex.Message, ex.StackTrace);
                                this.Visible = true;
                            }
                        }
                        else
                        {
                            Process.Start(path);
                        }
                    }
                    if (stopAndRefresh)
                    {
                        break;
                    }
                }
                if (stopAndRefresh)
                {
                    InitFormFilesAndButtons();
                }
            }
        }
        private void ChangeIcon_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.ico, *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.ico;*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Image img = null;
                    if (Path.GetExtension(dlg.FileName) == ".ico")
                    {
                        Icon icon = Icon.ExtractAssociatedIcon(dlg.FileName);
                        img = icon.ToBitmap();
                    }
                    else
                    {
                        img = Image.FromFile(dlg.FileName);
                    }
                    int id = clsFileManager.SaveIcon(ref m_iconsData, img);
                    string path = ((sender as MenuItem).GetContextMenu()).SourceControl.Tag.ToString();
                    var res = (m_iconsData.AsEnumerable().Where(dr => dr.Field<string>("Path") == path).FirstOrDefault()) as DataRow;
                    res["Icon"] = id;
                    clsFileManager.ReplaceXMLFile(m_iconsData);
                    this.Invoke((Action)(() =>
                    {
                        InitFormFilesAndButtons();
                    }));
                }
            }
        }

        private dynamic GetIcon(DataRow dr)
        {
            if (!string.IsNullOrEmpty(dr["Icon"].ToString()))
            {
                Image img = (new Bitmap(Image.FromFile(ConfigurationManager.AppSettings["IconPath"] + dr["Icon"].ToString() + ".png"), new Size(48, 48)));
                return img;
            }
            var fileExtension = Path.GetExtension(dr["Path"].ToString());
            dynamic icon = string.Empty;
            if (string.IsNullOrEmpty(fileExtension))
            {
                icon = Resources.folder;
                if (dr["Path"].ToString().Split(',').Count() > 1)
                {
                    icon = Resources.Layers;
                }
            }
            else
            {
                if (dr["Path"].ToString().Split(',').Count() > 1)
                {
                    icon = Resources.Layers;
                }
                else
                {
                    if (fileExtension.Equals(".lnk"))
                    {
                        icon = Resources.software;
                    }
                    else
                    {
                        if (fileExtension.Equals(".txt"))
                        {
                            icon = Resources.doc;
                        }
                        else
                        {
                            icon = Resources.Other;
                        }
                    }
                }
            }
            return icon;
        }
        #endregion

        #region Mouse Clicks

        private void Btn_Click(object sender, EventArgs e)
        {
            switch (((MouseEventArgs)e).Button)
            {
                case MouseButtons.Left:
                    string paths = ((Button)sender).Tag.ToString();
                    bool stopAndRefresh = false;
                    string[] parsedPaths = paths.Split(',');
                    var rowFromTable = (m_iconsData.AsEnumerable().Where(dr => dr.Field<string>("Path") == paths).FirstOrDefault()) as DataRow;
                    foreach (var path in parsedPaths)
                    {
                        if (!File.Exists(path) && !Directory.Exists(path))
                        {
                            this.Visible = false;
                            MessageBox.Show("This File doesnt exists anymore");
                            this.Visible = true;
                            if (!string.IsNullOrEmpty(rowFromTable["Icon"].ToString()))
                            {
                                clsFileManager.DeleteIcon(rowFromTable["Icon"].ToString());
                            }
                            m_iconsData.Rows.Remove(rowFromTable);
                            clsFileManager.ReplaceXMLFile(m_iconsData);
                            stopAndRefresh = true;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(rowFromTable["User"].ToString()) && !string.IsNullOrEmpty(rowFromTable["Pass"].ToString()))
                            {
                                SecureString passSecure = new NetworkCredential("", rowFromTable["Pass"].ToString()).SecurePassword;
                                try
                                {
                                    Process.Start(path, rowFromTable["User"].ToString(), passSecure, "localhost");
                                }
                                catch (Exception ex)
                                {
                                    this.Visible = false;
                                    MessageBox.Show("Wrong Credentials");
                                    clsFileManager.WriteError(ex.Message, ex.StackTrace);
                                    this.Visible = true;
                                }
                            }
                            else
                            {
                                Process.Start(path);
                            }
                        }
                        if (stopAndRefresh)
                        {
                            break;
                        }
                    }
                    if (stopAndRefresh)
                    {
                        InitFormFilesAndButtons();
                        break;
                    }
                    break;
                case MouseButtons.Right:
                    ((Button)sender).ContextMenu.Show(this, new Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y));
                    break;
                case MouseButtons.Middle:
                    this.Location = new Point(Screen.PrimaryScreen.Bounds.Right - this.Width, Screen.PrimaryScreen.Bounds.Top + 30);
                    break;
                default:
                    break;
            }
        }


        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void DragDropForm(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            List<string> finalList = new List<string>();
            foreach (string file in fileList)
            {
                string sourcePath = clsFileManager.GetShortcutTargetFile(file);
                sourcePath = string.IsNullOrEmpty(Path.GetExtension(sourcePath)) ? file : sourcePath;
                if (!string.IsNullOrEmpty(sourcePath))
                {
                    finalList.Add(sourcePath);
                }
                else
                {
                    finalList.Add(file);
                }
            }
            string iconPath = string.Empty;
            dynamic id = null;
            if (fileList.Length == 1 && !clsFileManager.IsPathDirectory(fileList[0]))
            {
                Icon icon = null;
                try
                {
                    icon = Icon.ExtractAssociatedIcon(fileList[0]);
                }
                catch (Exception ex)
                {
                    icon = this.Icon;
                    clsFileManager.WriteError(ex.Message, ex.StackTrace);
                }

                id = clsFileManager.SaveIcon(ref m_iconsData, icon.ToBitmap());
            }
            string paths = String.Join(",", finalList);
            if (id != null)
            {
                m_iconsData.Rows.Add(paths, id, "", "");
            }
            else
            {
                m_iconsData.Rows.Add(paths, "", "", "");
            }
            clsFileManager.ReplaceXMLFile(m_iconsData);
            this.Invoke((Action)(() =>
            {
                InitFormFilesAndButtons();
            }));

        }
        private void DeleteItem_Click(object sender, EventArgs e)
        {
            string lineToDelete = ((sender as MenuItem).GetContextMenu()).SourceControl.Tag.ToString();
            var rowToDelete = (m_iconsData.AsEnumerable().Where(dr => dr.Field<string>("Path") == lineToDelete).FirstOrDefault()) as DataRow;
            if (!string.IsNullOrEmpty(rowToDelete["Icon"].ToString()))
            {
                clsFileManager.DeleteIcon(rowToDelete["Icon"].ToString());
            }
            m_iconsData.Rows.Remove(rowToDelete);
            clsFileManager.ReplaceXMLFile(m_iconsData);
            this.Invoke((Action)(() =>
            {
                InitFormFilesAndButtons();
            }));
        }
        private void DragEnterForm(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        private void btnDeployToProd_Click(object sender, EventArgs e)
        {
            List<string> files = Directory.EnumerateFiles(ConfigurationManager.AppSettings["deployToQAPath"], "*.*", SearchOption.AllDirectories)
                                .Where(n => Path.GetExtension(n) == ".zip").ToList();
            if (files.Count() == 0)
            {
                this.Visible = false;
                MessageBox.Show("nothing to deploy");
                this.Visible = true;
                Process.Start(ConfigurationManager.AppSettings["deployToQAPath"]);
                Process.Start(ConfigurationManager.AppSettings["deployToProdPath"]);
                return;
            }
            string filesString = Environment.NewLine;

            foreach (var fileToDeploy in files)
            {
                filesString = filesString + "• " + Path.GetFileName(fileToDeploy.ToString()) + Environment.NewLine;
            }
            this.Visible = false;
            if (MessageBox.Show("Are you sure you want to deploy these files:" + filesString, "FolderWidget", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    foreach (var fileToDeploy in files)
                    {
                        string deployedPath = fileToDeploy.Replace(ConfigurationManager.AppSettings["deployToQAPath"], ConfigurationManager.AppSettings["deployToProdPath"]);
                        File.Copy(fileToDeploy, deployedPath, true);
                    }
                    MessageBox.Show("Files Deployed successfully");
                    clsFileManager.WriteError("new Deployment", filesString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("an error occured");
                    clsFileManager.WriteError(ex.Message, ex.StackTrace);
                    Process.Start(ConfigurationManager.AppSettings["deployToQAPath"]);
                    Process.Start(ConfigurationManager.AppSettings["deployToProdPath"]);
                }
            }
            this.Visible = true;
        }

        #endregion

        #region KeyHooksFunctions
        private void HandleHotkey()
        {
            this.Visible = !this.Visible;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }

}
