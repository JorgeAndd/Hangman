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
		private Hangman hangman;

        public MainPage()
        {
            this.InitializeComponent();
        }

		// Evento de clique de um botão de letra
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

			int gameEnd = hangman.CheckEndGame();

			// Checa se o jogo acabou
			if (gameEnd == 0)
				return;
			else if (gameEnd == 1)
			{
				txtInstruction.Text = "Você venceu! =D";
			}
			else if (gameEnd == 2)
			{
				txtInstruction.Text = "Você perdeu =(";
			}


			txtInstruction.Visibility = Visibility.Visible;
			btnStart.Visibility = Visibility.Collapsed;
			btnRestart.Visibility = Visibility.Visible;

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

			// Seleciona a url da imagem correta
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

		// Inicia o jogo
		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			string word = tbPalavra.Text;

			hangman = new Hangman(word);

			tbPalavra.Visibility = Visibility.Collapsed;
			btnStart.Visibility = Visibility.Collapsed;
			txtInstruction.Visibility = Visibility.Collapsed;

			CreateLettersBoxes();
			SetLetterButtons(true);
		}
		
		// Reinicia o jogo
		private void btnRestart_Click(object sender, RoutedEventArgs e)
		{
			tbPalavra.Text = "";
			tbPalavra.Visibility = Visibility.Visible;
			txtInstruction.Text = "Insira uma palavra";

			StackWord.Children.Clear();
			SetLetterButtons(false);

			ChangeHangmanPicture(0);

			btnStart.Visibility = Visibility.Visible;
			btnRestart.Visibility = Visibility.Collapsed;
		}

		// Seta os todos botões de letras como enabled ou não
		private void SetLetterButtons(bool isEnabled)
		{
			int n = VisualTreeHelper.GetChildrenCount(StackLetters);

			for (int i = 0; i < n; i++)
			{
				StackPanel sp = (StackPanel)VisualTreeHelper.GetChild(StackLetters, i);
				int n2 = VisualTreeHelper.GetChildrenCount(sp);

				for (int j = 0; j < n2; j++)
				{
					Button btn = (Button)VisualTreeHelper.GetChild(sp, j);
					btn.IsEnabled = isEnabled;
				}

			}
		}

		// Crias os Grids onde irão aparecer as letras
		private void CreateLettersBoxes()
		{
			StackPanel sp = StackWord;

			int length = hangman.WordLength;

			for (int i = 0; i < length; i++)
			{
				// É criado um Grid com um TexBlock dentro dele
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

		// Checa se existe texto na Texbox de entrada e se ele contém apenas letras
		private void tbPalavra_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
		{
			string palavra = sender.Text;

			// Verifica se existe o texto e se o texto contém apenas letras
			if (palavra.Length > 0 && palavra.All(x => char.IsLetter(x)))
				btnStart.IsEnabled = true;
			else
				btnStart.IsEnabled = false;
		}
	}
}
