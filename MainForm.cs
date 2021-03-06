﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaxBot
{
    public partial class MainForm : Form
    {
        public bool IsCancelled = false;
        private NotifyIcon notifyIcon;
        private MenuItem logMenuItem;
        private StepList stepList = new StepList();
        public static bool LogEnabled { get; private set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //var list = new StepList();
            //stepList = (StepList)list.OrderBy(x => (int)(x.StepType)).ToList();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (notifyIcon != null)
            {
                if (notifyIcon.Visible)
                {
                    notifyIcon.Visible = false;
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Exit(object sender, EventArgs e)
        {
            Close();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            Visible = false;
            var showMenuItem = new MenuItem("Показать", new EventHandler(ShowForm));
            logMenuItem = new MenuItem("Вести лог", new EventHandler(EnableLog));
            var hideIconItem = new MenuItem("Скрыть", new EventHandler(HideIcon));
            var exitMenuItem = new MenuItem("Выход", new EventHandler(Exit));
            logMenuItem.Checked = LogEnabled;
            notifyIcon = new NotifyIcon
            {
                Icon = Properties.Resources.AppIcon,
                ContextMenu =
                    new ContextMenu(
                        new MenuItem[] { showMenuItem, logMenuItem, hideIconItem, exitMenuItem }),
                Visible = true
            };

            notifyIcon.Click += ShowForm;
        }

        private void HideIcon(object sender, EventArgs e)
        {
            DialogResult dr =
                MessageBox.Show(
                    "Вы действительно хотите скрыть программу? Программа продолжит работу в скрытом режиме.",
                    "Скрыть программу",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

            if (dr == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                if (notifyIcon != null)
                {
                    if (notifyIcon.Visible)
                    {
                        notifyIcon.Visible = false;
                    }
                }
            }
        }

        private void EnableLog(object sender, EventArgs e)
        {
            if (logMenuItem.Checked)
            {
                logMenuItem.Checked = false;
                LogEnabled = false;
            }
            else
            {
                logMenuItem.Checked = true;
                LogEnabled = true;
            }
        }

        private void ShowForm(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Visible = true;
            Show();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!bgwMain.IsBusy)
            {
                progressBar1.Value = 0;
                IsCancelled = false;

                bgwMain.RunWorkerAsync();
            }
        }

        private void UpdateLog(string message)
        {
            if (LogEnabled)
            {
                Logger.AppendAction(message);
            }

            List<string> lines = txbLog.Lines.ToList();
            lines.Add(message);

            txbLog.Invoke((MethodInvoker)delegate {
                txbLog.Lines = lines.ToArray();
            });
        }

        private void bgwMain_DoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            if (backgroundWorker == null)
            {
                return;
            }

            int progress = 1;
            backgroundWorker.ReportProgress(progress);
            UpdateLog("Operation started");

            foreach (Step step in stepList)
            {
                if (IsCancelled)
                {
                    break;
                }

                Output output = StepProvider.Current.StepInvoke(step);

                if (!IsCancelled)
                {
                    UpdateLog(output.Message);
                    progress += 100 / stepList.Count;
                    backgroundWorker.ReportProgress(progress);
                }
            }

            UpdateLog("DoWork process is conpleted");
        }

        private void bgwMain_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void bgwMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("Task completed");
            UpdateLog("RunWorkerCompleted process is completed");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            bgwMain.CancelAsync();
            IsCancelled = true;
            progressBar1.Value = 0;
            UpdateLog("Operation cancelled by user");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
    }
}
