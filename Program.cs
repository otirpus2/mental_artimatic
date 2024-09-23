using System;
using System.Threading.Tasks;

class MentalMath
{
    static Random random = new Random();

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Mental Math Practice Program!");
        Console.WriteLine("You have 15 seconds to answer each question.");
        Console.WriteLine("Press 'q' at any time to quit.\n");

        while (true)
        {
            // Generate a random arithmetic question and the correct answer
            string question = GenerateQuestion(out int correctAnswer);
            char correctOption = (char)('a' + random.Next(0, 4)); // Randomly choose correct option position

            // Generate options
            string[] options = GenerateOptions(correctAnswer, correctOption);

            // Display the question and options
            Console.WriteLine($"Solve: {question}");
            foreach (var option in options)
            {
                Console.WriteLine(option);
            }

            // Get the user's input with a time limit of 15 seconds
            string input = GetUserInputWithTimeLimit(15000).Result;

            // Check if the user ran out of time
            if (input == null)
            {
                Console.WriteLine("Time's up! You ran out of time.\n");
                continue;
            }

            // Check if the user wants to quit
            if (input.ToLower() == "q")
            {
                Console.WriteLine("Thanks for playing! Goodbye!");
                break;
            }

            // Check if the answer is correct
            if (input.ToLower() == correctOption.ToString())
            {
                Console.WriteLine("Correct! Well done.\n");
            }
            else
            {
                Console.WriteLine($"Incorrect. The correct answer was option {correctOption}: {correctAnswer}.\n");
            }
        }
    }

    static string GenerateQuestion(out int correctAnswer)
    {
        correctAnswer = 0;
        int num1 = random.Next(1, 101);
        int num2 = random.Next(1, 101);
        int operation = random.Next(0, 4); // 0: +, 1: -, 2: *, 3: /

        string question = "";

        switch (operation)
        {
            case 0:
                question = $"{num1} + {num2}";
                correctAnswer = num1 + num2;
                break;
            case 1:
                question = $"{num1} - {num2}";
                correctAnswer = num1 - num2;
                break;
            case 2:
                question = $"{num1} * {num2}";
                correctAnswer = num1 * num2;
                break;
            case 3:
                if (num2 != 0)
                {
                    correctAnswer = num1 / num2;
                    question = $"{num1} / {num2}";
                }
                else
                {
                    question = $"{num1} + {num2}";
                    correctAnswer = num1 + num2;
                }
                break;
        }
        return question;
    }

    static string[] GenerateOptions(int correctAnswer, char correctOption)
    {
        string[] options = new string[4];
        int correctIndex = correctOption - 'a'; // Convert character to index (0-3)

        // Assign the correct answer to its random position
        options[correctIndex] = $"{correctOption}. {correctAnswer}";

        // Generate random incorrect options
        for (int i = 0; i < options.Length; i++)
        {
            if (i != correctIndex)
            {
                int incorrectAnswer;
                do
                {
                    // Generate an incorrect answer (ensuring it's not the correct answer)
                    incorrectAnswer = random.Next(correctAnswer - 10, correctAnswer + 10);
                } while (incorrectAnswer == correctAnswer);
                
                options[i] = $"{(char)('a' + i)}. {incorrectAnswer}";
            }
        }

        return options;
    }

    static async Task<string> GetUserInputWithTimeLimit(int timeoutMilliseconds)
    {
        Task<string> inputTask = Task.Run(() => Console.ReadLine());
        Task delayTask = Task.Delay(timeoutMilliseconds);

        Task completedTask = await Task.WhenAny(inputTask, delayTask);

        if (completedTask == inputTask)
        {
            return inputTask.Result;  // Return the user input
        }
        else
        {
            return null;  // Return null if time limit exceeded
        }
    }
}
