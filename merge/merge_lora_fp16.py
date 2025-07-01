from transformers import AutoModelForCausalLM, AutoTokenizer
from peft import PeftModel
import torch
import os

base_model_id = "mistralai/Mistral-7B-v0.1"
lora_path = "./elira_mistral_lora_fp16_model"       # Your LoRA weights dir
output_path = "./elira_merged_fp16_full"            # Output merged dir

hf_token = os.environ.get("HF_TOKEN")               # Use environment variable

# Load base model in fp16 (do NOT use 8-bit)
base_model = AutoModelForCausalLM.from_pretrained(
    base_model_id,
    torch_dtype=torch.float16,
    device_map="auto",
    token=hf_token
)

# Load LoRA weights
model = PeftModel.from_pretrained(base_model, lora_path)

# Merge LoRA weights into base model
model = model.merge_and_unload()

# Save the final merged model
model.save_pretrained(output_path)

# Save tokenizer
tokenizer = AutoTokenizer.from_pretrained(base_model_id, token=hf_token)
tokenizer.save_pretrained(output_path)

print("✅ Merge complete. Saved to:", output_path)
