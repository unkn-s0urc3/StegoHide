using System;
using System.Drawing;
using System.Text;

namespace StegoHide
{
    class Steganography
    {
        // Method to embed a message into an image
        // Метод для встраивания сообщения в изображение
        public static void EmbedMessage(string imagePath, string message, string outputPath, int bitDensity)
        {
            // Reading the image
            // Чтение изображения
            Bitmap bitmap = new Bitmap(imagePath);

            // Converting the message string to a byte array
            // Преобразование строки сообщения в массив байт
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] messageWithLength = new byte[messageBytes.Length + sizeof(int)];

            // Writing the message length at the beginning
            // Запись длины сообщения в начало
            Array.Copy(BitConverter.GetBytes(messageBytes.Length), messageWithLength, sizeof(int));
            Array.Copy(messageBytes, 0, messageWithLength, sizeof(int), messageBytes.Length);

            // Position for embedding the message
            // Позиция для вставки
            int messageIndex = 0;
            int bitIndex = 0;

            // Embedding the message into the pixels
            // Вставка сообщения в пиксели
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    if (messageIndex < messageWithLength.Length)
                    {
                        // Get the pixel color
                        // Получаем цвет пикселя
                        Color pixel = bitmap.GetPixel(x, y);

                        // Extract RGB values
                        // Извлечение значений RGB
                        byte red = pixel.R;
                        byte green = pixel.G;
                        byte blue = pixel.B;

                        // Extract the current bit of the message
                        // Извлечение текущего бита сообщения
                        byte bitToEmbed = (byte)((messageWithLength[messageIndex] >> (7 - bitIndex)) & 0x01);

                        // Embed the bit into the least significant bit of the red channel
                        // Вставка бита в наименее значимый бит красного канала
                        red = (byte)((red & 0xFE) | bitToEmbed);

                        // Set the modified pixel
                        // Запись измененного пикселя
                        bitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));

                        bitIndex++;
                        if (bitIndex == 8)
                        {
                            bitIndex = 0;
                            messageIndex++;
                        }
                    }
                    if (messageIndex >= messageWithLength.Length)
                    {
                        break;
                    }
                }
                if (messageIndex >= messageWithLength.Length)
                {
                    break;
                }
            }

            // Save the modified image
            // Сохранение нового изображения
            bitmap.Save(outputPath);
        }

        // Method to extract a hidden message from an image
        // Метод для извлечения скрытого сообщения из изображения
        public static string ExtractMessage(string imagePath, int bitDensity)
        {
            // Reading the image
            // Чтение изображения
            Bitmap bitmap = new Bitmap(imagePath);

            // Array to store the extracted data
            // Массив для хранения извлеченных данных
            byte[] extractedData = new byte[bitmap.Width * bitmap.Height];  // Size of the array for extracting bits
            // Размер массива для извлечения битов
            int byteIndex = 0;
            int bitIndex = 0;
            int messageLength = 0;

            // Extracting the message
            // Извлечение сообщения
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    byte red = pixel.R;

                    // Extracting the least significant bit from the red channel
                    // Извлечение наименее значимого бита из красного канала
                    byte extractedBit = (byte)(red & 0x01);

                    // Add the extracted bit to the array
                    // Добавляем извлеченный бит в массив
                    extractedData[byteIndex] |= (byte)(extractedBit << (7 - bitIndex));

                    bitIndex++;
                    if (bitIndex == 8)
                    {
                        bitIndex = 0;
                        byteIndex++;
                    }

                    if (byteIndex >= extractedData.Length)
                    {
                        break;
                    }
                }
                if (byteIndex >= extractedData.Length)
                {
                    break;
                }
            }

            // Reading the length of the message
            // Считывание длины сообщения
            messageLength = BitConverter.ToInt32(extractedData, 0);

            // Checking if the length of the message is valid
            // Проверка на корректность длины сообщения
            if (messageLength > 0 && messageLength <= extractedData.Length - sizeof(int))
            {
                byte[] messageBytes = new byte[messageLength];
                Array.Copy(extractedData, sizeof(int), messageBytes, 0, messageLength);

                // Convert the byte array to a string using UTF-8 encoding
                // Преобразуем массив байт в строку с использованием кодировки UTF-8
                return Encoding.UTF8.GetString(messageBytes);
            }
            else
            {
                // If message length is invalid, return an error message
                // Если длина сообщения неверная, возвращаем сообщение об ошибке
                return "Error extracting message: invalid data length.";
            }
        }
    }
}