import numpy as np
from flask import Flask, request, jsonify
from simpletransformers.classification import ClassificationModel

model_args = {
    "num_train_epochs": 15,
    "learning_rate": 1e-5,
    "max_seq_length": 512,
    "silent": True
}

model = ClassificationModel(
    'xlmroberta',
    'roberta_folder',
    args=model_args,
    use_cuda=False)

app = Flask(__name__)


@app.route('/classify', methods=['POST'])
def translate_endpoint():
    data = request.get_json()
    text = data.get('text', '')

    if text:
        prediction, logit_output = model.predict([text])
        result = prediction.tolist()[0]
        label = model.config.id2label[result]

        return jsonify({f'result': result, 'label': label})
    else:
        return jsonify({'error': 'BODY should contain \'text\' field'}), 400


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=6000, debug=False)
