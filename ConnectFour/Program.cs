using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

class GameRow //Create Column Class
{
    public List<string> rowIndexes { get; set; }

    public GameRow() //Init
    {
        rowIndexes = genColumns();
    }

    public List<string> genColumns() //Generate 6 Rows per Column
    {
        List<string> rowIndexes = new List<string>();

        for (int i = 0; i < 7; i++)
        {
            rowIndexes.Add(" ");
        }

        return rowIndexes;
    }
}

class GameGrid //Create Grid Class
{
    public List<GameRow> gameRows { get; set; }
    public bool hasWon = false;

    public GameGrid() //Init
    {
        gameRows = addRows();
    }

    public List<GameRow> addRows() //Generate 7 Rows
    {
        List<GameRow> gameRows = new List<GameRow>();

        for (int i = 0; i < 6; i++)
        {
            gameRows.Add(new GameRow());
        }

        return gameRows;
    }
}

class Program
{
    static void Menu() //Output Beginning Title
    {
        Console.Clear();
        Console.WriteLine("CONNECT FOUR");
        Console.WriteLine("------------");
        Console.WriteLine("PRESS ANY KEY TO START...");
        Console.ReadKey();
        Console.Clear();
    }

    static void outputGameGrid(GameGrid gameGrid) //Outputs current Grid
    {
        Console.Clear(); //Clear Previous Grid
        Console.WriteLine("CONNECT FOUR");
        Console.WriteLine("------------");
        Console.WriteLine("  1    2    3    4    5    6    7"); //Column Numbers
        List<string> output = new List<string>();

        //Output gameGrid
        for (int i = 0; i < gameGrid.gameRows.Count(); i++)
        {
            for (int x = 0; x < gameGrid.gameRows[i].rowIndexes.Count(); x++)
            {
                output.Add("| " + gameGrid.gameRows[i].rowIndexes[x] + " |");
            }

            //Output all Cells in Row (also colours dependant on player)
            output.ForEach(cell => { if (cell == "| X |") { Console.ForegroundColor = ConsoleColor.Blue; } else if (cell == "| O |") { Console.ForegroundColor = ConsoleColor.Red; } else { Console.ForegroundColor = ConsoleColor.White; } Console.Write(cell); });
            Console.ForegroundColor = ConsoleColor.White; //Reset Colour to White to avoid all text being blue/red
            Console.Write("\n");
            output.Clear(); //Clears current stored row data for next loop
        }
    }

    static void detectWin(GameGrid gameGrid)
    {
        //Detect win for X or O
        for (int i = 0; i < gameGrid.gameRows.Count(); i++) //Loop over all rows
        {
            //Horizontal Win Detection
            try
            {
                for (int x = 0; x < gameGrid.gameRows[i].rowIndexes.Count(); x++) //Loop columns in row
                {
                    if (gameGrid.gameRows[i].rowIndexes[x] == "X" && gameGrid.gameRows[i].rowIndexes[x + 1] == "X" && gameGrid.gameRows[i].rowIndexes[x + 2] == "X" && gameGrid.gameRows[i].rowIndexes[x + 3] == "X") //Check X horizontal win
                    {
                        gameGrid.hasWon = true;
                    }
                    else if (gameGrid.gameRows[i].rowIndexes[x] == "O" && gameGrid.gameRows[i].rowIndexes[x + 1] == "O" && gameGrid.gameRows[i].rowIndexes[x + 2] == "O" && gameGrid.gameRows[i].rowIndexes[x + 3] == "O") //Check O horizontal win
                    {
                        gameGrid.hasWon = true;
                    }
                }

                //Vertical Win Detection
                for (int x = 0; x < gameGrid.gameRows[i].rowIndexes.Count(); x++) //Loop columns in row
                {
                    if (gameGrid.gameRows[i].rowIndexes[x] == "X" && gameGrid.gameRows[i + 1].rowIndexes[x] == "X" && gameGrid.gameRows[i + 2].rowIndexes[x] == "X" && gameGrid.gameRows[i + 3].rowIndexes[x] == "X") //Check X vertical win
                    {
                        gameGrid.hasWon = true;
                    }
                    else if (gameGrid.gameRows[i].rowIndexes[x] == "O" && gameGrid.gameRows[i + 1].rowIndexes[x] == "O" && gameGrid.gameRows[i + 2].rowIndexes[x] == "O" && gameGrid.gameRows[i + 3].rowIndexes[x] == "O") //Check O horizontal win
                    {
                        gameGrid.hasWon = true;
                    }
                }

                //Diagonal Win Detection
                for (int x = 0; x < gameGrid.gameRows[i].rowIndexes.Count(); x++) //Loop columns in row
                {
                    if (gameGrid.gameRows[i].rowIndexes[x] == "X" && gameGrid.gameRows[i + 1].rowIndexes[x + 1] == "X" && gameGrid.gameRows[i + 2].rowIndexes[x + 2] == "X" && gameGrid.gameRows[i + 3].rowIndexes[x + 3] == "X") //Check X diagonal win (right to left)
                    {
                        gameGrid.hasWon = true;
                    }
                    else if (gameGrid.gameRows[i].rowIndexes[x] == "O" && gameGrid.gameRows[i + 1].rowIndexes[x + 1] == "O" && gameGrid.gameRows[i + 2].rowIndexes[x + 2] == "O" && gameGrid.gameRows[i + 3].rowIndexes[x + 3] == "O") //Check O diagonal win (right to left)
                    {
                        gameGrid.hasWon = true;
                    }
                }

                //Opposite Diagonal Win Detection
                for (int x = 0; x < gameGrid.gameRows[i].rowIndexes.Count(); x++)
                {
                    if (gameGrid.gameRows[i].rowIndexes[x] == "X" && gameGrid.gameRows[i + 1].rowIndexes[x - 1] == "X" && gameGrid.gameRows[i + 2].rowIndexes[x - 2] == "X" && gameGrid.gameRows[i + 3].rowIndexes[x - 3] == "X") //Check X diagonal win (left to right)
                    {
                        gameGrid.hasWon = true;
                    }
                    else if (gameGrid.gameRows[i].rowIndexes[x] == "O" && gameGrid.gameRows[i + 1].rowIndexes[x - 1] == "O" && gameGrid.gameRows[i + 2].rowIndexes[x - 2] == "O" && gameGrid.gameRows[i + 3].rowIndexes[x - 3] == "O") //Check O diagonal win (left to right)
                    {
                        gameGrid.hasWon = true;
                    }
                }
            }
            catch (Exception e) //Stop errors on out of bounds as detection can go out of bounds
            {
                //Do nothing
            }
        }

    }

    static bool updateColumn(int column, GameGrid gameGrid, string player) //Updates Column
    {
        for (int i = gameGrid.gameRows.Count() - 1; i >= 0; i--) //Loops over column
        {
            if (gameGrid.gameRows[i].rowIndexes[column - 1] == " ") //Checks next available slot
            {
                gameGrid.gameRows[i].rowIndexes[column - 1] = player; //Sets slot to current player
                detectWin(gameGrid); //Detects if player has won
                return true; //Token Placed
            }
            else if (i == 0) //Checks if column is full
            {
                Console.WriteLine("\nColumn is full, please choose another column.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        return false; //Token not placed
    }

    static void Main(string[] args) //Main
    {
        bool continueGame = true;

        do
        {
            try
            {
                //Init Variables
                GameGrid gameGrid = new GameGrid();
                string player = "X";
                bool success = false;
                string userChoice = "";

                Menu(); //Load Menu

                do //Loop whilst no winner
                {
                    try //Catch any errors, e.g. Out of bounds or data type
                    {
                        outputGameGrid(gameGrid); //Display Current Grid

                        Console.Write("\nPlayer " + (player == "X" ? "1" : "2") + ", enter column: "); //Get user input
                        int playerChoice = int.Parse(Console.ReadLine());

                        success = updateColumn(playerChoice, gameGrid, player); //Update Column and check if token placed

                        if (success) player = player == "X" ? "O" : "X"; //Change player if token placed
                    }
                    catch (Exception e) //Error Catching
                    {
                        Console.WriteLine("Invalid Move!");
                        Console.WriteLine("Press Any Key To Continue...");
                        Console.ReadKey();
                    }

                } while (gameGrid.hasWon == false);

                if (gameGrid.hasWon) //If game has been won
                {
                    outputGameGrid(gameGrid); //Display Current Grid
                    Console.WriteLine("\nPlayer " + (player == "X" ? "2" : "1") + " has won!"); //Display Winner
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();

                    do
                    {
                        Console.WriteLine("\nWould you like to play again?");
                        userChoice = Console.ReadLine();


                        if (userChoice == "yes" || userChoice == "y") //If user wants to play again
                        {
                            Console.Clear();
                        }
                        else if (userChoice == "no" || userChoice == "n")//If user wants to quit
                        {
                            continueGame = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input!");
                        }
                    } while (userChoice != "yes" && userChoice != "y" && userChoice != "no" && userChoice != "n"); //Loop until valid input
                }
            }
            catch (Exception e) //Catch Errors - Should never hopefully hit this detection
            {
                Console.WriteLine("Fatal error: " + e);
                Console.WriteLine("Restarting...");
            }
        } while (continueGame == true);
    }
}