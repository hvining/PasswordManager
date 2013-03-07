using PasswordManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PasswordManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplashScreen
    {
        Rect _splashImageRect; // Rect to store splash screen image coordinates.
        bool _dismissed = false; // Variable to track splash screen dismissal status.
        Frame _rootFrame;

        SplashScreen _splashScreen;

        public ExtendedSplashScreen(SplashScreen splashScreen, Boolean loadState)
        {
            this.InitializeComponent(); 
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

            _splashScreen = splashScreen;

            if (_splashScreen != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                _splashScreen.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);

                // Retrieve the window coordinates of the splash screen image.
                _splashImageRect = _splashScreen.ImageLocation;
                PositionImage();
            }

            // Create a Frame to act as the navigation context 
            _rootFrame = new Frame();

            // Restore the saved session state if necessary
            RestoreStateAsync(loadState);
        }

        private async Task RestoreStateAsync(bool loadState)
        {
            if (loadState)
                await SuspensionManager.RestoreAsync();
        }

        private void PositionImage()
        {
            splashImage.SetValue(Canvas.LeftProperty, _splashImageRect.X);
            splashImage.SetValue(Canvas.TopProperty, _splashImageRect.Y);
            splashImage.Height = _splashImageRect.Height;
            splashImage.Width = _splashImageRect.Width;
        }

        private void DismissedEventHandler(SplashScreen sender, object args)
        {
            _dismissed = true;
        }

        private void ExtendedSplash_OnResize(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (_splashScreen != null)
            {
                // Update the coordinates of the splash screen image.
                _splashImageRect = _splashScreen.ImageLocation;
                PositionImage();
            }
        }
    }
}
