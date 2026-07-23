<div align="center">

# 🎧 RTASS — Real-Time Audio Steganography System

### Hide secret messages inside sound. Recover them from the echo.

*A .NET 8 steganography toolkit that embeds hidden text into audio using the **Echo Hiding** technique — imperceptible to the ear, recoverable through signal analysis.*

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![NAudio](https://img.shields.io/badge/NAudio-2.3.0-1DB954?logo=soundcloud&logoColor=white)](https://github.com/naudio/NAudio)
[![Math.NET](https://img.shields.io/badge/Math.NET_Numerics-5.0-orange)](https://numerics.mathdotnet.com/)
[![Platform](https://img.shields.io/badge/platform-Windows-0078D6?logo=windows&logoColor=white)](#)

</div>

---

## 🔍 What is this?

**RTASS** hides arbitrary text messages inside an audio signal so that the carrier still sounds completely normal, while a matching decoder can pull the hidden message back out. Unlike naïve LSB tricks, it uses **Echo Hiding** — encoding bits as tiny, inaudible echoes in the waveform, which survive the analog character of sound far better than bit-flipping.

The project is split into an **Encoder** (embed a message) and a **Decoder** (listen for and extract messages), sharing a common signal-processing core.

## 🧠 How Echo Hiding works

Each bit of the secret message is encoded as an echo added to a short segment of audio, where the **delay of the echo** carries the information:

| Bit | Echo delay | Meaning |
|-----|-----------|---------|
| `1` | ~10 ms (`0.010s`) | "one" kernel |
| `0` | ~7 ms (`0.007s`) | "zero" kernel |

- A **sync pattern** (`10101010 01010101`) marks the start of a message so the decoder can lock on.
- A **32-bit length header** tells the decoder exactly how many bits to read.
- The decoder analyzes each segment's **cepstrum (FFT-based)** to measure the echo delay and recover the original bit.

Default signal parameters (`RTASS.Common/Constants/AppConstants.cs`):

```
Sample rate    : 48 kHz          Segment length : 8192 samples
Bit depth      : 16-bit mono     Decay rate     : 0.2
Delay (bit 1)  : 10 ms           Mixing rate    : 0.7
Delay (bit 0)  : 7 ms
```

## 🏗️ Architecture

```
RTASS.sln
├── RTASS.Common        → shared signal-processing core
│   ├── Audio/          → capture, read, write & process WAV/PCM (NAudio)
│   ├── Steganography/  → Echo Hiding kernels & encoder/decoder interfaces
│   ├── Helpers/        → FFT, bit packing, windowing, file utils
│   ├── Business/       → encoding / decoding orchestration
│   └── Constants/      → tunable DSP parameters
├── RTASS.Encoder       → console app: embed a message into audio
└── RTASS.Decoder       → console app: detect & extract hidden messages
```

**Tech stack:** .NET 8 · [NAudio](https://github.com/naudio/NAudio) (audio capture & I/O) · [Math.NET Numerics](https://numerics.mathdotnet.com/) (FFT / DSP) · Echo-Hiding steganography

## ▶️ Getting started

**Prerequisites:** [.NET 8 SDK](https://dotnet.microsoft.com/download) on Windows.

```bash
git clone https://github.com/emreatlihan/Real-Time-Audio-Steganography.git
cd Real-Time-Audio-Steganography
dotnet build
```

Both apps share a working directory at `C:\RTASS_Shared` (created automatically). The carrier audio lives there as `audio_carrier.wav`.

### 1. Encode a message

```bash
dotnet run --project RTASS.Encoder
```

You'll be prompted for the text to hide. The encoder embeds it into the carrier and writes the result back to the shared folder. *(If no carrier exists yet, a short test tone is generated automatically.)*

### 2. Decode it

```bash
dotnet run --project RTASS.Decoder
```

The decoder reads the carrier, locks onto the sync pattern, and prints the recovered message.

## 🎯 Use cases

- Covert-channel and DSP research / coursework
- Demonstrating audio watermarking & steganalysis concepts
- Experimenting with echo-kernel design and FFT-based cepstral analysis

> ⚠️ **Educational project.** Built to explore audio steganography and digital signal processing — not intended as a hardened security product.

## 📄 License

Released under the [MIT License](LICENSE).

---

<div align="center">
Crafted with 🎛️ and .NET by <a href="https://github.com/emreatlihan">emreatlihan</a>
</div>
