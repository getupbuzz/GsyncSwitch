﻿using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using Microsoft.Win32;
using WindowsInput;
using WindowsInput.Native;
using System.Runtime.InteropServices;
using System.IO;
using NHotkey.WindowsForms;
using System.Globalization;

namespace GsyncSwitch
{
    class ControlContainer : IContainer
    {

        ComponentCollection _components;

        public ControlContainer()
        {
            _components = new ComponentCollection(new IComponent[] { });
        }

        public void Add(IComponent component) { }
        public void Add(IComponent component, string Name) { }
        public void Remove(IComponent component) { }

        public ComponentCollection Components
        {
            get { return _components; }
        }

        public void Dispose()
        {
            _components = null;
        }
    }

    static class Program
    {
        
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IconClass sc = new IconClass();
//            sc.notifyIcon1.ShowBalloonTip(1000);

            Application.Run();
        }
    }

    class IconClass
    {
        ControlContainer container = new ControlContainer();
        public NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem switchOnBoth;
        private ToolStripMenuItem switchOffBoth;
        private ToolStripMenuItem switchOnGsync;
        private ToolStripMenuItem switchOffGsync;
        private ToolStripMenuItem switchOnHDR;
        private ToolStripMenuItem switchOffHDR;
        private ToolStripMenuItem exitApplication;
        private ToolStripMenuItem launchAtStartup;

        // The path to the key where Windows looks for startup applications
        public RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public IconClass()
        {
            this.notifyIcon1 = new NotifyIcon(container);
            this.notifyIcon1.Icon = new Icon(this.GetType(), "Letter_G.ico");
            this.notifyIcon1.Text = "Gsync Switch by KwizatZ";

            this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipTitle = "Gsync Switch";
            this.notifyIcon1.BalloonTipText = "made by KwizatZ";

            this.notifyIcon1.Visible = true;

            contextMenu = new ContextMenuStrip();
            switchOffBoth = new ToolStripMenuItem();
            switchOnBoth = new ToolStripMenuItem();
            switchOnGsync = new ToolStripMenuItem();
            switchOffGsync = new ToolStripMenuItem();
            switchOnHDR = new ToolStripMenuItem();
            switchOffHDR = new ToolStripMenuItem();
            exitApplication = new ToolStripMenuItem();
            launchAtStartup = new ToolStripMenuItem();

            contextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
//            contextMenu.Renderer = new KwizatZMenuRenderer();

            this.notifyIcon1.ContextMenuStrip = contextMenu;

            switchOnBoth.Text = "On";
            switchOnBoth.Click += new EventHandler(SwitchOnBoth_Click);
            switchOnBoth.ShortcutKeys = Keys.Control | Keys.Alt | Keys.R;
            contextMenu.Items.Add(switchOnBoth);
            HotkeyManager.Current.AddOrReplace("On", Keys.Control | Keys.Alt | Keys.R, SwitchOnBoth_Click);

            switchOffBoth.Text = "Off";
            switchOffBoth.Click += new EventHandler(SwitchOffBoth_Click);
            switchOffBoth.ShortcutKeys = Keys.Control | Keys.Alt | Keys.S;
            contextMenu.Items.Add(switchOffBoth);
            HotkeyManager.Current.AddOrReplace("Off", Keys.Control | Keys.Alt | Keys.S, SwitchOffBoth_Click);

            switchOnGsync.Text = "Gsync On";
            switchOnGsync.Image = GsyncSwitch.Properties.Resources.nvidia_logo ;  
            switchOnGsync.Click += new EventHandler(SwitchOnGsync_Click);
            contextMenu.Items.Add(switchOnGsync);


            switchOffGsync.Text = "Gsync Off";
            switchOffGsync.Image = GsyncSwitch.Properties.Resources.nvidia_logo;
            switchOffGsync.Click += new EventHandler(SwitchOffGsync_Click);
            contextMenu.Items.Add(switchOffGsync);

            switchOnHDR.Text = "HDR On";
            switchOnHDR.Image = GsyncSwitch.Properties.Resources.hdr;
            switchOnHDR.Click += new EventHandler(SwitchOnHDR_Click);
            contextMenu.Items.Add(switchOnHDR);

            switchOffHDR.Text = "HDR Off";
            switchOffHDR.Image = GsyncSwitch.Properties.Resources.hdr;
            switchOffHDR.Click += new EventHandler(SwitchOffHDR_Click);
            contextMenu.Items.Add(switchOffHDR);

            exitApplication.Text = "Exit..";
            exitApplication.Click += new EventHandler(ExitApplication_Click);
            contextMenu.Items.Add(exitApplication);

            launchAtStartup.Text = "Launch at Windows startup";
            // Check to see the current state (running at startup or not)
            if (rkApp.GetValue("GsyncSwitch") == null)
            {
                // The value doesn't exist, the application is not set to run at startup
                launchAtStartup.Checked = false;
            }
            else
            {
                // The value exists, the application is set to run at startup
                launchAtStartup.Checked = true;
            }
            launchAtStartup.Click += new EventHandler(LaunchAtStartup_Click);
            contextMenu.Items.Add(launchAtStartup);

        }

        private void SwitchOnHDR_Click(object sender, EventArgs e)
        {
            HDRController.SetGlobalHDRState(true);
        }
        private void SwitchOffHDR_Click(object sender, EventArgs e)
        {
            HDRController.SetGlobalHDRState(false);
        }

        private void LaunchAtStartup_Click(object sender, EventArgs e)
        {
            if (!launchAtStartup.Checked)
            {
                // Add the value in the registry so that the application runs at startup
                rkApp.SetValue("GsyncSwitch", Application.ExecutablePath);
                launchAtStartup.Checked = true;
            }
            else
            {
                // Remove the value from the registry so that the application doesn't start
                rkApp.DeleteValue("GsyncSwitch", false);
                launchAtStartup.Checked = false;
            }
        }

        private void ExitApplication_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;

            System.Windows.Forms.Application.Exit();
        }

        private void SwitchGsync(object sender, EventArgs e, string args = "")
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Process gsyncSwitchEXE = new Process();
            gsyncSwitchEXE.StartInfo.FileName = "GsyncSwitchEXE.exe";
            gsyncSwitchEXE.StartInfo.Arguments = args;
            gsyncSwitchEXE.Start();
        }

        private void SwitchOnGsync_Click(object sender, EventArgs e)
        {
            SwitchGsync(sender, e, "1");
        }

        private void SwitchOffGsync_Click(object sender, EventArgs e)
        {
            SwitchGsync(sender, e, "0");
        }

        private void SwitchOnBoth_Click(object sender, EventArgs e)
        {
            SwitchOnGsync_Click(sender, e);
            SwitchOnHDR_Click(sender, e);
        }

        private void SwitchOffBoth_Click(Object sender, EventArgs e)
        {
            SwitchOffGsync_Click(sender, e);
            SwitchOffHDR_Click(sender, e);

        }
    }

}
