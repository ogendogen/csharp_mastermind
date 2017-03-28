using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Mastermind
{
    class Program
    {
        public static void updateTurns(int turns)
        {
            // zapis aktualnej pozycji
            int old_top = Console.CursorTop;
            int old_left = Console.CursorLeft;

            // ustawiamy się na pozycji licznika i czyścimy go
            Console.CursorTop = 0;
            Console.CursorLeft = Console.WindowWidth - 3;
            Console.Write(" ");
            Console.Write(" ");

            // w wolne pole wpisujemy nową wartość
            Console.CursorLeft = Console.WindowWidth - 3;
            Console.Write(turns);

            // powrót na starą pozycję
            Console.CursorTop = old_top;
            Console.CursorLeft = old_left;
        }

        static void Main(string[] args)
        {
            try
            {
                Console.Title = "Mastermind";
                Console.WriteLine("Gra Mastermind\nSzare gwiazdki - brak\nŻółte gwiazdki - złe miejsce\nZielone gwiazdki - poprawne miejsce");
                Console.ReadKey();
                Console.Clear();

                Game game = Game.getInstance();
                game.init();
                game.start();
                Console.CursorLeft = Console.WindowWidth - 10; // RUCHY:_XY_
                Console.Write("Ruchy: " + game.getCurrentTurns());
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
                StringBuilder question = new StringBuilder();
                while(true) // główna pętla gry
                {
                    question.Clear();
                    for (int i=0; i<4; i++) // zbieranie kolorów do pytania
                    {
                        ConsoleKeyInfo c = Console.ReadKey(true);
                        while (c.KeyChar < '1' || c.KeyChar > '8')
                        {
                            if (Console.CursorLeft > 0)
                            {
                                Console.Write(" ");
                                Console.CursorLeft--;
                            }
                            c = Console.ReadKey(true);
                        }
                        Console.Write(c.KeyChar);
                        question.Append(c.KeyChar);
                        Console.CursorLeft += 2;
                    }
                    Console.Write("|");
                    Console.CursorLeft += 2;
                    try
                    {
                        game.addQuestion(question.ToString());
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }

                    List<int> answer = game.getLastAnswer();
                    for (int i=0; i<4; i++) // wydrukowanie odpowiedzi
                    {
                        Console.ResetColor();
                        switch (answer[i])
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write('*');
                                break;
                            case 1:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write('*');
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write('*');
                                break;
                        }
                        Console.CursorLeft += 2;
                    }
                    
                    Console.ResetColor();
                    Console.CursorLeft = 0;
                    Console.CursorTop++;
                    if (game.isWin())
                    {
                        MessageBox.Show("Odgadłeś układ!", "Zwycięstwo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Console.Clear();
                        Console.CursorTop = 0;
                        Console.CursorLeft = 0;
                        game.start();
                        Console.CursorLeft = Console.WindowWidth - 10; // RUCHY:_XY_
                        Console.Write("Ruchy: " + game.getCurrentTurns());
                        Console.CursorLeft = 0;
                        Console.CursorTop = 0;
                        continue;
                    }
                    if (game.isLose())
                    {
                        MessageBox.Show("Koniec gry! Przegrałeś!", "Przegrana!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Console.Clear();
                        Console.CursorTop = 0;
                        Console.CursorLeft = 0;
                        game.start();
                        Console.CursorLeft = Console.WindowWidth - 10; // RUCHY:_XY_
                        Console.Write("Ruchy: " + game.getCurrentTurns());
                        Console.CursorLeft = 0;
                        Console.CursorTop = 0;
                        continue;
                    }
                    updateTurns(game.getCurrentTurns());
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                MessageBox.Show(e.Message, "Wyjątek!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
