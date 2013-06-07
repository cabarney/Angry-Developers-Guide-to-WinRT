using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HelloWinRT
{
    public class UpdatingTextBox : TextBox
    {
        public string BindableText
        {
            get { return (string)GetValue(BindableTextProperty); }
            set { SetValue(BindableTextProperty, value); }
        }

        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.Register("BindableText", typeof(string), typeof(UpdatingTextBox), new PropertyMetadata("", OnBindableTextChanged));

        private static void OnBindableTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            ((UpdatingTextBox)sender).OnBindableTextChanged((string)eventArgs.OldValue, (string)eventArgs.NewValue);
        }

        public UpdatingTextBox()
        {
            TextChanged += BindableTextBox_TextChanged;
        }

        private void OnBindableTextChanged(string oldValue, string newValue)
        {
            Text = newValue ?? string.Empty;
        }

        private void BindableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindableText = Text;
        }
    }
}
