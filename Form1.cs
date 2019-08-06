using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;

using System.IO;

namespace SATAPortTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //添加拉鈕
            contentBox.ScrollBars = ScrollBars.Vertical;

            string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            IniFile ini = new IniFile(str + "\\SATATest.ini");
            if (ini.Read("SATA1") == "1") { checkBox1.Checked = true; } else { checkBox1.Checked = false; }
            if (ini.Read("SATA2") == "1") { checkBox2.Checked = true; } else { checkBox2.Checked = false; }
            if (ini.Read("SATA3") == "1") { checkBox3.Checked = true; } else { checkBox3.Checked = false; }
            if (ini.Read("SATA4") == "1") { checkBox5.Checked = true; } else { checkBox5.Checked = false; }
            if (ini.Read("SATA5") == "1") { checkBox4.Checked = true; } else { checkBox4.Checked = false; }
            if (ini.Read("SATA6") == "1") { checkBox6.Checked = true; } else { checkBox6.Checked = false; }
            if (ini.Read("AUTO") == "1")
            {
                checkBox7.Checked = true;
                contentBox.AppendText("   [Auto Test]\r\n");
                Test_Click(sender, e);
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string str1 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            IniFile ini = new IniFile(str1 + "\\SATATest.ini");
            if (checkBox1.Checked == true) { ini.Write("SATA1", "1"); } else { ini.Write("SATA1", "0"); }
            if (checkBox2.Checked == true) { ini.Write("SATA2", "1"); } else { ini.Write("SATA2", "0"); }
            if (checkBox3.Checked == true) { ini.Write("SATA3", "1"); } else { ini.Write("SATA3", "0"); }
            if (checkBox5.Checked == true) { ini.Write("SATA4", "1"); } else { ini.Write("SATA4", "0"); }
            if (checkBox4.Checked == true) { ini.Write("SATA5", "1"); } else { ini.Write("SATA5", "0"); }
            if (checkBox6.Checked == true) { ini.Write("SATA6", "1"); } else { ini.Write("SATA6", "0"); }
            if (checkBox7.Checked == true) { ini.Write("AUTO", "1"); } else { ini.Write("AUTO", "0"); }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            contentBox.Text = "";
        }

        private void Scan_Click(object sender, EventArgs e)
        {

            CleanAll();

            DiskName();
            
            contentBox.AppendText("Total Number of Serial ATA Storage: " + GlobalVarable.SataPortCount + "\r\n");
        }

        private void Test_Click(object sender, EventArgs e)
        {


            DiskName();

            CleanPicBox();

            contentBox.AppendText("   [Test Start]\r\n");

            CreateFile_textBox_added();

            GlobalVarable.log_flag = 0;

            //Test whether checkBox check or not
            int[] CheckBoxBool = new int[7];
            if (checkBox1.Checked) { CheckBoxBool[1] = 1; } else { CheckBoxBool[1] = 0; }
            if (checkBox2.Checked) { CheckBoxBool[2] = 1; } else { CheckBoxBool[2] = 0; }
            if (checkBox3.Checked) { CheckBoxBool[3] = 1; } else { CheckBoxBool[3] = 0; }
            if (checkBox4.Checked) { CheckBoxBool[4] = 1; } else { CheckBoxBool[4] = 0; }
            if (checkBox5.Checked) { CheckBoxBool[5] = 1; } else { CheckBoxBool[5] = 0; }
            if (checkBox6.Checked) { CheckBoxBool[6] = 1; } else { CheckBoxBool[6] = 0; }

            //測試Error Code 是否返回0(正常)
            for (int i = 1; i <= 6; i++)
            {
                //如果CheckBox有被勾選 
                if (CheckBoxBool[i] == 1)
                {
                    //Test 預設值為錯誤
                    bool test = false;
                    string str;
                    switch (i)
                    {

                        case 1: str = textBox7.Text.ToString(); if (str == "0") { test = true; } break;
                        case 2: str = textBox8.Text.ToString(); if (str == "0") { test = true; } break;
                        case 3: str = textBox9.Text.ToString(); if (str == "0") { test = true; } break;
                        case 4: str = textBox10.Text.ToString(); if (str == "0") { test = true; } break;
                        case 5: str = textBox11.Text.ToString(); if (str == "0") { test = true; } break;
                        case 6: str = textBox12.Text.ToString(); if (str == "0") { test = true; } break;
                    }

                    //Test為是 更改圖片為成功
                    if (test == true)
                    {
                        switch (i)
                        {
                            case 1: pictureBox1.Image = Properties.Resources.bmp00006; break;
                            case 2: pictureBox2.Image = Properties.Resources.bmp00006; break;
                            case 3: pictureBox3.Image = Properties.Resources.bmp00006; break;
                            case 4: pictureBox4.Image = Properties.Resources.bmp00006; break;
                            case 5: pictureBox5.Image = Properties.Resources.bmp00006; break;
                            case 6: pictureBox6.Image = Properties.Resources.bmp00006; break;

                        }
                    }
                    //如為否 則更改圖片為失敗
                    else
                    {
                        switch (i)
                        {
                            case 1: pictureBox1.Image = Properties.Resources.bmp00005; GlobalVarable.log_flag = 1; break;
                            case 2: pictureBox2.Image = Properties.Resources.bmp00005; GlobalVarable.log_flag = 1; break;
                            case 3: pictureBox3.Image = Properties.Resources.bmp00005; GlobalVarable.log_flag = 1; break;
                            case 4: pictureBox4.Image = Properties.Resources.bmp00005; GlobalVarable.log_flag = 1; break;
                            case 5: pictureBox5.Image = Properties.Resources.bmp00005; GlobalVarable.log_flag = 1; break;
                            case 6: pictureBox6.Image = Properties.Resources.bmp00005; GlobalVarable.log_flag = 1; break;

                        }
                    }
                }
            }
            int flag_testcheck = 0;

            //add Time
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            contentBox.AppendText("     Time :" + date + "\r\n");

            //add SATA PORT Each result
            if (checkBox1.Checked == true)
            {
                contentBox.AppendText("     Copy File Test\r\n");
                if (textBox7.Text == "0") { contentBox.AppendText("     SATA Port #01:PASS\r\n"); }
                else { contentBox.AppendText("      SATA Port #01:FAIL\r\n"); }
                flag_testcheck = 1;
            }
            if (checkBox2.Checked == true)
            {
                contentBox.AppendText("     Copy File Test\r\n");
                if (textBox8.Text == "0") { contentBox.AppendText("     SATA Port #02:PASS\r\n"); }
                else { contentBox.AppendText("      SATA Port #02:FAIL\r\n"); }
                flag_testcheck = 1;
            }
            if (checkBox3.Checked == true)
            {
                contentBox.AppendText("     Copy File Test\r\n");
                if (textBox9.Text == "0") { contentBox.AppendText("     SATA Port #03:PASS\r\n"); }
                else { contentBox.AppendText("      SATA Port #03:FAIL\r\n"); }
                flag_testcheck = 1;
            }
            if (checkBox4.Checked == true)
            {
                contentBox.AppendText("     Copy File Test\r\n");
                if (textBox10.Text == "0") { contentBox.AppendText("        SATA Port #04:PASS\r\n"); }
                else { contentBox.AppendText("      SATA Port #04:FAIL\r\n"); }
                flag_testcheck = 1;
            }
            if (checkBox5.Checked == true)
            {
                contentBox.AppendText("     Copy File Test\r\n");
                if (textBox11.Text == "0") { contentBox.AppendText("        SATA Port #05:PASS\r\n"); }
                else { contentBox.AppendText("      SATA Port #05:FAIL\r\n"); }
                flag_testcheck = 1;
            }
            if (checkBox6.Checked == true)
            {
                contentBox.AppendText("     Copy File Test\r\n");
                if (textBox12.Text == "0") { contentBox.AppendText("        SATA Port #06:PASS\r\n"); }
                else { contentBox.AppendText("      SATA Port #06:FAIL\r\n"); }
                flag_testcheck = 1;
            }

            //add SATA Port Total Result
            if (flag_testcheck == 0 && GlobalVarable.log_flag == 0)
            {
                MessageBox.Show("Warning !!! Please Check At Least One !");
                //Scan_Click(sender, e);
                Environment.ExitCode = 1;
            }
            else if (GlobalVarable.log_flag == 0)
            {
                contentBox.AppendText("     Test  Result ----------------> PASS\r\n");
                Environment.ExitCode = 0;
            }
            else
            {
                contentBox.AppendText("     Test  Result ----------------> FAIL\r\n");
                Environment.ExitCode = 1;
            }
            contentBox.AppendText("   [Test End]\r\n\r\n");

            //製作結果檔
            CreateLogfile();

            //自動程式關閉
            string str1 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            IniFile ini = new IniFile(str1 + "\\SATATest.ini");
            if (GlobalVarable.log_flag == 0 && ini.Read("AUTO") == "1")
            { timer1.Start(); timer1_Tick_1(sender, e); }




        }

        public class GlobalVarable
        {
            public static int SataPortCount = 0;
            public static int log_flag = 0;
        }

        int timeLeft = 1;
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (checkBox7.Checked == false)
            {
                timeLeft = 1;
            }
            else if (timeLeft > 0)
            {
                timeLeft = timeLeft - 1;
            }
            else
            {
                //timer1.Stop();
                CloseWindow();
            }
        }

        public void CloseWindow()
        {

            Application.Exit();
        }

        public class IniFile   // revision 11
        {
            string Path;
            string EXE = Assembly.GetExecutingAssembly().GetName().Name;

            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

            public IniFile(string IniPath = null)
            {
                Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
            }

            public string Read(string Key, string Section = null)
            {
                var RetVal = new StringBuilder(255);
                GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
                return RetVal.ToString();
            }

            public void Write(string Key, string Value, string Section = null)
            {
                WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
            }

            public void DeleteKey(string Key, string Section = null)
            {
                Write(Key, null, Section ?? EXE);
            }

            public void DeleteSection(string Section = null)
            {
                Write(null, null, Section ?? EXE);
            }

            public bool KeyExists(string Key, string Section = null)
            {
                return Read(Key, Section).Length > 0;
            }
        }

        public void CreateLogfile()
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            string strTimeYear = string.Format("{0:D4}", currentTime.Year);
            string strTimeMonth = string.Format("{0:D2}", currentTime.Month);
            string strTimeDay = string.Format("{0:D2}", currentTime.Day);
            string strTimeHour = string.Format("{0:D2}", currentTime.Hour);
            string strTimeMinute = string.Format("{0:D2}", currentTime.Minute);
            string strTimeSecond = string.Format("{0:D2}", currentTime.Second);

            string logfilename = "SATA_Result_" + strTimeYear + strTimeMonth + strTimeDay + ".log";
            if (false == System.IO.File.Exists(Directory.GetCurrentDirectory() + "\\" + logfilename))
            {
                try
                {
                    StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + "\\" + logfilename, true);
                    writer.Write("SATA Port Test [ V0.1] : \r\n");
                    writer.Write("-------------------------------------- \r\n");
                    writer.Write("Time\tResult\r\n");
                    writer.Close();

                }
                catch
                {
                    MessageBox.Show("Create File.log Error");
                }
            }

            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (GlobalVarable.log_flag == 0)
            {
                File.AppendAllText(logfilename, "\r\n" + date + "--------->PASS\r\n");
            }
            else
            {
                File.AppendAllText(logfilename, "\r\n" + date + "--------->FAIL\r\n");
            }

        }

        public void CreateFile_textBox_added()
        {
            int count = 0;

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType.ToString() == "Fixed")
                {
                    count += 1;
                    switch (count)
                    {
                        case 1: textBox7.Text = CreateFileTest(d.Name); break;
                        case 2: textBox8.Text = CreateFileTest(d.Name); break;
                        case 3: textBox9.Text = CreateFileTest(d.Name); break;
                        case 4: textBox10.Text = CreateFileTest(d.Name); break;
                        case 5: textBox11.Text = CreateFileTest(d.Name); break;
                        case 6: textBox12.Text = CreateFileTest(d.Name); break;
                        

                    }
                }
            }
        }

        public static string CreateFileTest(string path)
        {

            try
            {
                // Create a file to write to.
                System.IO.StreamWriter File = new System.IO.StreamWriter(path + "SATATest.txt");
                File.WriteLine("SATATest");
                File.Close();

                FileInfo fInfo = new FileInfo(path + "SATATest.txt");
                string length = "0";
                length = fInfo.Length.ToString();
                fInfo.Delete();

                if (length == "10") { return "0"; }
                else { return "1"; }
            }
            catch
            {
                return "1";
            }

        }

        public void DiskName()
        {
            int count = 0;

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType.ToString() == "Fixed")
                {
                    count += 1;
                    GlobalVarable.SataPortCount += 1;
                    switch (count)
                    {
                        case 1: textBox1.Text = (d.Name + "Serial ATA Storage"); break;
                        case 2: textBox2.Text = (d.Name + "Serial ATA Storage"); break;
                        case 3: textBox3.Text = (d.Name + "Serial ATA Storage"); break;
                        case 4: textBox4.Text = (d.Name + "Serial ATA Storage"); break;
                        case 5: textBox5.Text = (d.Name + "Serial ATA Storage"); break;
                        case 6: textBox6.Text = (d.Name + "Serial ATA Storage"); break;

                    }
                }
            }
        }
        public void CleanAll()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            CleanTextBox7_12();
            contentBox.AppendText("----------Scan Serial ATA Storage----------\r\n");
            CleanPicBox();
            GlobalVarable.SataPortCount = 0;
        }
        public void CleanCheckBox()
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
        }
        public void CleanPicBox()
        {
            pictureBox1.Image = Properties.Resources.bmp00002;
            pictureBox2.Image = Properties.Resources.bmp00002;
            pictureBox3.Image = Properties.Resources.bmp00002;
            pictureBox4.Image = Properties.Resources.bmp00002;
            pictureBox5.Image = Properties.Resources.bmp00002;
            pictureBox6.Image = Properties.Resources.bmp00002;
        }
        public void CleanTextBox7_12()
        {
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            //Auto Run

            if (checkBox7.Checked) //設置開機自啟動  
            {
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("JcShutdown", path);
                rk2.Close();
                rk.Close();

                //寫入ini檔為true 
                string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                IniFile ini = new IniFile(str + "\\SATATest.ini");
                ini.Write("AUTO", "1");
                if (checkBox1.Checked == true) { ini.Write("SATA1", "1"); } else { ini.Write("SATA1", "0"); }
                if (checkBox2.Checked == true) { ini.Write("SATA2", "1"); } else { ini.Write("SATA2", "0"); }
                if (checkBox3.Checked == true) { ini.Write("SATA3", "1"); } else { ini.Write("SATA3", "0"); }
                if (checkBox5.Checked == true) { ini.Write("SATA4", "1"); } else { ini.Write("SATA4", "0"); }
                if (checkBox4.Checked == true) { ini.Write("SATA5", "1"); } else { ini.Write("SATA5", "0"); }
                if (checkBox6.Checked == true) { ini.Write("SATA6", "1"); } else { ini.Write("SATA6", "0"); }

            }
            else //取消開機自啟動  
            {
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.DeleteValue("JcShutdown", false);
                rk2.Close();
                rk.Close();

                //寫入ini檔為false
                string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                IniFile ini = new IniFile(str + "\\SATATest.ini");
                ini.Write("AUTO", "0");
            }

        }










    }
}
