from transformers import AutoModelForCausalLM, AutoTokenizer, pipeline

model_path = "./elira_merged_fp16_full"
prompt = """Respond as Elira, a scared but hopeful village girl.
### Player: hi there
### Elira:"""

# Load model and tokenizer
tokenizer = AutoTokenizer.from_pretrained(model_path)
model = AutoModelForCausalLM.from_pretrained(model_path, torch_dtype="auto", device_map="auto")

# Use pipeline for simplicity
pipe = pipeline("text-generation", model=model, tokenizer=tokenizer, device_map="auto")

# Run inference
output = pipe(
    prompt,
    max_new_tokens=80,
    do_sample=True,
    top_p=0.9,
    temperature=0.7,
    eos_token_id=tokenizer.eos_token_id,
    pad_token_id=tokenizer.eos_token_id,
)[0]['generated_text']

print("\n=== Model Output ===\n")
print(output)
