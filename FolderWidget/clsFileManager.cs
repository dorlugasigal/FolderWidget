using Shell32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderWidget
{
    static class clsFileManager
    {
        static public bool CreateIfDoesntExists()
        {
            CreateFolder();
            return CreateFile();
        }
        static public DataTable GetAllLines()
        {
            try
            {
                if (File.Exists(ConfigurationManager.AppSettings["mainPath"]))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(ConfigurationManager.AppSettings["mainPath"]);
                    return ds.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                WriteError("GetAllLines: " + ex.Message, ex.StackTrace);
                return null;
            }
        }
        static public DataTable GetInitData()
        {
            CreateIfDoesntExists();
            return GetAllLines();
        }
        static private bool CreateFile()
        {
            try
            {
                if (!File.Exists(ConfigurationManager.AppSettings["mainPath"]))
                {
                    DataTable dt = new DataTable("icons");
                    dt.Columns.Add("Path");
                    dt.Columns.Add("Icon");
                    dt.Columns.Add("User");
                    dt.Columns.Add("Pass"); //should encrypt
                    dt.WriteXmlSchema(ConfigurationManager.AppSettings["mainPath"]);
                    WriteError("Added xml File", ConfigurationManager.AppSettings["mainPath"]);
                    File.SetAttributes(ConfigurationManager.AppSettings["mainPath"], File.GetAttributes(ConfigurationManager.AppSettings["mainPath"]) | FileAttributes.Hidden);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                WriteError("Create FIle: " + ex.Message, ex.StackTrace);
                return false;
            }
        }
        static private bool CreateFolder()
        {
            try
            {
                string folderPath = Path.GetDirectoryName(ConfigurationManager.AppSettings["mainPath"]);
                if (!Directory.Exists(folderPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(folderPath);
                    WriteError("Added Folder", folderPath);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                WriteError(ex.Message, ex.StackTrace);
                return false;
            }
        }
        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            try
            {
                string pathOnly = Path.GetDirectoryName(shortcutFilename);
                string fileNameOnly = Path.GetFileName(shortcutFilename);
                Shell shell = new Shell();
                Folder folder = shell.NameSpace(pathOnly);
                FolderItem folderItem = folder.ParseName(fileNameOnly);
                if (folderItem != null)
                {
                    ShellLinkObject link = (ShellLinkObject)folderItem.GetLink;
                    return link.Path;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                WriteError("GetShortcutTargetFile: " + ex.Message, ex.StackTrace);
                return string.Empty;
            }
        }
        static public void ReplaceXMLFile(DataTable dt)
        {
            try
            {
                File.Delete(ConfigurationManager.AppSettings["mainPath"]);
                if (dt.Rows.Count > 0)
                {
                    dt.WriteXml(ConfigurationManager.AppSettings["mainPath"]);
                }
                else
                {
                    dt.WriteXmlSchema(ConfigurationManager.AppSettings["mainPath"]);
                }
                WriteError("Xml file updated", ConfigurationManager.AppSettings["mainPath"]);
                File.SetAttributes(ConfigurationManager.AppSettings["mainPath"], File.GetAttributes(ConfigurationManager.AppSettings["mainPath"]) | FileAttributes.Hidden);
            }
            catch (Exception ex)
            {
                WriteError("ReplaceXMLFile: " + ex.Message, ex.StackTrace);
            }
        }
        static public int SaveIcon(ref DataTable dt, Image icon)
        {
            try
            {
                int max = 0;
                try
                {
                    max = Convert.ToInt32(dt.AsEnumerable().Max(row => row["icon"]));
                }
                catch (Exception)
                {
                    max = 0;
                }
                if (!Directory.Exists(ConfigurationManager.AppSettings["iconPath"]))
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["iconPath"]);
                }
                icon.Save(ConfigurationManager.AppSettings["iconPath"] + (max + 1) + ".png");
                WriteError("icon added", ConfigurationManager.AppSettings["iconPath"] + (max + 1) + ".png");
                return max + 1;
            }
            catch (Exception ex)
            {
                WriteError("SaveIcon: " + ex.Message, ex.StackTrace);
                return -1;
            }
        }
        static public bool IsPathDirectory(string path)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                WriteError("IsPathDirectory: " + ex.Message, ex.StackTrace);
                return false;
            }
        }

        internal static void DeleteIcon(string ico)
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                File.Delete(ConfigurationManager.AppSettings["iconPath"] + ico + ".png");
                WriteError("Deleted Icon: ", ConfigurationManager.AppSettings["iconPath"] + ico + ".png");
            }
            catch (Exception ex)
            {
                WriteError("DeleteIcon: " + ex.Message, ex.StackTrace);
            }
        }
        public static void WriteError(string message, string stackTrace)
        {
            try
            {
                string msg = Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine + "Message: " + message + Environment.NewLine + "StackTrace: " + stackTrace + Environment.NewLine + Environment.NewLine + "───────────────────────────────────────────────────────────────────────────────────────────" + Environment.NewLine;
                File.AppendAllText(ConfigurationManager.AppSettings["errorPath"], msg);
                if ((File.GetAttributes(ConfigurationManager.AppSettings["errorPath"]) & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    File.SetAttributes(ConfigurationManager.AppSettings["errorPath"], File.GetAttributes(ConfigurationManager.AppSettings["errorPath"]) | FileAttributes.Hidden);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("התרחשה שגיאה לא צפויה", "התרחשה שגיאה לא צפויה", MessageBoxButtons.OK);
            }
        }
    }
}
