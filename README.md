# AI-Driven NPC Interaction in a Story-Based 3D Game Using Locally Fine-Tuned LLMs in GGUF Format

## Unity Game: Elira NPC Demo

This Unity project is a 3D game demo that lets you chat with an AI-powered NPC (Elira) using natural language. The game uses a locally fine-tuned LLM (Large Language Model) in GGUF format, served via Flask (with Ollama/llama.cpp), to provide real-time, emotion-driven dialogue and immersive gameplay—all fully offline.

---

## Project Overview

**Elira-NPC-LLM** is an open-source, local Large Language Model system for building deeply emotional NPC dialogue in games.

- **Built on:** Mistral-7B-Instruct-v0.2
- **Fine-tuned on:** 500+ handcrafted, emotion-tagged dialogue lines
- **Exports to:** GGUF for blazing-fast local inference (llama.cpp, Ollama, etc.)
- **Modular code:** train, merge, inference, dataset, and conversion steps

---

## How It Works

1. Approach Elira (the NPC) in the village scene.
2. Press **E** when prompted to open the chatbox.
3. Type your message and press **Enter** to send.
4. Your message is sent from Unity to a Python Flask API running locally.
5. The LLM (fine-tuned Mistral-7B in GGUF format, running via Ollama) generates Elira’s response and emotion tag, which are sent back to Unity.
6. The chatbox displays Elira’s reply, and her character’s animation updates to match the detected emotion.

---

## Features

- Natural language chat with an NPC, powered by a custom fine-tuned LLM.
- Emotion detection in NPC responses, triggering matching Unity animations.
- Proximity-based chatbox UI: appears when you get near Elira for smooth, immersive gameplay.
- Works fully offline—all LLM inference is local, no internet required after setup.

---

## Requirements

- Unity 2022.3 LTS or newer.
- Python (for running the Flask server and LLM backend).
- Local LLM model in GGUF format (see below for download/setup).
- Ollama or llama.cpp for model inference.

---

## Setup

1. Clone/download this Unity folder into your projects.
2. See the main project README for backend (Flask/LLM) setup and integration instructions.
3. Start the Flask server and LLM backend before running the Unity game.
4. Open the Unity project and play the scene.

---

## Download the GGUF Model

Download the latest Elira NPC GGUF model from Hugging Face:

- [**elira-npc-gguf** on Hugging Face (Direct Download)](https://huggingface.co/Manofaction/elira-npc-gguf)  
  *File size: 14.5 GB*

---

## Download Links

- 🧠 **Model (GGUF format):**  
  [elira_gguf_model.gguf (Direct Download, 14.5 GB)](https://huggingface.co/Manofaction/elira-npc-gguf)
- 📚 **Dataset:**  
  [cleaned_formatted_dataset.jsonl (Direct Download)](https://huggingface.co/datasets/Manofaction/elira-npc-dataset)
- 🏁 **Training Checkpoints:**  
  [Hugging Face Checkpoints Repo](https://huggingface.co/Manofaction/elira-npc-checkpoints)

**How to use:**
- Download the GGUF model for local inference with llama.cpp, Ollama, or compatible backends.
- Use the dataset for further fine-tuning, inspection, or academic purposes.
- The checkpoints repo contains intermediate/final training artifacts (weights, logs, etc).
- See the Hugging Face links above for extra files, usage instructions, and version history.

---

## Credits

- All assets used are free or open source (Mixamo, Kenney, Quaternius, etc.).
- AI backend uses a locally fine-tuned Mistral-7B LLM, served with Flask and GGUF for fast, private inference.
- For technical details and methodology, see the research paper in `/docs/`.

---

