using System.Windows;
using System.Windows.Controls;
using MedTeleHelp.WPF.Models;

namespace MedTeleHelp.WPF.UserControls
{
    public partial class DoctorProfileCard : UserControl
    {
        public static readonly DependencyProperty DoctorProperty =
            DependencyProperty.Register(
                "Doctor", 
                typeof(Doctor), 
                typeof(DoctorProfileCard), 
                new PropertyMetadata(null));

        public Doctor Doctor
        {
            get => (Doctor)GetValue(DoctorProperty);
            set => SetValue(DoctorProperty, value);
        }

        public DoctorProfileCard()
        {
            InitializeComponent();
        }
    }
}