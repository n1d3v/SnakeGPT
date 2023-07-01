using System;
using System.Collections.Generic;
using System.Threading;

class SnakeGame
{
    // Game settings
    const int ConsoleWidth = 40;
    const int ConsoleHeight = 20;
    const int GameSpeed = 200; // Milliseconds

    // Snake properties
    static int snakeX;
    static int snakeY;
    static int score;
    static bool gameOver;
    static Direction direction;
    static List<int> tailX;
    static List<int> tailY;
    static int foodX;
    static int foodY;
    static Random random;

    // Enum for direction
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    static void Main()
    {
        InitializeGame();
        while (!gameOver)
        {
            if (Console.KeyAvailable)
                ProcessInput(Console.ReadKey(true).Key);
            MoveSnake();
            if (CheckCollision())
                gameOver = true;
            DrawGame();
            Thread.Sleep(GameSpeed);
        }

        Console.Clear();
        Console.WriteLine("Game Over! Your score: " + score);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static void InitializeGame()
    {
        Console.Title = "Snake Game";
        Console.CursorVisible = false;
        Console.SetWindowSize(ConsoleWidth, ConsoleHeight);
        Console.SetBufferSize(ConsoleWidth, ConsoleHeight);

        snakeX = ConsoleWidth / 2;
        snakeY = ConsoleHeight / 2;
        score = 0;
        gameOver = false;
        direction = Direction.Right;

        tailX = new List<int>();
        tailY = new List<int>();

        random = new Random();

        PlaceFood();
    }

    static void ProcessInput(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.W:
            case ConsoleKey.UpArrow:
                if (direction != Direction.Down)
                    direction = Direction.Up;
                break;

            case ConsoleKey.S:
            case ConsoleKey.DownArrow:
                if (direction != Direction.Up)
                    direction = Direction.Down;
                break;

            case ConsoleKey.A:
            case ConsoleKey.LeftArrow:
                if (direction != Direction.Right)
                    direction = Direction.Left;
                break;

            case ConsoleKey.D:
            case ConsoleKey.RightArrow:
                if (direction != Direction.Left)
                    direction = Direction.Right;
                break;

            case ConsoleKey.Escape:
                gameOver = true;
                break;
        }
    }

    static void MoveSnake()
    {
        tailX.Insert(0, snakeX);
        tailY.Insert(0, snakeY);

        if (tailX.Count > score + 1)
        {
            tailX.RemoveAt(tailX.Count - 1);
            tailY.RemoveAt(tailY.Count - 1);
        }

        switch (direction)
        {
            case Direction.Up:
                snakeY--;
                break;

            case Direction.Down:
                snakeY++;
                break;

            case Direction.Left:
                snakeX--;
                break;

            case Direction.Right:
                snakeX++;
                break;
        }
    }


    static bool CheckCollision()
    {
        if (snakeX < 0 || snakeX >= ConsoleWidth || snakeY < 0 || snakeY >= ConsoleHeight)
            return true;

        for (int i = 1; i < tailX.Count; i++)
        {
            if (snakeX == tailX[i] && snakeY == tailY[i])
                return true;
        }

        if (snakeX == foodX && snakeY == foodY)
        {
            score++;
            PlaceFood();
        }

        return false;
    }

    static void PlaceFood()
    {
        foodX = random.Next(0, ConsoleWidth);
        foodY = random.Next(0, ConsoleHeight);
    }

    static void DrawGame()
    {
        Console.Clear();

        // Draw snake
        Console.SetCursorPosition(snakeX, snakeY);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("O");

        // Draw tail
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        for (int i = 0; i < tailX.Count; i++)
        {
            if (tailX[i] >= 0 && tailX[i] < ConsoleWidth && tailY[i] >= 0 && tailY[i] < ConsoleHeight)
            {
                Console.SetCursorPosition(tailX[i], tailY[i]);
                Console.Write("o");
            }
        }

        // Draw food
        Console.SetCursorPosition(foodX, foodY);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("@");

        // Draw score
        Console.SetCursorPosition(0, ConsoleHeight - 1);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Score: " + score);
    }
}