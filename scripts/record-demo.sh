#!/usr/bin/env bash
# Enregistre la vidéo de démo de 30 s sur le téléphone relié en USB (ou en adb sans fil).
#   1) installe l'APK, 2) démarre screenrecord (30 s), 3) lance l'app avec --ez demo true,
#      ce qui déclenche la visite guidée automatique (DemoTourService), 4) rapatrie le .mp4.
#
# Usage : ./scripts/record-demo.sh [chemin/vers/app.apk] [sortie.mp4]
set -euo pipefail

ADB="${ADB:-$HOME/android-sdk/platform-tools/adb}"
PKG="com.stagelink.app"
ACTIVITY="$PKG/com.stagelink.app.MainActivity"
APK="${1:-$(find "$(dirname "$0")/../bin/Release/net8.0-android" -name '*-Signed.apk' | head -n 1)}"
OUT="${2:-stagelink-demo.mp4}"
REMOTE="/sdcard/stagelink-demo.mp4"

echo "== Appareils connectés =="
"$ADB" devices
"$ADB" get-state >/dev/null   # échoue proprement si aucun appareil

echo "== Installation de $APK =="
"$ADB" install -r "$APK"

"$ADB" shell am force-stop "$PKG"
"$ADB" shell rm -f "$REMOTE"
"$ADB" shell input keyevent KEYCODE_WAKEUP

echo "== Enregistrement de 30 s =="
"$ADB" shell screenrecord --time-limit 30 --bit-rate 6000000 "$REMOTE" &
REC_PID=$!
sleep 1

"$ADB" shell am start -n "$ACTIVITY" --ez demo true >/dev/null

wait "$REC_PID" || true
sleep 1

echo "== Récupération de la vidéo =="
"$ADB" pull "$REMOTE" "$OUT"
echo "Vidéo enregistrée : $OUT"
