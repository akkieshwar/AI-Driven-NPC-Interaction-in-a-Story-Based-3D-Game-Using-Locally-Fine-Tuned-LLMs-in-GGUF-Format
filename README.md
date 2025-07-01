# Elira-NPC-LLM

Fine-tuned Emotional LLM for NPC Dialogue in Games (Unity-Ready, GGUF Export)

---

## Project Overview

**Elira-NPC-LLM** is an open-source, local Large Language Model for building deeply emotional NPC dialogue in any game.  
- Built on [Mistral-7B-Instruct-v0.2](https://huggingface.co/mistralai/Mistral-7B-Instruct-v0.2)
- Fine-tuned on 500+ lines of handcrafted, emotion-tagged conversation
- Exports to GGUF for blazing-fast local inference (llama.cpp, Ollama, etc)
- Modular code: train, merge, inference, dataset, and conversion steps

---

## Download the GGUF Model

You can download the latest Elira NPC GGUF model from HuggingFace:

**[elira-npc-gguf on HuggingFace (Direct Download)](https://huggingface.co/Manofaction/elira-npc-gguf/resolve/main/elira_gguf_model.gguf)**  
(File size: 14.5 GB)

Repo link: [https://huggingface.co/Manofaction/elira-npc-gguf](https://huggingface.co/Manofaction/elira-npc-gguf)

---

# Download Links

## 🧠 Model (GGUF format)
- [elira_gguf_model.gguf (Direct Download, 14.5 GB)](https://huggingface.co/Manofaction/elira-npc-gguf/resolve/main/elira_gguf_model.gguf)
- [HuggingFace Model Repo](https://huggingface.co/Manofaction/elira-npc-gguf)

## 📚 Dataset
- [cleaned_formatted_dataset.jsonl (Direct Download)](https://huggingface.co/datasets/Manofaction/elira-npc-dataset/resolve/main/cleaned_formatted_dataset.jsonl)
- [HuggingFace Dataset Repo](https://huggingface.co/datasets/Manofaction/elira-npc-dataset)

## 🏁 Training Checkpoints
- [HuggingFace Checkpoints Repo](https://huggingface.co/Manofaction/elira-npc-checkpoints)

---

**How to use:**  
1. Download the model (GGUF) for inference with llama.cpp or compatible backends.
2. Dataset is provided for further fine-tuning or inspection.
3. Checkpoints repo contains intermediate or final training artifacts (weights, logs, etc).

---

*See the HuggingFace pages above for additional files, usage instructions, and version history.*


