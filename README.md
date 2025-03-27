# StegoHide

## Description
**StegoHide** is a command-line program for steganographic hiding and extraction of text messages in 24-bit BMP images. The program uses the Least Significant Bit (LSB) method, embedding information in the least significant bits of the red (R) channel of each pixel.

## Features
- Embeds a text message into a BMP image.
- Extracts hidden messages from images.
- Supports adjustable embedding density (from 1 to 8 bits per byte).
- Validates input parameters and handles errors.

## Installation
To use the program, you need to have .NET SDK installed.

1. Clone the repository:
   ```sh
   git clone https://github.com/your-username/StegoHide.git
   cd StegoHide
   ```
2. Build the project:
   ```sh
   dotnet build
   ```

## Usage

### Embedding a Message
```sh
StegoHide embed "image.bmp" "output.bmp" "Hello World!" 8
```
- `image.bmp` — the original BMP image.
- `output.bmp` — the image where the message will be hidden.
- `"Hello World!"` — the text message to embed.
- `8` — embedding density (bits per byte, from 1 to 8).

### Extracting a Message
```sh
StegoHide extract "output.bmp" 8
```
- `output.bmp` — the image with the hidden message.
- `8` — the bit density used during embedding.

## How It Works

### Embedding a Message (EmbedMessage)
1. The BMP image is loaded.
2. The message is converted into a byte array, including a header with its length (4 bytes).
3. The message bits are sequentially written into the least significant bits of the red (R) channel of each pixel.
4. The modified image is saved to a file.

### Extracting a Message (ExtractMessage)
1. The BMP image is loaded.
2. The least significant bits of the red (R) channel of the pixels are read.
3. The message length is reconstructed from the first 4 bytes.
4. The remaining bits are read and converted back to text using UTF-8 encoding.

## Error Handling
- Checks the number of arguments and displays usage instructions if incorrect.
- If the specified file is not found, an error message is displayed.
- If errors occur during embedding or extraction, an appropriate message is shown.
