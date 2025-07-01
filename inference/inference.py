import torch
from transformers import AutoTokenizer, AutoModelForCausalLM

# Load tokenizer and model
tokenizer = AutoTokenizer.from_pretrained("elira_mistral_lora_fp16_tokenizer", trust_remote_code=True)
model = AutoModelForCausalLM.from_pretrained("elira_mistral_lora_fp16_model", trust_remote_code=True)
model.eval()

# Prompts to test
instruction = "Respond as Elira, a scared but hopeful village girl."
player_inputs = [
    "Why are you crying?",
    "What scares you the most?",
    "Do you believe the village will survive?",
    "Have you lost someone dear to you?",
    "Can I help you in any way?"
]

# Run for each player input
for player_input in player_inputs:
    prompt = f"{instruction}\n### Player: {player_input}\n### Elira:"
    inputs = tokenizer(prompt, return_tensors="pt", padding=True).to(model.device)

    with torch.no_grad():
        output = model.generate(
            **inputs,
            max_new_tokens=80,
            do_sample=True,
            temperature=0.7,
            top_p=0.9,
            repetition_penalty=1.15,
            eos_token_id=tokenizer.eos_token_id,
            pad_token_id=tokenizer.pad_token_id
        )

    # Decode and clean output
    decoded = tokenizer.decode(output[0], skip_special_tokens=True)
    response = decoded.split("### Elira:")[-1].strip().split("###")[0].strip()

    # Display
    print(f"\n🧍 Player: {player_input}")
    print(f"👧 Elira: {response}")
