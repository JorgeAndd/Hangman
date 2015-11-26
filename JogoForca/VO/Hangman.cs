using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoForca.VO
{
	class Hangman
	{
		private string word;
		private char?[] guessedLetters;
		private int errors;
		private List<char> wrongLetters;
		private List<char> correctLetters;

		public Hangman(string word)
		{
			int size = word.Length;
			guessedLetters = new char?[size];
			this.word = word;

			errors = 0;

			for (int i = 0; i < size; i++)
			{
				guessedLetters[i] = null;
			}
		}

		public char?[] GetGuessedLetters()
		{
			return guessedLetters;
		}

		public void SetWord(string word)
		{
			this.word = word;
		}

		public bool TryLetter(char letter)
		{

			bool guessedCorrectly = false;
			int size = word.Length;

			letter = letter.ToString().Normalize(NormalizationForm.FormD)[0];

			for (int i = 0; i < size; i++)
			{
				//Converte o caracter que vai ser checado para letra maíscula e ignorando acentuação
				char wordLetter = word[i].ToString().Normalize(NormalizationForm.FormD).ToUpper()[0];

				if(letter == wordLetter)
				{ 
					guessedLetters[i] = word[i];
					guessedCorrectly = true;
				}

			}

			if (guessedCorrectly == false)
				errors++;

			return guessedCorrectly;
		}

		public int GetWordLength()
		{
			return word.Length;
		}

		public int GetErrorCount()
		{
			return errors;
		}
	}
}

		