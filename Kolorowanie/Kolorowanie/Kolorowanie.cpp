// Kolorowanie.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <math.h>
#include "omp.h"
#include <iostream>
#include <bitset>
#include <algorithm>
#include <ctime>
using namespace std;
# ifndef _coloring_h_
 # define _coloring_h_

   /**
   * Kolorowanie listy trzema kolorami .
   * @param [in] n liczba wierzcho ³ków w liœ cie lub drzewie ( wierzcho ³ki numerowane od 0 do n -1)
   * @param [in] list tablica nast ê pnik ów wierzcho ³ków listy ( nast ê pnikiem ostatniego jest on sam )
   * @param [ out ] coloring tablica zawieraj ¹ca kolory wierzcho ³ków ( wynik procedury )
   */
void list_three_coloring(int n, int list[], int coloring[]);

    /**
    * Kolorowanie listy sze œ cioma kolorami .
    * @param [in] n liczba wierzcho ³ków w liœ cie lub drzewie ( wierzcho ³ki numerowane od 0 do n -1)
    * @param [in] list tablica nast ê pnik ów wierzcho ³ków listy ( nast ê pnikiem ostatniego jest on sam )
    * @param [ out ] coloring tablica zawieraj ¹ca kolory wierzcho ³ków ( wynik procedury )
    */
void list_six_coloring(int n, int list[], int coloring[]);

    /**
    * Kolorowanie drzewa trzema kolorami .
    * @param [in] n liczba wierzcho ³ków w liœ cie lub drzewie ( wierzcho ³ki numerowane od 0 do n -1)
    * @param [in] tree tablica wskazuj ¹ca na ojca ka¿ dego wierzcho ³ka ( ojcem korzenia jest on sam )
    * @param [ out ] coloring tablica zawieraj ¹ca kolory wierzcho ³ków ( wynik procedury )
    */
void tree_three_coloring(int n, int tree[], int coloring[]);

    /**
    * Kolorowanie drzewa sze œ cioma kolorami .
    * @param [in] n liczba wierzcho ³ków w liœ cie lub drzewie ( wierzcho ³ki numerowane od 0 do n -1)
    * @param [in] tree tablica wskazuj ¹ca na ojca ka¿ dego wierzcho ³ka ( ojcem korzenia jest on sam )
    * @param [ out ] coloring tablica zawieraj ¹ca kolory wierzcho ³ków ( wynik procedury )
    */
void tree_six_coloring(int n, int tree[], int coloring[]);
# endif
string DecimalToBinaryString(int i);

int MinDivI(string A, string B);

int FindMax(string strings[]);

int main()
{
	const int n = 8;
	int list[n] = {2,0,-1,6,1,7,5,4};
	int tree[n] = {3,1,5,1,2,1,3,5};
	int coloring[n];
}
int MinDivI(string A, string B)
{
	reverse(A.begin(), A.end());
	reverse(B.begin(), B.end());
	for (int i = 0; i < A.length();i++)
	{
		if (A[i]!=B[i])
		{
			return i;
		}
	}
	return A.length();
}
int FindMax(string strings[])
{
	int max = 0;
	for (int i = 0; i < strings->length();i++)
	{
		int tmp = stoi(strings[i], nullptr, 2);
		if (max < tmp)
		{
			max = tmp;
		}
	}
	return max;
}
string DecimalToBinaryString(int a)
{
	string binary = "";
	int mask = 1;
	for (int i = 0; i < 31; i++)
	{
		if ((mask&a) >= 1)
			binary = "1" + binary;
		else
			binary = "0" + binary;
		mask <<= 1;
	}
	return binary;
}
void list_three_coloring(int n, int list[], int coloring[])
{
	const int N = n;
	int ranking[N];

#pragma omp parallel for schedule(guided)
	for (int i = 0; i < N; i++)
	{
		if (list[i] == -1)
		{
			ranking[i] = 0;
		}
		else
			ranking[i] = 1;
	}
	for (int k = 1; k <= ceil(log(N)); k++)
	{
#pragma omp parallel for schedule(guided)
		for (int i = 0; i < N; i++)
		{
			if (list[i] != -1)
			{
				ranking[i] = ranking[i] + ranking[list[i]];
				list[i] = list[list[i]];
			}
		}
	}
	for (int i = 0; i < N; i++)
	{
		if (ranking[i] != 0)
		{
			if (ranking[i] % 2 == 0)
			{
				coloring[i] = 2;
			}
			else
			{
				coloring[i] = 1;
			}
		}
		else
		{
			coloring[i] = 0;
		}
		printf("color(%d) ranking(%d)\n", coloring[i], ranking[i]);
	}
}

void list_six_coloring(int n, int list[], int coloring[])
{
	const int N = n;
	int ranking[N];

#pragma omp parallel for schedule(guided)
	for (int i = 0; i < N; i++)
	{
		if (list[i] == -1)
		{
			ranking[i] = 0;
		}
		else
			ranking[i] = 1;
	}
	for (int k = 1; k <= ceil(log(N))+1; k++)
	{
#pragma omp parallel for schedule(guided)
		for (int i = 0; i < N; i++)
		{
			if (list[i] != -1)
			{
				ranking[i] = ranking[i] + ranking[list[i]];
				list[i] = list[list[i]];
			}
		}
	}
	for (int i = 0; i < N; i++)
	{
		if (ranking[i] != 0)
		{
			
			if (ranking[i] % 5 == 0)
			{
				coloring[i] = 5;
			}
			else if (ranking[i] % 4 == 0)
			{
				coloring[i] = 4;
			}
			else if (ranking[i] % 3 == 0)
			{
				coloring[i] = 3;
			}
			else if (ranking[i] % 2 == 0)
			{
				coloring[i] = 2;
			}
			
			else
			{
				coloring[i] = 1;
			}

		}
		else
		{
			coloring[i] = 0;
		}
		printf("color(%d) ranking(%d)\n", coloring[i],ranking[i]);
	}
}

void tree_three_coloring(int n, int tree[], int coloring[])
{
	tree_six_coloring(n, tree, coloring);
	const int N = n;
	bool marked[N];
	bool M[N];
#pragma omp parallel for schedule(guided)
	for (int i = 0; i < N; i++)
	{
		marked[i] = false;
	}
	for (int k = 0; k < 6;k++)
	{
		copy(begin(marked), end(marked), begin(M));
#pragma omp parallel for schedule(guided)
		for (int i = 0; i < N; i++)
		{
			if (coloring[i]==k && !M[tree[i]])
			{
				marked[i] = true;
			}
		}
		

		copy(begin(marked), end(marked), begin(M));
#pragma omp parallel for schedule(guided)
		for (int i = 0; i < N; i++)
		{
			if (i==tree[i])
			{
				srand(time(NULL));
				coloring[i] = rand() % 6;
				marked[i] = !M[i];
			}
			else
			{
				{
					coloring[i] = coloring[tree[i]];
					marked[i] = M[tree[i]]; 
				}
			}
		}
		
	}
	copy(begin(marked), end(marked), begin(M));
#pragma omp parallel for schedule(guided)
	for (int i = 0; i < N; i++)
	{
		if (M[i])
		{
			coloring[i] = 0;
		}
		else if (M[tree[i]])
		{
			coloring[i] = 1;
		}
		else
		{
			coloring[i] = 2;
		}
	}
	for (int i = 0; i < N; i++)
	{
		cout << coloring[i] << endl;
	}
}

void tree_six_coloring(int n, int tree[], int coloring[])
{
	const int N = n;
	int j[N];
	char b[N];
	string C[N];
	const unsigned int temp = static_cast<int>(log2(n));
	int Nc = N;
#pragma omp parallel for schedule(guided)
	for (int i = 0; i < N; i++)
	{
		C[i] = DecimalToBinaryString(i).substr(31 - temp, temp);
	}
	while (Nc>6)
	{
#pragma omp parallel for schedule(guided)
		for (int i = 0; i < N; i++)
		{
			if (i == tree[i])
			{
				j[i] = 0;
				b[i] = C[i][0];
			}
			else
			{
				j[i] = MinDivI(C[i], C[tree[i]]);
				b[i] = C[i][C[i].length() - (j[i] + 1)];
			}
			int tmp = static_cast<int>(log2(temp));
			C[i] = DecimalToBinaryString(j[i]).substr(31 - tmp, tmp) + b[i];
		}
		Nc = FindMax(C);
	}
	for (int i = 0; i < n; i++)
	{
		coloring[i] = stoi(C[i], nullptr, 2);
		cout << coloring[i] << endl;
	}
}
