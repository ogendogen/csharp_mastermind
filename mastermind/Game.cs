using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Mastermind
{
    class Game
    {
        // Singleton
        private static Game Instance = null;
        private Game() { }
        public static Game getInstance()
        {
            if (Instance == null) Instance = new Game();
            return Instance;
        }

        private static readonly Random rand = new Random();
        private List<int> test; // szukany układ
        private List<List<int>> questions; // zaproponowany układ
        private List<List<int>> answers; // odpowiedzi
        private bool is_initialized = false; // gra zainicjowana
        private int current; // aktualna pozycja
        private int colors; // ilosc kolorow, domyslnie 8
        private const int max_turns = 12; // maksymalna ilość ruchów
        private int current_turns; // aktualna ilość ruchów 

        public void init() // inicjacja gry, uruchamiamy tylko raz
        {
            if (is_initialized) return;
            test = new List<int>(4);
            // stworzenie dwuwymiarowych tablic
            questions = new List<List<int>>(12);
            answers = new List<List<int>>(12);
            for (int i=0; i<12; i++)
            {
                questions.Add(new List<int>(4));
                answers.Add(new List<int>(4));
            }
            is_initialized = true;
        }

        public void start() // start gry
        {
            current = 0;
            colors = 8;
            current_turns = 12;
            test.Clear();

            for (int i = 0; i < 12; i++)
            {
                questions[i].Clear();
                answers[i].Clear();
                for (int j = 0; j < 4; j++)
                {
                    //questions[i][j] = 0;
                    //answers[i][j] = 0;
                    questions[j].Add(0);
                    answers[j].Add(0);
                }
            }

            generateTest();
        }

        public void printQuestions()
        {
            Debug.WriteLine(questions.Count());
        }

        private void generateTest() // wylosuj test
        {
            if (test == null) throw new Exception("Gra nie jest zainicjowana!");
            for (int i = 0; i < 4; i++) test.Add(rand.Next(1, colors));//test[i] = rand.Next(1, colors);
        }

        private List<int> getLastQuestion() // pobierz ostatnio zaproponowany układ
        {
            return questions.Last(x => x.All(y => y != 0)); // ostatni gdzie wszystkie różne od zera => ostatnio wysłana próba / ostatnia lista bez zer
        }

        public bool isWin() // czy wygrana ?
        {
            return getLastQuestion().SequenceEqual(test); // dwie struktury z dokładnie taką samą zawartością i identyczną kolejnością
        }

        public bool isLose() // czy przegrana ?
        {
            //var is_used_all_slots = questions.All(x => x.All(y => y != 0));
            //var first_empty = questions.First(x => x.All(y => y != 0)); // pierwszy z samymi zerami => jeżeli takowy istnieje to gra toczy się dalej
            //return first_empty == null ? true : false;
            return getCurrentTurns() == 0 ? true : false;
        }

        public List<int> getLastAnswer() // struktura odpowiedzi: 0 - brak informacji, 1 - istnieje ale na innym miejscu, 2 - trafiony
        {
            return answers.Last();
        }

        private void generateAnswer() // struktura odpowiedzi: 0 - brak informacji, 1 - istnieje ale na innym miejscu, 2 - trafiony
        {
            var last_question = getLastQuestion();
            List<int> answer = new List<int>(4)
            {
                0,
                0,
                0,
                0
            };

            for (int i=0; i<4; i++)
            {
                if (last_question[i] == test[i]) answer[i] = 2;
            }

            for (int i=0; i<4; i++)
            {
                for (int j=0; j<4; j++)
                {
                    if (answer[i] != 2 && last_question[i] == test[j] && i != j) answer[i] = 1;
                }
            }

            if (answer.Count() != 4) throw new Exception("Błąd! Odpowiedź różna od 4 !");
            answers.Add(answer);
            current_turns--;
        }

        public void addQuestion(string question)
        {
            if (question == null) throw new Exception("Puste zapytanie !");
            if (question.Length != 4) throw new Exception("Zapytanie musi mieć dokładnie 4 liczby!");
            var is_digits_only = question.All(x => x >= '1' && x <= '8');
            if (!is_digits_only) throw new Exception("Zapytanie może zawierać tylko liczby z zakresu 1-8!");

            //questions.Add(question.Cast<int>().ToList()); // mozliwe uzycie arraylist
            List<int> i_question = new List<int>(4);
            foreach (char sign in question) i_question.Add((int)sign - 48); // 49 to '1'
            questions.Add(i_question);
            current++;

            generateAnswer();
        }

        public int getCurrentTurns()
        {
            return this.current_turns;
        }
    }
}
