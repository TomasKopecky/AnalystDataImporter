﻿#pragma checksum "..\..\..\WindowsWPF\TestWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1AA7C56ACB7FB1E1106815A0C17780C1E582D691EC139164BCF7F2CC814A6C72"
//------------------------------------------------------------------------------
// <auto-generated>
//     Tento kód byl generován nástrojem.
//     Verze modulu runtime:4.0.30319.42000
//
//     Změny tohoto souboru mohou způsobit nesprávné chování a budou ztraceny,
//     dojde-li k novému generování kódu.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using AnalystDataImporter.WindowsWPF;


namespace AnalystDataImporter.WindowsWPF {
    
    
    /// <summary>
    /// TestWindow
    /// </summary>
    public partial class TestWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 37 "..\..\..\WindowsWPF\TestWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grd1Zdroje;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\WindowsWPF\TestWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dtGrdZdroje;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\WindowsWPF\TestWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAktualizovat;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\WindowsWPF\TestWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grd2Import;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\WindowsWPF\TestWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dtGrdImport;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\WindowsWPF\TestWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnZpet;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\WindowsWPF\TestWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDalsi;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\WindowsWPF\TestWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnImport;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\WindowsWPF\TestWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grd3OProgramu;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AnalystDataImporter;component/windowswpf/testwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\WindowsWPF\TestWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\WindowsWPF\TestWindow.xaml"
            ((AnalystDataImporter.WindowsWPF.TestWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 10 "..\..\..\WindowsWPF\TestWindow.xaml"
            ((System.Windows.Controls.Grid)(target)).Drop += new System.Windows.DragEventHandler(this.Grid_Drop);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\WindowsWPF\TestWindow.xaml"
            ((System.Windows.Controls.Grid)(target)).DragOver += new System.Windows.DragEventHandler(this.Grid_DragOver);
            
            #line default
            #line hidden
            return;
            case 3:
            this.grd1Zdroje = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.dtGrdZdroje = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 5:
            this.btnAktualizovat = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.grd2Import = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.dtGrdImport = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 8:
            this.btnZpet = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.btnDalsi = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.btnImport = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.grd3OProgramu = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

