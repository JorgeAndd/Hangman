using JogoForca.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JogoForca
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		private string currentHangmanImgSource;
		private Hangman hangman;

        public MainPage()
        {
            this.InitializeComponent();
        }

		private void btnLetter_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)sender;
			char letter = ((string)btn.Tag)[0];

			btn.IsEnabled = false;

			if (hangman.TryLetter(letter))
			{
				FillLetterBoxes();
			}
			else
			{
				int nErrors = hangman.GetErrorCount();
				ChangeHangmanPicture(nErrors);
			}

			return;
		}

		private void FillLetterBoxes()
		{
			int n = VisualTreeHelper.GetChildrenCount(StackWord);
			char?[] guessedLetters = hangman.GetGuessedLetters();

			for(int i = 0; i < n; i++)
			{

				if(guessedLetters[i] != null)
				{
					Grid g = (Grid)VisualTreeHelper.GetChild(StackWord, i);
					TextBlock tb = (TextBlock)g.Children.OfType<TextBlock>().FirstOrDefault();

					tb.Text = guessedLetters[i].ToString();
				}

			}
		}

		private async void ChangeHangmanPicture(int errors)
		{

			Uri uri;

			switch (errors)
			{
				case 1:
					uri = new Uri("ms-appx:///Assets/Graphics/hang_1.jpg");
					break;
				case 2:
					uri = new Uri("ms-appx:///Assets/Graphics/hang_2.jpg");
					break;
				case 3:
					uri = new Uri("ms-appx:///Assets/Graphics/hang_3.jpg");
					break;
				case 4:
					uri = new Uri("ms-appx:///Assets/Graphics/hang_4.jpg");
					break;
				case 5:
					uri = new Uri("ms-appx:///Assets/Graphics/hang_5.jpg");
					break;
				case 6:
					uri = new Uri("ms-appx:///Assets/Graphics/hang_final.jpg");
					break;
				default:
					uri = new Uri("ms-appx:///Assets/Graphics/hang_initial.jpg");
					break;
			}


			ImageBrush img = new ImageBrush();
			StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);


			//Carregamento asíncrono da imagem, evita que a imagem "pisque"
			using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
			{
				BitmapImage bitmapImage = new BitmapImage();
				await bitmapImage.SetSourceAsync(fileStream);
				img.ImageSource = bitmapImage;
				recHangman.Fill = img;
			}

		}

		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			string word = tbPalavra.Text;

			hangman = new Hangman(word);

			tbPalavra.Visibility = Visibility.Collapsed;
			btnStart.Visibility = Visibility.Collapsed;
			txtInstruction.Visibility = Visibility.Collapsed;

			CreateLettersBoxes();
		}

		private void CreateLettersBoxes()
		{
			StackPanel sp = StackWord;

			int length = hangman.GetWordLength();

			for (int i = 0; i < length; i++)
			{
				Grid g = new Grid();
				g.Margin = new Thickness(2);
				g.Height = 30;
				g.Width = 30;
				g.Background = Resources["SystemControlHighlightAccentBrush"] as Brush;

				TextBlock txt = new TextBlock();
				txt.Text = "";
				txt.HorizontalAlignment = HorizontalAlignment.Center;
				txt.VerticalAlignment = VerticalAlignment.Center;
				txt.Foreground = Resources["AppBarBackgroundThemeBrush"] as Brush;

				g.Children.Add(txt);
				sp.Children.Add(g);
			}
		}
	}
}
