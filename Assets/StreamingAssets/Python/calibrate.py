import sys, json, time
from eyeGestures.utils import VideoCapture
import cv2
import numpy as np
from eyeGestures import EyeGestures_v3
import socket

# Настройка UDP-сокета для отправки данных
UDP_IP = "127.0.0.1"  # Локальный адрес; замените, если Unity работает на другом компьютере
UDP_PORT = 5005  # Порт для обмена данными
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Загрузка конфигурации из JSON (путь передается как аргумент командной строки)
config_path = sys.argv[1] if len(sys.argv) > 1 else "calibration_config.json"
sys.stdout.write(f"Используем конфигурацию: {config_path}")
with open(config_path, 'r') as f:
    config = json.load(f)
screen_width = config["screen_width"]
screen_height = config["screen_height"]

# Преобразуем список точек из JSON в список списков [x, y]
calib_points = []
for pair in config["calibration_points"]:
    calib_points.append([float(pair['x']), float(pair['y'])])
sys.stdout.write(f"Screen: {screen_width}, {screen_height}, len = {len(calib_points)}, Calib points:, {calib_points}")

# Инициализация движка EyeGestures_v3
gestures = EyeGestures_v3()
gestures.uploadCalibrationMap(calib_points, context="my_context")

# Открываем камеру
cap = VideoCapture(0)
if not cap.cap.isOpened():
    sys.stdout.write("Error: Camera not accessible")
    sys.exit(1)

current_index = 0
prev_x = None
prev_y = None

# Основной цикл калибровки
while True:
    try:
        ret, frame = cap.read()
        frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

        # frame = np.rot90(frame)
        frame = np.flip(frame, axis=1)

        calibrate = current_index < len(calib_points)
        # Передаем кадр в движок EyeGestures_v3, включаем режим калибровки (True) с указанными размерами и контекстом
        event, calibration = gestures.step(frame, calibrate, screen_width, screen_height,
                                           context="my_context")
        if event is not None or calibration is not None:
            if calibrate:
                if calibration.point[0] != prev_x or calibration.point[1] != prev_y:
                    current_index += 1
                    prev_x = calibration.point[0]
                    prev_y = calibration.point[1]

                    sys.stdout.write("FIXED\n")
                    sys.stdout.flush()
                    time.sleep(0.2)
            else:
                pass
        if event is not None:
            gaze_coords = event.point  # Это массив [x, y]
            normalized_x = gaze_coords[0] / screen_width
            normalized_y = gaze_coords[1] / screen_height
            message = f"{normalized_x},{normalized_y}"
            sock.sendto(message.encode(), (UDP_IP, UDP_PORT))
    except TypeError as e:
        sys.stdout.write("Face dont identify\n")
    except Exception as e:
        sys.stdout.write(f"Error: {str(e)}")

# Завершаем работу
cap.cap.release()
