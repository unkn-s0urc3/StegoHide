using System;

namespace StegoHide
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Check for the presence of arguments
            // Проверка на наличие аргументов
            if (args.Length < 1)
            {
                ShowUsage();
                return;
            }

            string action = args[0].ToLower(); // Action: embed or extract
            // Действие: embed (встраивание) или extract (извлечение)

            switch (action)
            {
                case "embed":
                    // Check for sufficient arguments for embedding
                    // Проверка на достаточное количество аргументов для встраивания
                    if (args.Length < 5)
                    {
                        Console.WriteLine("Error: For embedding a message, 4 arguments are required.");
                        // Ошибка: для встраивания сообщения требуется 4 аргумента.
                        ShowUsage();
                        return;
                    }

                    string embedImagePath = args[1]; // Path to the input image
                    // Путь к исходному изображению
                    string embedOutputPath = args[2]; // Path to save the output image
                    // Путь к сохраненному изображению
                    string message = args[3];    // Message to embed
                    // Сообщение для встраивания
                    int bitDensity;

                    // Check bit density
                    // Проверка битовой плотности
                    if (!int.TryParse(args[4], out bitDensity) || bitDensity < 1 || bitDensity > 8)
                    {
                        Console.WriteLine("Error: Bit density must be a number between 1 and 8.");
                        // Ошибка: битовая плотность должна быть числом от 1 до 8.
                        return;
                    }

                    // Check if the input file exists
                    // Проверка на существование файла
                    if (!System.IO.File.Exists(embedImagePath))
                    {
                        Console.WriteLine($"Error: The file {embedImagePath} does not exist.");
                        // Ошибка: файл {embedImagePath} не найден.
                        return;
                    }

                    try
                    {
                        // Embed the message into the image
                        // Вставка сообщения в изображение
                        Steganography.EmbedMessage(embedImagePath, message, embedOutputPath, bitDensity);
                        Console.WriteLine("Message successfully embedded into the image.");
                        // Сообщение успешно встроено в изображение
                        Console.WriteLine($"Image saved at: {embedOutputPath}");
                        // Изображение сохранено по пути: {embedOutputPath}
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error embedding the message: {ex.Message}");
                        // Ошибка при встраивании сообщения: {ex.Message}
                    }
                    break;

                case "extract":
                    // Check for sufficient arguments for extraction
                    // Проверка на достаточное количество аргументов для извлечения
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Error: For extracting a message, 2 arguments are required.");
                        // Ошибка: для извлечения сообщения требуется 2 аргумента.
                        ShowUsage();
                        return;
                    }

                    string extractImagePath = args[1]; // Path to the image for message extraction
                    // Путь к изображению для извлечения сообщения
                    int extractBitDensity;

                    // Check bit density
                    // Проверка битовой плотности
                    if (!int.TryParse(args[2], out extractBitDensity) || extractBitDensity < 1 || extractBitDensity > 8)
                    {
                        Console.WriteLine("Error: Bit density must be a number between 1 and 8.");
                        // Ошибка: битовая плотность должна быть числом от 1 до 8.
                        return;
                    }

                    // Check if the input file exists
                    // Проверка на существование файла
                    if (!System.IO.File.Exists(extractImagePath))
                    {
                        Console.WriteLine($"Error: The file {extractImagePath} does not exist.");
                        // Ошибка: файл {extractImagePath} не найден.
                        return;
                    }

                    try
                    {
                        // Extract the message from the image
                        // Извлечение сообщения из изображения
                        string extractedMessage = Steganography.ExtractMessage(extractImagePath, extractBitDensity);
                        Console.WriteLine("Extracted message: " + extractedMessage);
                        // Извлеченное сообщение: {extractedMessage}
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error extracting the message: {ex.Message}");
                        // Ошибка при извлечении сообщения: {ex.Message}
                    }
                    break;

                default:
                    Console.WriteLine("Error: Unknown action. Use 'embed' for embedding or 'extract' for extracting.");
                    // Ошибка: неизвестное действие. Используйте 'embed' для встраивания или 'extract' для извлечения.
                    ShowUsage();
                    break;
            }
        }

        // Method to show usage instructions
        // Метод для отображения подсказки по использованию
        static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            // Использование:
            Console.WriteLine("StegoHide embed <imagePath> <outputPath> <message> <bitDensity>");
            // StegoHide embed <imagePath> <outputPath> <message> <bitDensity>
            Console.WriteLine("StegoHide extract <imagePath> <bitDensity>");
            // StegoHide extract <imagePath> <bitDensity>
            Console.WriteLine();
            Console.WriteLine("Example for embedding:");
            // Пример для встраивания:
            Console.WriteLine("StegoHide embed \"image.bmp\" \"output.bmp\" \"Hello World!\" 8");
            // StegoHide embed \"image.bmp\" \"output.bmp\" \"Hello World!\" 8
            Console.WriteLine("Example for extracting:");
            // Пример для извлечения:
            Console.WriteLine("StegoHide extract \"output.bmp\" 8");
            // StegoHide extract \"output.bmp\" 8
        }
    }
}