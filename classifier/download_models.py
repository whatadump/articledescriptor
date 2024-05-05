from simpletransformers.classification import ClassificationModel
import os

model_args = {
    "num_train_epochs": 15,
    "learning_rate": 1e-5,
    "max_seq_length": 512,
    "silent": True
}

classifier = ClassificationModel(
    "xlmroberta",
    'classla/xlm-roberta-base-multilingual-text-genre-classifier',
    args=model_args,
    use_cuda=False)

classifier.model.save_pretrained('roberta_folder')
classifier.tokenizer.save_pretrained('roberta_folder')
classifier.config.save_pretrained('roberta_folder/')
