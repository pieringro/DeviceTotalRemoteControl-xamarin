using DTRC.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace DTRC.Utility {
    public static class FrontEndElements {

        /// <summary>
        /// Costruisce la popup per la password sulla pagina di settings
        /// </summary>
        /// <param name="popupLayout"></param>
        /// <param name="rootLayout"></param>
        /// <param name="pageCaller"></param>
        public static void BuildSettingsPasswordPopup(ref PopupLayout popupLayout,
            ref StackLayout rootLayout, ContentPage pageCaller) {

            popupLayout = new PopupLayout();
            PopupLayout innerPopupLayout = popupLayout;

            popupLayout.Content = pageCaller.Content;
            pageCaller.Content = popupLayout;

            rootLayout = new StackLayout();
            rootLayout.BackgroundColor = Color.FromHex("C4C4C4");
            rootLayout.Opacity = 1.0d;
            rootLayout.Orientation = StackOrientation.Horizontal;
            rootLayout.WidthRequest = pageCaller.Width / 2;
            rootLayout.HeightRequest = pageCaller.Height / 3;
            rootLayout.Padding = new Thickness(10, 0, 10, 0);

            StackLayout innerRootLayout = rootLayout;

            StackLayout rootStk = new StackLayout();
            //rootStk.Margin = new Thickness(5, 0, 5, 0);
            rootStk.HorizontalOptions = LayoutOptions.FillAndExpand;
            rootStk.VerticalOptions = LayoutOptions.Center;

            Label lblPassword = new Label();
            lblPassword.WidthRequest = 70;
            lblPassword.Text = "Password";
            lblPassword.FontAttributes = FontAttributes.Bold;
            lblPassword.VerticalOptions = LayoutOptions.Center;

            Entry passwordEntry = new Entry();
            passwordEntry.IsPassword = true;
            passwordEntry.HorizontalOptions = LayoutOptions.FillAndExpand;

            StackLayout buttonsStk = new StackLayout();
            buttonsStk.Orientation = StackOrientation.Horizontal;

            Button submitBtn = new Button();
            submitBtn.Text = Application.Current.Resources["unlock"].ToString();
            submitBtn.BackgroundColor = Color.Blue;
            submitBtn.TextColor = Color.White;
            //submitBtn.HorizontalOptions = LayoutOptions.FillAndExpand;
            //submitBtn.VerticalOptions = LayoutOptions.End;
            submitBtn.Clicked += async (sender, e) => {
                //controllo che la password inserita sia corretta, se lo e' apro l'altra pagina
                if (passwordEntry.Text != null && passwordEntry.Text.Equals(App.config.GetPassUser())) {
                    await pageCaller.Navigation.PushAsync(new SettingsPage());
                    innerPopupLayout.DismissPopup();
                }
                else {
                    StartAnimationBlinkTwice(innerRootLayout);
                }

                passwordEntry.Text = null;
            };

            Button cancelBtn = new Button();
            cancelBtn.Text = Application.Current.Resources["cancel"].ToString();
            cancelBtn.BackgroundColor = Color.Blue;
            cancelBtn.TextColor = Color.Red;
            cancelBtn.Clicked += (sender, e) => {
                innerPopupLayout.DismissPopup();
            };

            buttonsStk.Children.Add(submitBtn);
            buttonsStk.Children.Add(cancelBtn);

            rootStk.Children.Add(lblPassword);
            rootStk.Children.Add(passwordEntry);
            rootStk.Children.Add(buttonsStk);

            rootLayout.Children.Add(rootStk);

            popupLayout.IsVisible = true;
        }


        public async static void StartAnimationBlinkTwice(View view) {
            Color prevBackColor = view.BackgroundColor;
            view.BackgroundColor = Color.Red;
            await view.FadeTo(0, 400);
            await view.FadeTo(1, 400);
            await view.FadeTo(0, 400);
            await view.FadeTo(1, 400);
            view.BackgroundColor = prevBackColor;
        }
        
    }
}
