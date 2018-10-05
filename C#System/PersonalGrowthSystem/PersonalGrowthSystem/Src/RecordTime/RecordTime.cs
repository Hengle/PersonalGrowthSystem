﻿using PersonalGrowthSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;

public class RecordTime
{
    public RecordTime()
    {
        GoogleCalendar.LoadCredential();

        GoogleCalendar.Report(DateTime.Now, "程序启动", "", 1);
    }

    //每10分钟执行一次
    public void TimerTask(object source, ElapsedEventArgs e)
    {
        Process processes = GetCurrentProcesses();

        if(processes != null)
        {
            GoogleCalendar.Report(DateTime.Now, processes.ProcessName, processes.MainWindowTitle, 10);
            MainWindow.Notify("已上报 ->" + processes.ProcessName, 1000);
        }
        else
        {
            GoogleCalendar.Report(DateTime.Now, "无活跃程序", "", 10);
        }

        //屏幕截屏
        ScreenShot.ShotAll(Config.GetConfig<ConfigData>().ShotPosition);
    }

    #region 分析进程
    Process GetCurrentProcesses()
    {
        int id = GetForegroundWindow().ToInt32();

        Process[] ps = Process.GetProcesses();
        foreach (Process p in ps)
        {
            string info = "";
            try
            {
                if(p.MainWindowHandle.ToInt32() == id)
                {
                    return p;
                }
            }
            catch (Exception e)
            {
                info = e.Message;
            }
        }

        return null;
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    #endregion
}