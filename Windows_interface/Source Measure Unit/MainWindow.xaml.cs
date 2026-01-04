using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Axes;
using MahApps.Metro.Controls;
using OxyPlot.Series;
using System.IO.Ports;
using System.IO;
using Microsoft.Win32;
using ControlzEx.Standard;
using System.Windows.Markup;

namespace Source_Measure_Unit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isDarkMode = false;

        private List<Tuple<double, double>> dataPoints = new List<Tuple<double, double>>();
        private List<Tuple<double, double>> dataPoints2 = new List<Tuple<double, double>>();

        private String parametersString = "";

        private String selectedSerialPort;
        private SerialPort _serialPort;

        PlotModel plotModel;
        PlotModel plotModel2;
        LineSeries series;
        LineSeries series2;
        LinearAxis plot1X;
        LinearAxis plot1Y;
        LinearAxis plot2X;
        LinearAxis plot2Y;

        private TabControl NewMeasureTab;
        private TabItem MeasureConfig;
        private TabItem RealTimeGraph;
        private StackPanel RealTimeGraphPanel;
        private PlotView GraphSMUChannel1;
        private PlotView GraphSMUChannel2;
        private StackPanel MeasureConfigPanel;
        private ComboBox comboBoxCh1;
        private ComboBox comboBoxCh2;
        private ComboBox COMselect;
        private Button COMconnectButton;
        private StackPanel ChannelPanel;
        private CheckBox channel1Check;
        private CheckBox channel2Check;
        private StackPanel TechniqueSelectPanel;

        private StackPanel ParametersPanel;
        private StackPanel Ch1ParametersPanel;
        private StackPanel Ch2ParametersPanel;
        private StackPanel Ch1ParametersTextPanel;
        private StackPanel Ch1ParametersLabelPanel;
        private StackPanel Ch2ParametersTextPanel;
        private StackPanel Ch2ParametersLabelPanel;

        private Button SubmitButton;

        #region LSV
        private Button LSVsubmitButton;
        private TextBox LSVtimeStepBox;
        private TextBox LSVtimeStepBox2;
        private Label LSVtimeStepLabel;
        private TextBox LSVstepVBox;
        private TextBox LSVstepVBox2;
        private Label LSVstepVLabel;
        private TextBox LSVfinalVBox;
        private TextBox LSVfinalVBox2;
        private Label LSVfinalVLabel;
        private Label LSVstartVLabel;
        private TextBox LSVstartVBox;
        private TextBox LSVstartVBox2;
        #endregion
        #region CV
        private TextBox CVtimeStepBox;
        private TextBox CVtimeStepBox2;
        private Label CVtimeStepLabel;
        private TextBox CVstepVBox;
        private TextBox CVstepVBox2;
        private Label CVstepVLabel;
        private TextBox CVfinalVBox;
        private TextBox CVfinalVBox2;
        private Label CVfinalVLabel;
        private Label CVstartVLabel;
        private TextBox CVstartVBox;
        private TextBox CVstartVBox2;
        private Label CVcycleLabel;
        private TextBox CVcycleBox;
        private TextBox CVcycleBox2;
        private Label CVpeakV1Label;
        private TextBox CVpeakV1Box;
        private TextBox CVpeakV1Box2;
        private Label CVpeakV2Label;
        private TextBox CVpeakV2Box;
        private TextBox CVpeakV2Box2;
        #endregion
        #region SWV
        private Label SWVstartVLabel;
        private TextBox SWVstartVBox;
        private TextBox SWVstartVBox2;
        private Label SWVfinalVLabel;
        private TextBox SWVfinalVBox;
        private TextBox SWVfinalVBox2;
        private Label SWVstepVLabel;
        private TextBox SWVstepVBox;
        private TextBox SWVstepVBox2;
        private Label SWVtimeStepLabel;
        private TextBox SWVtimeStepBox;
        private TextBox SWVtimeStepBox2;
        private Label SWVAmpLabel;
        private TextBox SWVAmpBox;
        private TextBox SWVAmpBox2;
        #endregion
        #region DPV
        private Label DPVstartVLabel;
        private TextBox DPVstartVBox;
        private TextBox DPVstartVBox2;
        private Label DPVfinalVLabel;
        private TextBox DPVfinalVBox;
        private TextBox DPVfinalVBox2;
        private Label DPVstepVLabel;
        private TextBox DPVstepVBox;
        private TextBox DPVstepVBox2;
        private Label DPVpulseLabel;
        private TextBox DPVpulseBox;
        private TextBox DPVpulseBox2;
        private Label DPVpulseTimeLabel;
        private TextBox DPVpulseTimeBox;
        private TextBox DPVpulseTimeBox2;
        private Label DPVbaseTimeLabel;
        private TextBox DPVbaseTimeBox;
        private TextBox DPVbaseTimeBox2;
        #endregion
        #region CP
        private Label CPcurrentLabel;
        private TextBox CPcurrentBox2;
        private TextBox CPcurrentBox;
        private Label CPsampleTLabel;
        private TextBox CPsampleTBox2;
        private TextBox CPsampleTBox;
        private Label CPsamplePLabel;
        private TextBox CPsamplePBox2;
        private TextBox CPsamplePBox;
        #endregion
        #region LSP
        private Label LSPstartILabel;
        private TextBox LSPstartIBox;
        private TextBox LSPstartIBox2;
        private Label LSPfinalILabel;
        private TextBox LSPfinalIBox;
        private TextBox LSPfinalIBox2;
        private Label LSPstepILabel;
        private TextBox LSPstepIBox;
        private TextBox LSPstepIBox2;
        private Label LSPtimeStepLabel;
        private TextBox LSPtimeStepBox;
        private TextBox LSPtimeStepBox2;
        #endregion
        #region CyP
        private Label CyPstartILabel;
        private TextBox CyPstartIBox;
        private TextBox CyPstartIBox2;
        private Label CyPfinalILabel;
        private TextBox CyPfinalIBox;
        private TextBox CyPfinalIBox2;
        private Label CyPpeakI1Label;
        private TextBox CyPpeakI1Box;
        private TextBox CyPpeakI1Box2;
        private Label CyPpeakI2Label;
        private TextBox CyPpeakI2Box;
        private TextBox CyPpeakI2Box2;
        private Label CyPstepILabel;
        private TextBox CyPstepIBox;
        private TextBox CyPstepIBox2;
        private Label CyPtimeStepLabel;
        private TextBox CyPtimeStepBox;
        private TextBox CyPtimeStepBox2;
        private Label CyPcycleLabel;
        private TextBox CyPcycleBox;
        private TextBox CyPcycleBox2;
        #endregion

        private TabControl HomeTabControl;
        private TabItem StartTab;

        public MainWindow()
        {
            InitializeComponent();
            ApplyTheme(); // Apply default theme (light)
            LoadMeasurementConfigs();
            InitializeMeasurementControls();
            // Create the start tab
            HomeTabControl = new TabControl
            {
                Margin = new Thickness(5)
            };
            Grid.SetRow(HomeTabControl, 2);
            Grid.SetColumn(HomeTabControl, 1);
            UserGrid.Children.Add(HomeTabControl);

            StartTab = new TabItem
            {
                Header = "Start"
            };
            HomeTabControl.Items.Add(StartTab);
            // --
        }

        private void InitializeMeasurementControls()
        {
            if (CPcurrentBox == null) CPcurrentBox = new TextBox();
            if (CPsampleTBox == null) CPsampleTBox = new TextBox();
            if (CPsamplePBox == null) CPsamplePBox = new TextBox();

            if (LSVstartVBox == null) LSVstartVBox = new TextBox();
            if (LSVfinalVBox == null) LSVfinalVBox = new TextBox();
            if (LSVstepVBox == null) LSVstepVBox = new TextBox();
            if (LSVtimeStepBox == null) LSVtimeStepBox = new TextBox();

            if (CVstartVBox == null) CVstartVBox = new TextBox();
            if (CVpeakV1Box == null) CVpeakV1Box = new TextBox();
            if (CVpeakV2Box == null) CVpeakV2Box = new TextBox();
            if (CVfinalVBox == null) CVfinalVBox = new TextBox();
            if (CVstepVBox == null) CVstepVBox = new TextBox();
            if (CVtimeStepBox == null) CVtimeStepBox = new TextBox();
            if (CVcycleBox == null) CVcycleBox = new TextBox();

            if (DPVstartVBox == null) DPVstartVBox = new TextBox();
            if (DPVfinalVBox == null) DPVfinalVBox = new TextBox();
            if (DPVstepVBox == null) DPVstepVBox = new TextBox();
            if (DPVpulseBox == null) DPVpulseBox = new TextBox();
            if (DPVpulseTimeBox == null) DPVpulseTimeBox = new TextBox();
            if (DPVbaseTimeBox == null) DPVbaseTimeBox = new TextBox();

            if (SWVstartVBox == null) SWVstartVBox = new TextBox();
            if (SWVfinalVBox == null) SWVfinalVBox = new TextBox();
            if (SWVstepVBox == null) SWVstepVBox = new TextBox();
            if (SWVAmpBox == null) SWVAmpBox = new TextBox();
            if (SWVtimeStepBox == null) SWVtimeStepBox = new TextBox();

            if (LSPstartIBox == null) LSPstartIBox = new TextBox();
            if (LSPfinalIBox == null) LSPfinalIBox = new TextBox();
            if (LSPstepIBox == null) LSPstepIBox = new TextBox();
            if (LSPtimeStepBox == null) LSPtimeStepBox = new TextBox();

            if (CyPstartIBox == null) CyPstartIBox = new TextBox();
            if (CyPpeakI1Box == null) CyPpeakI1Box = new TextBox();
            if (CyPpeakI2Box == null) CyPpeakI2Box = new TextBox();
            if (CyPfinalIBox == null) CyPfinalIBox = new TextBox();
            if (CyPstepIBox == null) CyPstepIBox = new TextBox();
            if (CyPtimeStepBox == null) CyPtimeStepBox = new TextBox();
            if (CyPcycleBox == null) CyPcycleBox = new TextBox();


            if (CPcurrentBox2 == null) CPcurrentBox2 = new TextBox();
            if (CPsampleTBox2 == null) CPsampleTBox2 = new TextBox();
            if (CPsamplePBox2 == null) CPsamplePBox2 = new TextBox();

            if (LSVstartVBox2 == null) LSVstartVBox2 = new TextBox();
            if (LSVfinalVBox2 == null) LSVfinalVBox2 = new TextBox();
            if (LSVstepVBox2 == null) LSVstepVBox2 = new TextBox();
            if (LSVtimeStepBox2 == null) LSVtimeStepBox2 = new TextBox();

            if (CVstartVBox2 == null) CVstartVBox2 = new TextBox();
            if (CVpeakV1Box2 == null) CVpeakV1Box2 = new TextBox();
            if (CVpeakV2Box2 == null) CVpeakV2Box2 = new TextBox();
            if (CVfinalVBox2 == null) CVfinalVBox2 = new TextBox();
            if (CVstepVBox2 == null) CVstepVBox2 = new TextBox();
            if (CVtimeStepBox2 == null) CVtimeStepBox2 = new TextBox();
            if (CVcycleBox2 == null) CVcycleBox2 = new TextBox();

            if (DPVstartVBox2 == null) DPVstartVBox2 = new TextBox();
            if (DPVfinalVBox2 == null) DPVfinalVBox2 = new TextBox();
            if (DPVstepVBox2 == null) DPVstepVBox2 = new TextBox();
            if (DPVpulseBox2 == null) DPVpulseBox2 = new TextBox();
            if (DPVpulseTimeBox2 == null) DPVpulseTimeBox2 = new TextBox();
            if (DPVbaseTimeBox2 == null) DPVbaseTimeBox2 = new TextBox();

            if (SWVstartVBox2 == null) SWVstartVBox2 = new TextBox();
            if (SWVfinalVBox2 == null) SWVfinalVBox2 = new TextBox();
            if (SWVstepVBox2 == null) SWVstepVBox2 = new TextBox();
            if (SWVAmpBox2 == null) SWVAmpBox2 = new TextBox();
            if (SWVtimeStepBox2 == null) SWVtimeStepBox2 = new TextBox();

            if (LSPstartIBox2 == null) LSPstartIBox2 = new TextBox();
            if (LSPfinalIBox2 == null) LSPfinalIBox2 = new TextBox();
            if (LSPstepIBox2 == null) LSPstepIBox2 = new TextBox();
            if (LSPtimeStepBox2 == null) LSPtimeStepBox2 = new TextBox();

            if (CyPstartIBox2 == null) CyPstartIBox2 = new TextBox();
            if (CyPpeakI1Box2 == null) CyPpeakI1Box2 = new TextBox();
            if (CyPpeakI2Box2 == null) CyPpeakI2Box2 = new TextBox();
            if (CyPfinalIBox2 == null) CyPfinalIBox2 = new TextBox();
            if (CyPstepIBox2 == null) CyPstepIBox2 = new TextBox();
            if (CyPtimeStepBox2 == null) CyPtimeStepBox2 = new TextBox();
            if (CyPcycleBox2 == null) CyPcycleBox2 = new TextBox();
        }

        private void ApplyTheme()
        {
            var theme = _isDarkMode ? "DarkTheme" : "LightTheme";
            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(new Uri($"/Themes/{theme}.xaml", UriKind.Relative)));
        }

        private void ToggleDarkMode_Click(object sender, RoutedEventArgs e)
        {
            _isDarkMode = true;
            ApplyTheme();
        }

        private void ToggleLightMode_Click(object sender, RoutedEventArgs e)
        {
            _isDarkMode = false;
            ApplyTheme();
        }

        private void ConfigureTechnique_Click(object sender, RoutedEventArgs e)
        {
            // Logic to configure the selected technique
            MessageBox.Show("Configure Technique Clicked");
        }

        private void StartMeasurement_Click(object sender, RoutedEventArgs e)
        {
            // Logic to start the measurement
            MessageBox.Show("Start Measurement Clicked");
        }

        private void AddStep_Click(object sender, RoutedEventArgs e)
        {
            // Logic to add a step to the custom technique
            MessageBox.Show("Add Step Clicked");
        }

        private void SaveTechnique_Click(object sender, RoutedEventArgs e)
        {
            // Logic to save the custom technique
            MessageBox.Show("Save Technique Clicked");
        }

        private void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void NewMeasure_Click(object sender, RoutedEventArgs e)
        {
            ClearEveryTabControl();
            NewMeasureTab = new TabControl
            {
                Margin = new Thickness(5)
            };
            Grid.SetRow(NewMeasureTab, 2);
            Grid.SetColumn(NewMeasureTab, 1);
            UserGrid.Children.Add(NewMeasureTab);

            MeasureConfig = new TabItem
            {
                Header = "Measure Configuration"
            };

            MeasureConfigPanel = new StackPanel
            {
                CanHorizontallyScroll = true,
                CanVerticallyScroll = true,
            };

            COMselect = new ComboBox
            {
                Width = 200,
                Margin = new Thickness(0, 0, 0, 10)
            };

            string[] ports = SerialPort.GetPortNames();

            for (int i = 0; i < ports.Length; i++)
            {
                COMselect.Items.Add(ports[i]);
            }
            MeasureConfigPanel.Children.Add(COMselect);


            COMconnectButton = new Button
            {
                Content = "Connect",
                Width = 150,
                FontSize = 15,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            COMconnectButton.Click += ToggleConnection;
            MeasureConfigPanel.Children.Add(COMconnectButton);

            ChannelPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            channel1Check = new CheckBox() { Content = "Channel 1", Margin = new Thickness(5) };
            channel2Check = new CheckBox() { Content = "Channel 2", Margin = new Thickness(5) };

            ChannelPanel.Children.Add(channel1Check);
            ChannelPanel.Children.Add(channel2Check);

            channel1Check.Click += Channel1Check_Changed;
            channel2Check.Click += Channel2Check_Changed;

            TechniqueSelectPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };


            MeasureConfigPanel.Children.Add(ChannelPanel);
            MeasureConfigPanel.Children.Add(TechniqueSelectPanel);

            ParametersPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            comboBoxCh1 = new ComboBox
            {
                Width = 200,
                Margin = new Thickness(10, 10, 10, 10)
            };

            comboBoxCh1.Items.Add(null);
            comboBoxCh1.Items.Add("Linear Sweep Voltammetry");
            comboBoxCh1.Items.Add("Cyclic Voltammetry");
            comboBoxCh1.Items.Add("Differential Pulse Voltammetry");
            comboBoxCh1.Items.Add("Square Wave Voltammetry");
            comboBoxCh1.Items.Add("Chronopotentiometry");
            comboBoxCh1.Items.Add("Linear Sweep Potentiometry");
            comboBoxCh1.Items.Add("Cyclic Potentiometry");

            TechniqueSelectPanel.Children.Add(comboBoxCh1);
            comboBoxCh1.IsEnabled = false;

            comboBoxCh1.SelectionChanged += ComboBoxCh1_SelectionChanged;

            comboBoxCh2 = new ComboBox
            {
                Width = 200,
                Margin = new Thickness(10, 10, 10, 10)
            };

            comboBoxCh2.Items.Add(null);
            comboBoxCh2.Items.Add("Linear Sweep Voltammetry");
            comboBoxCh2.Items.Add("Cyclic Voltammetry");
            comboBoxCh2.Items.Add("Differential Pulse Voltammetry");
            comboBoxCh2.Items.Add("Square Wave Voltammetry");
            comboBoxCh2.Items.Add("Chronopotentiometry");
            comboBoxCh2.Items.Add("Linear Sweep Potentiometry");
            comboBoxCh2.Items.Add("Cyclic Potentiometry");

            TechniqueSelectPanel.Children.Add(comboBoxCh2);
            comboBoxCh2.IsEnabled = false;

            comboBoxCh2.SelectionChanged += ComboBoxCh2_SelectionChanged;

            Ch1ParametersPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            Ch2ParametersPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            ParametersPanel.Children.Add(Ch1ParametersPanel);
            ParametersPanel.Children.Add(Ch2ParametersPanel);

            MeasureConfigPanel.Children.Add(ParametersPanel);

            SubmitButton = new Button
            {
                Content = "Start",
                Width = 150,
                FontSize = 15,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };
            SubmitButton.IsEnabled = false;

            MeasureConfigPanel.Children.Add(SubmitButton);
            SubmitButton.Click += SendParameters;

            MeasureConfig.Content = MeasureConfigPanel;
            NewMeasureTab.Items.Add(MeasureConfig);
            //-------------------------------
            #region Real-Time Graph
            RealTimeGraph = new TabItem
            {
                Header = "Real-Time Graph"
            };

            RealTimeGraphPanel = new StackPanel
            {
                CanHorizontallyScroll = true,
                CanVerticallyScroll = true,
            };

            StackPanel Plot1Panel = new StackPanel
            {
                CanHorizontallyScroll = true,
                CanVerticallyScroll = true,
            };

            Plot1Panel.Orientation = Orientation.Vertical;
            Plot1Panel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Plot1Panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            StackPanel Plot2Panel = new StackPanel
            {
                CanHorizontallyScroll = true,
                CanVerticallyScroll = true,
            };

            Plot2Panel.Orientation = Orientation.Vertical;
            Plot2Panel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Plot2Panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = RealTimeGraphPanel
            };

            RealTimeGraphPanel.Orientation = Orientation.Horizontal;
            RealTimeGraphPanel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            RealTimeGraphPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            GraphSMUChannel1 = new PlotView
            {
                Height = 500,
                Width = 600
            };
            GraphSMUChannel2 = new PlotView
            {
                Height = 500,
                Width = 600
            };
            
            // Creating a plot model
            plotModel = new PlotModel { Title = "Channel 1/Single Channel" };
            plotModel2 = new PlotModel { Title = "Channel 2" };

            // Axes x and y
            plot1X = new LinearAxis { Position = AxisPosition.Bottom, Title = "" };
            plot1Y = new LinearAxis { Position = AxisPosition.Left, Title = "" };
            plot2X = new LinearAxis { Position = AxisPosition.Bottom, Title = "" };
            plot2Y = new LinearAxis { Position = AxisPosition.Left, Title = "" };

            plotModel.Axes.Add(plot1X);
            plotModel.Axes.Add(plot1Y);
            plotModel2.Axes.Add(plot2X);
            plotModel2.Axes.Add(plot2Y);

            series = new LineSeries
            {
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                Color = OxyColors.RoyalBlue
            };
            series2 = new LineSeries
            {
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                Color = OxyColors.RoyalBlue
            };

            /*series.Points.Add(new DataPoint(0, 0));
            series.Points.Add(new DataPoint(1, 2));
            series.Points.Add(new DataPoint(2, 4));
            series.Points.Add(new DataPoint(3, 6));
            series2.Points.Add(new DataPoint(0, 0));
            series2.Points.Add(new DataPoint(1, 2));
            */
            plotModel.Series.Add(series);
            plotModel2.Series.Add(series2);

            GraphSMUChannel1.Model = plotModel;
            GraphSMUChannel2.Model = plotModel2;

            Button savePNG1 = new Button
            {
                Content = "Save plot 1 as PNG",
                Width = 150,
                FontSize = 15,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };
            savePNG1.IsEnabled = true;

            Button savePNG2 = new Button
            {
                Content = "Save plot 2 as PNG",
                Width = 150,
                FontSize = 15,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };
            savePNG1.IsEnabled = true;

            Button saveCSV1 = new Button
            {
                Content = "Save plot 1 as CSV",
                Width = 150,
                FontSize = 15,
                Margin = new Thickness(25),
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };
            savePNG1.IsEnabled = true;

            Button saveCSV2 = new Button
            {
                Content = "Save plot 2 as CSV",
                Width = 150,
                FontSize = 15,
                Margin = new Thickness(25),
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };
            savePNG1.IsEnabled = true;


            Plot1Panel.Children.Add(GraphSMUChannel1);
            Plot1Panel.Children.Add(savePNG1);
            Plot1Panel.Children.Add(saveCSV1);
            savePNG1.Click += SaveGraphAsImage_Click;
            saveCSV1.Click += SaveDataAsCsv_Click;

            RealTimeGraphPanel.Children.Add(Plot1Panel);


            Plot2Panel.Children.Add(GraphSMUChannel2);
            Plot2Panel.Children.Add(savePNG2);
            Plot2Panel.Children.Add(saveCSV2);
            savePNG2.Click += SaveGraphAsImage2_Click;
            saveCSV2.Click += SaveDataAsCsv_Click2;

            RealTimeGraphPanel.Children.Add(Plot2Panel);


            scrollViewer.Content = RealTimeGraphPanel;
            RealTimeGraph.Content = scrollViewer;

            NewMeasureTab.Items.Add(RealTimeGraph);
            #endregion
        }
        private void SendParameters(object sender, RoutedEventArgs e)
        {
            parametersString = "";
            if (channel1Check.IsChecked == true && comboBoxCh1.SelectedItem != null) 
            {
                NewMeasureTab.SelectedIndex = 1;
                ClearGraph();
                switch (comboBoxCh1.SelectedItem.ToString())
                {
                    case "Linear Sweep Voltammetry":
                        if (string.IsNullOrEmpty(LSVstartVBox.Text) ||
                            string.IsNullOrEmpty(LSVfinalVBox.Text) ||
                            string.IsNullOrEmpty(LSVstepVBox.Text) ||
                            string.IsNullOrEmpty(LSVtimeStepBox.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else
                        {
                            parametersString = "LSV" + ";" + LSVstartVBox.Text + ";" + LSVfinalVBox.Text + ";" + LSVstepVBox.Text + ";" + LSVtimeStepBox.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("LSV Channel 1 Parameters Sent");
                        }
                        break;

                    case "Cyclic Voltammetry":
                        if (string.IsNullOrEmpty(CVstartVBox.Text) ||
                            string.IsNullOrEmpty(CVpeakV1Box.Text) ||
                            string.IsNullOrEmpty(CVpeakV2Box.Text) ||
                            string.IsNullOrEmpty(CVfinalVBox.Text) ||
                            string.IsNullOrEmpty(CVstepVBox.Text) ||
                            string.IsNullOrEmpty(CVtimeStepBox.Text) ||
                            string.IsNullOrEmpty(CVcycleBox.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else
                        {
                            parametersString = "CV" + ";" + CVstartVBox.Text + ";" + CVpeakV1Box.Text + ";" + CVpeakV2Box.Text + ";" + CVfinalVBox.Text + ";" + CVstepVBox.Text + ";" + CVtimeStepBox.Text + ";" + CVcycleBox.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("CV Channel 1 Parameters Sent");
                        }
                        break;

                    case "Differential Pulse Voltammetry":
                        if (string.IsNullOrEmpty(DPVstartVBox.Text) ||
                            string.IsNullOrEmpty(DPVfinalVBox.Text) ||
                            string.IsNullOrEmpty(DPVstepVBox.Text) ||
                            string.IsNullOrEmpty(DPVpulseBox.Text) ||
                            string.IsNullOrEmpty(DPVpulseTimeBox.Text) ||
                            string.IsNullOrEmpty(DPVbaseTimeBox.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else
                        {
                            parametersString = "DPV" + ";" + DPVstartVBox.Text + ";" + DPVfinalVBox.Text + ";" + DPVstepVBox.Text + ";" + DPVpulseBox.Text + ";" + DPVpulseTimeBox.Text + ";" + DPVbaseTimeBox.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("DPV Channel 1 Parameters Sent");
                        }
                        break;

                    case "Square Wave Voltammetry":
                        if (string.IsNullOrEmpty(SWVstartVBox.Text) ||
                            string.IsNullOrEmpty(SWVfinalVBox.Text) ||
                            string.IsNullOrEmpty(SWVstepVBox.Text) ||
                            string.IsNullOrEmpty(SWVAmpBox.Text) ||
                            string.IsNullOrEmpty(SWVtimeStepBox.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else
                        {
                            parametersString = "SWV" + ";" + SWVstartVBox.Text + ";" + SWVfinalVBox.Text + ";" + SWVstepVBox.Text + ";" + SWVAmpBox.Text + ";" + SWVtimeStepBox.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("SWV Channel 1 Parameters Sent");
                        }
                        break;

                    case "Chronopotentiometry":
                        if (string.IsNullOrEmpty(CPcurrentBox.Text) ||
                            string.IsNullOrEmpty(CPsampleTBox.Text) ||
                            string.IsNullOrEmpty(CPsamplePBox.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else
                        {
                            parametersString = "CP" + ";" + CPcurrentBox.Text + ";" + CPsampleTBox.Text + ";" + CPsamplePBox.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("CP Channel 1 Parameters Sent");
                        }
                        break;

                    case "Linear Sweep Potentiometry":
                        if (string.IsNullOrEmpty(LSPstartIBox.Text) ||
                            string.IsNullOrEmpty(LSPfinalIBox.Text) ||
                            string.IsNullOrEmpty(LSPstepIBox.Text) ||
                            string.IsNullOrEmpty(LSPtimeStepBox.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else
                        {
                            parametersString = "LSP" + ";" + LSPstartIBox.Text + ";" + LSPfinalIBox.Text + ";" + LSPstepIBox.Text + ";" + LSPtimeStepBox.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("LSP Channel 1 Parameters Sent");
                        }
                        break;

                    case "Cyclic Potentiometry":
                        if (string.IsNullOrEmpty(CyPstartIBox.Text) ||
                            string.IsNullOrEmpty(CyPpeakI1Box.Text) ||
                            string.IsNullOrEmpty(CyPpeakI2Box.Text) ||
                            string.IsNullOrEmpty(CyPfinalIBox.Text) ||
                            string.IsNullOrEmpty(CyPstepIBox.Text) ||
                            string.IsNullOrEmpty(CyPtimeStepBox.Text) ||
                            string.IsNullOrEmpty(CyPcycleBox.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else
                        {
                            parametersString = "CyP" + ";" + CyPstartIBox.Text + ";" + CyPpeakI1Box.Text + ";" + CyPpeakI2Box.Text + ";" + CyPfinalIBox.Text + ";" + CyPstepIBox.Text + ";" + CyPtimeStepBox.Text + ";" + CyPcycleBox.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("CyP Channel 1 Parameters Sent");
                        }
                        break;
                    default:
                        MessageBox.Show("Please select a valid technique for Channel 1.");
                        break;
                }
            }
            parametersString += ",";
            if (channel2Check.IsChecked == true && comboBoxCh2.SelectedItem != null)
            {
                NewMeasureTab.SelectedIndex = 1;
                ClearGraph();
                switch (comboBoxCh2.SelectedItem.ToString())
                {
                    case "Linear Sweep Voltammetry":
                        if (string.IsNullOrEmpty(LSVstartVBox2.Text) ||
                            string.IsNullOrEmpty(LSVfinalVBox2.Text) ||
                            string.IsNullOrEmpty(LSVstepVBox2.Text) ||
                            string.IsNullOrEmpty(LSVtimeStepBox2.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else
                        {
                            parametersString += "2" + "LSV" + ";" + LSVstartVBox2.Text + ";" + LSVfinalVBox2.Text + ";" + LSVstepVBox2.Text + ";" + LSVtimeStepBox2.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("LSV Channel 2 Parameters Sent");
                        }
                        break;

                    case "Cyclic Voltammetry":
                        if (string.IsNullOrEmpty(CVstartVBox2.Text) ||
                            string.IsNullOrEmpty(CVpeakV1Box2.Text) ||
                            string.IsNullOrEmpty(CVpeakV2Box2.Text) ||
                            string.IsNullOrEmpty(CVfinalVBox2.Text) ||
                            string.IsNullOrEmpty(CVstepVBox2.Text) ||
                            string.IsNullOrEmpty(CVtimeStepBox2.Text) ||
                            string.IsNullOrEmpty(CVcycleBox2.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else { 
                            parametersString += "2" + "CV" + ";" + CVstartVBox2.Text + ";" + CVpeakV1Box2.Text + ";" + CVpeakV2Box2.Text + ";" + CVfinalVBox2.Text + ";" + CVstepVBox2.Text + ";" + CVtimeStepBox2.Text + ";" + CVcycleBox2.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("CV Channel 2 Parameters Sent");
                        }
                        break;

                    case "Differential Pulse Voltammetry":
                        if (string.IsNullOrEmpty(DPVstartVBox2.Text) ||
                            string.IsNullOrEmpty(DPVfinalVBox2.Text) ||
                            string.IsNullOrEmpty(DPVstepVBox2.Text) ||
                            string.IsNullOrEmpty(DPVpulseBox2.Text) ||
                            string.IsNullOrEmpty(DPVpulseTimeBox2.Text) ||
                            string.IsNullOrEmpty(DPVbaseTimeBox2.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else
                        {
                            parametersString += "2" + "DPV" + ";" + DPVstartVBox2.Text + ";" + DPVfinalVBox2.Text + ";" + DPVstepVBox2.Text + ";" + DPVpulseBox2.Text + ";" + DPVpulseTimeBox2.Text + ";" + DPVbaseTimeBox2.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("DPV Channel 2 Parameters Sent");
                        }
                        
                        break;

                    case "Square Wave Voltammetry":
                        if (string.IsNullOrEmpty(SWVstartVBox2.Text) ||
                            string.IsNullOrEmpty(SWVfinalVBox2.Text) ||
                            string.IsNullOrEmpty(SWVstepVBox2.Text) ||
                            string.IsNullOrEmpty(SWVAmpBox2.Text) ||
                            string.IsNullOrEmpty(SWVtimeStepBox2.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else 
                        {
                            parametersString += "2" + "SWV" + ";" + SWVstartVBox2.Text + ";" + SWVfinalVBox2.Text + ";" + SWVstepVBox2.Text + ";" + SWVAmpBox2.Text + ";" + SWVtimeStepBox2.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("SWV Channel 2 Parameters Sent");
                        }
                        
                        break;

                    case "Chronopotentiometry":
                        if (string.IsNullOrEmpty(CPcurrentBox2.Text) ||
                            string.IsNullOrEmpty(CPsampleTBox2.Text) ||
                            string.IsNullOrEmpty(CPsamplePBox2.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else 
                        {
                            parametersString += "2" + "CP" + ";" + CPcurrentBox2.Text + ";" + CPsampleTBox2.Text + ";" + CPsamplePBox2.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("CP Channel 2 Parameters Sent");
                        }
                        
                        break;

                    case "Linear Sweep Potentiometry":
                        if (string.IsNullOrEmpty(LSPstartIBox2.Text) ||
                            string.IsNullOrEmpty(LSPfinalIBox2.Text) ||
                            string.IsNullOrEmpty(LSPstepIBox2.Text) ||
                            string.IsNullOrEmpty(LSPtimeStepBox2.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else 
                        {
                            parametersString += "2" + "LSP" + ";" + LSPstartIBox2.Text + ";" + LSPfinalIBox2.Text + ";" + LSPstepIBox2.Text + ";" + LSPtimeStepBox2.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("LSP Channel 2 Parameters Sent");
                        }
                        
                        break;

                    case "Cyclic Potentiometry":
                        if (string.IsNullOrEmpty(CyPstartIBox2.Text) ||
                            string.IsNullOrEmpty(CyPpeakI1Box2.Text) ||
                            string.IsNullOrEmpty(CyPpeakI2Box2.Text) ||
                            string.IsNullOrEmpty(CyPfinalIBox2.Text) ||
                            string.IsNullOrEmpty(CyPstepIBox2.Text) ||
                            string.IsNullOrEmpty(CyPtimeStepBox2.Text) ||
                            string.IsNullOrEmpty(CyPcycleBox2.Text))
                        {
                            MessageBox.Show("You must fill all the parameters.", "Error");
                            break;
                        }
                        else
                        {
                            parametersString += "2" + "CyP" + ";" + CyPstartIBox2.Text + ";" + CyPpeakI1Box2.Text + ";" + CyPpeakI2Box2.Text + ";" + CyPfinalIBox2.Text + ";" + CyPstepIBox2.Text + ";" + CyPtimeStepBox2.Text + ";" + CyPcycleBox2.Text;
                            _serialPort.Write(parametersString);
                            Console.WriteLine("CyP Channel 2 Parameters Sent");
                        }
                        
                        break;
                    default:
                        MessageBox.Show("Please select a valid technique for Channel 2.");
                        break;
                }
            }
            Console.WriteLine("Parameters String: " + parametersString);
        }

        private void Channel1Check_Changed(object sender, RoutedEventArgs e)
        {
            if (channel1Check.IsChecked == true)
            {
                comboBoxCh1.IsEnabled = true;
            }
            else
            {
                comboBoxCh1.IsEnabled = false;
                if (Ch1ParametersPanel != null)
                {
                    Ch1ParametersPanel.Children.Clear();
                }
                comboBoxCh1.SelectedItem = null;
                if (channel2Check.IsChecked == false) SubmitButton.IsEnabled = false;
            }
        }
        private void Channel2Check_Changed(object sender, RoutedEventArgs e)
        {
            if (channel2Check.IsChecked == true)
            {
                comboBoxCh2.IsEnabled = true;
            }
            else
            {
                comboBoxCh2.IsEnabled = false;
                if (Ch2ParametersPanel != null)
                {
                    Ch2ParametersPanel.Children.Clear();
                }
                comboBoxCh2.SelectedItem = null;
                if (channel1Check.IsChecked == false) SubmitButton.IsEnabled = false;
            }
        }
        private void ComboBoxCh1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (channel1Check.IsChecked == true)
            {
                string selectedContent = comboBoxCh1.SelectedItem.ToString();
                Console.WriteLine("LSV Selected");

                switch (selectedContent)
                {
                    case "Linear Sweep Voltammetry":
                        if (Ch1ParametersPanel != null)
                        {
                            Ch1ParametersPanel.Children.Clear();
                        }
                        if (Ch1ParametersLabelPanel != null)
                        {
                            Ch1ParametersLabelPanel.Children.Clear();
                            Ch1ParametersTextPanel.Children.Clear();
                        }
                        LSV_CreateConfigPanelItemsCh1();
                        break;
                    case "Cyclic Voltammetry":
                        if (Ch1ParametersPanel != null)
                        {
                            Ch1ParametersPanel.Children.Clear();
                        }
                        if (Ch1ParametersLabelPanel != null)
                        {
                            Ch1ParametersLabelPanel.Children.Clear();
                            Ch1ParametersTextPanel.Children.Clear();
                        }
                        CV_CreateConfigPanelItemsCh1();
                        break;
                    case "Differential Pulse Voltammetry":
                        if (Ch1ParametersPanel != null)
                        {
                            Ch1ParametersPanel.Children.Clear();
                        }
                        if (Ch1ParametersLabelPanel != null)
                        {
                            Ch1ParametersLabelPanel.Children.Clear();
                            Ch1ParametersTextPanel.Children.Clear();
                        }
                        DPV_CreateConfigPanelItemsCh1();
                        break;
                    case "Square Wave Voltammetry":
                        if (Ch1ParametersPanel != null)
                        {
                            Ch1ParametersPanel.Children.Clear();
                        }
                        if (Ch1ParametersLabelPanel != null)
                        {
                            Ch1ParametersLabelPanel.Children.Clear();
                            Ch1ParametersTextPanel.Children.Clear();
                        }
                        SWV_CreateConfigPanelItemsCh1();
                        break;
                    case "Chronopotentiometry":
                        if (Ch1ParametersPanel != null)
                        {
                            Ch1ParametersPanel.Children.Clear();
                        }
                        if (Ch1ParametersLabelPanel != null)
                        {
                            Ch1ParametersLabelPanel.Children.Clear();
                            Ch1ParametersTextPanel.Children.Clear();
                        }
                        CP_CreateConfigPanelItemsCh1();
                        break;
                    case "Linear Sweep Potentiometry":
                        if (Ch1ParametersPanel != null)
                        {
                            Ch1ParametersPanel.Children.Clear();
                        }
                        if (Ch1ParametersLabelPanel != null)
                        {
                            Ch1ParametersLabelPanel.Children.Clear();
                            Ch1ParametersTextPanel.Children.Clear();
                        }
                        LSP_CreateConfigPanelItemsCh1();
                        break;
                    case "Cyclic Potentiometry":
                        if (Ch1ParametersPanel != null)
                        {
                            Ch1ParametersPanel.Children.Clear();
                        }
                        if (Ch1ParametersLabelPanel != null)
                        {
                            Ch1ParametersLabelPanel.Children.Clear();
                            Ch1ParametersTextPanel.Children.Clear();
                        }
                        CyP_CreateConfigPanelItemsCh1();
                        break;
                    default:

                        break;
                }

            }

        }
        
        private void ComboBoxCh2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (channel2Check.IsChecked == true)
            {
                string selectedContent = comboBoxCh2.SelectedItem.ToString();
                Console.WriteLine("LSV Selected");

                switch (selectedContent)
                {
                    case "Linear Sweep Voltammetry":
                        //DestroyItems
                        if (Ch2ParametersPanel != null)
                        {
                            Ch2ParametersPanel.Children.Clear();
                        }
                        if (Ch2ParametersLabelPanel != null)
                        {
                            Ch2ParametersLabelPanel.Children.Clear();
                            Ch2ParametersTextPanel.Children.Clear();
                        }
                        LSV_CreateConfigPanelItemsCh2();
                        break;
                    case "Cyclic Voltammetry":
                        if (Ch2ParametersPanel != null)
                        {
                            Ch2ParametersPanel.Children.Clear();
                        }
                        if (Ch2ParametersLabelPanel != null)
                        {
                            Ch2ParametersLabelPanel.Children.Clear();
                            Ch2ParametersTextPanel.Children.Clear();
                        }
                        CV_CreateConfigPanelItemsCh2();
                        break;
                    case "Differential Pulse Voltammetry":
                        if (Ch2ParametersPanel != null)
                        {
                            Ch2ParametersPanel.Children.Clear();
                        }
                        if (Ch2ParametersLabelPanel != null)
                        {
                            Ch2ParametersLabelPanel.Children.Clear();
                            Ch2ParametersTextPanel.Children.Clear();
                        }
                        DPV_CreateConfigPanelItemsCh2();
                        break;
                    case "Square Wave Voltammetry":
                        if (Ch2ParametersPanel != null)
                        {
                            Ch2ParametersPanel.Children.Clear();
                        }
                        if (Ch2ParametersLabelPanel != null)
                        {
                            Ch2ParametersLabelPanel.Children.Clear();
                            Ch2ParametersTextPanel.Children.Clear();
                        }
                        SWV_CreateConfigPanelItemsCh2();
                        break;
                    case "Chronopotentiometry":
                        if (Ch2ParametersPanel != null)
                        {
                            Ch2ParametersPanel.Children.Clear();
                        }
                        if (Ch2ParametersLabelPanel != null)
                        {
                            Ch2ParametersLabelPanel.Children.Clear();
                            Ch2ParametersTextPanel.Children.Clear();
                        }
                        CP_CreateConfigPanelItemsCh2();
                        break;
                    case "Linear Sweep Potentiometry":
                        if (Ch2ParametersPanel != null)
                        {
                            Ch2ParametersPanel.Children.Clear();
                        }
                        if (Ch2ParametersLabelPanel != null)
                        {
                            Ch2ParametersLabelPanel.Children.Clear();
                            Ch2ParametersTextPanel.Children.Clear();
                        }
                        LSP_CreateConfigPanelItemsCh2();
                        break;
                    case "Cyclic Potentiometry":
                        if (Ch2ParametersPanel != null)
                        {
                            Ch2ParametersPanel.Children.Clear();
                        }
                        if (Ch2ParametersLabelPanel != null)
                        {
                            Ch2ParametersLabelPanel.Children.Clear();
                            Ch2ParametersTextPanel.Children.Clear();
                        }
                        CyP_CreateConfigPanelItemsCh2();
                        break;
                    default:

                        break;

                }
            }
            
        }

        private void ToggleConnection(object sender, RoutedEventArgs e)
        {
            if (_serialPort != null)
            {
                Console.WriteLine("disconnect");
                SubmitButton.IsEnabled = false;
                _serialPort.Close();
                _serialPort = null;
                COMconnectButton.Content = "Connect";
            }
            else if (!String.IsNullOrEmpty(COMselect.Text))
            {
                try
                {
                    Console.WriteLine("connect");
                    selectedSerialPort = COMselect.Text;
                    SubmitButton.IsEnabled = true;
                    InitializeSerialPort();
                    COMconnectButton.Content = "Disconnect";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao conectar: {ex.Message}");
                }
            }
        }

        private void InitializeSerialPort()
        {
            try
            {
                _serialPort = new SerialPort(selectedSerialPort, 115200);
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
            }
            catch
            {
                MessageBox.Show("Nonexistent COM or incomplete form.", "Error");
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = _serialPort.ReadExisting();

                Console.WriteLine(data);
                data = data.Trim();

                string[] parts = data.Split(';');

                if (parts.Length == 4) PlotDualChannel(parts);
                else if (parts.Length == 3) PlotSingleChannel(parts);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PlotSingleChannel(string[] parts)
        {
            string technique = parts[0];

            double value1 = double.Parse(parts[1]);
            double value2 = double.Parse(parts[2]);

            if (technique.Equals("p"))
            {
                plot1X.Title = "Voltage (V)";
                plot1Y.Title = "Current (A)";
                dataPoints.Add(new Tuple<double, double>(value1, value2));

                series.Points.Add(new DataPoint(value1, value2));
                plotModel.InvalidatePlot(true);
            }
            else if (technique == "g")
            {
                plot1X.Title = "Current (mA)";
                plot1Y.Title = "Voltage (V)";
                dataPoints.Add(new Tuple<double, double>(value1, value2));

                series.Points.Add(new DataPoint(value1, value2));
                plotModel.InvalidatePlot(true);
            }
        }

        private void PlotDualChannel(string[] parts)
        {
            string channel = parts[0];

            string technique = parts[1];

            double value1 = double.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture);
            double value2 = double.Parse(parts[3], System.Globalization.CultureInfo.InvariantCulture);

            if (channel == "ch1")
            {
                if (technique == "p")
                {
                    plot1X.Title = "Voltage (V)";
                    plot1Y.Title = "Current (mA)";
                    dataPoints.Add(new Tuple<double, double>(value1, value2));
                    series.Points.Add(new DataPoint(value1, value2));
                    plotModel.InvalidatePlot(true);
                }
                else if (technique == "g")
                {
                    plot1X.Title = "Current (mA)";
                    plot1Y.Title = "Voltage (V)";
                    dataPoints.Add(new Tuple<double, double>(value1, value2));
                    series.Points.Add(new DataPoint(value1, value2));
                    plotModel.InvalidatePlot(true);
                }
            }
            else if (channel == "ch2") 
            {
                if (technique == "p")
                {
                    plot2X.Title = "Voltage (V)";
                    plot2Y.Title = "Current (mA)";
                    dataPoints2.Add(new Tuple<double, double>(value1, value2));
                    series2.Points.Add(new DataPoint(value1, value2));
                    plotModel2.InvalidatePlot(true);
                }
                else if (technique == "g")
                {
                    plot2X.Title = "Current (mA)";
                    plot2Y.Title = "Voltage (V)";
                    dataPoints2.Add(new Tuple<double, double>(value1, value2));
                    series2.Points.Add(new DataPoint(value1, value2));
                    plotModel2.InvalidatePlot(true);
                }
            }            
        }

        private void StopMeasure_Click(object sender, RoutedEventArgs e) 
        {
            if (_serialPort != null)
            {
                _serialPort.Write("stopMeasure");
                MessageBox.Show("Measure aborted.");
            }
        }

        private void ClearGraph()
        {
            dataPoints.Clear();
            dataPoints2.Clear();

            Dispatcher.Invoke(() => plotModel.Series.Clear());
            Dispatcher.Invoke(() => plotModel2.Series.Clear());

            plotModel.InvalidatePlot(true);
            plotModel2.InvalidatePlot(true);
            series = null;
            series2 = null;

            series = new LineSeries
            {
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                Color = OxyColors.RoyalBlue
            };
            series2 = new LineSeries
            {
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                Color = OxyColors.RoyalBlue
            };

            plotModel.Series.Add(series);
            plotModel2.Series.Add(series2);
            GraphSMUChannel1.Model = plotModel;
            GraphSMUChannel2.Model = plotModel2;
        }
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            ClearEveryTabControl();
            HomeTabControl = new TabControl
            {
                Margin = new Thickness(5)
            };
            Grid.SetRow(HomeTabControl, 2);
            Grid.SetColumn(HomeTabControl, 1);
            UserGrid.Children.Add(HomeTabControl);

            StartTab = new TabItem
            {
                Header = "Start"
            };
            HomeTabControl.Items.Add(StartTab);
        }
        private void ClearEveryTabControl()
        {
            UserGrid.Children.Remove(NewMeasureTab);
        }
        private void SaveMeasurement_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(parametersString))
            {
                try
                {
                    string filePath;
                    string defaultName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";

                    var saveFileDialog = new SaveFileDialog
                    {
                        Title = "Save measure configuration",
                        Filter = "TXT File (*.txt)|*.txt",
                        DefaultExt = "txt",
                        FileName = defaultName
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        filePath = saveFileDialog.FileName;


                        File.WriteAllText(filePath, parametersString);

                        Console.WriteLine($"Arquivo salvo em: {filePath}");

                        MessageBox.Show($"Measure configuration saved at {filePath}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao salvar o arquivo: {ex.Message}");
                }
            }
            else MessageBox.Show("You must perform a measurement before saving its configuration.");
        }
        private void ReloadMeasurementHistory_Click(object sender, RoutedEventArgs e)
        {
            LoadMeasurementConfigs();
        }
        private void LoadMeasurementConfigs()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string configFolderPath = System.IO.Path.Combine(documentsPath, "SMU");

            if (!Directory.Exists(configFolderPath))
            {
                Directory.CreateDirectory(configFolderPath);
            }

            List<MeasurementConfig> configs = new List<MeasurementConfig>();

            foreach (var file in Directory.GetFiles(configFolderPath, "*.txt"))
            {
                var info = new FileInfo(file);
                configs.Add(new MeasurementConfig
                {
                    Name = System.IO.Path.GetFileNameWithoutExtension(file),
                    Timestamp = info.LastWriteTime.ToString("dd/MM/yyyy HH:mm"),
                    FilePath = file
                });
            }
            HistoryList.ItemsSource = configs;
        }
        private void LoadMeasurement_Click(object sender, RoutedEventArgs e)
        {
            if (Ch1ParametersPanel != null)
            {
                Ch1ParametersPanel.Children.Clear();
            }
            if (Ch1ParametersLabelPanel != null)
            {
                Ch1ParametersLabelPanel.Children.Clear();
                Ch1ParametersTextPanel.Children.Clear();
            }
            if (Ch2ParametersPanel != null)
            {
                Ch2ParametersPanel.Children.Clear();
            }
            if (Ch2ParametersLabelPanel != null)
            {
                Ch2ParametersLabelPanel.Children.Clear();
                Ch2ParametersTextPanel.Children.Clear();
            }

            var selectedConfig = HistoryList.SelectedItem as MeasurementConfig;

            if (selectedConfig != null)
            {
                string fileContent = File.ReadAllText(selectedConfig.FilePath);
                Console.WriteLine(fileContent);
                ReadMeasurementConfig(fileContent);
                NewMeasure_Click(null, null);
            }
            else
            {
                MessageBox.Show("Please select a configuration first.", "No Selection");
            }
        }
        private void ReadMeasurementConfig(string fileContent)
        {

            string[] channels = fileContent.Split(',');

            foreach (string channelData in channels)
            {
                if (string.IsNullOrWhiteSpace(channelData))
                    continue;

                bool isChannel2 = channelData.StartsWith("2");
                string data = isChannel2 ? channelData.Substring(1) : channelData;

                string[] parts = data.Split(';');
                string technique = parts[0];

                switch (technique)
                {
                    case "LSV":
                        if (!isChannel2)
                        {
                            LSVstartVBox.Text = parts[1];
                            LSVfinalVBox.Text = parts[2];
                            LSVstepVBox.Text = parts[3];
                            LSVtimeStepBox.Text = parts[4];
                        }
                        else
                        {
                            LSVstartVBox2.Text = parts[1];
                            LSVfinalVBox2.Text = parts[2];
                            LSVstepVBox2.Text = parts[3];
                            LSVtimeStepBox2.Text = parts[4];
                        }
                        break;

                    case "CV":
                        if (!isChannel2)
                        {
                            CVstartVBox.Text = parts[1];
                            CVpeakV1Box.Text = parts[2];
                            CVpeakV2Box.Text = parts[3];
                            CVfinalVBox.Text = parts[4];
                            CVstepVBox.Text = parts[5];
                            CVtimeStepBox.Text = parts[6];
                            CVcycleBox.Text = parts[7];
                        }
                        else
                        {
                            CVstartVBox2.Text = parts[1];
                            CVpeakV1Box2.Text = parts[2];
                            CVpeakV2Box2.Text = parts[3];
                            CVfinalVBox2.Text = parts[4];
                            CVstepVBox2.Text = parts[5];
                            CVtimeStepBox2.Text = parts[6];
                            CVcycleBox2.Text = parts[7];
                        }
                        break;

                    case "DPV":
                        if (!isChannel2)
                        {
                            DPVstartVBox.Text = parts[1];
                            DPVfinalVBox.Text = parts[2];
                            DPVstepVBox.Text = parts[3];
                            DPVpulseBox.Text = parts[4];
                            DPVpulseTimeBox.Text = parts[5];
                            DPVbaseTimeBox.Text = parts[6];
                        }
                        else
                        {
                            DPVstartVBox2.Text = parts[1];
                            DPVfinalVBox2.Text = parts[2];
                            DPVstepVBox2.Text = parts[3];
                            DPVpulseBox2.Text = parts[4];
                            DPVpulseTimeBox2.Text = parts[5];
                            DPVbaseTimeBox2.Text = parts[6];
                        }
                        break;

                    case "SWV":
                        if (!isChannel2)
                        {
                            SWVstartVBox.Text = parts[1];
                            SWVfinalVBox.Text = parts[2];
                            SWVstepVBox.Text = parts[3];
                            SWVAmpBox.Text = parts[4];
                            SWVtimeStepBox.Text = parts[5];
                        }
                        else
                        {
                            SWVstartVBox2.Text = parts[1];
                            SWVfinalVBox2.Text = parts[2];
                            SWVstepVBox2.Text = parts[3];
                            SWVAmpBox2.Text = parts[4];
                            SWVtimeStepBox2.Text = parts[5];
                        }
                        break;

                    case "CP":
                        if (!isChannel2)
                        {
                            CPcurrentBox.Text = parts[1];
                            CPsampleTBox.Text = parts[2];
                            CPsamplePBox.Text = parts[3];
                        }
                        else
                        {
                            CPcurrentBox2.Text = parts[1];
                            CPsampleTBox2.Text = parts[2];
                            CPsamplePBox2.Text = parts[3];
                        }
                        break;

                    case "LSP":
                        if (!isChannel2)
                        {
                            LSPstartIBox.Text = parts[1];
                            LSPfinalIBox.Text = parts[2];
                            LSPstepIBox.Text = parts[3];
                            LSPtimeStepBox.Text = parts[4];
                        }
                        else
                        {
                            LSPstartIBox2.Text = parts[1];
                            LSPfinalIBox2.Text = parts[2];
                            LSPstepIBox2.Text = parts[3];
                            LSPtimeStepBox2.Text = parts[4];
                        }
                        break;

                    case "CyP":
                        if (!isChannel2)
                        {
                            CyPstartIBox.Text = parts[1];
                            CyPpeakI1Box.Text = parts[2];
                            CyPpeakI2Box.Text = parts[3];
                            CyPfinalIBox.Text = parts[4];
                            CyPstepIBox.Text = parts[5];
                            CyPtimeStepBox.Text = parts[6];
                            CyPcycleBox.Text = parts[7];
                        }
                        else
                        {
                            CyPstartIBox2.Text = parts[1];
                            CyPpeakI1Box2.Text = parts[2];
                            CyPpeakI2Box2.Text = parts[3];
                            CyPfinalIBox2.Text = parts[4];
                            CyPstepIBox2.Text = parts[5];
                            CyPtimeStepBox2.Text = parts[6];
                            CyPcycleBox2.Text = parts[7];
                        }
                        break;
                }
                for (int i = 0; i < parts.Length; i++)
                {
                    Console.WriteLine($"parts[{i}] = {parts[i]}");
                }
            }
        }
        private void DeleteMeasurement_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryList.SelectedItem is MeasurementConfig selectedConfig)
            {
                if (File.Exists(selectedConfig.FilePath))
                {
                    var result = MessageBox.Show(
                        $"Are you sure you want to delete '{selectedConfig.Name}'?",
                        "Confirm Delete",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        File.Delete(selectedConfig.FilePath);
                        MessageBox.Show("File deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadMeasurementConfigs();
                    }
                }
                else
                {
                    MessageBox.Show("File not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a file to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ExportGraph_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Export Graph Clicked");
        }
        
        #region Save as image
        private void SaveGraphAsImage_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save Chart as PNG",
                Filter = "PNG Files (*.png)|*.png",
                DefaultExt = "png",
                FileName = "image.png"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                ExportToPng(filePath);
            }
        }

        private void ExportToPng(string filePath)
        {
            plotModel.Background = OxyColors.White;
            var pngExporter = new PngExporter { Width = 1280, Height = 960 };
            pngExporter.ExportToFile(plotModel, filePath);
            //MessageBox.Show("Chart successfully saved as an image in: " + filePath);
        }
        private void SaveGraphAsImage2_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save Chart as PNG",
                Filter = "PNG Files (*.png)|*.png",
                DefaultExt = "png",
                FileName = "image.png"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                ExportToPng2(filePath);
            }
        }

        private void ExportToPng2(string filePath)
        {
            plotModel2.Background = OxyColors.White;
            var pngExporter = new PngExporter { Width = 1280, Height = 960 };
            pngExporter.ExportToFile(plotModel2, filePath);
            //MessageBox.Show("Chart successfully saved as an image in: " + filePath);
        }
        #endregion
        #region Save as csv
        private void SaveDataAsCsv_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save data as CSV",
                Filter = "CSV Files (*.csv)|*.csv",
                DefaultExt = "csv",
                FileName = "data.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveToCsv(saveFileDialog.FileName);
                //MessageBox.Show("Data successfully saved to: " + saveFileDialog.FileName);
            }
        }

        private void SaveToCsv(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                //writer.WriteLine(measureData);

                foreach (var point in dataPoints)
                {
                    writer.WriteLine($"{point.Item1},{point.Item2}");
                }
            }
        }

        private void SaveDataAsCsv_Click2(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save data as CSV",
                Filter = "CSV Files (*.csv)|*.csv",
                DefaultExt = "csv",
                FileName = "data.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveToCsv2(saveFileDialog.FileName);
                //MessageBox.Show("Data successfully saved to: " + saveFileDialog.FileName);
            }
        }

        private void SaveToCsv2(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                //writer.WriteLine(measureData);

                foreach (var point in dataPoints2)
                {
                    writer.WriteLine($"{point.Item1},{point.Item2}");
                }
            }
        }
        #endregion
        private void LSV_CreateConfigPanelItemsCh1()
        {
            Ch1ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersPanel.Children.Add(Ch1ParametersLabelPanel);
            Ch1ParametersPanel.Children.Add(Ch1ParametersTextPanel);

            #region Initial Voltage
            LSVstartVLabel = new Label
            {
                Content = "Initial E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSVstartVBox.Width = 100;
            LSVstartVBox.Margin = new Thickness(5);
            LSVstartVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSVstartVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSVstartVBox.FontSize = 17;
            LSVstartVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(LSVstartVLabel);
            Ch1ParametersTextPanel.Children.Add(LSVstartVBox);
            #endregion
            #region Final Voltage
            LSVfinalVLabel = new Label
            {
                Content = "Final E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSVfinalVBox.Width = 100;
            LSVfinalVBox.Margin = new Thickness(5);
            LSVfinalVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSVfinalVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSVfinalVBox.FontSize = 17;
            LSVfinalVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(LSVfinalVLabel);
            Ch1ParametersTextPanel.Children.Add(LSVfinalVBox);
            #endregion
            #region Step Voltage
            LSVstepVLabel = new Label
            {
                Content = "Step size (mV):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSVstepVBox.Width = 100;
            LSVstepVBox.Margin = new Thickness(5);
            LSVstepVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSVstepVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSVstepVBox.FontSize = 17;
            LSVstepVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(LSVstepVLabel);
            Ch1ParametersTextPanel.Children.Add(LSVstepVBox);
            #endregion
            #region Time Step
            LSVtimeStepLabel = new Label
            {
                Content = "Scan rate (mV/s)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSVtimeStepBox.Width = 100;
            LSVtimeStepBox.Margin = new Thickness(5);
            LSVtimeStepBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSVtimeStepBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSVtimeStepBox.FontSize = 17;
            LSVtimeStepBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(LSVtimeStepLabel);
            Ch1ParametersTextPanel.Children.Add(LSVtimeStepBox);
            #endregion

            LSVsubmitButton = new Button
            {
                Content = "Start",
                Width = 150,
                FontSize = 15,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };

            //ParametersPanel.Children.Add(Ch1ParametersPanel);
            //LSVsubmitButton.Click += LSV_SendMeasureParameters;
        }
        private void LSV_CreateConfigPanelItemsCh2()
        {
            Ch2ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersPanel.Children.Add(Ch2ParametersLabelPanel);
            Ch2ParametersPanel.Children.Add(Ch2ParametersTextPanel);

            #region Initial Voltage
            LSVstartVLabel = new Label
            {
                Content = "Initial E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSVstartVBox2.Width = 100;
            LSVstartVBox2.Margin = new Thickness(5);
            LSVstartVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSVstartVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSVstartVBox2.FontSize = 17;
            LSVstartVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(LSVstartVLabel);
            Ch2ParametersTextPanel.Children.Add(LSVstartVBox2);
            #endregion
            #region Final Voltage
            LSVfinalVLabel = new Label
            {
                Content = "Final E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSVfinalVBox2.Width = 100;
            LSVfinalVBox2.Margin = new Thickness(5);
            LSVfinalVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSVfinalVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSVfinalVBox2.FontSize = 17;
            LSVfinalVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(LSVfinalVLabel);
            Ch2ParametersTextPanel.Children.Add(LSVfinalVBox2);
            #endregion
            #region Step Voltage
            LSVstepVLabel = new Label
            {
                Content = "Step size (mV):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSVstepVBox2.Width = 100;
            LSVstepVBox2.Margin = new Thickness(5);
            LSVstepVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSVstepVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSVstepVBox2.FontSize = 17;
            LSVstepVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(LSVstepVLabel);
            Ch2ParametersTextPanel.Children.Add(LSVstepVBox2);
            #endregion
            #region Time Step
            LSVtimeStepLabel = new Label
            {
                Content = "Scan rate (mV/s)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSVtimeStepBox2.Width = 100;
            LSVtimeStepBox2.Margin = new Thickness(5);
            LSVtimeStepBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSVtimeStepBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSVtimeStepBox2.FontSize = 17;
            LSVtimeStepBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(LSVtimeStepLabel);
            Ch2ParametersTextPanel.Children.Add(LSVtimeStepBox2);
            #endregion

            LSVsubmitButton = new Button
            {
                Content = "Start",
                Width = 150,
                FontSize = 15,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };

            //ParametersPanel.Children.Add(Ch2ParametersPanel);
            //LSVsubmitButton.Click += LSV_SendMeasureParameters;
        }
        private void CV_CreateConfigPanelItemsCh1()
        {
            Ch1ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersPanel.Children.Add(Ch1ParametersLabelPanel);
            Ch1ParametersPanel.Children.Add(Ch1ParametersTextPanel);

            #region Initial Voltage
            CVstartVLabel = new Label
            {
                Content = "Initial E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            
            CVstartVBox.Width = 100;
            CVstartVBox.Margin = new Thickness(5);
            CVstartVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVstartVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVstartVBox.FontSize = 17;
            CVstartVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CVstartVLabel);
            Ch1ParametersTextPanel.Children.Add(CVstartVBox);

            #endregion
            #region Peak1 Voltage
            CVpeakV1Label = new Label
            {
                Content = "Peak1 E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVpeakV1Box.Width = 100;
            CVpeakV1Box.Margin = new Thickness(5);
            CVpeakV1Box.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVpeakV1Box.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVpeakV1Box.FontSize = 17;
            CVpeakV1Box.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CVpeakV1Label);
            Ch1ParametersTextPanel.Children.Add(CVpeakV1Box);

            #endregion
            #region Peak2 Voltage
            CVpeakV2Label = new Label
            {
                Content = "Peak2 E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVpeakV2Box.Width = 100;
            CVpeakV2Box.Margin = new Thickness(5);
            CVpeakV2Box.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVpeakV2Box.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVpeakV2Box.FontSize = 17;
            CVpeakV2Box.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CVpeakV2Label);
            Ch1ParametersTextPanel.Children.Add(CVpeakV2Box);
            
            #endregion
            #region Final Voltage
            CVfinalVLabel = new Label
            {
                Content = "Final E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVfinalVBox.Width = 100;
            CVfinalVBox.Margin = new Thickness(5);
            CVfinalVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVfinalVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVfinalVBox.FontSize = 17;
            CVfinalVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CVfinalVLabel);
            Ch1ParametersTextPanel.Children.Add(CVfinalVBox);

            #endregion
            #region Step Voltage
            CVstepVLabel = new Label
            {
                Content = "Step size (mV):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVstepVBox.Width = 100;
            CVstepVBox.Margin = new Thickness(5);
            CVstepVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVstepVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVstepVBox.FontSize = 17;
            CVstepVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CVstepVLabel);
            Ch1ParametersTextPanel.Children.Add(CVstepVBox);

            #endregion
            #region Time Step
            CVtimeStepLabel = new Label
            {
                Content = "Scan rate (mV/s):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVtimeStepBox.Width = 100;
            CVtimeStepBox.Margin = new Thickness(5);
            CVtimeStepBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVtimeStepBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVtimeStepBox.FontSize = 17;
            CVtimeStepBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CVtimeStepLabel);
            Ch1ParametersTextPanel.Children.Add(CVtimeStepBox);

            #endregion
            #region Cycle
            CVcycleLabel = new Label
            {
                Content = "Cycles:",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVcycleBox.Width = 100;
            CVcycleBox.Margin = new Thickness(5);
            CVcycleBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVcycleBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVcycleBox.FontSize = 17;
            CVcycleBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CVcycleLabel);
            Ch1ParametersTextPanel.Children.Add(CVcycleBox);

            #endregion

        }
        private void CV_CreateConfigPanelItemsCh2()
        {
            Ch2ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersPanel.Children.Add(Ch2ParametersLabelPanel);
            Ch2ParametersPanel.Children.Add(Ch2ParametersTextPanel);

            #region Initial Voltage
            CVstartVLabel = new Label
            {
                Content = "Initial E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVstartVBox2.Width = 100;
            CVstartVBox2.Margin = new Thickness(5);
            CVstartVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVstartVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVstartVBox2.FontSize = 17;
            CVstartVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CVstartVLabel);
            Ch2ParametersTextPanel.Children.Add(CVstartVBox2);

            #endregion
            #region Peak1 Voltage
            CVpeakV1Label = new Label
            {
                Content = "Peak1 E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVpeakV1Box2.Width = 100;
            CVpeakV1Box2.Margin = new Thickness(5);
            CVpeakV1Box2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVpeakV1Box2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVpeakV1Box2.FontSize = 17;
            CVpeakV1Box2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CVpeakV1Label);
            Ch2ParametersTextPanel.Children.Add(CVpeakV1Box2);

            #endregion
            #region Peak2 Voltage
            CVpeakV2Label = new Label
            {
                Content = "Peak2 E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVpeakV2Box2.Width = 100;
            CVpeakV2Box2.Margin = new Thickness(5);
            CVpeakV2Box2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVpeakV2Box2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVpeakV2Box2.FontSize = 17;
            CVpeakV2Box2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CVpeakV2Label);
            Ch2ParametersTextPanel.Children.Add(CVpeakV2Box2);

            #endregion
            #region Final Voltage
            CVfinalVLabel = new Label
            {
                Content = "Final E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            
            CVfinalVBox2.Width = 100;
            CVfinalVBox2.Margin = new Thickness(5);
            CVfinalVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVfinalVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVfinalVBox2.FontSize = 17;
            CVfinalVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CVfinalVLabel);
            Ch2ParametersTextPanel.Children.Add(CVfinalVBox2);

            #endregion
            #region Step Voltage
            CVstepVLabel = new Label
            {
                Content = "Step size (mV):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVstepVBox2.Width = 100;
            CVstepVBox2.Margin = new Thickness(5);
            CVstepVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVstepVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVstepVBox2.FontSize = 17;
            CVstepVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CVstepVLabel);
            Ch2ParametersTextPanel.Children.Add(CVstepVBox2);

            #endregion
            #region Time Step
            CVtimeStepLabel = new Label
            {
                Content = "Scan rate (mV/s):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVtimeStepBox2.Width = 100;
            CVtimeStepBox2.Margin = new Thickness(5);
            CVtimeStepBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVtimeStepBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVtimeStepBox2.FontSize = 17;
            CVtimeStepBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CVtimeStepLabel);
            Ch2ParametersTextPanel.Children.Add(CVtimeStepBox2);

            #endregion
            #region Cycle
            CVcycleLabel = new Label
            {
                Content = "Cycles:",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CVcycleBox2.Width = 100;
            CVcycleBox2.Margin = new Thickness(5);
            CVcycleBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CVcycleBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CVcycleBox2.FontSize = 17;
            CVcycleBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CVcycleLabel);
            Ch2ParametersTextPanel.Children.Add(CVcycleBox2);

            #endregion

        }
        private void SWV_CreateConfigPanelItemsCh1()
        {
            Ch1ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersPanel.Children.Add(Ch1ParametersLabelPanel);
            Ch1ParametersPanel.Children.Add(Ch1ParametersTextPanel);
            #region Initial Voltage
            SWVstartVLabel = new Label
            {
                Content = "Initial E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            SWVstartVBox.Width = 100;
            SWVstartVBox.Margin = new Thickness(5);
            SWVstartVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            SWVstartVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SWVstartVBox.FontSize = 17;
            SWVstartVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(SWVstartVLabel);
            Ch1ParametersTextPanel.Children.Add(SWVstartVBox);

            #endregion
            #region Final Voltage
            SWVfinalVLabel = new Label
            {
                Content = "Final E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            SWVfinalVBox.Width = 100;
            SWVfinalVBox.Margin = new Thickness(5);
            SWVfinalVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            SWVfinalVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SWVfinalVBox.FontSize = 17;
            SWVfinalVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(SWVfinalVLabel);
            Ch1ParametersTextPanel.Children.Add(SWVfinalVBox);

            #endregion
            #region Step Voltage
            SWVstepVLabel = new Label
            {
                Content = "Step size (mV):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            SWVstepVBox.Width = 100;
            SWVstepVBox.Margin = new Thickness(5);
            SWVstepVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            SWVstepVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SWVstepVBox.FontSize = 17;
            SWVstepVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(SWVstepVLabel);
            Ch1ParametersTextPanel.Children.Add(SWVstepVBox);

            #endregion
            #region Amplitude
            SWVAmpLabel = new Label
            {
                Content = "Amplitude (mV)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            SWVAmpBox.Width = 100;
            SWVAmpBox.Margin = new Thickness(5);
            SWVAmpBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            SWVAmpBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SWVAmpBox.FontSize = 17;
            SWVAmpBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(SWVAmpLabel);
            Ch1ParametersTextPanel.Children.Add(SWVAmpBox);

            #endregion

            #region Time Step
            SWVtimeStepLabel = new Label
            {
                Content = "Frequency (Hz)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            SWVtimeStepBox.Width = 100;
            SWVtimeStepBox.Margin = new Thickness(5);
            SWVtimeStepBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            SWVtimeStepBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SWVtimeStepBox.FontSize = 17;
            SWVtimeStepBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(SWVtimeStepLabel);
            Ch1ParametersTextPanel.Children.Add(SWVtimeStepBox);

            #endregion

        }
        private void SWV_CreateConfigPanelItemsCh2()
        {
            Ch2ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersPanel.Children.Add(Ch2ParametersLabelPanel);
            Ch2ParametersPanel.Children.Add(Ch2ParametersTextPanel);
            #region Initial Voltage
            SWVstartVLabel = new Label
            {
                Content = "Initial E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            SWVstartVBox2.Width = 100;
            SWVstartVBox2.Margin = new Thickness(5);
            SWVstartVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            SWVstartVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SWVstartVBox2.FontSize = 17;
            SWVstartVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(SWVstartVLabel);
            Ch2ParametersTextPanel.Children.Add(SWVstartVBox2);

            #endregion
            #region Final Voltage
            SWVfinalVLabel = new Label
            {
                Content = "Final E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            SWVfinalVBox2.Width = 100;
            SWVfinalVBox2.Margin = new Thickness(5);
            SWVfinalVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            SWVfinalVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SWVfinalVBox2.FontSize = 17;
            SWVfinalVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(SWVfinalVLabel);
            Ch2ParametersTextPanel.Children.Add(SWVfinalVBox2);

            #endregion
            #region Step Voltage
            SWVstepVLabel = new Label
            {
                Content = "Step size (mV):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            SWVstepVBox2.Width = 100;
            SWVstepVBox2.Margin = new Thickness(5);
            SWVstepVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            SWVstepVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SWVstepVBox2.FontSize = 17;
            SWVstepVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(SWVstepVLabel);
            Ch2ParametersTextPanel.Children.Add(SWVstepVBox2);

            #endregion
            #region Amplitude
            SWVAmpLabel = new Label
            {
                Content = "Amplitude (mV)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            SWVAmpBox2.Width = 100;
            SWVAmpBox2.Margin = new Thickness(5);
            SWVAmpBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            SWVAmpBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SWVAmpBox2.FontSize = 17;
            SWVAmpBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(SWVAmpLabel);
            Ch2ParametersTextPanel.Children.Add(SWVAmpBox2);

            #endregion

            #region Time Step
            SWVtimeStepLabel = new Label
            {
                Content = "Frequency (Hz)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            SWVtimeStepBox2.Width = 100;
            SWVtimeStepBox2.Margin = new Thickness(5);
            SWVtimeStepBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            SWVtimeStepBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SWVtimeStepBox2.FontSize = 17;
            SWVtimeStepBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(SWVtimeStepLabel);
            Ch2ParametersTextPanel.Children.Add(SWVtimeStepBox2);

            #endregion

        }
        private void DPV_CreateConfigPanelItemsCh1()
        {
            Ch1ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersPanel.Children.Add(Ch1ParametersLabelPanel);
            Ch1ParametersPanel.Children.Add(Ch1ParametersTextPanel);
            #region Initial Voltage
            DPVstartVLabel = new Label
            {
                Content = "Initial E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVstartVBox.Width = 100;
            DPVstartVBox.Margin = new Thickness(5);
            DPVstartVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVstartVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVstartVBox.FontSize = 17;
            DPVstartVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(DPVstartVLabel);
            Ch1ParametersTextPanel.Children.Add(DPVstartVBox);

            #endregion
            #region Final Voltage
            DPVfinalVLabel = new Label
            {
                Content = "Final E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVfinalVBox.Width = 100;
            DPVfinalVBox.Margin = new Thickness(5);
            DPVfinalVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVfinalVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVfinalVBox.FontSize = 17;
            DPVfinalVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(DPVfinalVLabel);
            Ch1ParametersTextPanel.Children.Add(DPVfinalVBox);

            #endregion
            #region Step Voltage
            DPVstepVLabel = new Label
            {
                Content = "Step size (mV):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVstepVBox.Width = 100;
            DPVstepVBox.Margin = new Thickness(5);
            DPVstepVBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVstepVBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVstepVBox.FontSize = 17;
            DPVstepVBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(DPVstepVLabel);
            Ch1ParametersTextPanel.Children.Add(DPVstepVBox);

            #endregion
            #region Pulse
            DPVpulseLabel = new Label
            {
                Content = "Pulse E (mV)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVpulseBox.Width = 100;
            DPVpulseBox.Margin = new Thickness(5);
            DPVpulseBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVpulseBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVpulseBox.FontSize = 17;
            DPVpulseBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(DPVpulseLabel);
            Ch1ParametersTextPanel.Children.Add(DPVpulseBox);

            #endregion

            #region Pulse time
            DPVpulseTimeLabel = new Label
            {
                Content = "Pulse Time (ms)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVpulseTimeBox.Width = 100;
            DPVpulseTimeBox.Margin = new Thickness(5);
            DPVpulseTimeBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVpulseTimeBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVpulseTimeBox.FontSize = 17;
            DPVpulseTimeBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(DPVpulseTimeLabel);
            Ch1ParametersTextPanel.Children.Add(DPVpulseTimeBox);

            #endregion
            #region Base time
            DPVbaseTimeLabel = new Label
            {
                Content = "Base Time (ms)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVbaseTimeBox.Width = 100;
            DPVbaseTimeBox.Margin = new Thickness(5);
            DPVbaseTimeBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVbaseTimeBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVbaseTimeBox.FontSize = 17;
            DPVbaseTimeBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(DPVbaseTimeLabel);
            Ch1ParametersTextPanel.Children.Add(DPVbaseTimeBox);

            #endregion

        }
        private void DPV_CreateConfigPanelItemsCh2()
        {
            Ch2ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersPanel.Children.Add(Ch2ParametersLabelPanel);
            Ch2ParametersPanel.Children.Add(Ch2ParametersTextPanel);
            #region Initial Voltage
            DPVstartVLabel = new Label
            {
                Content = "Initial E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVstartVBox2.Width = 100;
            DPVstartVBox2.Margin = new Thickness(5);
            DPVstartVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVstartVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVstartVBox2.FontSize = 17;
            DPVstartVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(DPVstartVLabel);
            Ch2ParametersTextPanel.Children.Add(DPVstartVBox2);

            #endregion
            #region Final Voltage
            DPVfinalVLabel = new Label
            {
                Content = "Final E (V):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVfinalVBox2.Width = 100;
            DPVfinalVBox2.Margin = new Thickness(5);
            DPVfinalVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVfinalVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVfinalVBox2.FontSize = 17;
            DPVfinalVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(DPVfinalVLabel);
            Ch2ParametersTextPanel.Children.Add(DPVfinalVBox2);

            #endregion
            #region Step Voltage
            DPVstepVLabel = new Label
            {
                Content = "Step size (mV):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVstepVBox2.Width = 100;
            DPVstepVBox2.Margin = new Thickness(5);
            DPVstepVBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVstepVBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVstepVBox2.FontSize = 17;
            DPVstepVBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(DPVstepVLabel);
            Ch2ParametersTextPanel.Children.Add(DPVstepVBox2);

            #endregion
            #region Pulse
            DPVpulseLabel = new Label
            {
                Content = "Pulse E (mV)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVpulseBox2.Width = 100;
            DPVpulseBox2.Margin = new Thickness(5);
            DPVpulseBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVpulseBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVpulseBox2.FontSize = 17;
            DPVpulseBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(DPVpulseLabel);
            Ch2ParametersTextPanel.Children.Add(DPVpulseBox2);

            #endregion

            #region Pulse time
            DPVpulseTimeLabel = new Label
            {
                Content = "Pulse Time (ms)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVpulseTimeBox2.Width = 100;
            DPVpulseTimeBox2.Margin = new Thickness(5);
            DPVpulseTimeBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVpulseTimeBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVpulseTimeBox2.FontSize = 17;
            DPVpulseTimeBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(DPVpulseTimeLabel);
            Ch2ParametersTextPanel.Children.Add(DPVpulseTimeBox2);

            #endregion
            #region Base time
            DPVbaseTimeLabel = new Label
            {
                Content = "Base Time (ms)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            DPVbaseTimeBox2.Width = 100;
            DPVbaseTimeBox2.Margin = new Thickness(5);
            DPVbaseTimeBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            DPVbaseTimeBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DPVbaseTimeBox2.FontSize = 17;
            DPVbaseTimeBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(DPVbaseTimeLabel);
            Ch2ParametersTextPanel.Children.Add(DPVbaseTimeBox2);

            #endregion

        }
        private void CP_CreateConfigPanelItemsCh1()
        {
            Ch1ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersPanel.Children.Add(Ch1ParametersLabelPanel);
            Ch1ParametersPanel.Children.Add(Ch1ParametersTextPanel);
            #region Initial Voltage
            CPcurrentLabel = new Label
            {
                Content = "Current (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            
            CPcurrentBox.Width = 100;
            CPcurrentBox.Margin = new Thickness(5);
            CPcurrentBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CPcurrentBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CPcurrentBox.FontSize = 17;
            CPcurrentBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CPcurrentLabel);
            Ch1ParametersTextPanel.Children.Add(CPcurrentBox);

            #endregion
            #region Final Voltage
            CPsampleTLabel = new Label
            {
                Content = "Sample time (ms):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CPsampleTBox.Width = 100;
            CPsampleTBox.Margin = new Thickness(5);
            CPsampleTBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CPsampleTBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CPsampleTBox.FontSize = 17;
            CPsampleTBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CPsampleTLabel);
            Ch1ParametersTextPanel.Children.Add(CPsampleTBox);

            #endregion
            #region Step Voltage
            CPsamplePLabel = new Label
            {
                Content = "Sample Period (s):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CPsamplePBox.Width = 100;
            CPsamplePBox.Margin = new Thickness(5);
            CPsamplePBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CPsamplePBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CPsamplePBox.FontSize = 17;
            CPsamplePBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CPsamplePLabel);
            Ch1ParametersTextPanel.Children.Add(CPsamplePBox);

            #endregion

        }
        private void CP_CreateConfigPanelItemsCh2()
        {
            Ch2ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersPanel.Children.Add(Ch2ParametersLabelPanel);
            Ch2ParametersPanel.Children.Add(Ch2ParametersTextPanel);
            #region Initial Voltage
            CPcurrentLabel = new Label
            {
                Content = "Current (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CPcurrentBox2.Width = 100;
            CPcurrentBox2.Margin = new Thickness(5);
            CPcurrentBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CPcurrentBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CPcurrentBox2.FontSize = 17;
            CPcurrentBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CPcurrentLabel);
            Ch2ParametersTextPanel.Children.Add(CPcurrentBox2);

            #endregion
            #region Final Voltage
            CPsampleTLabel = new Label
            {
                Content = "Sample time (ms):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CPsampleTBox2.Width = 100;
            CPsampleTBox2.Margin = new Thickness(5);
            CPsampleTBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CPsampleTBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CPsampleTBox2.FontSize = 17;
            CPsampleTBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CPsampleTLabel);
            Ch2ParametersTextPanel.Children.Add(CPsampleTBox2);

            #endregion
            #region Step Voltage
            CPsamplePLabel = new Label
            {
                Content = "Sample Period (s):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CPsamplePBox2.Width = 100;
            CPsamplePBox2.Margin = new Thickness(5);
            CPsamplePBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CPsamplePBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CPsamplePBox2.FontSize = 17;
            CPsamplePBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CPsamplePLabel);
            Ch2ParametersTextPanel.Children.Add(CPsamplePBox2);

            #endregion

        }
        private void LSP_CreateConfigPanelItemsCh1()
        {
            Ch1ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersPanel.Children.Add(Ch1ParametersLabelPanel);
            Ch1ParametersPanel.Children.Add(Ch1ParametersTextPanel);

            #region Initial Voltage
            LSPstartILabel = new Label
            {
                Content = "Initial I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSPstartIBox.Width = 100;
            LSPstartIBox.Margin = new Thickness(5);
            LSPstartIBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSPstartIBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSPstartIBox.FontSize = 17;
            LSPstartIBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(LSPstartILabel);
            Ch1ParametersTextPanel.Children.Add(LSPstartIBox);
            #endregion
            #region Final Voltage
            LSPfinalILabel = new Label
            {
                Content = "Final I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSPfinalIBox.Width = 100;
            LSPfinalIBox.Margin = new Thickness(5);
            LSPfinalIBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSPfinalIBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSPfinalIBox.FontSize = 17;
            LSPfinalIBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(LSPfinalILabel);
            Ch1ParametersTextPanel.Children.Add(LSPfinalIBox);
            #endregion
            #region Step Current
            LSPstepILabel = new Label
            {
                Content = "Step size (μA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSPstepIBox.Width = 100;
            LSPstepIBox.Margin = new Thickness(5);
            LSPstepIBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSPstepIBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSPstepIBox.FontSize = 17;
            LSPstepIBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(LSPstepILabel);
            Ch1ParametersTextPanel.Children.Add(LSPstepIBox);
            #endregion
            #region Time Step
            LSPtimeStepLabel = new Label
            {
                Content = "Scan rate (μA/s)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSPtimeStepBox.Width = 100;
            LSPtimeStepBox.Margin = new Thickness(5);
            LSPtimeStepBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSPtimeStepBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSPtimeStepBox.FontSize = 17;
            LSPtimeStepBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");


            Ch1ParametersLabelPanel.Children.Add(LSPtimeStepLabel);
            Ch1ParametersTextPanel.Children.Add(LSPtimeStepBox);
            #endregion
        }
        private void LSP_CreateConfigPanelItemsCh2()
        {
            Ch2ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersPanel.Children.Add(Ch2ParametersLabelPanel);
            Ch2ParametersPanel.Children.Add(Ch2ParametersTextPanel);

            #region Initial Voltage
            LSPstartILabel = new Label
            {
                Content = "Initial I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSPstartIBox2.Width = 100;
            LSPstartIBox2.Margin = new Thickness(5);
            LSPstartIBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSPstartIBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSPstartIBox2.FontSize = 17;
            LSPstartIBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(LSPstartILabel);
            Ch2ParametersTextPanel.Children.Add(LSPstartIBox2);
            #endregion
            #region Final Voltage
            LSPfinalILabel = new Label
            {
                Content = "Final I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSPfinalIBox2.Width = 100;
            LSPfinalIBox2.Margin = new Thickness(5);
            LSPfinalIBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSPfinalIBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSPfinalIBox2.FontSize = 17;
            LSPfinalIBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(LSPfinalILabel);
            Ch2ParametersTextPanel.Children.Add(LSPfinalIBox2);
            #endregion
            #region Step Current
            LSPstepILabel = new Label
            {
                Content = "Step size (μA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSPstepIBox2.Width = 100;
            LSPstepIBox2.Margin = new Thickness(5);
            LSPstepIBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSPstepIBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSPstepIBox2.FontSize = 17;
            LSPstepIBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(LSPstepILabel);
            Ch2ParametersTextPanel.Children.Add(LSPstepIBox2);
            #endregion
            #region Time Step
            LSPtimeStepLabel = new Label
            {
                Content = "Scan rate (μA/s)",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            LSPtimeStepBox2.Width = 100;
            LSPtimeStepBox2.Margin = new Thickness(5);
            LSPtimeStepBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            LSPtimeStepBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            LSPtimeStepBox2.FontSize = 17;
            LSPtimeStepBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(LSPtimeStepLabel);
            Ch2ParametersTextPanel.Children.Add(LSPtimeStepBox2);
            #endregion
        }
        private void CyP_CreateConfigPanelItemsCh1()
        {
            Ch1ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch1ParametersPanel.Children.Add(Ch1ParametersLabelPanel);
            Ch1ParametersPanel.Children.Add(Ch1ParametersTextPanel);

            #region Initial Voltage
            CyPstartILabel = new Label
            {
                Content = "Initial I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPstartIBox.Width = 100;
            CyPstartIBox.Margin = new Thickness(5);
            CyPstartIBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPstartIBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPstartIBox.FontSize = 17;
            CyPstartIBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CyPstartILabel);
            Ch1ParametersTextPanel.Children.Add(CyPstartIBox);

            #endregion
            #region Peak1 Current
            CyPpeakI1Label = new Label
            {
                Content = "Peak1 I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPpeakI1Box.Width = 100;
            CyPpeakI1Box.Margin = new Thickness(5);
            CyPpeakI1Box.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPpeakI1Box.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPpeakI1Box.FontSize = 17;
            CyPpeakI1Box.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CyPpeakI1Label);
            Ch1ParametersTextPanel.Children.Add(CyPpeakI1Box);

            #endregion
            #region Peak2 Current
            CyPpeakI2Label = new Label
            {
                Content = "Peak2 I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPpeakI2Box.Width = 100;
            CyPpeakI2Box.Margin = new Thickness(5);
            CyPpeakI2Box.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPpeakI2Box.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPpeakI2Box.FontSize = 17;
            CyPpeakI2Box.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CyPpeakI2Label);
            Ch1ParametersTextPanel.Children.Add(CyPpeakI2Box);

            #endregion
            #region Final Current
            CyPfinalILabel = new Label
            {
                Content = "Final I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPfinalIBox.Width = 100;
            CyPfinalIBox.Margin = new Thickness(5);
            CyPfinalIBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPfinalIBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPfinalIBox.FontSize = 17;
            CyPfinalIBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CyPfinalILabel);
            Ch1ParametersTextPanel.Children.Add(CyPfinalIBox);

            #endregion
            #region Step Current
            CyPstepILabel = new Label
            {
                Content = "Step size (μA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPstepIBox.Width = 100;
            CyPstepIBox.Margin = new Thickness(5);
            CyPstepIBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPstepIBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPstepIBox.FontSize = 17;
            CyPstepIBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CyPstepILabel);
            Ch1ParametersTextPanel.Children.Add(CyPstepIBox);

            #endregion
            #region Time Step
            CyPtimeStepLabel = new Label
            {
                Content = "Scan rate (μA/s):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPtimeStepBox.Width = 100;
            CyPtimeStepBox.Margin = new Thickness(5);
            CyPtimeStepBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPtimeStepBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPtimeStepBox.FontSize = 17;
            CyPtimeStepBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CyPtimeStepLabel);
            Ch1ParametersTextPanel.Children.Add(CyPtimeStepBox);

            #endregion
            #region Cycle
            CyPcycleLabel = new Label
            {
                Content = "Cycles:",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPcycleBox.Width = 100;
            CyPcycleBox.Margin = new Thickness(5);
            CyPcycleBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPcycleBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPcycleBox.FontSize = 17;
            CyPcycleBox.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch1ParametersLabelPanel.Children.Add(CyPcycleLabel);
            Ch1ParametersTextPanel.Children.Add(CyPcycleBox);

            #endregion

        }
        private void CyP_CreateConfigPanelItemsCh2()
        {
            Ch2ParametersLabelPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersTextPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            Ch2ParametersPanel.Children.Add(Ch2ParametersLabelPanel);
            Ch2ParametersPanel.Children.Add(Ch2ParametersTextPanel);

            #region Initial Voltage
            CyPstartILabel = new Label
            {
                Content = "Initial I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPstartIBox2.Width = 100;
            CyPstartIBox2.Margin = new Thickness(5);
            CyPstartIBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPstartIBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPstartIBox2.FontSize = 17;
            CyPstartIBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CyPstartILabel);
            Ch2ParametersTextPanel.Children.Add(CyPstartIBox2);

            #endregion
            #region Peak1 Current
            CyPpeakI1Label = new Label
            {
                Content = "Peak1 I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPpeakI1Box2.Width = 100;
            CyPpeakI1Box2.Margin = new Thickness(5);
            CyPpeakI1Box2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPpeakI1Box2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPpeakI1Box2.FontSize = 17;
            CyPpeakI1Box2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CyPpeakI1Label);
            Ch2ParametersTextPanel.Children.Add(CyPpeakI1Box2);

            #endregion
            #region Peak2 Current
            CyPpeakI2Label = new Label
            {
                Content = "Peak2 I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPpeakI2Box2.Width = 100;
            CyPpeakI2Box2.Margin = new Thickness(5);
            CyPpeakI2Box2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPpeakI2Box2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPpeakI2Box2.FontSize = 17;
            CyPpeakI2Box2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CyPpeakI2Label);
            Ch2ParametersTextPanel.Children.Add(CyPpeakI2Box2);

            #endregion
            #region Final Current
            CyPfinalILabel = new Label
            {
                Content = "Final I (mA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPfinalIBox2.Width = 100;
            CyPfinalIBox2.Margin = new Thickness(5);
            CyPfinalIBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPfinalIBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPfinalIBox2.FontSize = 17;
            CyPfinalIBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CyPfinalILabel);
            Ch2ParametersTextPanel.Children.Add(CyPfinalIBox2);

            #endregion
            #region Step Current
            CyPstepILabel = new Label
            {
                Content = "Step size (μA):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPstepIBox2.Width = 100;
            CyPstepIBox2.Margin = new Thickness(5);
            CyPstepIBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPstepIBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPstepIBox2.FontSize = 17;
            CyPstepIBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CyPstepILabel);
            Ch2ParametersTextPanel.Children.Add(CyPstepIBox2);

            #endregion
            #region Time Step
            CyPtimeStepLabel = new Label
            {
                Content = "Scan rate (μA/s):",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPtimeStepBox2.Width = 100;
            CyPtimeStepBox2.Margin = new Thickness(5);
            CyPtimeStepBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPtimeStepBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPtimeStepBox2.FontSize = 17;
            CyPtimeStepBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CyPtimeStepLabel);
            Ch2ParametersTextPanel.Children.Add(CyPtimeStepBox2);

            #endregion
            #region Cycle
            CyPcycleLabel = new Label
            {
                Content = "Cycles:",
                FontSize = 17,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            };

            CyPcycleBox2.Width = 100;
            CyPcycleBox2.Margin = new Thickness(5);
            CyPcycleBox2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CyPcycleBox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CyPcycleBox2.FontSize = 17;
            CyPcycleBox2.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");

            Ch2ParametersLabelPanel.Children.Add(CyPcycleLabel);
            Ch2ParametersTextPanel.Children.Add(CyPcycleBox2);

            #endregion

        }
    }
}

public class MeasurementConfig
{
    public string Name { get; set; }
    public string Timestamp { get; set; }
    public string FilePath { get; set; }
}