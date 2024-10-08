using System;
using System.Text;
using UnityEngine;

namespace SinmaiAssist.GUI;

public class MainGUI
{
    private enum Toolbar
    {
        AutoPlay,
        FastSkip,
        ChartTimer,
        DummyLogin,
        UserData,
        Graphic,
        Debug
    }
    
    private const float PanelWidth = 300f;
    private const int ButtonsPerRow = 3;
    
    private Rect _panelWindow;
    private Toolbar _toolbar = Toolbar.AutoPlay;
    private bool _backspaceKeyDown = false;
    
    public static readonly Style Style = new Style();
    
    public MainGUI()
    {
        
    }
    
    public void OnGUI()
    {
        if (DebugInput.GetKey(KeyCode.Backspace))
        {
            if(!_backspaceKeyDown) SinmaiAssist.config.ModSetting.ShowPanel = !SinmaiAssist.config.ModSetting.ShowPanel;
            _backspaceKeyDown = true;
        }
        else
        {
            _backspaceKeyDown = false;
        }
                
        if (SinmaiAssist.config.ModSetting.ShowPanel)
        {
            _panelWindow = GUILayout.Window(10086, _panelWindow, MainPanel, $"{BuildInfo.Name} {BuildInfo.Version}");
            SinmaiAssist.config.ModSetting.ShowPanel = true;
        }
        else
        {
            _panelWindow = new Rect();
        }
    }
    
    private void MainPanel(int id)
    {
        GUILayout.BeginVertical($"{BuildInfo.Name} {BuildInfo.Version} ({BuildInfo.CommitHash??"NOT SET"})", GUILayout.Height(20f));
        ToolBarPanel();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Width(PanelWidth), GUILayout.Height(330f));
        if (_toolbar == Toolbar.AutoPlay && SinmaiAssist.config.Cheat.AutoPlay) AutoPlayPanel.OnGUI();
        else if (_toolbar == Toolbar.FastSkip && SinmaiAssist.config.Cheat.FastSkip) FastSkipPanel.OnGUI();
        else if (_toolbar == Toolbar.ChartTimer && SinmaiAssist.config.Cheat.ChartTimer) ChartTimerPanel.OnGUI();
        else if (_toolbar == Toolbar.DummyLogin && SinmaiAssist.config.Common.DummyLogin.Enable) DummyLoginPanel.OnGUI();
        else if (_toolbar == Toolbar.UserData) UserDataPanel.OnGUI();
        else if (_toolbar == Toolbar.Graphic) GraphicPanel.OnGUI();
        else if (_toolbar == Toolbar.Debug) DebugPanel.OnGUI();
        else DisablePanel();
        GUILayout.EndVertical();
        UnityEngine.GUI.DragWindow();
    }
    
    private void ToolBarPanel()
    {
        string[] toolbarNames = Enum.GetNames(typeof(Toolbar));
        int toolbarCount = toolbarNames.Length;
        int rowCount = Mathf.CeilToInt((float)toolbarCount / ButtonsPerRow);
        int selectedToolbar = (int)_toolbar;

        for (int row = 0; row < rowCount; row++)
        {
            GUILayout.BeginHorizontal();

            int startIndex = row * ButtonsPerRow;
            int endIndex = Mathf.Min(startIndex + ButtonsPerRow, toolbarCount);

            string[] rowToolbarNames = new string[endIndex - startIndex];
            Array.Copy(toolbarNames, startIndex, rowToolbarNames, 0, endIndex - startIndex);

            int rowSelection = GUILayout.Toolbar(
                selectedToolbar - startIndex,
                rowToolbarNames,
                GUILayout.Width(PanelWidth),
                GUILayout.Height(20f)
            );
            
            selectedToolbar = startIndex + rowSelection;

            GUILayout.EndHorizontal();
        }
        _toolbar = (Toolbar)selectedToolbar;
    }
    
    private void DisablePanel()
    {
        GUILayout.Label(_toolbar.ToString(), Style.DisablePanel, GUILayout.Width(PanelWidth), GUILayout.Height(120f));
        GUILayout.Label("is Disable", Style.DisablePanel, GUILayout.Width(PanelWidth), GUILayout.Height(160f));
    }
}