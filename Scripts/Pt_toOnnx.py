# Установка зависимостей
!pip install ultralytics onnx onnxruntime

import os
from ultralytics import YOLO
import subprocess

# 🔹 Параметры
MODEL_PT = "/content/models/yolo11n.pt"  # загрузи сюда свою модель
EXPORT_DIR = "/content/models"
EXPORT_NAME = "yolo11n.onnx"
FIXED_EXPORT_NAME = "yolo11n_fixed.onnx"
IMG_SIZE = 640
DEVICE = "0"  # GPU 0

# Создаём папку экспорта
os.makedirs(EXPORT_DIR, exist_ok=True)

# 🔹 Экспорт YOLOv11n в ONNX
print("Экспорт модели в ONNX...")
model = YOLO(MODEL_PT)
onnx_path = os.path.join(EXPORT_DIR, EXPORT_NAME)
model.export(
    format="onnx",
    imgsz=IMG_SIZE,
    dynamic=False,   # статическая форма
    simplify=True,
    device=DEVICE,
    project=EXPORT_DIR,
    name=os.path.splitext(EXPORT_NAME)[0]
)
print(f"Модель экспортирована в {onnx_path}")