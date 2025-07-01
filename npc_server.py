from flask import Flask, request, jsonify
from flask_cors import CORS
import requests

app = Flask(__name__)
CORS(app)

OLLAMA_URL = "http://localhost:11434/api/generate"
OLLAMA_MODEL = "elira-npc"  # Change to your model's name

@app.route('/chat', methods=['POST'])
def chat():
    # Expect: { "message": "hi there" }
    data = request.get_json()
    if not data or 'message' not in data:
        return jsonify({"error": "No message provided"}), 400

    user_msg = data['message']
    # Format the prompt in the style your LLM expects
    prompt = f"""You are Elira, a brave village girl in a fantasy world.

### Player: {user_msg}
### Elira:"""

    payload = {
        "model": OLLAMA_MODEL,
        "prompt": prompt,
        "max_tokens": 150,
        "temperature": 0.7,
        "stop": ["###"],  # Stop generation at any new section
        "repeat_penalty": 1.2,  # match your curl, or use 1.4 if you prefer
        "top_p": 0.85
    }

    try:
        response = requests.post(OLLAMA_URL, json=payload, timeout=60)
        response.raise_for_status()
        result = response.json()

        # Ollama's LLM returns the reply in either 'response' or 'completion'
        npc_reply = result.get("response") or result.get("completion") or ""
        npc_reply = npc_reply.strip()

        # Always send back { "content": "..." } for Unity
        return jsonify({"content": npc_reply}), 200

    except Exception as e:
        print("🔥 Exception:", repr(e))
        return jsonify({"content": "Sorry, I can't talk right now."}), 500

if __name__ == '__main__':
    app.run(host="127.0.0.1", port=5000)
