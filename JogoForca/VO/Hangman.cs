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
		public int WordLength { get; }
		private char?[] guessedLetters;
		private int errorCount;
		private int correctCount;

		public Hangman(string word)
		{
			WordLength = word.Length;
			guessedLetters = new char?[WordLength];
			this.word = word;

			errorCount = 0;
			correctCount = 0;

			for (int i = 0; i < WordLength; i++)
			{
				guessedLetters[i] = null;
			}
		}

		// Retorna um array de char? com as letras já descobertas
		public char?[] GetGuessedLetters()
		{
			return guessedLetters;
		}

		// Verifica se uma letra está presente na palavra
		// Retorna true se está, caso contrário retorna falso
		public bool TryLetter(char letter)
		{

			bool guessedCorrectly = false;
			int size = word.Length;

			for (int i = 0; i < size; i++)
			{
				//Converte o caracter que vai ser checado para letra maíscula e ignorando acentuação
				char wordLetter = word[i].ToString().Normalize(NormalizationForm.FormD).ToUpper()[0];

				if(letter == wordLetter)
				{ 
					guessedLetters[i] = word[i];
					guessedCorrectly = true;
					correctCount++;
				}

			}

			if (guessedCorrectly == false)
				errorCount++;

			return guessedCorrectly;
		}

		// Verifica se o jogo terminou
		// Retorna:
		// 0: Se o jogo não terminou
		// 1: Se o jogador venceu
		// 2: Se o jogador perdeu
		public int CheckEndGame()
		{
			if (errorCount >= 6)
				return 2;
			if (correctCount >= WordLength)
				return 1;

			return 0;
		}

		// Retorna o número de erros que o jogador cometeu
		public int GetErrorCount()
		{
			return errorCount;
		}
	}
}

		