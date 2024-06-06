using System.Media;
using Spectre.Console;

namespace WordScrambleGame
{
    public class Game : GameBase, IGame
    {
        private int score;
        private SoundPlayer player;
        private string playerName;

        public Game()
        {
            score = 0;

        }

        public void ShowMainMenu()
        {


            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(
                    new FigletText("Word Scramble")
                        .Centered()
                        .Color(Color.Yellow));

                AnsiConsole.MarkupLine("[bold yellow]¡Bienvenido al juego de combinación de palabras![/]");
                AnsiConsole.MarkupLine(" Que te gustaría hacer?");
                AnsiConsole.WriteLine();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold green]Menú principal[/]")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "Jugar", "Instrucciones", "Exit"
                        }));

                switch (choice)
                {
                    case "Jugar":
                        AnsiConsole.MarkupLine("[bold yellow]Introduzca su nombre:[/]");
                        playerName = Console.ReadLine();
                        PlayGame();
                        break;
                    case "Instrucciones":
                        ShowInstructions();
                        break;
                    case "Exit":
                        return;
                }
            }
        }

        private void ShowInstructions()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Panel(new Markup("[bold yellow]Instrucciones:[/]\n\n1. Descifra la palabra para formar una palabra válida.\n2. Cada respuesta correcta te otorga puntos.\n3. Puedes jugar varias rondas e intentar superar tu puntuación más alta.\n4. Las respuestas incorrectas restarán 2 puntos de tu puntuación.\n5. Si tu puntuación llega a 0, el juego terminará."))
                    .Header("[bold blue]Instrucciones[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderStyle(new Style(Color.Blue))
                    .Padding(2, 2));

            AnsiConsole.MarkupLine("\nPresione cualquier tecla para volver al menú principal.");
            Console.ReadKey(true);
        }

        private void PlayGame()
        {
            List<string> words = new List<string> { "codigo", "mundo", "consola", "hola", "red", "programacion" };
            List<string> remainingWords = new List<string>(words);
            Random random = new Random();
            score = 3;

            while (true)
            {
                if (remainingWords.Count == 0)
                {
                    ShowCompletionMessage();
                    break;
                }

                string originalWord = remainingWords[random.Next(remainingWords.Count)];
                string scrambledWord = ScrambleWord(originalWord);

                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"[bold yellow]Scrambled Word:[/] {scrambledWord}");
                AnsiConsole.MarkupLine("Ingrese su suposición:");

                string userGuess = Console.ReadLine();

                if (userGuess.Equals(originalWord, StringComparison.OrdinalIgnoreCase))
                {
                    AnsiConsole.MarkupLine("[bold green]¡Correcta![/]");
                    AnsiConsole.Status()
                        .Start("Celebrando...", ctx =>
                        {
                            PlaySound("C:\\Users\\Gerardo\\Desktop\\WordScrambleGame\\WordScrambleGame\\sound\\correct.wav");
                            ctx.Status("Celebrando...");
                            ctx.Spinner(Spinner.Known.Star);
                            ctx.SpinnerStyle(Style.Parse("green"));
                            AnsiConsole.MarkupLine($"[bold green]Felicidades, {playerName}! Adivinaste la palabra correctamente.[/]");
                            System.Threading.Thread.Sleep(2000);
                        });
                    score++;
                    remainingWords.Remove(originalWord);
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]¡Incorrecta![/]");
                    PlaySound("C:\\Users\\Gerardo\\Desktop\\WordScrambleGame\\WordScrambleGame\\sound\\incorrect.wav");
                    score -= 2;
                }

                AnsiConsole.MarkupLine($"Tu puntuación actual: {score}");

                if (score <= 0)
                {
                    ShowGameOverMessage();
                    break;
                }

                AnsiConsole.MarkupLine("¿Quieres continuar? (s/n)");

                if (Console.ReadLine().Trim().ToLower() != "s")
                {
                    break;
                }
            }
        }

        private void ShowCompletionMessage()
        {
            AnsiConsole.Clear();
            PlaySound("C:\\Users\\Gerardo\\Desktop\\WordScrambleGame\\WordScrambleGame\\sound\\completion.wav"); // Asegúrate de tener un archivo de sonido llamado completion.wav
            AnsiConsole.Write(
                new Panel(new Markup($"[bold yellow]Felicidades, {playerName}![/]\n\nHas completado con éxito todas las palabras del juego."))
                    .Header("[bold green]Juego completado[/]")
                    .Border(BoxBorder.Double)
                    .BorderStyle(new Style(Color.Green))
                    .Padding(2, 2)
                    .Expand());
            AnsiConsole.MarkupLine("\n\r\nPresione cualquier tecla para volver al menú principal.");
            Console.ReadKey(true);
        }

        private void ShowGameOverMessage()
        {
            AnsiConsole.Clear();
            PlaySound("C:\\Users\\Gerardo\\Desktop\\WordScrambleGame\\WordScrambleGame\\sound\\gameover.wav"); // Asegúrate de tener un archivo de sonido llamado gameover.wav
            AnsiConsole.Write(
                new Panel(new Markup($"[bold red]Juego terminado, {playerName}![/]\n\n\r\nTu puntuación llegó a 0. ¡Más suerte la próxima vez!\n\n[red] x_x [/]")
                    .Centered())
                    .Header("[bold red]Juego terminado[/]")
                    .Border(BoxBorder.Double)
                    .BorderStyle(new Style(Color.Red))
                    .Padding(2, 2)
                    .Expand());
            AnsiConsole.MarkupLine("\n\r\nPresione cualquier tecla para volver al menú principal.");
            Console.ReadKey(true);
        }

        private string ScrambleWord(string word)
        {
            Random random = new Random();
            char[] array = word.ToCharArray();

            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }

            return new string(array);
        }
    }
}







