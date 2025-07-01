import pandas as pd
import torch
from datasets import Dataset
from transformers import (
    AutoTokenizer, AutoModelForCausalLM, TrainingArguments, Trainer,
    DataCollatorForLanguageModeling
)
from peft import LoraConfig, get_peft_model, prepare_model_for_kbit_training
import os

# Step 1: Load and format dataset (no emotion tags, fixed spacing)
df = pd.read_json("cleaned_formatted_dataset.jsonl", lines=True)

def format_example(row):
    return f"{row['instruction']}\n### Player: {row['input']}\n### Elira: {row['output']}"
df['text'] = df.apply(format_example, axis=1)

dataset = Dataset.from_pandas(df[['text']])
split = dataset.train_test_split(test_size=0.05)
train_dataset, eval_dataset = split['train'], split['test']

# Step 2: Load tokenizer and full-precision model
model_name = "mistralai/Mistral-7B-v0.1"
tokenizer = AutoTokenizer.from_pretrained(model_name, trust_remote_code=True)
tokenizer.pad_token = tokenizer.eos_token
tokenizer.padding_side = "left"

model = AutoModelForCausalLM.from_pretrained(model_name, trust_remote_code=True)

# Step 3: Apply LoRA
model = prepare_model_for_kbit_training(model)
lora_config = LoraConfig(
    r=32,
    lora_alpha=64,
    target_modules=["q_proj", "k_proj", "v_proj", "o_proj", "gate_proj", "up_proj", "down_proj"],
    bias="none",
    lora_dropout=0.05,
    task_type="CAUSAL_LM"
)
model = get_peft_model(model, lora_config)
model.enable_input_require_grads()

# Step 4: Tokenize
def tokenize(batch):
    tokens = tokenizer(
        batch["text"],
        padding="max_length",
        truncation=True,
        max_length=512
    )
    tokens.pop("token_type_ids", None)  # Prevents unwanted errors
    return tokens

train_dataset = train_dataset.map(tokenize, batched=True, remove_columns=["text"])
eval_dataset = eval_dataset.map(tokenize, batched=True, remove_columns=["text"])

# Step 5: Training arguments
training_args = TrainingArguments(
    output_dir="elira_mistral_lora_fp16",
    per_device_train_batch_size=6,
    gradient_accumulation_steps=2,
    logging_steps=20,
    evaluation_strategy="steps",
    eval_steps=50,
    learning_rate=2e-4,
    num_train_epochs=3,
    warmup_steps=50,
    fp16=True,
    save_total_limit=2,
    save_steps=100,
    optim="adamw_torch",
    report_to="none"
)

# Step 6: Trainer setup
collator = DataCollatorForLanguageModeling(
    tokenizer=tokenizer,
    mlm=False
)

trainer = Trainer(
    model=model,
    train_dataset=train_dataset,
    eval_dataset=eval_dataset,
    tokenizer=tokenizer,
    args=training_args,
    data_collator=collator
)

# Step 7: Train
trainer.train()

# Step 8: Save model and tokenizer
model.save_pretrained("elira_mistral_lora_fp16_model")
tokenizer.save_pretrained("elira_mistral_lora_fp16_tokenizer")
