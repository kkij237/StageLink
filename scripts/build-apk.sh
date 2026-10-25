#!/usr/bin/env bash
# Build de l'APK signé (Release) sans Trimming ni AOT — évite l'Exit Code 137 (OOM) sur Chromebook.
set -euo pipefail
cd "$(dirname "$0")/.."

dotnet build -f net8.0-android -c Release -t:SignAndroidPackage \
  -p:AndroidSdkDirectory="$HOME/android-sdk" \
  -p:AndroidPackageFormat=apk \
  -p:PublishTrimmed=false \
  -p:RunAOTCompilation=false

echo
echo "APK généré :"
find bin/Release/net8.0-android -name "*-Signed.apk" -print
